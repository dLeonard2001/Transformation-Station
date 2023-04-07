using System;
using UnityEngine;

namespace RuntimeSceneGizmo
{
    public class CameraController2 : MonoBehaviour
    {
        [SerializeField] private Transform mainCamera;
        
        // Negative values so the camera faces and sees the mapOrigin rather than overshooting it (using positive values) 
        [SerializeField] private float maxCamDistance = -10f;
        [SerializeField] private float minCamDistance = -50f;
        
        [SerializeField] private float sensitivity = 3.0f;
        [SerializeField] private float rotationSpeed = 50f;

        [SerializeField] private float maxShiftDistance = 100f;
        [SerializeField] private float minShiftDistance = 10f;
        [SerializeField] private float downShiftSpeed = 10f;
        [SerializeField] private float upShiftSpeed = 10f;

        private Transform mainCamParent;

        private void Awake()
        {
            mainCamParent = Camera.main.transform.parent;
            
            
        }

        private void Update()
        {
            CameraShift();
            
            float horizontal = -Input.GetAxis("Horizontal");
            float vertical = -Input.GetAxis("Vertical");

            // Rotate the camera around the y-axis based on horizontal input
            mainCamParent.Rotate(Vector3.up, horizontal * rotationSpeed * Time.deltaTime, Space.World);

            // Rotate the camera around the x-axis based on vertical input
            Vector3 rot = mainCamParent.localEulerAngles;
            
            while( rot.x > 180f )
                rot.x -= 360f;
            while( rot.x < -180f )
                rot.x += 360f;
            rot.x = Mathf.Clamp( rot.x - (vertical * rotationSpeed * Time.deltaTime), -89.8f, 89.8f );

            mainCamParent.localEulerAngles = rot;
        }

        private void LateUpdate()
        {
            // Note: The camera is looking in the direction of the camera parent game object
            // Zooming in and out using the main camera's local position based on scroll wheel input
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            Vector3 camLocalPos = mainCamera.localPosition;
            if (scroll < 0)
            {
                camLocalPos -= Vector3.forward * sensitivity * Time.deltaTime;
            }
            else if (scroll > 0)
            {
                camLocalPos += Vector3.forward * sensitivity * Time.deltaTime;
            }

            mainCamera.localPosition = new Vector3(0, 0, Math.Clamp(camLocalPos.z, minCamDistance, maxCamDistance));
        }

        private void CameraShift()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Vector3 mPos = mainCamParent.position;
                mPos = new Vector3(
                    mPos.x,
                    mPos.y - (Time.deltaTime * downShiftSpeed),
                    mPos.z
                );

                if (mPos.y < minShiftDistance) mPos.y = minShiftDistance;
                
                mainCamParent.position = mPos;
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                Vector3 mPos = mainCamParent.position;
                mPos = new Vector3(
                    mPos.x,
                    mPos.y + (Time.deltaTime * upShiftSpeed),
                    mPos.z
                );
                
                if (mPos.y > maxShiftDistance) mPos.y = maxShiftDistance;
                
                mainCamParent.position = mPos;
            }
        }
    }
}