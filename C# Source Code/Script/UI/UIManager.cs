using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rmdtya
{
    public class UIManager : MonoBehaviour
    {

        public PlayerInventory playerInventory;
        EquipmentWindowUI equipmentWindowUI;

        [Header ("UI Windows")]
        public GameObject hudWindow;
        public GameObject selectWindow;
        public GameObject weaponInventoryWindow;

        [Header("Weapon Inventory")]
        public GameObject weaponInventorySlotPrefab;                // Menduplikat prefab dari weaponinventoryslots     // slot prefab untuk setiap weapon dan kemudian akan dimasukan ke array weapon inventory
        public Transform weaponInventorySlotsParent;
        WeaponInventorySlot[] weaponInventorySlots;

        private void Awake(){
            equipmentWindowUI = FindObjectOfType<EquipmentWindowUI>();
        }

        private void Start(){
           // playerInventory = FindObjectOfType<PlayerInventory>();
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
//            equipmentWindowUI.LoadWeaponOnEquipmentScreen(playerInventory);
        }
        
        public void UpdateUI(){

            #region Weapon Inventory Slots
                for (int i = 0; i < weaponInventorySlots.Length; i ++)
                {
                    if(i < playerInventory.weaponInventory.Count)
                    {
                        if(weaponInventorySlots.Length < playerInventory.weaponInventory.Count)
                        {
                            Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                        }    
                        weaponInventorySlots[i].AddItem(playerInventory.weaponInventory[i]);
                    }
                    else{
                        weaponInventorySlots[i].ClearInventorySlot();
                    }
                }


            #endregion

        }

        public void OpenSelectWindow(){
            selectWindow.SetActive(true);
        }

        public void CloseSelectWindow(){
            selectWindow.SetActive(false);
        }

        public void CloseAllInventoryWindow(){
            weaponInventoryWindow.SetActive(false);
        }
        
    }
}
