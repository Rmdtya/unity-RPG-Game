using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rmdtya{
    public class DamageCollider : MonoBehaviour
    {
        Collider damageCollider;
        public int currentWeaponDamage = 25;

        private void Awake(){
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        public void EnableDamageCollider(){
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider(){
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider collision){                                                                // Fungsi Akan aktif ketika collider menyentuh Object lain
            if(collision.tag == "Player"){

                PlayerStats playerStats = collision.GetComponent<PlayerStats>();

                if(playerStats != null){
                    playerStats.TakeDamage(currentWeaponDamage);
                }
            }

            if(collision.tag == "Enemy"){

                EnemyStats enemyStats = collision.GetComponent<EnemyStats>();

                if(enemyStats != null){
                    enemyStats.TakeDamage(currentWeaponDamage);
                }

             }
        }

    }

}
