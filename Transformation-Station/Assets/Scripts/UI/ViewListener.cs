using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ViewListener : MonoBehaviour
{
    [SerializeField] private UnityEvent onClick;
    
    private void OnMouseDown()
    {
        onClick.Invoke();
    }
}
