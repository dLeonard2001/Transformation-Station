using System;
using UnityEngine;

namespace RuntimeSceneGizmo
{
    public class CameraController2 : MonoBehaviour
    {
        [SerializeField] private Transform mainCamera;
        [SerializeField] private float maxDistance = 20.0f;
        [SerializeField] private float minDistance = 1.0f;
        [SerializeField] private float sensitivity = 3.0f;
        [SerializeField] private float rotationSpeed = 50f;

        private Transform mainCamParent;
        private float localDistance;

        private void Awake()
        {
            mainCamParent = Camera.main.transform.parent;
            
            localDistance = mainCamera.transform.localPosition.z;
        }

        private void Update()
        {
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

            mainCamera.localPosition = new Vector3(0, 0, Math.Clamp(camLocalPos.z, minDistance, maxDistance));
        }
    }
}