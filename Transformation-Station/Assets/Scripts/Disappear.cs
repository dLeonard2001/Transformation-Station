using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to make the "Level Completed" UI card disappear and not stand out during scene transitioning
public class Disappear : MonoBehaviour
{
    [SerializeField] private float lifespan = 1.5f;
    private bool _isActive = false;

    void Update()
    {
        if (!_isActive) return;
        
        if (lifespan < 0)
        {
            gameObject.SetActive(false);
        }
        
        lifespan -= Time.deltaTime;
    }

    private void OnEnable()
    {
        _isActive = true;
    }
}
