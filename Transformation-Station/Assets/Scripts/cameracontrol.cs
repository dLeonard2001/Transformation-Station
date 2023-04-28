using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Vector3 = UnityEngine.Vector3;

public class cameracontrol : MonoBehaviour
{
    private float yRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            Debug.Break();
        
        float x = Input.GetAxis("Mouse X") * Time.deltaTime * 100;
        float y = Input.GetAxis("Mouse Y") * Time.deltaTime * 100;

        yRotation -= y;
        yRotation = Mathf.Clamp(yRotation, -90, 90);
        //Debug.Log($"{x} : {y}");

        transform.eulerAngles = new Vector3(yRotation, transform.eulerAngles.y, 0f);
        
        transform.Rotate(Vector3.up * x);
        
        //Debug.Log(Camera.main.cameraToWorldMatrix);
    }
}
