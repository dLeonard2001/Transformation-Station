using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    [SerializeField] private Animator transition;
    [SerializeField] private int transitionTime = 1;
    
    private static readonly int fadeIn = Animator.StringToHash("Start");
    
    [SerializeField] private AudioSource sceneStartSource;

    [SerializeField] private AudioClip sceneStartSound;

    [SerializeField] private bool playStartSound;

    private void Start()
    {
        // play the "SceneStart" sound
        
        if(playStartSound) 
            sceneStartSource.PlayOneShot(sceneStartSound);
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
        
        transition.SetTrigger(fadeIn);

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

    }
}
