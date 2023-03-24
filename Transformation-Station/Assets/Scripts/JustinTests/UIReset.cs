using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIReset : MonoBehaviour
{
    public GameObject inputField;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void reset()
    {
        inputField.GetComponent<TMP_InputField>().text = "0";
    }
}
