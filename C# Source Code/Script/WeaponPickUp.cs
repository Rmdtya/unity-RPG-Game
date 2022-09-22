using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rmdtya{

    public class WeaponPickUp : Interactable
    {
        public WeaponItem weapon;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            PickUpItem(playerManager);
        }

        private void PickUpItem(PlayerManager playerManager){

            PlayerInventory playerInventory;
            PlayerLocomotion playerLocomotion;
            AnimatorHandler animatorHandler;
            playerInventory = playerManager.GetComponent<PlayerInventory>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();

            playerLocomotion.rigidbody.velocity = Vector3.zero;     //Player akan berhenti tidak bergerak ketika mengambil item

            animatorHandler.PlayTargetAnimmation("Pick_Up_Item_01", true);      // Memainknan Animasi Pick Up
            playerInventory.weaponInventory.Add(weapon);

            playerManager.itemInteractableGameObject.GetComponentInChildren<Text>().text = weapon.itemName;            // Text dari pop up akan sesuai dengan nama senjatanya
            playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture  = weapon.itemIcon.texture;
            playerManager.itemInteractableGameObject.SetActive(true);           // setelah mengambil senjata kita akan mengaktifkan pop up box
            
            playerManager.itemDescriptionGameObject.GetComponentInChildren<Text>().text = weapon.descriptionItem;
            playerManager.itemDescriptionGameObject.SetActive(true);

            
            

            Destroy(gameObject);



        }
    }
}
