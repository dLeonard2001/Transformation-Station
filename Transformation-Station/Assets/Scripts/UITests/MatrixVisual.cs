using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MatrixVisual : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public bool textSelected;

	// Start is called before the first frame update
    void Start()
    {
        textSelected = false;
    }

	public void OnPointerEnter(PointerEventData eventData)
    {
        textSelected = true;
    }

	public void OnPointerExit(PointerEventData eventData) 
	{
         textSelected = false;
     }
}
