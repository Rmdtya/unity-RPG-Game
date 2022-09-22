using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Rmdtya{
    public class AnimatorHandler : MonoBehaviour
    {
        PlayerManager playerManager;
        public Animator anim;
        InputHandler inputHandler;
        PlayerLocomotion playerLocomotion;
        int vertical;
        int horizontal;
        public bool canRotate;

        public void Initialize(){
            playerManager = GetComponentInParent<PlayerManager>();
            anim = GetComponent<Animator>();
            inputHandler = GetComponentInParent<InputHandler>();                // Memanggil script dari parent object ini (parent charachter)
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");

        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting){     // Nilai dari (action) kemudian dimasukan ke dalam fungsi ini dan diteruskan ke PlayerLocomotion
           
            #region Vertical
            float v = 0;

            if (verticalMovement > 0  && verticalMovement < 0.55f){
                v = 0.5f;
            } else if (verticalMovement > 0.55f){
                v = 1;
            } else if (verticalMovement < 0 && verticalMovement > -0.55f){
                v = -0.5f;
            } else if (verticalMovement < -0.55f){
                v = -1;
            } else {
                v = 0;
            } 
            #endregion

             #region Horizontal
            float h = 0;

            if (horizontalMovement > 0  && horizontalMovement < 0.55f){
                h = 0.5f;
            } else if (horizontalMovement > 0.55f){
                h = 1;
            } else if (horizontalMovement < 0 && horizontalMovement > -0.55f){
                h = -0.5f;
            } else if (horizontalMovement < -0.55f){
                h = -1;
            } else {
                h = 0;
            } 
            #endregion

            if (isSprinting){
                v = 2;
                h = horizontalMovement;
            }

            anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);

        }

        public void PlayTargetAnimmation(string targetAnim, bool isInteracting){
                anim.applyRootMotion = isInteracting;
                anim.SetBool("isInteracting", isInteracting);
                anim.CrossFade(targetAnim, 0.2f);

        }

        public void CanRotate(){
            canRotate = true;
        }

        public void StopRotation(){
            canRotate = false;
        }

        public void EnableCombo(){
            anim.SetBool("canDoCombo", true);  
        }

        public void DisableCombo(){
            anim.SetBool("canDoCombo", false);
        }
        private void OnAnimatorMove(){
            if(playerManager.isInteracting == false)
                return;

           float delta = Time.deltaTime;
           playerLocomotion.rigidbody.drag = 0;                                    // Tarikan pada rigid body player
           Vector3 deltaPosition = anim.deltaPosition;
           deltaPosition.y = 0;
           Vector3 velocity = deltaPosition / delta;                               // pergerakan animasi charackter akan disesuaikan kembali dengan titik pusat player object
           playerLocomotion.rigidbody.velocity = velocity;

        


            
        }
    }
}