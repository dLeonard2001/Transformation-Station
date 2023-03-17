using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public Image img;
    public Sprite normal;
    public Sprite pressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        img.sprite = pressed;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        img.sprite = normal;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
