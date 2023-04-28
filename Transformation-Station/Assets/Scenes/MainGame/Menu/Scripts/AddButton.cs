using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class AddButton : MonoBehaviour
{
    public Button buttonPrefab;
    public Canvas canvas;
    public Vector2 cornerTopRight;
    public Vector2 cornerBottomLeft;
    public float Xpos;
    public float Ypos;
    public String editedText;

    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            CreateButton(buttonPrefab, canvas, cornerTopRight, cornerBottomLeft, Xpos, Ypos, editedText);
        }
    }

    public static Button CreateButton(Button buttonPrefab, Canvas canvas, Vector2 cornerTopRight, Vector2 cornerBottomLeft, float Xpos, float Ypos, String editedText)
    {
        
        var button = Object.Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity) as Button;
        var rectTransform = button.GetComponent<RectTransform>();
        rectTransform.SetParent(canvas.transform);
        rectTransform.anchorMax = cornerTopRight;
        rectTransform.anchorMin = cornerBottomLeft;
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.sizeDelta = new Vector2(160, 30);
        rectTransform.anchoredPosition = new Vector2(Xpos, Ypos);
        rectTransform.localPosition = new Vector3(Xpos, Ypos, 0);
        rectTransform.localScale = new Vector3(1.3094f, 1.3094f, 1.3094f);
        button.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = editedText;
        return button;
    }
}
