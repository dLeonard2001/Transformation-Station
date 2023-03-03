using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    private RaycastHit hit;

    private void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) && Input.GetKeyDown(KeyCode.Mouse0))
        {
            MatrixTransformation matrix = hit.transform.GetComponent<MatrixTransformation>();
            if (matrix)
            {
                Debug.Log("yeet");
            }
        }
    }

    private void TurnOnUI()
    {
        
    }

    private void UpdateUI(Queue<Matrix4x4> q)
    {
        
    }
    
    
}
