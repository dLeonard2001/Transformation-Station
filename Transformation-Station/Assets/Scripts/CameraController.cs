using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // the object you want to rotate around
    public Transform targetOrigin; // the "world origin" to rotate around
    public float distance = 5.0f; // the distance between the camera and the target
    public float maxDistance = 20.0f; // the maximum distance that the camera can be from target
    public float minDistance = 1.0f; // the minimum distance that the camera can be from target
    public float sensitivity = 3.0f; // the sensitivity of the mouse scroll wheel
    public float rotateSpeed = 50.0f; // the speed at which you want to rotate the camera
    
    public string detectTagObject; // tag used to detect a specific object (should be 'Planet')
    public Camera mainCamera; // main camera for raycasting
    
    private float currentDistance; // the current distance between the camera and the target
    private float xAngle; // the current angle of rotation around the x-axis
    private float yAngle; // the current angle of rotation around the y-axis
    
    void Start()
    {
        currentDistance = Vector3.Distance(transform.position, target.position);
        
        var eulerAngles = transform.eulerAngles;
        xAngle = eulerAngles.x;
        yAngle = eulerAngles.y;
    }

    private void Update()
    {
        // pressing space brings the camera back to the "world origin"
        if (Input.GetKey(KeyCode.Space)) target = targetOrigin;
        
        if (!Input.GetMouseButtonDown(0)) return;
        
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); // creating a ray from camera to mouse position

        // change pivot object to the object that was clicked on, specifically tagged 'Planet'
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity) && hit.collider.CompareTag(detectTagObject))
        {
            Debug.Log("Hit object: " + hit.collider.gameObject.name);
            target = hit.transform;
        }
    }

    void LateUpdate()
    {
        // check for mouse scroll wheel input to zoom in or out
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentDistance -= scroll * sensitivity;
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance); // clamp the distance between 1 and 10 units

        // check for keyboard input to rotate the camera
        if (Input.GetKey(KeyCode.A))
        {
            yAngle += rotateSpeed * Time.deltaTime;
            yAngle = Mathf.Clamp(yAngle, -170f, 170f); // clamp the y-angle between -170 and 170 degrees
        }
        if (Input.GetKey(KeyCode.D))
        {
            yAngle -= rotateSpeed * Time.deltaTime;
            yAngle = Mathf.Clamp(yAngle, -170f, 170f); // clamp the y-angle between -170 and 170 degrees
        }
        if (Input.GetKey(KeyCode.W))
        {
            xAngle += rotateSpeed * Time.deltaTime;
            xAngle = Mathf.Clamp(xAngle, -80f, 80f); // clamp the x-angle between -80 and 80 degrees
        }
        if (Input.GetKey(KeyCode.S))
        {
            xAngle -= rotateSpeed * Time.deltaTime;
            xAngle = Mathf.Clamp(xAngle, -80f, 80f); // clamp the x-angle between -80 and 80 degrees
        }
        
        // brings camera to max distance of target when initial far away (via in editor scene)
        if (currentDistance > maxDistance) currentDistance = maxDistance;

        // calculate the position of the camera based on the current distance and rotation angles
        Vector3 position = new Vector3(0.0f, 0.0f, -currentDistance);
        Quaternion rotation = Quaternion.Euler(xAngle, yAngle, 0.0f);
        Vector3 rotatedPosition = rotation * position;
        transform.position = target.position + rotatedPosition;
        
        // make the camera look at the target object
        transform.LookAt(target);
    }
}
