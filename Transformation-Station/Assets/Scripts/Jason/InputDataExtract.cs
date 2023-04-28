using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngineInternal;
using Object = System.Object;

public class InputDataExtract : MonoBehaviour
{
    public GameObject planet;
    
    [SerializeField] private List<GameObject> panelsUI;
    
    private String detectTagObject = "Planet";
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); // creating a ray from camera to mouse position

        // change pivot object to the object that was clicked on, specifically tagged 'Planet'
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity) && hit.collider.CompareTag(detectTagObject))
        {
            planet = hit.transform.gameObject;
            if (planet.GetComponent<MatrixTransformation>().GetSize() == 0)
            {
                planet.GetComponent<MatrixTransformation>().AddMatrix();
            }
        }
    }

    public void ReadPanelCardData()
    {
        int dropDownValue = new int();;
        TMP_InputField inputField1 = null;
        TMP_InputField inputField2 = null;
        TMP_InputField inputField3 = null;
        
        for (int i = 0; i < panelsUI.Count; i++)
        {
            if (!panelsUI[i].gameObject.activeInHierarchy) continue;

            var cardNumber = i;
            for (int x = 0; x < panelsUI.Count; x++)
            {
                switch (x)
                {
                    case 0:
                        dropDownValue = panelsUI[i].transform.GetChild(x).gameObject.GetComponent<TMP_Dropdown>().value;
                        break;
                    case 1:
                        inputField1 = panelsUI[i].transform.GetChild(x).gameObject.GetComponent<TMP_InputField>();
                        break;
                    case 2:
                        inputField2 = panelsUI[i].transform.GetChild(x).gameObject.GetComponent<TMP_InputField>();
                        break;
                    case 3:
                        inputField3 = panelsUI[i].transform.GetChild(x).gameObject.GetComponent<TMP_InputField>();
                        break;
                    default:
                        Debug.Log("Error");
                        break;
                }
            }
            ApplyMatrix(cardNumber, dropDownValue, inputField1, inputField2, inputField3);
            // StringBuilderOutput(cardNumber, dropDownValue, inputField1, inputField2, inputField3);
        }
    }

    private void ApplyMatrix(int cardNumber, int dropDownValue, TMP_InputField c, TMP_InputField d, TMP_InputField e)
    {
        var matrixTransformations = planet.GetComponent<MatrixTransformation>(); 
        String transformationStr;
        switch (dropDownValue)
        {
            case 0:
                transformationStr = "Translate";
                break;
            case 1:
                transformationStr = "Rotate";
                break;
            case 2:
                transformationStr = "Scale";
                break;
            default:
                transformationStr = "Null";
                break;
        }


        float x = 0f, y = 0f, z = 0f;
        if (c.text.Length != 0) x = float.Parse(c.text);
        if (d.text.Length != 0) y = float.Parse(d.text);
        if (e.text.Length != 0) z = float.Parse(e.text);
            
        // matrixTransformations.EditMatrix(
        //     new Vector3(x, y, z),
        //     transformationStr,
        //     cardNumber
        //     );
        
        matrixTransformations.ApplyTransformations(cardNumber);
    }
    
    // Debug.log() does not like to output a lot of stuff at once
    private void StringBuilderOutput(int a, int b, TMP_InputField c, TMP_InputField d, TMP_InputField e)
    {
        Debug.Log(
                  "Panel Card: " + a + "\n" +
                  "TMP_Dropdown value: " + b + "\n"
                  );
        Debug.Log("TMP_InputField value[1]: " + c.text + "\n" +
                  "TMP_InputField value[2]: " + d.text + "\n"
                  );
        Debug.Log("TMP_InputField value[3]: " + e.text);
    }
}
