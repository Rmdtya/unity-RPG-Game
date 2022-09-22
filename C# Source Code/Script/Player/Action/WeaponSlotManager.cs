using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rmdtya{

    public class WeaponSlotManager : MonoBehaviour
    {
        public WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot;
        public WeaponItem attackingWeapon;

        DamageCollider leftHandDamageCollider;
        DamageCollider rightHandDamageCollider;

        

        Animator animator;
        QuickSlotsUI quickSlotsUI;
        PlayerStats playerStats;

        private void Awake(){
            animator = GetComponent<Animator>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
            playerStats = GetComponentInParent<PlayerStats>();

            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots){
                if(weaponSlot.isLeftHandSlot){
                    leftHandSlot = weaponSlot;
                }else if(weaponSlot.isRightHandSlot){
                    rightHandSlot = weaponSlot;
                }
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft){
            if(isLeft){
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftweaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                #region Handle Weapon Idle Animation Left Arm

                if(weaponItem != null){
                    animator.CrossFade(weaponItem.left_hand_idle_01, 0.2f);
                } else{
                    animator.CrossFade("Left Arm Empty", 0.2f);
                }

                #endregion

            }else {
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                
                quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                #region Handle Weapon Idle Animation Right Arm

                if(weaponItem != null){
                    animator.CrossFade(weaponItem.right_hand_idle_01, 0.2f);
                } else{
                    animator.CrossFade("Right Arm Empty", 0.2f);
                }

                #endregion
            }
        }

        #region Handle Weapons Damage Colliders

        private void LoadLeftweaponDamageCollider(){
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        private void LoadRightWeaponDamageCollider(){
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        public void OpenRightDamageCollider(){
            rightHandDamageCollider.EnableDamageCollider();
        }

        public void OpenLeftDamageCollider(){
            leftHandDamageCollider.EnableDamageCollider();
        }

        public void CloseRightHandDamgeCollider(){
            rightHandDamageCollider.DisableDamageCollider();
        }

        public void CloseLeftHandDamageCollider(){
            leftHandDamageCollider.DisableDamageCollider();
        }

        #endregion
    
        #region Handle Weapon's Stamina Drain

        public void DrainStaminaLightAttack(){
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
        }

        public void DrainStaminaHeavyAttack(){
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
        }

        #endregion

    }
}
