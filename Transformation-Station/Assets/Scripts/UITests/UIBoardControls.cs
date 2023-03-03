using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBoardControls : MonoBehaviour
{
    // count of cards used in transformation
    private int cardCount;
    
    // UI Card
    public GameObject UICard;
    
    // list of cards
    private List<GameObject> currentCards;
    
    // max number of UI cards
    private int maxCards;
    private int minCards;
    
    // Start is called before the first frame update
    void Start()
    {
        cardCount = 0;
        maxCards = 5;
        minCards = 1;

        currentCards = new List<GameObject>();
    }

    public void CreateNewCard()
    {
        // check for max number of cards
        if (cardCount < maxCards)
        {
            // create position for the new card
            Vector3 cardPosition = this.gameObject.transform.GetChild(0).position;
            
            // spawn the new card
            var newCard = Instantiate(UICard, cardPosition, Quaternion.identity, this.gameObject.transform.GetChild(1));

            // add the card to list
            currentCards.Add(newCard);

            cardCount++;
        }
    }

    public void DeleteCard()
    {
        // check for min number of cards
        if (cardCount > minCards)
        {
            // delete the latest card and remove from list
            GameObject.Destroy(currentCards[cardCount-1]);
            currentCards.RemoveAt(cardCount-1);
            cardCount--;
        }
    }
}
