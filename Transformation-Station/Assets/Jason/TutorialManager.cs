using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tutorialCardsUI;
    
    private bool _tutorialMode = false;
    private int _tutorialProgress = 0;

    private void Update()
    {
        SetTimeScale();
    }

    private void SetTimeScale()
    {
        Time.timeScale = _tutorialMode ? 0 : 1;
    }

    private void SetActiveCardUI(bool x)
    {
        if (_tutorialProgress < tutorialCardsUI.Length && tutorialCardsUI[_tutorialProgress])
        {
            tutorialCardsUI[_tutorialProgress].SetActive(x);
        }
    }

    public void PauseGame()
    {
        // Missing UI popup card or index out of range or if there is no more tutorial
        if (_tutorialProgress >= tutorialCardsUI.Length) return;
        
        _tutorialMode = true;

        SetActiveCardUI(_tutorialMode);
    }
    
    public void UnPauseGame()
    {
        _tutorialMode = false;
        
        SetActiveCardUI(_tutorialMode);

        _tutorialProgress++;
    }
}
