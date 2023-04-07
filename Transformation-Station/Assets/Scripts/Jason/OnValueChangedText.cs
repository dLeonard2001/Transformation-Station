using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OnValueChangedText : MonoBehaviour
{
    private TextMeshProUGUI ValueText;

    private void Start()
    {
        ValueText = GetComponent<TextMeshProUGUI>();
    }

    public void OnSliderValueChanged(float value)
    {
        ValueText.text = value.ToString("0.00");
    }
}
