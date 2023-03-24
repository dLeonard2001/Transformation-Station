using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class OnClickObject : MonoBehaviour
{
    [SerializeField] private GameObject windowUI;
    private GameObject windowUIInstance;
    
    private Canvas currCanvas;

    private void Start()
    {
        currCanvas = FindObjectOfType<Canvas>();
    }

    private void OnMouseDown()
    {
        DisableActiveUIWindows();

        if (!windowUIInstance)
        {
            windowUIInstance = Instantiate(windowUI, currCanvas.transform, false);
        }

        windowUIInstance.SetActive(true);
    }

    private void DisableActiveUIWindows()
    {
        if (currCanvas.transform.childCount == 0) return; 
        
        for (int i = 0; i < currCanvas.transform.childCount; i++)
        {
            if (currCanvas.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                currCanvas.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
