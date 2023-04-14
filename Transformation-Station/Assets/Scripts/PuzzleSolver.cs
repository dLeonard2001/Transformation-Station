using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PuzzleSolver : MonoBehaviour
{
    [Tooltip("How much the object must be correct in order to pass the puzzle \n" +
             "1 = 100% correctness needed \n" +
             "0 = 0% correctness needed")]
    [SerializeField] [Range(1, 0)] private float errorMargin;
    [SerializeField] private Transform puzzleSolution;
    [SerializeField] private Transform puzzlePieces;

    private ParticleSystem particle;
    private bool isFinished;

    // Start is called before the first frame update
    void Start()
    {
        puzzleSolution = GetComponent<Transform>();

        particle = GetComponent<ParticleSystem>();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F1))
        {
            puzzlePieces.position = puzzleSolution.position;
        }

        if (CheckPosition() && CheckRotation() && CheckScale() && !isFinished)
        {
            // completed a puzzle
            StartCoroutine(nameof(PassPuzzle));
        }
    }

    IEnumerator PassPuzzle()
    {
        isFinished = true;
        // insert whatever you want when the player finishes the a puzzle
        // for now it only prints to the console that we have passed the level

        float time = 20;

        while (time >= 0)
        {
            if (Mathf.FloorToInt(time) % 5 == 0)
            {
                particle.Play();
            }

            yield return new WaitForSeconds(1);

            time--;
        }

        Debug.Log("puzzle solved ");
    }

    private bool CheckPosition()
    {
        return (puzzleSolution.position - puzzlePieces.position).magnitude <= errorMargin;
    }

    private bool CheckRotation()
    {
        // basis vectors
            // transform.forward
            // transform.up
            // transform.right

        bool isForward = Vector3.Angle(puzzlePieces.forward, puzzleSolution.forward) <= errorMargin;
        bool isUp = Vector3.Angle(puzzlePieces.up, puzzleSolution.up) <= errorMargin;
        bool isRight = Vector3.Angle(puzzlePieces.right, puzzleSolution.right) <= errorMargin;

        return isForward && isUp && isRight;
    }

    private bool CheckScale()
    {
        return (puzzleSolution.localScale - puzzlePieces.localScale).magnitude <= errorMargin;
    }
}
