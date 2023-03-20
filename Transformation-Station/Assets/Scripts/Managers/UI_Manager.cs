using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Manager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform cardParent;
    [SerializeField] private GameObject ui_card_prefab;

    [SerializeField] private GameObject ui_matrix_values;

    private MatrixTransformation currentObject;

    private Matrix4x4 matrix_total;
    
    [Header("Animations")] 
    [SerializeField] private Animator ui_animator;

    // needed for raycast
    private Camera mainCamera;
    private Ray myRay;
    private RaycastHit hitTarget;

    private void Start()
    {
        mainCamera = Camera.main;
        
        matrix_total = Matrix4x4.identity;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // make ray from camera to where mouseclick
            myRay = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(myRay, out hitTarget, Mathf.Infinity))
            {
                // check if clicked object should have a board
                if (hitTarget.collider.GetComponent<MatrixTransformation>())
                {
                    UnloadCards();
                    currentObject = hitTarget.collider.GetComponent<MatrixTransformation>();
                    LoadCards(); // load cards
                    
                    // display cards
                    ui_animator.CrossFade("UI_slide_in", 0f, 0);
                }
            }
            else
            {
                // if the player clicks on a UI element
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                
                ui_animator.CrossFade("UI_slide_out", 0f, 0);
            }

        }
    }

    #region CardFunctions

    public void AddCard()
    {
        GameObject newCard = Instantiate(ui_card_prefab, cardParent);

        newCard.transform.name = $"{currentObject.GetSize()}_card";
        
        newCard.GetComponent<ChangeCard>().UpdateCard(newCard.transform.GetChild(0).GetComponent<TMP_Dropdown>());

        currentObject.AddCard(newCard);
        currentObject.AddMatrix();
    }

    // add remove button to individual cards
    public void RemoveCard(int index)
    {
        currentObject.RemoveMatrix(index);
        
        UpdateCardNames();
    }

    private void UpdateCardNames()
    {
        int count = 0;
        foreach (var c in currentObject.GetCurrentCards())
        {
            c.transform.name = $"{count}_card";
            count++;
        }
    }
    
    #endregion

    private void LoadCards()
    {
        foreach (var c in currentObject.GetCurrentCards())
        {
            c.SetActive(true);
        }
    }

    private void UnloadCards()
    {
        if (currentObject == null)
            return;

        foreach (var c in currentObject.GetCurrentCards())
        {
            c.SetActive(false);
        }
    }

    private void LoadMatrixValues()
    {
        
    }

    private void UnloadMatrixValues()
    {
        
    }

    public void Reset()
    {
        currentObject.Reset();
        
        // adjust the second screen
        matrix_total = currentObject.GetTotal();
        SetValues();
    }
    
    public void PreviewValues(int index)
    {
        Vector3 input;
        
        List<GameObject> cards = currentObject.GetCurrentCards();
        for (int c =0; c <= index; c++)
        {
            input = new Vector3();
            
            string transformation = cards[c].transform.GetChild(0).GetComponent<TMP_Dropdown>().captionText.text;
            TMP_InputField[] array = cards[c].GetComponentsInChildren<TMP_InputField>();
            
            foreach (var a in array)
            {
                EditVector(ref input, a.text.Length == 0 ? 0 : float.Parse(a.text), a.transform.name[0]);
            }
            
            currentObject.EditMatrix(input, transformation, c);
        }
        
        currentObject.ApplyTransformations(index + 1);
    }

    public void Execute()
    {
        Vector3 input;
        
        int count = 0;
        foreach (var c in currentObject.GetCurrentCards())
        {
            input = new Vector3();
            
            string transformation = c.transform.GetChild(0).GetComponent<TMP_Dropdown>().captionText.text;
            TMP_InputField[] array = c.GetComponentsInChildren<TMP_InputField>();

            foreach (var a in array)
            {
                EditVector(ref input, a.text.Length == 0 ? 0 : float.Parse(a.text), a.transform.name[0]);
            }
            
            currentObject.EditMatrix(input, transformation, count);
            count++;
        }
        
        currentObject.ApplyTransformations(currentObject.GetSize());
        
        // adjust the second screen
        matrix_total = currentObject.GetTotal();
        SetValues();
    }

    private void EditVector(ref Vector3 vec, float value, char c)
    {
        switch (c)
        {
            case 'X':
                vec.x = value;
                break;
            case 'Y':
                vec.y = value;
                break;
            case 'Z':
                vec.z = value;
                break;
        }
    }

    public MatrixTransformation GetCurrentObject()
    {
        return currentObject;
    }

    private void SetValues()
    {
        ui_matrix_values.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text =
            matrix_total.m00.ToString("F2");
        ui_matrix_values.transform.GetChild(0).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text =
            matrix_total.m00.ToString("F2");
        ui_matrix_values.transform.GetChild(0).GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text =
            matrix_total.m00.ToString("F2");
        ui_matrix_values.transform.GetChild(0).GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text =
            matrix_total.m00.ToString("F2");
        ui_matrix_values.transform.GetChild(1).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text =
            matrix_total.m10.ToString("F2");
        ui_matrix_values.transform.GetChild(1).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text =
            matrix_total.m11.ToString("F2");
        ui_matrix_values.transform.GetChild(1).GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text =
            matrix_total.m12.ToString("F2");
        ui_matrix_values.transform.GetChild(1).GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text =
            matrix_total.m13.ToString("F2");
        ui_matrix_values.transform.GetChild(2).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text =
            matrix_total.m20.ToString("F2");
        ui_matrix_values.transform.GetChild(2).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text =
            matrix_total.m21.ToString("F2");
        ui_matrix_values.transform.GetChild(2).GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text =
            matrix_total.m22.ToString("F2");
        ui_matrix_values.transform.GetChild(2).GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text =
            matrix_total.m23.ToString("F2");
        ui_matrix_values.transform.GetChild(3).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text =
            matrix_total.m30.ToString("F2");
        ui_matrix_values.transform.GetChild(3).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text =
            matrix_total.m31.ToString("F2");
        ui_matrix_values.transform.GetChild(3).GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text =
            matrix_total.m32.ToString("F2");
        ui_matrix_values.transform.GetChild(3).GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text =
            matrix_total.m33.ToString("F2");
    }
}
