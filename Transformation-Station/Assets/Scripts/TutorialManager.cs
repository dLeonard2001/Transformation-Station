using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tutorialCardsUI;
    [SerializeField] private AudioClip[] tutorialVoicelines;
    [SerializeField] private AudioSource audioSource;

    // default is 1 second for a message to generate on the screen
    [SerializeField] private float cardGenerateDuration = 1;
    
    private float textGenerateTime;

    private bool _tutorialMode = false;
    private int _tutorialProgress = 0;
    private int maxTutorialParts;

    private bool cr;

    private void Awake()
    {
        maxTutorialParts = tutorialCardsUI.Length;
    }

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
        if (tutorialCardsUI[_tutorialProgress] && _tutorialProgress < maxTutorialParts)
        {
            tutorialCardsUI[_tutorialProgress].transform.localScale = new Vector3(0, 1, 1);
            
            tutorialCardsUI[_tutorialProgress].SetActive(x);
            
            audioSource.clip = tutorialVoicelines[_tutorialProgress];
            
            StopAllCoroutines();

            StartCoroutine(GenerateMessage(tutorialCardsUI[_tutorialProgress].transform));
        }
    }

    public void PauseGame()
    {
        // Missing UI popup card or index out of range or if there is no more tutorial
        if (_tutorialProgress >= tutorialCardsUI.Length) return;

        // audioSource.clip = tutorialVoicelines[_tutorialProgress];
        // audioSource.Play();
        
        _tutorialMode = true;

        SetActiveCardUI(_tutorialMode);
    }

    private IEnumerator GenerateMessage(Transform t)
    {
        TextMeshProUGUI tmp = tutorialCardsUI[_tutorialProgress].GetComponentInChildren<TextMeshProUGUI>();

        string text = tmp.text;
        
        tmp.text = "";

        float timeElapsed = 0;
        float time = timeElapsed / cardGenerateDuration;

        while (time <= 1)
        {
            timeElapsed += Time.deltaTime;

            time = timeElapsed / cardGenerateDuration;
            
            t.localScale = t.localScale = new Vector3(Mathf.Lerp(0, 1, time), t.localScale.y, 1);
            
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        
        StartCoroutine(GenerateText(t, tmp, text));
    }

    private IEnumerator GenerateText(Transform t, TextMeshProUGUI tmp, string startText)
    {
        
        audioSource.Play();
        
        int maxLetterCount = startText.Length;
        string startingText = startText;

        // Debug.Log(tmp.text);

        textGenerateTime = tutorialVoicelines[_tutorialProgress].length / maxLetterCount;
        textGenerateTime -= 0.006f;
        
        // Debug.Log(textGenerateTime);

        int letterCount = 0;
        
        string newStr = tmp.text.Substring(0, letterCount);

        while (letterCount < maxLetterCount)
        {
            tmp.text = newStr;

            yield return new WaitForSeconds(textGenerateTime);
            
            letterCount++;
            
            newStr = startingText.Substring(0, letterCount);
        }
        
        // Debug.Log("done generating text on screen");

        yield return new WaitForSeconds(3f);

        StartCoroutine(DegenerateMessage(t));
    }
    
    private IEnumerator DegenerateMessage(Transform t)
    {
        float timeElapsed = 0;

        float time = timeElapsed / cardGenerateDuration;

        while (time <= 1)
        {
            timeElapsed += Time.deltaTime;

            time = timeElapsed / cardGenerateDuration;
            
            t.localScale = new Vector3(Mathf.Lerp(1, 0, time), t.localScale.y, 1);
            
            yield return null;
        }

        _tutorialProgress++;
    }
    
    private IEnumerator DegenerateThenGenerateMessage(Transform t)
    {
        float timeElapsed = 0;

        float time = timeElapsed / cardGenerateDuration;

        while (time <= 1)
        {
            timeElapsed += Time.deltaTime;

            time = timeElapsed / cardGenerateDuration;
            
            t.localScale = new Vector3(Mathf.Lerp(1, 0, time), t.localScale.y, 1);
            
            yield return null;
        }
        
        Debug.Log("going to next card");

        _tutorialProgress++;
        
        SetActiveCardUI(true);
    }
    
    public void NextTutorial()
    {
        audioSource.Stop();

        StopAllCoroutines();

        StartCoroutine(DegenerateThenGenerateMessage(tutorialCardsUI[_tutorialProgress].transform));
    }
}
