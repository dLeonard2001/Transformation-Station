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
    [SerializeField] private GameObject matrix_value_preview;

    private MatrixTransformation currentObject;
    
    [Header("Animations")] 
    [SerializeField] private Animator ui_animator;

    // needed for raycast
    private Camera mainCamera;
    private Ray myRay;
    private RaycastHit hitTarget;

    private void Start()
    {
        mainCamera = Camera.main;
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

    public void Reset()
    {
        currentObject.Reset();
    }
    
    public Matrix4x4 PreviewValues(int index)
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
        
        Matrix4x4 m = currentObject.GetMatrix(0);

        for (int i=1; i <= index; i++)
        {
           m = currentObject.GetMatrix(i) * m;
        }

        return m;
    }

    public GameObject GetMatrixPreviewObject()
    {
        return matrix_value_preview;
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
        
        currentObject.ApplyTransformations();
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
}
