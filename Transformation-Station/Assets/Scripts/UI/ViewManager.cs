using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    [SerializeField] private float animDuration;
    [SerializeField] private Camera cam;

    private bool crIsActive;

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
                StartCoroutine(ViewChanger(new Vector3(0, -180, 0)));
                break;
            case "back":
                StartCoroutine(ViewChanger(new Vector3(0f, 0, 0f)));
                break;
            case "right":
                StartCoroutine(ViewChanger(new Vector3(0f, -90, 0f)));
                break;
            case "left":
                StartCoroutine(ViewChanger(new Vector3(0f, 90, 0f)));
                break;
            case "top":
                StartCoroutine(ViewChanger(new Vector3(90, 0f, 0f)));
                break;
            case "bottom":
                StartCoroutine(ViewChanger(new Vector3(-90, 0f, 0f)));
                break;
        }
    }

    private IEnumerator ViewChanger(Vector3 eulerAngles)
    {
        crIsActive = true;
        
        float elapsedTime = 0;
        float time = elapsedTime / animDuration;

        float camX = cam.transform.localEulerAngles.x;
        float camY = cam.transform.localEulerAngles.y;
        float camZ = cam.transform.localEulerAngles.z;

        while (time <= 1)
        {
            
            elapsedTime += Time.deltaTime;
            time = elapsedTime / animDuration;
            
            cam.transform.localEulerAngles =
                new Vector3(Mathf.Lerp(camX, eulerAngles.x, time),
                    Mathf.Lerp(camY, eulerAngles.y, time),
                    Mathf.Lerp(camZ, eulerAngles.z, time));
            yield return null;
        }

        crIsActive = false;
    }

    private void OnMouseDown()
    {
        cam.orthographic = !cam.orthographic;
    }
}
