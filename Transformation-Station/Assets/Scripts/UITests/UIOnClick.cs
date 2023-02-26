using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOnClick : MonoBehaviour
{
    // UI Board
    public GameObject UIBoard;

    // store what objects have boards made
    private List<GameObject> currentBoards;
    private List<string> objectNames;

    private bool foundName;
    private int nameNum;
    private string targetName;
    
    // needed for raycast
    private Camera mainCamera;
    private Ray myRay;
    private RaycastHit hitTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        currentBoards = new List<GameObject>();
        objectNames = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // make ray from camera to where mouseclick
            myRay = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(myRay, out hitTarget, 2000f))
            {
                // check if clicked object should have a board
                if (hitTarget.collider.tag == "Planet")
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
                else if (hitTarget.collider.tag == "Wall")
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
            }
        }

        /*if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log(objectNames.Count);
        }*/
    }

    public void SpawnBoard(string newName)
    {
        // create position for the new board
        Vector3 boardPosition = this.gameObject.transform.position;
        boardPosition.y = -110f;
        
        // create board
        var newBoard = Instantiate(UIBoard, boardPosition, Quaternion.identity, this.gameObject.transform);
        
        // add the board to list
        currentBoards.Add(newBoard);
        
        // update list of names
        objectNames.Add(newName);
    }

    public void ShowBoard()
    {

    }
}
