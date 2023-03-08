using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UI_Manager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject canvas;
    [SerializeField] private RectTransform cardParent;
    [SerializeField] private GameObject ui_card_prefab;
    [SerializeField] private float x_offset;
    [SerializeField] private float y_offset;
    [SerializeField] private float changePos_offset;
    private List<GameObject> cards;
    private MatrixTransformation currentObject;
    
    private RaycastHit hit;

    private void Start()
    {
        cards = new List<GameObject>();
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
        cards.Insert(0, newCard);
        
        newCard.transform.localPosition = new Vector3(x_offset, 0f, 0f);
        newCard.transform.name = $"{currentObject.getSize()}_card";
        
        currentObject.AddMatrix();
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

    // [1_card, 0_card]
    public void Execute()
    {
        Vector3 currentInput;
        
        int count = 0;
        foreach (var c in cards)
        {
            currentInput = new Vector3();
            
            string transformation = c.transform.GetChild(0).GetComponent<TMP_Dropdown>().captionText.text;
            TMP_InputField[] array = c.GetComponentsInChildren<TMP_InputField>();

            currentInput.x = array[0].text.Length == 0 ? 0 : float.Parse(array[0].text);
            currentInput.y = array[1].text.Length == 0 ? 0 : float.Parse(array[1].text);
            currentInput.z = array[2].text.Length == 0 ? 0 : float.Parse(array[2].text);
            
            currentObject.EditMatrix(currentInput, transformation, count);
            count++;
        }
        
        
        currentObject.ApplyTransformations();
    }

}
