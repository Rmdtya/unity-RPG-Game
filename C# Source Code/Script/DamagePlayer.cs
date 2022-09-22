using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rmdtya{

    public class DamagePlayer : MonoBehaviour
    {
        public int damage = 25;

        private void OnTriggerEnter(Collider other){
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            EnemyStats enemyStats = other.GetComponent<EnemyStats>();

            if(playerStats != null){
                playerStats.TakeDamage(damage);
            }
            
        }
            
    }
}
