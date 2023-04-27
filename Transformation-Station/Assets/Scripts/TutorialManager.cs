using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tutorialCardsUI;
    [SerializeField] private AudioClip[] tutorialVoicelines;
    [SerializeField] private AudioSource audioSource;
    
    private bool _tutorialMode = false;
    private int _tutorialProgress = 0;

    private void Update()
    {
        SetTimeScale();
    }

    private void SetTimeScale()
    {
        // Do Nothing
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

        audioSource.clip = tutorialVoicelines[_tutorialProgress];
        audioSource.Play();
        
        _tutorialMode = true;

        SetActiveCardUI(_tutorialMode);
    }
    
    public void UnPauseGame()
    {
        audioSource.Stop();
        
        _tutorialMode = false;
        
        SetActiveCardUI(_tutorialMode);

        _tutorialProgress++;
    }
}
