using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatePointer : MonoBehaviour
{
    public float speed = 1f;
    public float maxDistance = 3f;
    
    private Camera _camera;
    
    private float startYPos;

    void Start()
    {
        _camera = Camera.main;
        startYPos = transform.localPosition.y;
    }

    void Update()
    {
        float newY = startYPos + Mathf.Sin(Time.time * speed) * maxDistance;

        var localPosition = transform.localPosition;
        localPosition = new Vector3(localPosition.x, newY, localPosition.z);
        transform.localPosition = localPosition;
        
        transform.LookAt(_camera.transform.position, Vector3.up);
    }
}
