using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIOnClick : MonoBehaviour
{
    // UI Board
    public GameObject UIBoard;
	public GameObject boardParent;

    [Header("Animations")] 
    [SerializeField] private Animator ui_animator;

    // needed for raycast
    private Camera mainCamera;
    private Ray myRay;
    private RaycastHit hitTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
