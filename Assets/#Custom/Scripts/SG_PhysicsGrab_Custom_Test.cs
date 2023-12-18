using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace SG
{

    public class SG_PhysicsGrab_Custom_Test : SG_GrabScript
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
        public int colIndex = 0;
        protected override void Start()
        {
            base.Start();
        }
        protected override void CreateComponents()
        {
            base.CreateComponents();
            fingerScripts = new SG_HoverCollider[5];
            fingerScripts[0] = thumbTouch;
            fingerScripts[1] = indexTouch;
            fingerScripts[2] = middleTouch;
            fingerScripts[3] = middleTouch;
            fingerScripts[4] = middleTouch;


            hoverScripts = new SG_HoverCollider[6];
            hoverScripts[0] = palmTouch;
            hoverScripts[1] = thumbTouch;
            hoverScripts[2] = indexTouch;
            hoverScripts[3] = middleTouch;
            hoverScripts[4] = ringTouch;
            hoverScripts[5] = pinkyTouch;

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


        

        /// <summary> Returns a list of fingers that are currently touching a particular interactable </summary>
        /// <returns></returns>
        public bool[] FingersTouching(SG_Interactable obj)
        {
            bool[] res = new bool[5];
            for (int f = 0; f < this.fingerScripts.Length; f++)
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
           
        }


        protected void EvaluateRelease()
        {
            
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

            // Evaluate Grabbing / Releasing
            if (this.IsGrabbing) //Check for release - Gesture Based
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
            Checklist[0] = thumbTouch.parentObject.isReadyGrab;
            Checklist[1] = indexTouch.parentObject.isReadyGrab;
            Checklist[2] = middleTouch.parentObject.isReadyGrab;
            Checklist[3] = ringTouch.parentObject.isReadyGrab;
            Checklist[4] = pinkyTouch.parentObject.isReadyGrab;
            Checklist[5] = palmTouch.parentObject.isReadyGrab;
        }
    }

    
}