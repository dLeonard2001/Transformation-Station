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

    // when the "x" button is clicked, remove this card from the UI
    public void RemoveCard(Transform obj)
    {
        ui_manager.RemoveCard((int) Char.GetNumericValue(obj.name[0]));
        
        Destroy(gameObject);
    }

    // set the current card to this card, whenever it is selected
    public void OnPointerClick(PointerEventData eventData)
    {
        ui_manager.SetCurrentCard(gameObject);
    }

    // changes the UI depending on the direction
    public void ChangeTransformationValue()
    {
        char value = dropdown.captionText.text[^1];
        TextMeshProUGUI tmp = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        
        // play the changesound
        ui_manager.PlayChangeSound();
        
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
