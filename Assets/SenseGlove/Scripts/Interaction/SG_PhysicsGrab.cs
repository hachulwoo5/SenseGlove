using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace SG
{

    public class SG_PhysicsGrab : SG_GrabScript
    {
        /// <summary> The Hand Palm collider, used when grabbing objects between the palm and finger (tool/handle grips) </summary>
        [ Header ( "Physics Grab Components" )]
        public SG_HoverCollider palmTouch;
        public bool isPalmTouch;
        /// <summary> Thumb collider, used to determine finger/thumb collision </summary>
        public SG_HoverCollider thumbTouch;
        /// <summary> Index collider, used to determine finger/thumb and finger/palm collision </summary>
        public SG_HoverCollider indexTouch;
        public SG_HoverCollider indexTouchSideL;
        public SG_HoverCollider indexTouchSideR;

        public SG_HoverCollider middleTouch;
        public SG_HoverCollider middleTouchSideL;
        public SG_HoverCollider middleTouchSideR;

        public SG_HoverCollider ringTouch;
        public SG_HoverCollider ringTouchSideL;
        public SG_HoverCollider ringTouchSideR;

        public SG_HoverCollider pinkyTouch;
        public SG_HoverCollider pinkyTouchSideL;
        public SG_HoverCollider pinkyTouchSideR;


        /// <summary> Keeps track of the 'grabbing' pose of fingers </summary>
        protected bool [ ] wantsGrab = new bool [ 5 ];
        /// <summary> Above these flexions, the hand is considered 'open' </summary>
        protected static float [ ] openHandThresholds = new float [ 5 ] { 0.1f , 0.2f , 0.2f , 0.2f , 0.3f };
        /// <summary> below these flexions, the hand is considered 'open' </summary>
        protected static float [ ] closedHandThresholds = new float [ 5 ] { 2 , 0.9f , 0.9f , 0.9f , 0.9f }; //set to -360 so it won;t trigger for now

        public float [ ] grabDiff = new float [ 5 ]; //DEBUG

        protected float releaseThreshold = 0.05f;
        public bool [ ] grabRelevance = new bool [ 5 ];
        protected bool snapFrame = false;

        /// <summary> All fingers, used to iterate through the fingers only. </summary>
        protected SG_HoverCollider [ ] fingerScripts = new SG_HoverCollider [ 0 ];
        /// <summary> All HoverScripts, easier to iterate trhough </summary>
        protected SG_HoverCollider [ ] hoverScripts = new SG_HoverCollider [ 0 ];

        protected static float overrideGrabThreshold = 0.01f;

        public float [ ] lastNormalized = new float [ 5 ];
        protected float [ ] normalizedOnGrab = new float [ 5 ];

        protected override void CreateComponents ( )
        {
            base. CreateComponents ( );
            fingerScripts = new SG_HoverCollider [ 13 ];
            fingerScripts [ 0 ] = thumbTouch;
            fingerScripts [ 1 ] = indexTouch;
            fingerScripts [ 2 ] = middleTouch;
            fingerScripts [ 3 ] = ringTouch;
            fingerScripts [ 4 ] = pinkyTouch;

            fingerScripts [ 5 ] = indexTouchSideL;
            fingerScripts [ 6 ] = indexTouchSideR;
            fingerScripts [ 7 ] = middleTouchSideL;
            fingerScripts [ 8 ] = middleTouchSideR;
            fingerScripts [ 9 ] = ringTouchSideL;
            fingerScripts [ 10 ] = ringTouchSideR;
            fingerScripts [ 11 ] = pinkyTouchSideL;
            fingerScripts [ 12 ] = pinkyTouchSideR;

            hoverScripts = new SG_HoverCollider [ 14 ];
            hoverScripts [ 0 ] = thumbTouch;
            hoverScripts [ 1 ] = indexTouch;
            hoverScripts [ 2 ] = middleTouch;
            hoverScripts [ 3 ] = palmTouch;
            hoverScripts [ 4 ] = ringTouch;
            hoverScripts [ 5 ] = pinkyTouch;

            hoverScripts [ 6 ] = indexTouchSideL;
            hoverScripts [ 7 ] = indexTouchSideR;
            hoverScripts [ 8 ] = middleTouchSideL;
            hoverScripts [ 9 ] = middleTouchSideR;
            hoverScripts [ 10 ] = ringTouchSideL;
            hoverScripts [ 11 ] = ringTouchSideR;
            hoverScripts [ 12 ] = pinkyTouchSideL;
            hoverScripts [ 13 ] = pinkyTouchSideR;




        }


        #region 내가안보는부분


        protected override void CollectDebugComponents ( out List<GameObject> objects , out List<MeshRenderer> renderers )
        {
            base. CollectDebugComponents ( out objects , out renderers );
            for ( int i = 0 ; i < this. hoverScripts. Length ; i++ )
            {
                Util. SG_Util. CollectComponent ( hoverScripts [ i ] , ref renderers );
                Util. SG_Util. CollectGameObject ( hoverScripts [ i ]. debugTxt , ref objects );
            }
        }

        protected override List<Collider> CollectPhysicsColliders ( )
        {
            List<Collider> res = base. CollectPhysicsColliders ( );
            for ( int i = 0 ; i < hoverScripts .LongLength; i++ )
            {
                SG. Util. SG_Util. GetAllColliders ( this. hoverScripts [ i ]. gameObject , ref res );
            }
            return res;
        }

        protected override void LinkToHand_Internal ( SG_TrackedHand newHand , bool firstLink )
        {
            base. LinkToHand_Internal ( newHand , firstLink );
            //link colliders
            SG_HandPoser3D trackingTargets = newHand. GetPoser ( SG_TrackedHand. TrackingLevel. VirtualPose );
            for ( int i = 0 ; i < hoverScripts .Length; i++ ) //link them to wherever they want to be
            {
                trackingTargets. ParentObject ( hoverScripts [ i ]. transform , hoverScripts [ i ]. linkMeTo ); //Instead of following a frame behind, we're childing.
                hoverScripts [ i ]. updateTime = SG_SimpleTracking. UpdateDuring. Off; //we still need it for hovering(!)
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
        public static bool IsInside ( SG_Interactable heldObject , List<SG_Interactable> objectsToGrab )
        {
            for ( int i = 0 ; i < objectsToGrab. Count ; i++ )
            {
                if ( GameObject. ReferenceEquals ( objectsToGrab [ i ]. gameObject , heldObject. gameObject ) )
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
        public SG_Interactable [ ] GetMatching ( int finger1 , int finger2 )
        {
            return GetMatching ( finger1 , fingerScripts [ finger2 ] );
        }


        /// <summary> Returns all grabables that both fingers are touchign </summary>
        /// <param name="finger1"></param>
        /// <param name="finger2"></param>
        /// <returns></returns>
        public SG_Interactable [ ] GetMatching ( int finger1 , SG_HoverCollider touch )
        {
            if ( fingerScripts [ finger1 ] != null && touch != null )
            {
                return fingerScripts [ finger1 ]. GetMatchingObjects ( touch );
            }
            return new SG_Interactable [ ] { };
        }
        #endregion

        /// <summary> Returns true if a specific fingers wants to grab on (when not grabbing). </summary>
        /// <param name="finger"></param>
        /// <returns></returns>
        protected bool WantsGrab ( int finger )
        {
            return lastNormalized [ finger ] >= openHandThresholds [ finger ] && lastNormalized [ finger ] <= closedHandThresholds [ finger ];
        }


        /// <summary> Returns a list of all objects that are grabable at this moment. </summary>
        /// <returns></returns>
        public List<SG_Interactable> ObjectsGrabableNow ( )
        {
            List<SG_Interactable> res = new List<SG_Interactable> ( );

            #region Customn 함수
            // 매개변수에 해당하는 finger 부분이 닿았는지 확인하고 bool 값을 반환하는 구문
            bool CheckTouchedFinger ( SG_HoverCollider finger )
            {
                if(finger.parentObject!=null)
                {
                    if (finger.HoveredCount() > 0 && finger.parentObject.touchPercentage > 0.3f)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    if(finger.HoveredCount() > 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }


              
            }

            // 현재손가락과 비교할 손가락이 같은 물체를 접촉했는지 확인하고 그 값을 반환하는 구문
            void CompareFinger ( int thisFinger , int compareFinger )
            {
                SG_Interactable[] matching = fingerScripts[thisFinger].GetMatchingObjects(fingerScripts[compareFinger]);
                for (int i = 0; i < matching.Length; i++)
                {
                    SG.Util.SG_Util.SafelyAdd(matching[i], res);
                }
            }
            #endregion


            // 조건 && wantsGrab [ 0 ] 들어간 이유 : 엄지를 펴고 다른 손가락끼리 그랩하려고 할 때 강제로 인식되서 넣어둠 
            if ( CheckTouchedFinger(thumbTouch) && wantsGrab [ 0 ] )
            {
                for ( int f = 1 ; f < 5 ; f++ )
                {
                    if ( wantsGrab [ f ] )
                    {
                        CompareFinger ( 0, f );
                    }
                }
            }

            #region 검지~새끼 주체 그랩 
            else if ( CheckTouchedFinger ( indexTouch ) )
            {              
                for ( int f = 1 ; f < 5 ; f++) // [1.검지] [2.중지] [3.약지] [4.새끼]
                {
                    if ( wantsGrab [ f ] && isPalmTouch )
                    {
                        CompareFinger ( 1 , f );
                    }
                }
            }
            else if ( CheckTouchedFinger ( middleTouch ) )
            {
                for ( int f = 2 ; f < 5 ; f++ )
                {
                    if ( wantsGrab [ f ] && isPalmTouch )
                    {
                        CompareFinger ( 2 , f );
                    }
                }
            }
            else if ( CheckTouchedFinger ( ringTouch ) )
            {
                for ( int f = 3 ; f < 5 ; f++ )
                {
                    if ( wantsGrab [ f ] && isPalmTouch )
                    {
                        CompareFinger ( 3 , f );
                    }
                }
            }
            else if ( CheckTouchedFinger ( pinkyTouch ) )
            {
                for ( int f = 4 ; f < 5 ; f++ )
                {
                    if ( wantsGrab [ f ] && isPalmTouch )
                    {
                        CompareFinger ( 4 , f );
                    }
                }
            }
          
            #endregion

            #region 손가락 옆면

            else if ( CheckTouchedFinger ( indexTouchSideR ) )
            {
                if ( CheckTouchedFinger ( middleTouchSideL ) )
                {
                    Debug. Log ( "검지와 중지 사이드 그랩" );
                    CompareFinger ( 6 , 7 );
                }
                else if ( CheckTouchedFinger ( ringTouchSideL ) )
                {
                    Debug. Log ( "검지와 약지 사이드 그랩" );

                    CompareFinger ( 6 , 9 );
                }
                else if ( CheckTouchedFinger ( pinkyTouchSideL ) )
                {
                    CompareFinger ( 6 , 11 );
                }

            }
            else if ( CheckTouchedFinger ( middleTouchSideR ) )
            {
                if ( CheckTouchedFinger ( ringTouchSideL ) )
                {
                    Debug. Log ( "중지와 약지 사이드 그랩" );
                    CompareFinger ( 8 , 9 );

                }
                else if ( CheckTouchedFinger ( pinkyTouchSideL ) )
                {
                    CompareFinger ( 8 ,11 );

                }


            }
        
            #endregion
            return res;
        }

        /// <summary> Returns a list of fingers that are currently touching a particular interactable </summary>
        /// <returns></returns>
        /// 기존 반복문 없앰. 손가락마다 케이스 정의별도로 함 / 세분화 정밀 / 코드 동작 원리 명확히
        /// 릴리즈 안정성을 위해 FingersTouching에 사이드도 포함시킴
        public bool [ ] FingersTouching ( SG_Interactable obj )
        {
            bool [ ] res = new bool [ 5 ];

            // 0 엄지 1 검지 2 중지 3 약지 4 새끼
            res [ 0 ] = this. fingerScripts [ 0 ]. IsTouching ( obj );
            res [ 1 ] = this. fingerScripts [ 1 ]. IsTouching ( obj ) || this. fingerScripts [ 5 ]. IsTouching ( obj ) || this. fingerScripts [ 6 ]. IsTouching ( obj );
            res [ 2 ] = this. fingerScripts [ 2 ]. IsTouching ( obj ) || this. fingerScripts [ 7 ]. IsTouching ( obj ) || this. fingerScripts [ 8 ]. IsTouching ( obj );
            res [ 3 ] = this. fingerScripts [ 3 ]. IsTouching ( obj ) || this. fingerScripts [ 9 ]. IsTouching ( obj ) || this. fingerScripts [ 10 ]. IsTouching ( obj );
            res [ 4 ] = this. fingerScripts [ 4 ]. IsTouching ( obj ) || this. fingerScripts [ 11 ]. IsTouching ( obj ) || this. fingerScripts [ 12 ]. IsTouching ( obj );

            return res;
        }
        
        public bool [ ] FingersSideTouching ( SG_Interactable obj )
        {
            bool [ ] res = new bool [ 4 ];

            // 0 검지 1 중지 2 약지 3 새끼
            res [ 0 ] = this. fingerScripts [ 5]. IsTouching ( obj ) || this. fingerScripts [ 6]. IsTouching ( obj );
            res [ 1 ] = this. fingerScripts [ 7 ]. IsTouching ( obj ) || this. fingerScripts [ 8 ]. IsTouching ( obj );
            res [ 2 ] = this. fingerScripts [ 9 ]. IsTouching ( obj ) || this. fingerScripts [ 10 ]. IsTouching ( obj );
            res [ 3 ] = this. fingerScripts [ 11 ]. IsTouching ( obj ) || this. fingerScripts [ 12 ]. IsTouching ( obj );

            return res;
        }



        public static bool [ ] GetGrabIntent ( float [ ] normalizedFlex )
        {
            bool [ ] res = new bool [ 5 ];
            for ( int f = 0 ; f < normalizedFlex. Length ; f++ ) //go through each finger -but- the thumb.?
            {
                res [ f ] = normalizedFlex [ f ] >= openHandThresholds [ f ] && normalizedFlex [ f ] <= closedHandThresholds [ f ];
            }
            return res;
        }


        public override void UpdateDebugger ( )
        {
            //Doesn't do anything
        }


        protected void EvaluateGrab ( )
        {
            // Collect objects that we're allowed to grab.
            List<SG_Interactable> objToGrab = this. ObjectsGrabableNow ( );

            //TODO; Check for a littlest bit of intent. Some for of flexion. Because now you can still slam your hand into something.
            if ( objToGrab. Count > 0 )
            {
                SG_Interactable [ ] sortedGrabables = SG. Util. SG_Util. SortByProximity ( this. ProximitySource. position , objToGrab. ToArray ( ) );
                //attempt to grab each object I can, starting with the closest
                for ( int i = 0 ; i < sortedGrabables. Length ; i++ )
                {
                    TryGrab ( sortedGrabables [ i ] );
                    if ( !CanGrabNewObjects ) { break; } //stop going through the objects if we can no longer grab one
                }
            }
            else if ( this. handPoseProvider != null && this. handPoseProvider. OverrideGrab ( ) > overrideGrabThreshold )
            {
                SG_Interactable [ ] grabablesInHover = this. virtualHoverCollider. GetTouchedObjects ( this. ProximitySource );
                //attempt to grab each object I can, starting with the first
                for ( int i = 0 ; i < grabablesInHover. Length ; i++ )
                {
                    TryGrab ( grabablesInHover [ i ] );
                    if ( !CanGrabNewObjects ) { break; } //stop going through the objects if we can no longer grab one
                }
            }
            if ( this. IsGrabbing ) //we managed to grab something.
            {
                this. grabRelevance = new bool [ 0 ]; //clear this so we re-register it the first frame
                snapFrame = false; //we don't check for collision the first frame after grabbing.
                //Debug.Log(Time.timeSinceLevelLoad + ": " + (this.handPoseProvider.TracksRightHand() ? "Right Hand" : "Left Hand") + " Grabbed Object(s)");
            }
        }


        protected void EvaluateRelease ( )
        {
            SG_Interactable heldObj = this. heldObjects [ 0 ];

            // 물체에 닿고 있는 손가락 및 손가락 사이드
            bool [ ] currentTouched = this. FingersTouching ( heldObj ); 
            bool [] currentSideTouched = this. FingersSideTouching ( heldObj ); 

            //Step 1 : Evaluate Intent - If ever there was any

            if ( this. grabRelevance. Length == 0 )
            {
                bool oneGrabRelevance = false;
                for ( int f = 1 ; f < currentTouched. Length ; f++ ) //Because of the bullshit snapping, I don't want to evaluate releasing until at least a finger (no thumb) touches the object
                {                                           //this should always be true unless we're snapping.
                    if ( currentTouched [ f ] )
                    {
                        oneGrabRelevance = true;
                        break;
                    }
                }
                if ( oneGrabRelevance ) //there is a t least one relevant finger now touching.
                {
                    this. grabRelevance = currentTouched;
                    this. normalizedOnGrab = Util. SG_Util. ArrayCopy ( this. lastNormalized );
                }
            }

            // 이미 이전에 손가락이 물체를 잡았던 상태이며, grabRelevance 배열의 변화를 확인하는 데 사용
            // 물건을 잡았기 때문에 grabRelevance배열이 들어있음. 이전에 grabRelevance가 false였는데 currentTouched가 true인지 확인해서
            // true면 grabRelevance을 true로 바꿈. <즉 전엔 터치 안했던 손가락이 현재는 터치 중이면 값 최신화 하는 것>
            // 역시 normalizedOnGrab배열에 lastNormalized배열을 넣는다 ( 위와 구조는 비슷함 )
            else
            {
                for ( int f = 1 ; f < 5 ; f++ )
                {
                    if ( !grabRelevance [ f ] && currentTouched [ f ] )
                    {
                        normalizedOnGrab [ f ] = lastNormalized [ f ];
                        grabRelevance [ f ] = true;
                    }
                }
            }

            // Step 2 - Evaluate finger angles
            if ( lastNormalized. Length > 0 ) //we successfully got some grab parameters.
            {
                //We will release if all relevant fingers are either above the "open threshold" OR have relevant fingers, and these have extended above / below
                //float[] grabDiff = new float[5]; //DEBUG
                int [ ] grabCodes = new int [ 5 ]; // 0 and up means grab, < zero means release.
                for ( int f = 0 ; f < 5 ; f++ )
                {
                    if ( lastNormalized [ f ] < openHandThresholds [ f ] ) // This finger is above the max extension
                    {
                        grabCodes [ f ] = -1;
                    }
                    else if ( lastNormalized [ f ] > closedHandThresholds [ f ] ) // This finger is below max flexion
                    {
                        grabCodes [ f ] = -2;
                    }
                    else if ( grabRelevance. Length > f && grabRelevance [ f ] ) // we're within the right threshold(s)
                    {
                        grabDiff [ f ] = this. normalizedOnGrab [ f ] - this. lastNormalized [ f ];                      
                        if ( grabDiff [ f ] > releaseThreshold )
                        {
                            grabCodes [ f ] = -3;
                        }
                    }
                    //Reset relevance if the gesture thinks we've released, and we're not currently touching.
                    if ( grabCodes [ f ] < 0 && grabRelevance. Length > f && grabRelevance [ f ] && !currentTouched [ f ] )
                    {
                        grabRelevance [ f ] = false;
                        // 버그 발생시 디버깅에 용이할 때 사용할 확률 높음
                        // 이 단계에서 grabRelevance가 false가 된다고 Release에 영향을 주는 것은 아님
                        // 스텝3~4에서 보는건 결국 grabCodes임 > grabDesired bool값으로 이어짐
                        // 주석 : 제스처가 우리가 해제했다고 생각하고 현재 접촉하지 않는 경우 관련성을 재설정합니다.
                    }
                }

                // Step 3 - After evaluating finger states, determine grab intent.
                // 여기서 grabDesired 값 안 흘리고 잘 넘겨야함

                bool grabDesired = false;

                // 엄지가 닿아있으면.. 검지~새끼중 하나라도 있으면 그랩 유지 ! 
                if (currentTouched[0])
                {
                    for (int f = 1; f < 5; f++)
                    {
                        if (grabCodes[f] > -1)
                        {
                            grabDesired = true;
                            break;
                        }
                    }
                }
                else                    // 엄지 안 닿은 상태
                {
                    if ( isPalmTouch )  // 손바닥이 닿은 상태
                    {
                                        // <검지~새끼> 손가락 중 최소 하나가 그랩 상태면 True
                        for ( int f = 1 ; f < 5 ; f++ ) 
                        {
                            if ( grabCodes [ f ] > -1 ) 
                            {
                                grabDesired = true;
                                break;
                            }
                        }
                    }
                    else                // 엄지도 없고 손바닥도 없다. 
                    {
                                        // 사이드 부분만 탐색해서 손가락 사이드가 2개 이상이면 True
                        if ( currentSideTouched. Count ( touch => touch ) > 1 )
                        {
                            grabDesired = true;
                        }
                        else
                        {
                            grabDesired = false;
                        }
                    }
                }



                //Step 4 - Compare with override to make a final judgement. Can be optimized by placing this before Step 2 and skipping it entirely while GrabOverride is true.

                bool nothingInHover = this. snapFrame && this. heldObjects. Count > 0 && this. virtualHoverCollider. HoveredCount ( ) == 0 && !this. heldObjects [ 0 ]. KinematicChanged; //no objects within the hover collider.
                if ( !snapFrame ) { snapFrame = true; } //set after nothinInHover is assigned so it stays false the first time.
                if ( nothingInHover )
                {
                    //Debug.Log(Time.timeSinceLevelLoad + ": There's nothing in the hover collider and that's not because the kinematics had changed!");
                    //TODO: Add a timing component? If not hovering for x frames / s?
                }

                //Step 4 - Compare with override to make a final judgement. Can be optimized by placing this before Step 2 and skipping it entirely while GrabOverride is true.
                bool overrideGrab = this. handPoseProvider != null && this. handPoseProvider. OverrideGrab ( ) > overrideGrabThreshold; //we start with wanting to release based on overriding.
                bool shouldRelease = !( grabDesired || overrideGrab );
                if ( shouldRelease ) //We can no longer grab anything
                {
                    this. ReleaseAll ( false );

                    // 릴리즈 할 때 혹시 모를 변수초기화 단계  필요없을 확률 높은데 그냥 하는거임
                    for ( int f = 0 ; f < currentTouched. Length ; f++ )
                    {
                        currentTouched [ f ] = false;
                    }
                    for (int f =0 ;f<currentSideTouched.Length ;f++ )
                    {
                        currentSideTouched [ f ] = false;
                    }
                }
            }
        }







        public override void UpdateGrabLogic ( float dT )
        {
            base. UpdateGrabLogic ( dT );  //updates reference location(s).

            // Update Physics Colliders
            for ( int i = 0 ; i < this. hoverScripts. Length ; i++ )
            {
                this. hoverScripts [ i ]. UpdateLocation ( );
            }

            // Re-collect Normalized Flexion
            //We'l try our best to retrieve the last flexions. If it fails, we use the one before it as backup.
            if ( this. handPoseProvider != null && this. handPoseProvider. IsConnected ( ) )
            {
                float [ ] currFlex;
                if ( this. handPoseProvider. GetNormalizedFlexion ( out currFlex ) ) //can fail because of a parsing error, in which case we must not assign it.
                {
                    this. lastNormalized = currFlex;
                }
            }
            this. wantsGrab = GetGrabIntent ( this. lastNormalized ); //doing this here so I can evaluate from inspector

            // Evaluate Grabbing / Releasing
            if ( this. IsGrabbing ) //Check for release - Gesture Based
            {
                EvaluateRelease ( );
            }
            else //Check for Grab (collision based)
            {
                EvaluateGrab ( );
            }

        }
    }
    }