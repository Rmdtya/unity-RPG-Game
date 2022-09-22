using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rmdtya{
    [CreateAssetMenu (menuName = "Items/ Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Idle Animation")]
        public string right_hand_idle_01;
        public string left_hand_idle_01;

        [Header("Attack Animation")]
        public string OH_Light_Attack_1;
        public string OH_Light_Attack_2;
        public string OH_Light_Attack_3;
        public string OH_Light_Attack_4;
        public string OH_Heavy_Attack_1;

        [Header ("Stamina Costs")]
        public int baseStamina;
        public float lightAttackMultiplier;
        public float heavyAttackMultiplier;

       

    }
}