using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIOnClick : MonoBehaviour
{
    // UI Board
    public GameObject UIBoard;
	public GameObject boardParent;
    public GameObject indicatorHolder;

    // store what objects have boards made
    private List<GameObject> currentBoards;
    private List<string> objectNames;

    private List<GameObject> targets;

    private bool foundName;
    private int nameNum;
    private string targetName;
    private GameObject targetObject;
    
    // needed for raycast
    private Camera mainCamera;
    private Ray myRay;
    private RaycastHit hitTarget;
    
    // needed for indicator holder
    public IndicatorHolderScript indicatorScript;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        currentBoards = new List<GameObject>();
        targets = new List<GameObject>();
        objectNames = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // make ray from camera to where mouseclick
            myRay = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(myRay, out hitTarget, Mathf.Infinity))
            {
                //Debug.Log(hitTarget.collider.gameObject);
				// if the player clicks on a UI element
				if (EventSystem.current.IsPointerOverGameObject())
				{
					return;
				}

                // check if clicked object should have a board
                else if (hitTarget.collider.tag == "Planet")
                {
                    foundName = false;
                    
                    // check if the name already has a board
                    for (int i = 0; i < objectNames.Count; i++)
                    {
                        // finds a match
                        if (hitTarget.collider.gameObject.name == objectNames[i])
                        {
                            foundName = true;
                            nameNum = i;
                        }
                    }
                    
                    // turn on the found board
                    if (foundName == true)
                    {
                        currentBoards[nameNum].SetActive(true);
                    }
                    
                    // if there was not match
                    if (foundName == false)
                    {
                        // add the new target
                        targets.Add(hitTarget.collider.gameObject);
                        
                        // spawn the new board
                        targetName = hitTarget.collider.gameObject.name;
                        targetObject = hitTarget.collider.gameObject;
                        SpawnBoard(targetName, targetObject, (targets.Count - 1));
                    }
                }
                
                // turn off all boards
                else
                {
                    for (int i = 0; i < objectNames.Count; i++)
                    {
                        currentBoards[i].SetActive(false);
                    }
                }
            }
            //ShowBoard();
        }
    }

    public void SpawnBoard(string newName, GameObject target, int listNum)
    {
        // create position for the new board
        Vector3 boardPosition = boardParent.transform.position;
        //boardPosition.y = -110f;
        
        // create board
        var newBoard = Instantiate(UIBoard, boardPosition, Quaternion.identity, gameObject.transform);
        
        // add the board to list
        currentBoards.Add(newBoard);
        
        // update list of names
        objectNames.Add(newName);
        
        // create a new spawnIndicatorZone
        //SpawnIndicatorZone(target);
        // get the location of the holder
        Vector3 spawnLocation = target.transform.GetChild(0).transform.position;
        
        // create the holder
        var newHolder = Instantiate(indicatorHolder, spawnLocation, Quaternion.identity, newBoard.transform);
        
        // pass the objects location to the holder
        indicatorScript = (IndicatorHolderScript)newHolder.GetComponent(typeof(IndicatorHolderScript));
        indicatorScript.setTracker(target);
    }
    
    // will create a scroll view to hold the indicators
    // indicator zone will be a child of the control board
    
    /*private void SpawnIndicatorZone(GameObject target)
    {
        // get the location of the holder
        Vector3 spawnLocation = target.transform.GetChild(0).transform.position;
        
    }*/

    /*private void ShowBoard()
    {
        // make ray from camera to where mouseclick
        myRay = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(myRay, out hitTarget, 2000f)) return;
        
        // check if clicked object should have a board
        if (hitTarget.collider.CompareTag("Planet")) 
        {
            foundName = false;
            // check if the name already has a board
            for (int i = 0; i < objectNames.Count; i++) 
            {
                // finds a match
                if (hitTarget.collider.gameObject.name == objectNames[i])
                {
                    foundName = true;
                    nameNum = i;
                }
            }
                
            // turn on the found board
            if (foundName == true)
            {
                currentBoards[nameNum].SetActive(true);
            }
                    
            // if there was not match
            if (foundName == false)
            {
                // spawn the new board
                targetName = hitTarget.collider.gameObject.name;
                SpawnBoard(targetName);
            }
        }
                
        // blocks the ui section that player might click
        else if (hitTarget.collider.CompareTag("Wall"))
        {
        }
                
        // turn off all boards
        else
        {
            for (int i = 0; i < objectNames.Count; i++)
            { 
                currentBoards[i].SetActive(false);
            }
        }
    }*/
}
