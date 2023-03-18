using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeCard : MonoBehaviour, IPointerClickHandler
{
    [Header("Input Fields Per Card")]
    [SerializeField] private GameObject x_input_field;
    [SerializeField] private GameObject y_input_field;
    [SerializeField] private GameObject z_input_field;

    private UI_Manager ui_manager;
    
    // Stores current dropdown text value
    private String transformationType;

    private void Start()
    {
        ui_manager = FindObjectOfType<UI_Manager>();
        transformationType = transform.GetChild(1).GetComponent<TMP_Dropdown>().captionText.text;
    }

    public void RemoveCard(Transform obj)
    {
        ui_manager.RemoveCard((int) Char.GetNumericValue(obj.name[0]));
        
        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // When selecting a card, get it's dropdown text value and send it along with the card UI
        transformationType = transform.GetChild(1).GetComponent<TMP_Dropdown>().captionText.text;
        ui_manager.SetCurrentCard(transform.gameObject, transformationType);
        Debug.Log(transformationType);
    }
}
