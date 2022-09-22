using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Rmdtya{
    public class AnimatorEnemy : MonoBehaviour
    {
        public Animator anim;

        public void Initialize(){
            anim = GetComponent<Animator>(); 

        }

        

        public void Play(string targetAnim){
                anim.CrossFade(targetAnim, 0.2f);

        }
    }
}