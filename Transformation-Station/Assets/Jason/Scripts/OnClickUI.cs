using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class OnClickUI : MonoBehaviour
{
    [SerializeField] private GameObject windowUI;
    private GameObject windowUIInstance;

    private int maxCards = 4;
    private int currNumCards = 0;

    private void OnMouseDown()
    {
        // Disable any currently active UI windows
        DisableActiveUIWindows();

        // Instantiate the UI window prefab
        windowUIInstance = Instantiate(windowUI);

        // Set the parent of the UI window to the canvas
        windowUIInstance.transform.SetParent(GameObject.FindWithTag("Canvas").transform, false);

        // Enable the UI window
        windowUIInstance.SetActive(true);
    }

    /*
     * TODO: rewrite function for
     *      - Existing UI window associated with the game object that was clicked (No duplicates)
     *      - Other UI elements not related to the UI window are safe from the Destroy function
     *      - Data stored within inputs are stored with the game object when UI window is not present
    */
    private void DisableActiveUIWindows()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas.transform.childCount == 0) return; 
        
        // Get all active GameObjects in the canvas and destroy their child GameObject
        // This could be a problem for other existing UI elements that are present (i.e. exit button, skip button)
        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            Destroy(canvas.transform.GetChild(i).gameObject);
        }
    }

}
