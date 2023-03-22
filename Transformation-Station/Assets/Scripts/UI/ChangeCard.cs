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
    [SerializeField] private TMP_Dropdown dropdown;
    
    private UI_Manager ui_manager;

    private void Start()
    {
        ui_manager = FindObjectOfType<UI_Manager>();
        
        ChangeTransformationValue();
    }

    public void RemoveCard(Transform obj)
    {
        ui_manager.RemoveCard((int) Char.GetNumericValue(obj.name[0]));
        
        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ui_manager.SetCurrentCard(gameObject);
    }

    public void ChangeTransformationValue()
    {
        char value = dropdown.captionText.text[^1];
        TextMeshProUGUI tmp = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        
        switch (value)
        {
            case 'X':
                tmp.text = "X: 0";
                break;
            case 'Y':
                tmp.text = "Y: 0";
                break;
            case 'Z':
                tmp.text = "Z: 0";
                break;
        }
    }
}
