using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    [SerializeField] private Animator transition;
    [SerializeField] private int transitionTime = 1;
    
    private static readonly int Start = Animator.StringToHash("Start");

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.RightShift))
        // {
        //     LoadNextLevel();
        // }
    }

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(LoadNextLevel(sceneName));
    }

    IEnumerator LoadNextLevel(string sceneName)
    {
        if (sceneName.Equals("Debug"))
        {
            Debug.Log("Debugging: Choose scene name to transition");
            yield break;
        }
        
        transition.SetTrigger(Start);

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }
}
