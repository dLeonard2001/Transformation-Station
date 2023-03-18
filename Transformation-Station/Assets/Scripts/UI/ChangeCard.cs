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

    private void Start()
    {
        ui_manager = FindObjectOfType<UI_Manager>();
    }

    public void RemoveCard(Transform obj)
    {
        ui_manager.RemoveCard((int) Char.GetNumericValue(obj.name[0]));
        
        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ui_manager.SetCurrentCard(transform.gameObject);
    }
}
