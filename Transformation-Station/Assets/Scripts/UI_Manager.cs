using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UI_Manager : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Button executeBtn;
    [SerializeField] private GameObject canvas;
    [SerializeField] private RectTransform cardParent;
    [SerializeField] private GameObject ui_card_prefab;
    [SerializeField] private float x_offset;
    [SerializeField] private float y_offset;
    [SerializeField] private float changePos_offset;

    private MatrixTransformation currentObject;
    
    private RaycastHit hit;

    private void Start()
    {
        canvas.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            currentObject = hit.transform.GetComponent<MatrixTransformation>();
            
            canvas.SetActive(true);
            DisplayInfo(); 
        }
    }

    public void AddCard()
    {
        x_offset += changePos_offset;

        GameObject newCard = Instantiate(ui_card_prefab, cardParent);
        newCard.transform.localPosition = new Vector3(x_offset, 0f, 0f);
        newCard.transform.name = $"{currentObject.getSize()}_card";
    }

    private void DisplayInfo()
    {
        if (currentObject == null)
        {
            // interpolate UI to be turned off
            
            
            canvas.SetActive(false);
        }
        else
        {
            // interpolate UI to be turned on
            // display all current cards (matrices) for this object
        }
    }

}
