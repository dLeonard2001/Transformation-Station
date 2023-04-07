using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finishLevel : MonoBehaviour
{
    [Tooltip("How much the object must be correct in order to pass the puzzle \n" +
             "1 = 100% correctness needed \n" +
             "0 = 0% correctness needed")]
    [SerializeField] [Range(1, 0)] private float percentageOffset;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform playerObject;
    
    // CorrectnessPercentage = 1
        // must be the exact match to pass
    // CorrectnessPercentage = 0
        // no error margin required to pass 
    // Example:
        // CorrectnessPercentage == 0.1
            // there can only be a 10% error difference between the puzzle objects and the target puzzle object
    private float CorrectnessPercentage;
    

    // Start is called before the first frame update
    void Start()
    {
        targetTransform = GetComponent<Transform>();

        CorrectnessPercentage = percentageOffset / 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log((targetTransform.position - playerObject.position).magnitude <= CorrectnessPercentage);
        Debug.Log(CorrectnessPercentage);
    }
}
