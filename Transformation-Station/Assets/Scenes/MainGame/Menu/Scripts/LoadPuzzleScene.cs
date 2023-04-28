using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPuzzleScene : MonoBehaviour
{
    public void LoadScene()
    {
        Debug.Log("Inside");
        SceneManager.LoadScene("PuzzleMode");
    }
}
