using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rmdtya{

    public class PlayerStats : MonoBehaviour
    {
            public int healthLevel = 10;
            public int maxHealth;
            public int currentHealth;

            public int staminaLevel = 10;
            public int maxStamina;
            public int currentStamina;

            HealthBar healthBar;
            StaminaBar staminaBar;
            

            AnimatorHandler animatorHandler;

            private void Awake(){
                healthBar = FindObjectOfType<HealthBar>();
                staminaBar = FindObjectOfType<StaminaBar>();
                animatorHandler = GetComponentInChildren<AnimatorHandler>();
            }

            void Start(){
                maxHealth = SetMaxHealthFromHealthLevel();
                currentHealth = maxHealth;
                healthBar.SetMaxHealth(maxHealth);

                maxStamina = SetMaxStaminaFromStaminaLevel();
                currentStamina = maxStamina;
                staminaBar.SetMaxStamina(currentStamina);

            }

        private int SetMaxHealthFromHealthLevel(){
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        private int SetMaxStaminaFromStaminaLevel(){
            maxStamina = staminaLevel * 100;
            return maxStamina;
        }

        public void TakeDamage(int damage){
            currentHealth = currentHealth - damage;

            healthBar.SetCurrentHealth(currentHealth);

            animatorHandler.PlayTargetAnimmation("Take_Hit_01", true);

            if(currentHealth <= 0){
                currentHealth = 0;
                animatorHandler.PlayTargetAnimmation("Death_01", true);
                // Handle Player Death
            }
        }

        public void TakeStaminaDamage(int damage){
            currentStamina = currentStamina - damage;

            staminaBar.SetCurrentStamina(currentStamina);
            
        }

    }
}
