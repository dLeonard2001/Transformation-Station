using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class CameraScript : MonoBehaviour
{

    [SerializeField] [Range(0.1f, 100f)] private float sensitivity;

    private Camera mainCam;

    private float t;
    private float forward;

    private void Awake()
    {

        mainCam = Camera.main;
    }


    private void LateUpdate()
    {
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime * sensitivity;
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * sensitivity;
        float tilt = 0;
        
        if (Input.GetKey(KeyCode.Q))
        {
            tilt += Time.deltaTime * 50f;
        }else if (Input.GetKey(KeyCode.E))
        {
            tilt -= Time.deltaTime * 50f;
        }
        
        if(Input.GetKey(KeyCode.LeftShift))
        {
            forward += Time.deltaTime;
            t = 0;
        }
        else if(Input.GetKey(KeyCode.Z))
        {
            forward -= Time.deltaTime;
            t = 0;
        }
        else
        {
            t += Time.deltaTime * 0.2f;
            t = Mathf.Clamp(t, 0, 1);
            forward = Mathf.Lerp(forward, 0, t);
        }

        forward = Mathf.Clamp(forward, -1, 1);

        mainCam.transform.Rotate(new Vector3(vertical, horizontal, tilt));
        mainCam.transform.Translate(new Vector3(0, 0, forward));
    }
}
