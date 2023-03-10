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
    [SerializeField] private TextMeshProUGUI transformation_type;

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
                transformation_type.text = "T";
                break;
            case "Rotate X":
                SetFieldActivity(true, false, false);
                transformation_type.text = "Rx";
                break;
            case "Rotate Y":
                SetFieldActivity(false, true, false);
                transformation_type.text = "Ry";
                break;
            case "Rotate Z":
                SetFieldActivity(false, false, true);
                transformation_type.text = "Rz";
                break;
            case "Scale":
                SetFieldActivity(true, true, true);
                transformation_type.text = "S";
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
