using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorHolderScript : MonoBehaviour
{
	// object to track
	private GameObject tracker;

	// required to convert worldspace to screenspace
	private RectTransform rectTransform;
	private RectTransform parentTransform;
    private Vector2 UIOffset;
	private Camera UICamera;
	private Canvas UICanvas;
	private GameObject tempCanvas;
	private Vector3 trackerPosition;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
		UICamera = Camera.main;
		tempCanvas = GameObject.Find("Canvas");
		parentTransform = tempCanvas.GetComponent<RectTransform>();
		//UIOffset = new Vector2((float)Canvas.sizeDelta.x / 2f, (float)Canvas.sizeDelta.y / 2f);
    }

    // Update is called once per frame
    void Update()
    {
     	// convert world space to screen space
           					//Vector2 anchoredPosition;
                        	//RectTransformUtility.ScreenPointToLocalPointInRectangle(parentTransform, Input.mousePosition, 
                            //uiCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : uiCamera, out anchoredPosition);
		Vector2 anchoredPosition;
		UICanvas = tempCanvas.GetComponent<Canvas>();
		//Debug.Log(tracker.transform.GetChild(0).name);
		RectTransformUtility.ScreenPointToLocalPointInRectangle(parentTransform,trackerPosition,UICanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : UICamera, out anchoredPosition);
		rectTransform.anchoredPosition = anchoredPosition;
	}

	public void setTracker(GameObject toTrack)
	{
		tracker = toTrack;
		//Debug.Log(tracker.transform.GetChild(0).transform.position);
		trackerPosition = new Vector3(tracker.transform.GetChild(0).transform.position.x, tracker.transform.GetChild(0).transform.position.y, tracker.transform.GetChild(0).transform.position.z);
		Debug.Log(trackerPosition);
	}
}
