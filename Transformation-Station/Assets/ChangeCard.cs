using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeCard : MonoBehaviour
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

    public void UpdateCard(TMP_Dropdown info)
    {
        switch (info.captionText.text)
        {
            case "Translate":
                SetFieldActivity(true, true, true);
                break;
            case "Rotate X":
                SetFieldActivity(true, false, false);
                break;
            case "Rotate Y":
                SetFieldActivity(false, true, false);
                break;
            case "Rotate Z":
                SetFieldActivity(false, false, true);
                break;
            case "Scale":
                SetFieldActivity(true, true, true);
                break;
        }
    }

    private void SetFieldActivity(bool x_field, bool y_field, bool z_field)
    {
        x_input_field.SetActive(x_field);
        y_input_field.SetActive(y_field);
        z_input_field.SetActive(z_field);
    }

    public void RemoveCard(Transform obj)
    {
        ui_manager.RemoveCard((int) Char.GetNumericValue(obj.name[0]));
        
        Destroy(gameObject);
    }

}
