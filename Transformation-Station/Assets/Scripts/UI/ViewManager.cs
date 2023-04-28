using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    [SerializeField] private Camera cam;
    
    // Update is called once per frame
    void LateUpdate()
    {
        transform.localEulerAngles = -Camera.main.transform.rotation.eulerAngles;
    }

    // code is a bit brittle
    // if you change the name of the object, you might not get the test case you want
    public void ChangeView(Transform direction)
    {
        switch (direction.name)
        {
            case "front":
                cam.transform.localEulerAngles = new Vector3(0f, -180, 0f);
                break;
            case "back":
                cam.transform.localEulerAngles = new Vector3(0f, 0, 0f);
                break;
            case "right":
                cam.transform.localEulerAngles = new Vector3(0f, -90, 0f);
                break;
            case "left":
                cam.transform.localEulerAngles = new Vector3(0f, 90, 0f);
                break;
            case "top":
                cam.transform.localEulerAngles = new Vector3(90, 0f, 0f);
                break;
            case "bottom":
                cam.transform.localEulerAngles = new Vector3(-90, 0f, 0f);
                break;
        }
    }

    private void OnMouseDown()
    {
        cam.orthographic = !cam.orthographic;
    }
}
