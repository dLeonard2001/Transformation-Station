using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // Loading back to puzzle mode menu (needs modification via inspector for the tutorial levels)
    [SerializeField] private String loadSceneName = "PuzzleMode";
    
    public static Action FinishedLevel;
    
    [SerializeField] private Animator transition;
    [SerializeField] private int transitionTime = 2;
    
    private static readonly int fadeIn = Animator.StringToHash("Start");
    
    [SerializeField] private AudioSource sceneStartSource;

    [SerializeField] private AudioClip sceneStartSound;

    [SerializeField] private bool playStartSound;

    private void Start()
    {
        // play the "SceneStart" sound

        FinishedLevel = LoadNextLevel;

        AudioListener.volume = 1;
        
        if(playStartSound) 
            sceneStartSource.PlayOneShot(sceneStartSound);
    }

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(LoadNextScene(sceneName));
    }

    IEnumerator LoadNextScene(string sceneName)
    {
        transition.SetTrigger(fadeIn);

        float time = 2;

        while (time >= 0)
        {
            AudioListener.volume -= Time.fixedDeltaTime;
            
            Debug.Log(AudioListener.volume);

            time -= Time.fixedDeltaTime;

            yield return null;
        }

        AudioListener.volume = 0;

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }
    
    IEnumerator LoadNextScene(int i)
    {
        transition.SetTrigger(fadeIn);

        float time = 2;

        while (time >= 0)
        {
            AudioListener.volume -= Time.fixedDeltaTime;
            
            Debug.Log(AudioListener.volume);

            time -= Time.fixedDeltaTime;

            yield return null;
        }

        AudioListener.volume = 0;

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadSceneAsync(i, LoadSceneMode.Single);
    }

    private void LoadNextLevel()
    {
        StartCoroutine(LoadNextScene(loadSceneName));
    }

}
