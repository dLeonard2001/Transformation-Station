using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownPause : MonoBehaviour
{
    [SerializeField] private float countdownTimer = 2f;
    [SerializeField] private bool startCountdown = false;
    
    private TutorialManager _tutorialManager;
    private bool _tutorialManagerFound = false;
    private bool _hasDoneCountdown = false;
    
    private void Awake()
    {
        _tutorialManager = FindObjectOfType<TutorialManager>();
        if (_tutorialManager) _tutorialManagerFound = true;
        else
        {
            Debug.Log("Missing Tutorial Manager in Game Scene");
        }
    }

    private void Update()
    {
        if (!CheckTutorialManager()) return;

        CountdownFinished();
        
        if (startCountdown && !_hasDoneCountdown) countdownTimer -= Time.deltaTime;
    }

    private bool CheckTutorialManager()
    {
        return _tutorialManager;
    }

    private void CountdownFinished()
    {
        if (countdownTimer < 0 && !_hasDoneCountdown)
        {
            _tutorialManager.PauseGame();
            _hasDoneCountdown = true;
        }
    }

    private void OnMouseDown()
    {
        _tutorialManager.NextTutorial();
    }

    private void StartCountdown()
    {
        startCountdown = true;
    }
}
