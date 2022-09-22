using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rmdtya{
    public class CameraHandler : MonoBehaviour
    {
        public Transform targetTransform;                                                                                       // target yang akan kamera tuju
        public Transform cameraTransform;                                                                                       // transformasi yang akan kamera jalankan 
        public Transform cameraPivotTransform;                                                                                  // kamera akan berputar disekitar pivot
        private Transform myTransform;                                                                                          // hanya untuk mengubah gameobject
        private Vector3 CameraTransformPosition;                                                                                // vector yang digunakan untuk mentransform camera
        public LayerMask ignoreLayers;     
        private Vector3 cameraFollowVelocity = Vector3.zero;                                                                                    // Agar kamera tidak menabrak object / menembus object


        public static CameraHandler singleton;
        public float lookSpeed = 0.1f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.03f;


        private float targetPosition;
        private float defaultPosition;
        private float lookAngle;
        private float pivotAngle;
        public float minimumPivot = -35;                                                                                        // Batas rotasi kamera atas dan bawah sehingga tidak berputar 360 derajat secara vertical
        public float maximumPivot = 35;

        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionsOffset = 0.2f;
        public float minmumCollisionsOffset = 0.2f;


        private void Awake(){
            singleton = this;
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
            Application.targetFrameRate = 120;
            targetTransform = FindObjectOfType<PlayerManager>().transform; 
            

        }

        public async void FollowTarget(float delta){
            Vector3 targetPosition = Vector3.SmoothDamp
                (myTransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);          // fungsi camera mengikuti target
            myTransform.position = targetPosition;

            HandleCameraCollisions(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput){    // fungsi akan terus berjalan ketika mouse memberikan nilai (artinya ketika mouse digerakan)
            lookAngle += (mouseXInput * lookSpeed) / delta;
            pivotAngle -= (mouseYInput * pivotSpeed) /delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);                                                       //pivot angle camera akan terjebak diantara dua nilai (-35 sampai 35)
        
            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            myTransform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = pivotAngle;

            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
        }

        private void HandleCameraCollisions(float delta){      // Fungsi untuk kamera yang menenbus tembok/object
            targetPosition = defaultPosition;    // mengatur kedalaman kamera akan kembali pada default position (sumbu z kamera)
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if(Physics.SphereCast   //Jika kita menabrak sesuatu yang posisi awal bermula pada posisi pivot camera, radiusnya 0.2, direction = jika mengenai sesuatu, variabel hit sebagai object yang akan memberikan informasi dan menyimpannya
                (cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers)){  //Menciptakan invisible bola disekitar object/poros camera dan memindahkan kearah yang dibuat

                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = - (dis - cameraCollisionsOffset);  //camera offset = semakin besar nilainya semakin besar jarak yang akan dibuat menjauh
            }      

            if (Mathf.Abs(targetPosition) < minmumCollisionsOffset){   // Jika ingin jump off atau menjauh dari object, target position camera harus lebih besar dari minmum offsetnya
                targetPosition = targetPosition - minmumCollisionsOffset;
            }

            CameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            cameraTransform.localPosition = CameraTransformPosition;
        }

    }
}