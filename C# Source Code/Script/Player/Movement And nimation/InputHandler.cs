using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rmdtya{

public class InputHandler : MonoBehaviour
{
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool b_Input;
        public bool a_Input;
        public bool rb_Input;
        public bool rt_Input;
        public bool jump_Input;

        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;
        public bool inventory_Input;
        



        public bool rollFlag;
        public bool sprintFlag;
        public float rollInputTimer;
        public bool comboFlag;
        public bool inventoryFlag;


        PlayerControls inputActions;
        // PlayerLocomotion playerLocomotion;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        PlayerManager playerManager;
        UIManager uIManager;
        

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake(){
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            uIManager = FindObjectOfType<UIManager>();
        }

        public void OnEnable(){
            if (inputActions == null){
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>(); // Membaca nilai vector dari playercontrols (Input Action) 
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();  
        }
        
        inputActions.Enable();
    }

        public void OnDisable(){
            inputActions.Disable();
        }

        public void TickInput(float delta){
            MoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
            HandleQuickSlotsInput();
            HandleInteractingButtonInput();
            HandleJumpInput();
            HandleInventoryInput();

        }

        private void MoveInput(float delta){
            horizontal = movementInput.x;       // Fungsi float bisa bernilai sama dengan vector asalkan ditambahkan dengan kordinat vectornya
            vertical = movementInput.y;
            
            mouseX = cameraInput.x;                                                     // Pergerakan camera
            mouseY = cameraInput.y;

            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));      // Pergerakan Player
        }
        
        private void HandleRollInput(float delta){
            
        b_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;        // Mendeteksi ketika tombol roll ditekan
        // inputActions.PlayerActions.Roll.performed += i => b_Input = true;
            if(b_Input){
                rollInputTimer += delta;
                sprintFlag = true;
            }   else {
                    if(rollInputTimer > 0 && rollInputTimer < 0.5f){
                        sprintFlag = false;
                        rollFlag = true;
                    }
                    rollInputTimer = 0;
            }
        }
        
        private void HandleAttackInput(float delta){
            inputActions.PlayerActions.RB.performed += i => rb_Input = true;
            inputActions.PlayerActions.RT.performed += i => rt_Input = true;

            if(rb_Input && playerInventory.rightWeapon != playerInventory.unarmedWeapon){

                if(playerManager.canDoCombo){
                    comboFlag = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                    comboFlag = false;
                } else{
                    if(playerManager.isInteracting)
                        return;

                    if(playerManager.canDoCombo)
                        return;
                        playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
                }
            }

            if(rt_Input && playerInventory.rightWeapon != playerInventory.unarmedWeapon){
                 if(playerManager.canDoCombo){
                    comboFlag = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                    comboFlag = false;
                } else{
                    if(playerManager.isInteracting)
                        return;

                    if(playerManager.canDoCombo)
                        return;
                        playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
                }
            }
        }
        
        private void HandleQuickSlotsInput(){

            inputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
            inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
            if(d_Pad_Right){
                playerInventory.ChangeRightWeapon();
            } 
            else if(d_Pad_Left){
                //playerInventory.ChangeLeftWeapon();
            }
    
        }
    
        private void HandleInteractingButtonInput(){
            inputActions.PlayerActions.F.performed += i => a_Input = true;
        }
    
        private void HandleJumpInput(){
            inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
        }

        private void HandleInventoryInput(){
            inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
            

            if(inventory_Input){
                inventoryFlag = !inventoryFlag;
                                                                                      playerManager.isInteracting = true;

                if(inventoryFlag){
                    uIManager.OpenSelectWindow();
                    uIManager.UpdateUI();
                    uIManager.hudWindow.SetActive(false);
                      
                } else{
                    uIManager.CloseSelectWindow();
                    uIManager.CloseAllInventoryWindow();
                    uIManager.hudWindow.SetActive(true);
                                                                                            playerManager.isInteracting = false;
                }
            }
        }
    }
}


   

   
