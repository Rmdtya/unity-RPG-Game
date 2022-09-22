using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rmdtya{

    public class EquipmentWindowUI : MonoBehaviour
    {
        public bool rightHandSlot01Selected;
        //public bool rightHandSlot02Selected;
        public bool leftHandSlot01Selected;
        //public bool leftHandSlot02Selected;

        HandEquipmentSlotUI[] handEquipmentSlotUI;

        private void Start(){
            handEquipmentSlotUI = GetComponentsInChildren<HandEquipmentSlotUI>();
        }

        public void LoadWeaponOnEquipmentScreen(PlayerInventory playerInventory){
            for (int i = 0; i < handEquipmentSlotUI.Length; i++)
                if(handEquipmentSlotUI[i].rightHandSlot01){
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponInRightHandSlots[0]);
                }
                        /* else if(handEquipmentSlotUI[i].rightHandSlot02){
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponInRightHandSlots[1]);
                        }       */

                else if(handEquipmentSlotUI[i].leftHandSlot01){
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponInLeftHandSlots[0]);
                }

                /*        else if(handEquipmentSlotUI[i].leftHandSlot02){
                            handEquipmentSlotUI[i].AddItem(playerInventory.weaponInLeftHandSlots[1])
                        }       */

                else{
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponInLeftHandSlots[1]);
                }

        }


        public void SelectRightHandSlot01(){
            rightHandSlot01Selected = true;
        }

        /* public void SelectRightHandSlot02(){
            rightHandSlotSelected = true;
        } */

        public void SelectLeftHandSlot01(){
            leftHandSlot01Selected = true;
        } 

        /* public void SelectRightHandSlot01(){
            rightHandSlotSelected = true;
        } */

    }
}
