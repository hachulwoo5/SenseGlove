using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace SG
{

    public class SG_PhysicsGrab_Custom : SG_GrabScript
    {
        /// <summary> The Hand Palm collider, used when grabbing objects between the palm and finger (tool/handle grips) </summary>
        [Header("Physics Grab Components")]
        public SG_HoverCollider palmTouch;
        /// <summary> Thumb collider, used to determine finger/thumb collision </summary>
        public SG_HoverCollider thumbTouch;
        public SG_HoverCollider indexTouch;
        public SG_HoverCollider middleTouch;
        public SG_HoverCollider ringTouch;
        public SG_HoverCollider pinkyTouch;
        public SG_HoverCollider thumbTouch_2;
        public SG_HoverCollider indexTouch_2;
        public SG_HoverCollider indexTouch_3;
        public SG_HoverCollider middleTouch_2;
        public SG_HoverCollider middleTouch_3;
        public SG_HoverCollider ringTouch_2;
        public SG_HoverCollider ringTouch_3;
        public SG_HoverCollider pinkyTouch_2;
        public SG_HoverCollider pinkyTouch_3;


        /// <summary> Keeps track of the 'grabbing' pose of fingers </summary>
        protected bool[] wantsGrab = new bool[3];
        /// <summary> Above these flexions, the hand is considered 'open' </summary>
        protected static float[] openHandThresholds = new float[5] { 0.1f, 0.2f, 0.2f, 0.2f, 0.2f };
        /// <summary> below these flexions, the hand is considered 'open' </summary>
        protected static float[] closedHandThresholds = new float[5] { 2, 0.9f, 0.9f, 0.9f, 0.9f }; //set to -360 so it won;t trigger for now

        protected float releaseThreshold = 0.05f;
        protected bool[] grabRelevance = new bool[5];
        protected bool snapFrame = false;

        /// <summary> All fingers, used to iterate through the fingers only. </summary>
        protected SG_HoverCollider[] fingerScripts = new SG_HoverCollider[0];
        /// <summary> All HoverScripts, easier to iterate trhough </summary>
        protected SG_HoverCollider[] hoverScripts = new SG_HoverCollider[0];

        protected static float overrideGrabThreshold = 0.01f;

        protected float[] lastNormalized = new float[5];
        protected float[] normalizedOnGrab = new float[5];

        public bool[] Checklist = new bool[6];
        public bool[] SideChecklist = new bool[5];

        public int pointIndex = 0;
        public int sidePointIndex = 0;
        public bool isNormalGrabbing = false;
        public bool isSideGrabbing =false;

        protected override void Start()
        {
            base.Start();
        }
        protected override void CreateComponents()
        {
            base.CreateComponents();
            fingerScripts = new SG_HoverCollider[15];
            fingerScripts[0] = thumbTouch;
            fingerScripts[1] = indexTouch;
            fingerScripts[2] = middleTouch;
            fingerScripts[3] = ringTouch;
            fingerScripts[4] = pinkyTouch;

            fingerScripts[5] = thumbTouch_2;
            fingerScripts[6] = indexTouch_2;
            fingerScripts[7] = indexTouch_3;
            fingerScripts[8] = middleTouch_2;
            fingerScripts[9] = middleTouch_3;
            fingerScripts[10] = ringTouch_2;
            fingerScripts[11] = ringTouch_3;
            fingerScripts[12] = pinkyTouch_2;
            fingerScripts[13] = pinkyTouch_3;
            fingerScripts[14] = palmTouch;

            hoverScripts = new SG_HoverCollider[15];
            hoverScripts[0] = thumbTouch;
            hoverScripts[1] = indexTouch;
            hoverScripts[2] = middleTouch;
            hoverScripts[3] = ringTouch;
            hoverScripts[4] = pinkyTouch;

            hoverScripts[5] = thumbTouch_2;
            hoverScripts[6] = indexTouch_2;
            hoverScripts[7] = indexTouch_3;
            hoverScripts[8] = middleTouch_2;
            hoverScripts[9] = middleTouch_3;
            hoverScripts[10] = ringTouch_2;
            hoverScripts[11] = ringTouch_3;
            hoverScripts[12] = pinkyTouch_2;
            hoverScripts[13] = pinkyTouch_3;

            hoverScripts[14] = palmTouch;


        }

        protected override void CollectDebugComponents(out List<GameObject> objects, out List<MeshRenderer> renderers)
        {
            base.CollectDebugComponents(out objects, out renderers);
            for (int i = 0; i < this.hoverScripts.Length; i++)
            {
                Util.SG_Util.CollectComponent(hoverScripts[i], ref renderers);
                Util.SG_Util.CollectGameObject(hoverScripts[i].debugTxt, ref objects);
            }
        }

        protected override List<Collider> CollectPhysicsColliders()
        {
            List<Collider> res = base.CollectPhysicsColliders();
            for (int i = 0; i < this.hoverScripts.Length; i++)
            {
                SG.Util.SG_Util.GetAllColliders(this.hoverScripts[i].gameObject, ref res);
            }
            return res;
        }

        protected override void LinkToHand_Internal(SG_TrackedHand newHand, bool firstLink)
        {
            base.LinkToHand_Internal(newHand, firstLink);
            //link colliders
            SG_HandPoser3D trackingTargets = newHand.GetPoser(SG_TrackedHand.TrackingLevel.VirtualPose);
            for (int i = 0; i < this.hoverScripts.Length; i++) //link them to wherever they want to be
            {
                trackingTargets.ParentObject(hoverScripts[i].transform, hoverScripts[i].linkMeTo); //Instead of following a frame behind, we're childing.
                hoverScripts[i].updateTime = SG_SimpleTracking.UpdateDuring.Off; //we still need it for hovering(!)
                //Transform target = trackingTargets.GetTransform(hoverScripts[i].linkMeTo);
                //hoverScripts[i].SetTrackingTarget(target, true);
                //hoverScripts[i].updateTime = SG_SimpleTracking.UpdateDuring.Off; //no longer needs to update...
            }
        }


        //----------------------------------------------------------------------------------------------
        // PhysicsGrab Functions


        /// <summary> Returns true if an SG_Interactable is inside a list of other SG_Interactables </summary>
        /// <param name="heldObject"></param>
        /// <param name="objectsToGrab"></param>
        /// <returns></returns>
        public static bool IsInside(SG_Interactable heldObject, List<SG_Interactable> objectsToGrab)
        {
            for (int i = 0; i < objectsToGrab.Count; i++)
            {
                if (GameObject.ReferenceEquals(objectsToGrab[i].gameObject, heldObject.gameObject))
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary> Returns all grabables that both fingers are touching </summary>
        /// <param name="finger1"></param>
        /// <param name="finger2"></param>
        /// <returns></returns>
        public SG_Interactable[] GetMatching(int finger1, int finger2)
        {
            return GetMatching(finger1, fingerScripts[finger2]);
        }


        /// <summary> Returns all grabables that both fingers are touchign </summary>
        /// <param name="finger1"></param>
        /// <param name="finger2"></param>
        /// <returns></returns>
        public SG_Interactable[] GetMatching(int finger1, SG_HoverCollider touch)
        {
            if (fingerScripts[finger1] != null && touch != null)
            {
                return fingerScripts[finger1].GetMatchingObjects(touch);
            }
            return new SG_Interactable[] { };
        }


        /// <summary> Returns true if a specific fingers wants to grab on (when not grabbing). </summary>
        /// <param name="finger"></param>
        /// <returns></returns>
        protected bool WantsGrab(int finger)
        {
            return lastNormalized[finger] >= openHandThresholds[finger] && lastNormalized[finger] <= closedHandThresholds[finger];
        }


        /// <summary> Returns a list of all objects that are grabable at this moment. </summary>
        /// <returns></returns>
        /// 현재 구성 방식은 단순히 3구역 이상이 닿았을 때 그랩을 한다.
        public List<SG_Interactable> ObjectsGrabableNow()
        {
            List<SG_Interactable> res = new List<SG_Interactable>();
                   
            if (pointIndex ==2)
            {
                // 두개 이상의 사이드 포인트, 즉 두손가락의 옆면이 닿아야 실행한다. 중복된 물체인지 확인을 한다.
               if(sidePointIndex>1)
                {
                    isSideGrabbing = true;
                    for (int i = 0; i < fingerScripts.Length; i++)
                    {
                        if (fingerScripts[i].parentObject.isReadyGrab)
                        {
                            for (int j = 0; j < fingerScripts.Length; j++)
                            {
                                if (i != j && fingerScripts[j].parentObject.isReadyGrab)
                                {
                                    SG_Interactable[] matching = fingerScripts[i].GetMatchingObjects(fingerScripts[j]);
                                    for (int k = 0; k < matching.Length; k++)
                                    {
                                        SG.Util.SG_Util.SafelyAdd(matching[k], res);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            // 감지 구역이 3개 이상이고, 엄지나 손바닥이 닿앗는지 확인한다. 일반 그랩 확인 절차
            if (pointIndex> 2)
            {
                isNormalGrabbing = true;
                if ( fingerScripts [0] .parentObject.isReadyGrab || fingerScripts [ 5 ]. parentObject. isReadyGrab ) // 엄지 체크
                {
                    for (int i = 0; i < fingerScripts. Length; i++)
                    {
                        if ( fingerScripts [ i]. parentObject. isReadyGrab )
                        {
                            for (int j = 0; j < fingerScripts. Length; j++)
                            {
                                // 자기 자신과의 매칭은 제외
                                if (i != j && fingerScripts [ j]. parentObject. isReadyGrab )
                                {
                                    SG_Interactable[] matching = fingerScripts[i].GetMatchingObjects(fingerScripts[j]);                                  
                                    for (int k = 0; k < matching.Length; k++)
                                    {
                                        SG.Util.SG_Util.SafelyAdd(matching[k], res);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (Checklist[0]) // 엄지 확인 후 없으면 손바닥 체크
                {
                    for (int i = 0; i < fingerScripts.Length; i++)
                    {
                        if (fingerScripts[i].parentObject.isReadyGrab)
                        {
                            for (int j = 0; j < fingerScripts.Length; j++)
                            {
                                if (i != j && fingerScripts[j].parentObject.isReadyGrab)
                                {
                                    SG_Interactable[] matching = fingerScripts[i].GetMatchingObjects(fingerScripts[j]);
                                    for (int k = 0; k < matching.Length; k++)
                                    {
                                        SG.Util.SG_Util.SafelyAdd(matching[k], res);
                                    }
                                }
                            }
                        }
                    }
                }
                

                // Debug.Log("Found " + matching.Length + " matching objects within detected fingers");
            }

            return res;
        }

        /// <summary> Returns a list of fingers that are currently touching a particular interactable </summary>
        /// <returns></returns>
        public bool[] FingersTouching(SG_Interactable obj)
        {
            bool[] res = new bool[5];
            for (int f = 0; f < 5; f++)
            {
                res[f] = this.fingerScripts[f].IsTouching(obj);
            }
            return res;
        }





        public static bool[] GetGrabIntent(float[] normalizedFlex)
        {
            bool[] res = new bool[5];
            for (int f = 0; f < normalizedFlex.Length; f++) //go through each finger -but- the thumb.?
            {
                res[f] = normalizedFlex[f] >= openHandThresholds[f] && normalizedFlex[f] <= closedHandThresholds[f];
            }
            return res;
        }


        public override void UpdateDebugger()
        {
            //Doesn't do anything
        }


        protected void EvaluateGrab()
        {
            // Collect objects that we're allowed to grab.
            List<SG_Interactable> objToGrab = this.ObjectsGrabableNow();

            //TODO; Check for a littlest bit of intent. Some for of flexion. Because now you can still slam your hand into something.
            if (objToGrab.Count > 0)
            {
                SG_Interactable[] sortedGrabables = SG.Util.SG_Util.SortByProximity(this.ProximitySource.position, objToGrab.ToArray());
                //attempt to grab each object I can, starting with the closest
                for (int i = 0; i < sortedGrabables.Length; i++)
                {
                    TryGrab(sortedGrabables[i]);
                    if (!CanGrabNewObjects) { break; } //stop going through the objects if we can no longer grab one
                }
            }
            else if (this.handPoseProvider != null && this.handPoseProvider.OverrideGrab() > overrideGrabThreshold)
            {
                SG_Interactable[] grabablesInHover = this.virtualHoverCollider.GetTouchedObjects(this.ProximitySource);
                //attempt to grab each object I can, starting with the first
                for (int i = 0; i < grabablesInHover.Length; i++)
                {
                    TryGrab(grabablesInHover[i]);
                    if (!CanGrabNewObjects) { break; } //stop going through the objects if we can no longer grab one
                }
            }
            if (this.IsGrabbing) //we managed to grab something.
            {
                this.grabRelevance = new bool[0]; //clear this so we re-register it the first frame
                snapFrame = false; //we don't check for collision the first frame after grabbing.
                //Debug.Log(Time.timeSinceLevelLoad + ": " + (this.handPoseProvider.TracksRightHand() ? "Right Hand" : "Left Hand") + " Grabbed Object(s)");
            }
        }


        protected void EvaluateRelease()
        {
            SG_Interactable heldObj = this.heldObjects[0];
            bool[] currentTouched = this.FingersTouching(heldObj); //the fingers that are currently touching the (first) object
            
            //Step 1 : Evaluate Intent - If ever there was any
            if (this.grabRelevance.Length == 0)
            {
                //first time after snapping. HoverColliders should have had a frame to catch up.
                bool oneGrabRelevance = false;
                for (int f = 1; f < currentTouched.Length; f++) //Because of the bullshit snapping, I don't want to evaluate releasing until at least a finger (no thumb) touches the object
                {                                           //this should always be true unless we're snapping.
                    if (currentTouched[f])
                    {
                        oneGrabRelevance = true;
                        break;
                    }
                }
                if (oneGrabRelevance) //there is a t least one relevant finger now touching.
                {
                    this.grabRelevance = currentTouched;
                    this.normalizedOnGrab = Util.SG_Util.ArrayCopy(this.lastNormalized);
                }
            }
            else //check for any changes in grabrelevance
            {
                for (int f = 1; f <5; f++)
                {
                    if (!grabRelevance[f] && currentTouched[f]) //first time this finger touches the object after grasping. Log its flexion.
                    {
                        normalizedOnGrab[f] = lastNormalized[f];
                        grabRelevance[f] = true;
                    }
                }
            }

                bool grabDesired;
                if(isNormalGrabbing)
                {
                    if (pointIndex > 2)
                    {
                    grabDesired = true;
                    }
                    else
                    {
                    grabDesired = false;
                    }
                }
                else if (isSideGrabbing)
                {
                    if (sidePointIndex > 1)
                    {
                        grabDesired = true;
                    }
                    else
                    {
                        grabDesired = false;
                    }
                }
                else
                {
                    grabDesired = false;
                }


                //Step 4 - Compare with override to make a final judgement. Can be optimized by placing this before Step 2 and skipping it entirely while GrabOverride is true.

                

                bool overrideGrab = this.handPoseProvider != null && this.handPoseProvider.OverrideGrab() > overrideGrabThreshold; //we start with wanting to release based on overriding.
                bool shouldRelease = !(grabDesired || overrideGrab);
                if (shouldRelease) //We can no longer grab anything
                {                
                        isNormalGrabbing = false;                
                        isSideGrabbing = false;
                    
                    this.ReleaseAll(false);
                }
            
        }

        





        public override void UpdateGrabLogic(float dT)
        {
            base.UpdateGrabLogic(dT);  //updates reference location(s).



            // Update Physics Colliders
            for (int i = 0; i < this.hoverScripts.Length; i++)
            {
                this.hoverScripts[i].UpdateLocation();
            }

            // Re-collect Normalized Flexion
            //We'l try our best to retrieve the last flexions. If it fails, we use the one before it as backup.
            if (this.handPoseProvider != null && this.handPoseProvider.IsConnected())
            {
                float[] currFlex;
                if (this.handPoseProvider.GetNormalizedFlexion(out currFlex)) //can fail because of a parsing error, in which case we must not assign it.
                {
                    this.lastNormalized = currFlex;
                }
            }
            this.wantsGrab = GetGrabIntent(this.lastNormalized); //doing this here so I can evaluate from inspector

            CheckSection ( );

            // Evaluate Grabbing / Releasing
            if ( this.IsGrabbing) //Check for release - Gesture Based
            {
                EvaluateRelease();
            }
            else //Check for Grab (collision based)
            {
                EvaluateGrab();
            }

           

        }

        /// <summary>
        /// 그랩 검사할 때 처음 CheckSection 진행으로 bool 배열에 전부 값 넣고
        /// checklist의 true 값 3개이상이면 그랩한다는 조건으로 수정
        /// </summary>
        void CheckSection()
        {
            Checklist[0] = palmTouch.parentObject.isReadyGrab;


            Checklist[1] = thumbTouch.parentObject.isReadyGrab || thumbTouch_2.parentObject.isReadyGrab;
            Checklist[2] = indexTouch.parentObject.isReadyGrab || indexTouch_2.parentObject.isReadyGrab || indexTouch_3.parentObject.isReadyGrab;
            Checklist[3] = middleTouch.parentObject.isReadyGrab || middleTouch_2.parentObject.isReadyGrab || middleTouch_3.parentObject.isReadyGrab;
            Checklist[4] = ringTouch.parentObject.isReadyGrab || ringTouch_2.parentObject.isReadyGrab || ringTouch_3.parentObject.isReadyGrab;
            Checklist[5] = pinkyTouch.parentObject.isReadyGrab || pinkyTouch_2.parentObject.isReadyGrab || pinkyTouch_3.parentObject.isReadyGrab;

            SideChecklist[0] = thumbTouch.parentObject.isSideGrab || thumbTouch_2.parentObject.isSideGrab || indexTouch_3.parentObject.isSideGrab;
            SideChecklist[1] = indexTouch.parentObject.isSideGrab || indexTouch_2.parentObject.isSideGrab || indexTouch_3.parentObject.isSideGrab;
            SideChecklist[2] = middleTouch.parentObject.isSideGrab || middleTouch_2.parentObject.isSideGrab || middleTouch_3.parentObject.isSideGrab;
            SideChecklist[3] = ringTouch.parentObject.isSideGrab || ringTouch_2.parentObject.isSideGrab || ringTouch_3.parentObject.isSideGrab;
            SideChecklist[4] = pinkyTouch.parentObject.isSideGrab || pinkyTouch_2.parentObject.isSideGrab || pinkyTouch_3.parentObject.isSideGrab;

            pointIndex = Checklist.Count(value => value);
            sidePointIndex = SideChecklist.Count(value => value);


        }
    }

    
}