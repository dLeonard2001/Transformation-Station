using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Object = System.Object;

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
            StringBuilderOutput(cardNumber, dropDownValue, inputField1, inputField2, inputField3);
        }
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
