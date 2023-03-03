using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputDataExtract : MonoBehaviour
{
    [SerializeField] private List<GameObject> panelsUI;

    private void Update()
    {
        // Debug.Log(panelsUI[0].transform.GetChild(0).gameObject.GetComponent<TMP_Dropdown>().value);
    }

    public void ReadPanelCardData()
    {
        int dropDownValue = new int();;
        String inputField1 = null;
        String inputField2 = null;
        String inputField3 = null;
        
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
                        inputField1 = panelsUI[i].transform.GetChild(x).gameObject.GetComponent<TMP_InputField>().text;
                        break;
                    case 2:
                        inputField2 = panelsUI[i].transform.GetChild(x).gameObject.GetComponent<TMP_InputField>().text;
                        break;
                    case 3:
                        inputField3 = panelsUI[i].transform.GetChild(x).gameObject.GetComponent<TMP_InputField>().text;
                        break;
                    default:
                        Debug.Log("Error");
                        break;
                }
            }
            StringBuilderOutput(cardNumber, dropDownValue, inputField1, inputField2, inputField3);
        }
    }

    private void StringBuilderOutput(int a, int b, String c, String d, String e)
    {
        Debug.Log(
            Time.deltaTime + "\n" +
                  "Panel Card: " + a + "\n" +
                  "TMP_Dropdown value: " + b + "\n" +
                  "TMP_InputField value[1]: " + c + "\n" +
                  "TMP_InputField value[2]: " + d + "\n" +
                  "TMP_InputField value[3]: " + e
            );
    }
}
