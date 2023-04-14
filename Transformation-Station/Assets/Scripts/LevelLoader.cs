using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    [SerializeField] private Animator transition;
    [SerializeField] private int transitionTime = 1;
    
    private static readonly int Start = Animator.StringToHash("Start");
    
    [SerializeField] private AudioSource sceneStartSource;

    [SerializeField] private AudioClip sceneStartSound;

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
        
        yield return new WaitForSeconds(2.0f);
        
        // play the "SceneStart" sound
        sceneStartSource.PlayOneShot(sceneStartSound);
    }
}
