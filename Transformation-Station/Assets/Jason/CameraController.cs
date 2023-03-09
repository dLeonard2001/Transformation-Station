using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // the object you want to rotate around
    public float distance = 5.0f; // the distance between the camera and the target
    public float sensitivity = 5.0f; // the sensitivity of the mouse scroll wheel
    public float rotateSpeed = 5.0f; // the speed at which you want to rotate the camera
    
    private float currentDistance; // the current distance between the camera and the target
    private float xAngle = 0.0f; // the current angle of rotation around the x-axis
    private float yAngle = 0.0f; // the current angle of rotation around the y-axis

    void Start()
    {
        currentDistance = distance;
    }

    void LateUpdate()
    {
        // check for mouse scroll wheel input to zoom in or out
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentDistance -= scroll * sensitivity;
        currentDistance = Mathf.Clamp(currentDistance, 1.0f, 10.0f); // clamp the distance between 1 and 10 units

        // check for keyboard input to rotate the camera
        if (Input.GetKey(KeyCode.A))
        {
            yAngle -= rotateSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            yAngle += rotateSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            xAngle += rotateSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            xAngle -= rotateSpeed * Time.deltaTime;
        }

        // calculate the position of the camera based on the current distance and rotation angles
        Vector3 position = new Vector3(0.0f, 0.0f, -currentDistance);
        Quaternion rotation = Quaternion.Euler(xAngle, yAngle, 0.0f);
        Vector3 rotatedPosition = rotation * position;
        transform.position = target.position + rotatedPosition;
        
        // make the camera look at the target object
        transform.LookAt(target);
    }
}
