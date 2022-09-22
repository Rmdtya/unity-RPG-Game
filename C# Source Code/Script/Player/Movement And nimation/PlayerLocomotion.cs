using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rmdtya{

    public class PlayerLocomotion : MonoBehaviour
    {
        PlayerManager playerManager;
        Transform cameraObject;
        InputHandler inputHandler;
        public Vector3 moveDirection;

        [HideInInspector]
        public Transform myTransform;
        [HideInInspector]
        public AnimatorHandler animatorHandler;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        [Header("Ground & Air Detection Stats")]                                              // Digunakan oleh raycast untuk mendeteksi 
        [SerializeField]
        float goroundDetectionRayStartPoint = 0.5f;                                           // Titik awal yang berada di player untuk menjadi titik awal dari raycast // nilai untuk menaikan ray cast origin 0.5f diatar pergerakan player
        [SerializeField]
        float minimumDistanceNeededToBeginFall = 1f;                                          // Jarak yang dibutuhkan pemain dari lantai untuk memulai animasi
        [SerializeField]
        float groundDirectionRayDistance = 0.2f;                                               // Mungkin ini tidak terlalu penting, namun nilai ini digunakakn untuk offset dari raycast dan hal ini bergantung juga pada model player // misalnya meletakan sedikit di depan atau belakang player
        LayerMask ignoreForGroundCheck;
        public float inAirTimer;




        [Header("Movement Stats")]
        [SerializeField]
        float walkingspeed = 1;
        [SerializeField]
        float movementSpeed = 3;
        [SerializeField]
        float sprintSpeed = 5;
        [SerializeField]
        float rotationSpeed = 10;
        [SerializeField]
        float fallingSpeed = 45;
        public float leapingVelocity;


        void Start() 
        {
            playerManager = GetComponent<PlayerManager>();
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();  // inChildren digunakan karena hal ini akan diterapkan pada model karakter buka object player
            cameraObject = Camera.main.transform;
            myTransform = transform;
            animatorHandler.Initialize();

            playerManager.isGrounded= true;
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
            Application.targetFrameRate = 120;
        }

        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;

        private void HandleRotation(float delta){
            Vector3 targetDir = Vector3.zero;

            targetDir = cameraObject.forward * inputHandler.vertical;
            targetDir += cameraObject.right * inputHandler.horizontal;

            targetDir.Normalize();
            targetDir.y = 0;

            if(targetDir == Vector3.zero){
                targetDir = myTransform.forward;
            }
                float rs = rotationSpeed;

                Quaternion tr = Quaternion.LookRotation(targetDir);
                Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

                myTransform.rotation = targetRotation;
            
        }

        public void HandleMovement(float delta){
                
                
                if (inputHandler.rollFlag)
                    return;

                if(playerManager.isInteracting)
                    return;

                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;
                moveDirection.Normalize();
                moveDirection.y = 0;

                float speed = movementSpeed;

                if(inputHandler.sprintFlag && inputHandler.moveAmount > 0.5f){
                    speed = sprintSpeed;
                    playerManager.isSprinting = true;
                    moveDirection *= speed;
                } else {
                        if(inputHandler.moveAmount < 0.5f){
                            moveDirection *= walkingspeed;
                            playerManager.isSprinting = false;
                        }else{
                        moveDirection *= speed;
                        playerManager.isSprinting = false;
                        }
                    }
                


                Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
                rigidbody.velocity = projectedVelocity;

                animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);

                if(animatorHandler.canRotate){
                    HandleRotation(delta);
                }

            }

        public void HandleRollingAndSprinting(float delta){
                    if(animatorHandler.anim.GetBool("isInteracting"))                                               // Membatasi pergerakan player ketika sedang berinteraksi dengan hal lain
                        return;

                    if(inputHandler.rollFlag){
                        moveDirection = cameraObject.forward * inputHandler.vertical;
                        moveDirection += cameraObject.right * inputHandler.horizontal;

                        if(inputHandler.moveAmount > 0.5f ){                                                            // jika player memiliki pergerakan atau movement maka akan rolling
                            animatorHandler.PlayTargetAnimmation("Roll", true);
                             moveDirection.y = 0;
                             Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                             myTransform.rotation = rollRotation;      // set kondisi jumping menjadi true
                            // rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                            
            
                             

                            /* Vector3 jumpingVelocity = Vector3.ProjectOnPlane(10 * moveDirection, normalVector);
                             Vector3 playerVelocity = jumpingVelocity;             // Mengambil arah lompatan sesuai dengan arah terakhir sebelum melompat
                             playerVelocity.y = 0;
                             rigidbody.velocity = playerVelocity;
                            */

                        } else if(inputHandler.moveAmount < 0.5f){
                            animatorHandler.PlayTargetAnimmation("BackStep", true);                                 // Jika player tidak melakukan movement maka akan back flip
                               // set kondisi jumping menjadi true
                               // rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                        }
                    }
                }
                
        public void HandleFalling(float delta, Vector3 moveDirection){
                playerManager.isGrounded = false;
                RaycastHit hit;
                Vector3 origin = myTransform.position;
                origin.y += goroundDetectionRayStartPoint;

                

                if(Physics.Raycast(origin, myTransform.forward, out hit, 0.4f)){
                    moveDirection = Vector3.zero;
                }

                if(playerManager.isInAir){
                    rigidbody.AddForce(-Vector3.up * fallingSpeed);
                    rigidbody.AddForce(moveDirection * fallingSpeed / 5f);                  // dengan fungsi ini player yang melompat akan sedikit bergerak sehingga tidak terjebak di pinggir object // anggap melompat dengan sedikit kekuatan
                }

                Vector3 dir = moveDirection;
                dir.Normalize();
                origin = origin + dir * groundDirectionRayDistance;
                
                targetPosition = myTransform.position;

                Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
                if(Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck)){
                    normalVector = hit.normal;
                    Vector3 tp = hit.point;
                    playerManager.isGrounded = true;
                    targetPosition.y = tp.y;
                    

                    if(playerManager.isInAir){
                        if(inAirTimer > 0.5f){
                            Debug.Log("You here in the air for" + inAirTimer);
                            animatorHandler.PlayTargetAnimmation("Land", true);
                            inAirTimer = 0;   
                                
                        }
                        else {
                            animatorHandler.PlayTargetAnimmation("Empty", false);
                            inAirTimer = 0;
                            
                                          
                        }
                        playerManager.isInAir = false;
                    }
                }

                else {
                    if(playerManager.isGrounded){
                        playerManager.isGrounded = false;
                    }

                    if(playerManager.isInAir == false){
                        if(playerManager.isInteracting == false){
                            animatorHandler.PlayTargetAnimmation("Falling", true);
                            
                        }

                        Vector3 vel = rigidbody.velocity;
                        vel.Normalize();
                        rigidbody.velocity = vel * (movementSpeed / 2);
                        playerManager.isInAir = true;
                        // rigidbody.constraints =RigidbodyConstraints.FreezeRotation;
                    }
                }

                
                    if(playerManager.isInteracting || inputHandler.moveAmount > 0){
                        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
                        // myTransform.position = Vector3.Slerp(myTransform.position, targetPosition, Time.deltaTime);
                    }
                    else{
                        myTransform.position = targetPosition;
                    }
                
                
                
                                                         
            }

        public void HandleJumping(){
            if(playerManager.isInteracting)
                return;

                if(inputHandler.jump_Input)
                {
                    /*if(inputHandler.moveAmount > 0 && playerManager.isSprinting)
                    {
                        moveDirection = cameraObject.forward * inputHandler.vertical;
                        moveDirection += cameraObject.right * inputHandler.horizontal;
                        animatorHandler.PlayTargetAnimmation("Sprint_Jump", true);

                        moveDirection.y = 0;
                        Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                        myTransform.rotation = jumpRotation;
                    }   */

                    if(inputHandler.moveAmount > 0)
                    {
                        animatorHandler.PlayTargetAnimmation("Run_Jump", true);
                    }

                     if(inputHandler.moveAmount <= 0)
                    {
                        moveDirection = cameraObject.forward * inputHandler.vertical;
                        moveDirection += cameraObject.right * inputHandler.horizontal;
                        animatorHandler.PlayTargetAnimmation("Jump_Up", true);

                    }

                    
                    
            }
        }


        
        
        #endregion

    

    }

}