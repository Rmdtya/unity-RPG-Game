using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rmdtya{
    public class PlayerManager : MonoBehaviour
    {
        InputHandler inputHandler;
        Animator anim;
        CameraHandler cameraHandler; 
        PlayerLocomotion playerLocomotion;
        InteractableUI interactableUI;
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableGameObject;
        public GameObject itemDescriptionGameObject;
        
        public bool isInteracting;                                                  // Memberi tahu ketika player sedang berinteraksi (misalnya ketika mengaktifkan mekanisme)

        [Header("Player Flags")]
        public bool isSprinting;                                                    // Untuk Flags isSprinting
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;

        
        public void Awake(){
            cameraHandler = FindObjectOfType<CameraHandler>();
            
        } 
        
        
        void Start(){
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            interactableUI = FindObjectOfType<InteractableUI>();
        }


        // Update is called once per frame
        void Update(){
            float delta = Time.fixedDeltaTime;

            isInteracting = anim.GetBool("isInteracting");
            canDoCombo = anim.GetBool("canDoCombo");
            anim.SetBool("isInAir", isInAir);

            inputHandler.TickInput(delta);
            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
            playerLocomotion.HandleJumping();

            CheckForInteractableObject();

        }


        public void FixedUpdate(){
            float delta = Time.fixedDeltaTime;

            if(cameraHandler != null){
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }
        }


        private void LateUpdate(){                                                                          // Mengatur Ulang Flags
            inputHandler.rollFlag = false;                                                                  // Mengatur ulang rollflag ketika animasi selesai
            inputHandler.sprintFlag = false;
            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;
            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.a_Input = false;
            inputHandler.jump_Input = false;
            inputHandler.inventory_Input = false;

            if(isInAir){
                playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
            }
        }

        public void CheckForInteractableObject(){
            RaycastHit hit;

            if(Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers)){

                if(hit.collider.tag == "Interactable"){
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();
                    if(interactableObject != null){
                        string interactableText = interactableObject.interactableText;
                        
                            interactableUI.InteractableText.text = interactableText;
                            interactableUIGameObject.SetActive(true);
                            

                        if(inputHandler.a_Input){
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }

            }

            else{
                if(interactableUIGameObject != null ){
                    interactableUIGameObject.SetActive(false);  // Ketika menjaduh dari object akan mematikan Pop up (Mengapa tidak memakai fungsi onTrigger) karena fungsi ini mendeteksi ketika player menjauh dari object tidak hanya ketika memasuki collidernya
                }

                if(itemInteractableGameObject != null && inputHandler.a_Input){
                    itemInteractableGameObject.SetActive(false);
                    itemDescriptionGameObject.SetActive(false);
                }
            }

        }
    }
}