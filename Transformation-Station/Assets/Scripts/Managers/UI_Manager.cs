using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [Header("Disable Mode")]
    [SerializeField] private bool disabled;
    
    [Header("References")]
    [SerializeField] private RectTransform cardParent;
    [SerializeField] private GameObject ui_card_prefab;

    [Header("Animation References")] 
    [SerializeField] private Animator animControlBoard;

    private static MatrixTransformation currentObject;
    private static GameObject currentCard;

    [SerializeField] private Camera valueCamera;
    [SerializeField] private Camera subvalueCamera;

    [Header("Color References")] 
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color selectedColor;
    
    [Header("Matrix Values")]
    [SerializeField] private GameObject ui_matrix_values;

    [SerializeField] private GameObject ui_matrix_subvalues;

    private Matrix4x4 matrix_total;
    private Matrix4x4 matrix_subtotal;

    private float valueCamDestination;
    private float subvalueCamDestination;

    private bool moveValue;
    private bool moveSubvalue;
    private bool hideCam;

    [Header("UI Sounds")] 
    [SerializeField] private AudioSource openBoardSource;

    [SerializeField] private AudioSource closeBoardSource;
    
    [SerializeField] private AudioSource selectCardSource;

    [SerializeField] private AudioSource changeCardTypeSource;

    [SerializeField] private AudioSource deleteCardSource;

    [SerializeField] private AudioClip openBoardSound;

    [SerializeField] private AudioClip closeBoardSound;
    
    [SerializeField] private AudioClip selectCardSound;

    [SerializeField] private AudioClip changeCardTypeSound;

    [SerializeField] private AudioClip deleteCardSound;


    // needed for raycast
    private Camera mainCamera;
    private Ray myRay;
    private RaycastHit hitTarget;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Start()
    {
        mainCamera = Camera.main;

        matrix_total = Matrix4x4.identity;
        matrix_subtotal = Matrix4x4.identity;
        
        // hides screen values
        if (!disabled)
        {
            valueCamera.rect = new Rect (-1.0f, 0.7f, 1, 1);
            subvalueCamera.rect = new Rect(-1.0f, -0.8f, 1, 1);            
        }

        valueCamDestination = -1.0f;
        subvalueCamDestination = -1.0f;

        moveSubvalue = false;
        moveValue = false;
        hideCam = true;
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
                    animControlBoard.CrossFade("UI_slide_in", 0f, 0);
                    
                    // display the totals
                    matrix_total = currentObject.GetTotal();
                    SetValues();
                    
                    // show screen values
                    valueCamDestination = -0.8f;
                    moveValue = true;
                    subvalueCamDestination = -0.82f;
                    moveSubvalue = true;
                    hideCam = false;

                    // play the "OpenBoard" sound
                    openBoardSource.PlayOneShot(openBoardSound);
                }
            }
            else
            {
                // if the player clicks on a UI element
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                
                // if we are here, then we have selected a false object to edit
                currentObject = null;
                animControlBoard.CrossFade("UI_slide_out", 0f, 0);
                
                // hide the totals
                matrix_total = Matrix4x4.identity;

                matrix_subtotal = Matrix4x4.identity;
                SetValues();
                SetSubtotal();
                
                // hide screen values
                valueCamDestination = -1.0f;
                moveValue = true;
                subvalueCamDestination = -1.0f;
                moveSubvalue = true;
                hideCam = true;

                // play the "CloseBoard" sound
            }

        }
        // set screen values
        if (currentObject != null)
        {
            matrix_total = currentObject.GetTotal();
            SetValues();
            
            // set the subvalues
            if (currentCard != null)
            {
                matrix_subtotal = currentObject.GetSubtotal((int) Char.GetNumericValue(currentCard.name[0]));
                SetSubtotal();
            }
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            if (hideCam)
            {
                // show screen values
                valueCamDestination = -0.8f;
                moveValue = true;
                subvalueCamDestination = -0.82f;
                moveSubvalue = true;
            
                hideCam = false;
            }

            else
            {
                // hide screen values
                valueCamDestination = -1.0f;
                moveValue = true;
                subvalueCamDestination = -1.0f;
                moveSubvalue = true;

                hideCam = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (moveSubvalue)
        {
            MoveSubCam(subvalueCamDestination, hideCam);
        }

        if (moveValue)
        {
            MoveValueCam(valueCamDestination, hideCam);
        }
    }

    #region CardFunctions

    // adds a card to the UI and current object's cards
    public void AddCard()
    {
        GameObject newCard = Instantiate(ui_card_prefab, cardParent);

        newCard.transform.name = $"{currentObject.GetSize()}_card";

        currentObject.AddCard(newCard);
        currentObject.AddMatrix();
        
        SetCurrentCard(newCard);
    }

    // add remove button to individual cards
    public void RemoveCard(int index)
    {
        currentObject.RemoveMatrix(index);
        
        UpdateCardNames();
        
        // play the "DeleteCard" sound
        deleteCardSource.PlayOneShot(deleteCardSound);
    }

    // update card names to match the correct index position
        // (could be changed if using a different way to get the current index position)
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

    // loads any cards, when selecting an object
    private void LoadCards()
    {
        foreach (var c in currentObject.GetCurrentCards())
        {
            c.SetActive(true);
        }
    }

    // unloads any cards, when de-selecting an object
    private void UnloadCards()
    {
        if (currentObject == null)
            return;

        foreach (var c in currentObject.GetCurrentCards())
        {
            c.SetActive(false);
        }
    }

    // resets the object to its origin
    public void Reset()
    {
        currentObject.Reset();
        
        // adjust the second screen
        currentObject.ResetTotal();
        matrix_total = currentObject.GetTotal();
        //SetValues();
    }

    // updates the current card's value to correspond with the input
    public static void UpdateCardValue(float num)
    {
        if (num == 0) // if no input then do nothing
            return;
        
        // get the current index in the sequence
        int cardNum = (int) Char.GetNumericValue(currentCard.name[0]); 

        // get the cards from the current object for later use
        List<GameObject> cards = currentObject.GetCurrentCards();
        
        // the currently selected card's text
        TextMeshProUGUI tmp = currentCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        // gets the current value of the selected card
        string[] strCards = tmp.text.Split(' '); 

        // adds the input value to the current value
            // then assigns it to the current card's text
        float newNum = float.Parse(strCards[1]) + num;
        tmp.text = $"{strCards[0]} {String.Format("{0:F2}", newNum)}";

        // edit the matrix at the current index/element within the sequence
        currentObject.EditMatrix(newNum, cards[cardNum].GetComponentInChildren<TMP_Dropdown>().captionText.text, cardNum);

        // apply the new transformations
        currentObject.ApplyTransformations(cardNum);
    }

    // returns if a card is selected 
    public static bool HasCardSelected()
    {
        return currentCard != null;
    } 

    // returns the currentObject that is selected
    public MatrixTransformation GetCurrentObject()
    {
        return currentObject;
    }

    // assigns the selected card for overall use 
    public void SetCurrentCard(GameObject t)
    {
        // selected 
            // set current card = t 
            // change image color to selectedColor
        // de-selected
            // change image color to defaultColor;
            // set current card to null

            if (t != null && currentCard == null)
            {
                currentCard = t;
                currentCard.GetComponentInChildren<Image>().color = selectedColor;

                currentObject.ApplyTransformations((int) Char.GetNumericValue(currentCard.name[0]));
            }
            else if (currentCard != null)
            {
                currentCard.GetComponentInChildren<Image>().color = defaultColor;
                currentCard = t;
                currentCard.GetComponentInChildren<Image>().color = selectedColor;
                
                currentObject.ApplyTransformations((int) Char.GetNumericValue(currentCard.name[0]));
            }
            else
            {
                currentCard.GetComponentInChildren<Image>().color = defaultColor;
                currentCard = t;
            }

            // adjust the second screen
            matrix_total = currentObject.GetTotal();
            
            // play the "SelectCard" sound
            selectCardSource.PlayOneShot(selectCardSound);
    }

    // returns the type of transformation on the current card
        // Translate, Rotate, Scale
    public static String TransformationType()
    {
        return currentCard.GetComponentInChildren<TMP_Dropdown>().captionText.text;
    }

    // returns the current direction to use the input on
    public static Char CardDirection()
    {
        return currentCard.GetComponentInChildren<TMP_Dropdown>().captionText.text[^1];
    }

    // sets the total values in the ui

    private void SetValues()
    {
        if (disabled) return;
        
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                ui_matrix_values.transform.GetChild(i+1).GetChild(j).GetComponent<TMPro.TextMeshProUGUI>().text = matrix_total[i, j].ToString("F2");
            }
        }
    }

    private void SetSubtotal()
    {
        if (disabled) return;
        
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                ui_matrix_subvalues.transform.GetChild(i+1).GetChild(j).GetComponent<TMPro.TextMeshProUGUI>().text = matrix_subtotal[i, j].ToString("F2");
            }
        }
    }

    private void MoveSubCam(float destination, bool hiddenCam)
    {
        if (disabled) return;
        
        if (subvalueCamera.rect.x < destination)
        {
            subvalueCamera.rect = new Rect((subvalueCamera.rect.x + 0.01f), -0.8f, 1, 1);
        }
        
        if (subvalueCamera.rect.x == destination)
        {
            moveSubvalue = false;
        }
        
        if (subvalueCamera.rect.x > destination)
        {
            subvalueCamera.rect = new Rect((subvalueCamera.rect.x - 0.01f), -0.8f, 1, 1);
        }

        if (subvalueCamera.rect.x == destination)
        {
            moveSubvalue = false;
        }
    }

    private void MoveValueCam(float destination, bool hiddenCam)
    {
        if (disabled) return;
        
        if (valueCamera.rect.x < destination)
        {
            valueCamera.rect = new Rect((valueCamera.rect.x + 0.01f),  0.7f, 1, 1);
        }
        
        if (valueCamera.rect.x == destination)
        {
            moveValue = false;
        }
        
        if (valueCamera.rect.x > destination)
        {
            valueCamera.rect = new Rect((valueCamera.rect.x - 0.01f),  0.7f, 1, 1);
        }

        if (valueCamera.rect.x == destination)
        {
            moveValue = false;
        }
    }

    public void PlayChangeSound()
    {
        // play the "ChangeCardType" sound
        changeCardTypeSource.PlayOneShot(changeCardTypeSound);
    }
}
