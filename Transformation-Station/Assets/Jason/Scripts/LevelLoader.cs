using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    [SerializeField] private Animator transition;
    [SerializeField] private string sceneNameToLoad = "SceneStart";
    [SerializeField] private int transitionTime = 1;
    
    private static readonly int Start = Animator.StringToHash("Start");

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(sceneNameToLoad));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        transition.SetTrigger(Start);

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }
}
