using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rmdtya{

    public class Interactable : MonoBehaviour
    {
        public float radius = 0.6f;
        public string interactableText;
        private void OnDrawGizmoSelected(){
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        public virtual void Interact(PlayerManager playerManager){

            Debug.Log("Ypu Interacted with an objest!");

        }


    }
}
