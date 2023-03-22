using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [Header("References")] 
    
    [SerializeField] private RectTransform cardParent;
    [SerializeField] private GameObject ui_card_prefab;
    
    [Header("Animation References")]
    [SerializeField] private RectTransform uiControlBoard;
    [SerializeField] private Vector3 animStartPosition;
    [SerializeField] private Vector3 animEndPosition;

    private static MatrixTransformation currentObject;
    private static GameObject selectedCard;
    
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

                currentObject = null;
                ui_animator.CrossFade("UI_slide_out", 0f, 0);
            }

        }
    }

    #region CardFunctions

    public void AddCard()
    {
        GameObject newCard = Instantiate(ui_card_prefab, cardParent);

        newCard.transform.name = $"{currentObject.GetSize()}_card";

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

    public static void UpdateCardValue(float num)
    {
        if (num == 0)
            return;
        
        // change the current cards value
        // combine/transform any transformations before 
        // then apply the transformations up until the current card selected

        int cardNum = (int) Char.GetNumericValue(selectedCard.name[0]);

        List<GameObject> cards = currentObject.GetCurrentCards();
        TextMeshProUGUI tmp = selectedCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        string[] strCards = tmp.text.Split(' ');
        
        // Debug.Log(num);
        
        float newNum = float.Parse(strCards[1]) + num;
        tmp.text = $"{strCards[0]} {String.Format("{0:F2}", newNum)}";

        for (int i = 0; i < cardNum + 1; i++)
        {
            currentObject.EditMatrix(newNum, cards[i].GetComponentInChildren<TMP_Dropdown>().captionText.text, i);
        }

        currentObject.ApplyTransformations(cardNum);
        currentObject.ResetMatrices();
        
    }

    public static bool HasCardSelected()
    {
        return selectedCard != null;
    } 

    public MatrixTransformation GetCurrentObject()
    {
        return currentObject;
    }

    public void SetCurrentCard(GameObject t)
    {
        if (selectedCard == null)
        {
            selectedCard = t;
            selectedCard.transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
        }
        else if(t != null)
        {
            selectedCard.transform.GetChild(0).GetComponent<Image>().color = Color.gray;
            selectedCard = t;
            selectedCard.transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            selectedCard.transform.GetChild(0).GetComponent<Image>().color = Color.gray;
            selectedCard = t;
        }
    }
}
