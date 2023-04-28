using System.Collections;
using UnityEngine;

public class PuzzleSolver : MonoBehaviour
{
    [Tooltip("How much the object must be correct in order to pass the puzzle \n" +
             "This represents the number of units the object can be incorrect for position and rotation")]
    [SerializeField] [Range(1, 0)] private float errorMargin;
    [SerializeField] private float hintMargin;
    [SerializeField] private Transform puzzleSolution;
    [SerializeField] private Transform puzzlePieces;
    
    [SerializeField] private AudioSource levelCompleteSource;

    [SerializeField] private AudioClip levelCompleteSound;

    [SerializeField] private AudioSource selfAudioSource;
    [SerializeField] private AudioClip levelCompleteVoiceline;
    
    [SerializeField] private GameObject levelFinishedUI;

    [Tooltip("the material that is used on the walls, to hint when the player is close to solving a puzzle")]
    [SerializeField] private MeshRenderer hintMeshRender;
    
    private ParticleSystem particle;
    private bool isFinished;

    private float CheckPosVar;
    private float CheckRotVar_forward;
    private float CheckRotVar_up;
    private float CheckRotVar_right;
    private float CheckScaleVar;

    private float value_pos;
    private float value_scale;
    private float value_rot;
    private float final_value;

    // Start is called before the first frame update
    void Start()
    {
        puzzleSolution = GetComponent<Transform>();

        particle = GetComponent<ParticleSystem>();

        selfAudioSource.clip = levelCompleteVoiceline;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F1))
        {
            puzzlePieces.position = puzzleSolution.position;
        }

        
        bool isPosCorrect = CheckPosition();
        bool isRotCorrect = CheckRotation();
        bool isScaleCorrect = CheckScale();

        if (isPosCorrect && isRotCorrect && isScaleCorrect&& !isFinished)
        {
            // completed a puzzle
            StartCoroutine(nameof(PassPuzzle));
        }

        AdjustHint();
    }

    private void AdjustHint()
    {
        float pos_time = errorMargin * hintMargin / CheckPosVar;
        float rot_time_forward = errorMargin * hintMargin / CheckRotVar_forward;
        float rot_time_up = errorMargin * hintMargin / CheckRotVar_up;
        float rot_time_right = errorMargin * hintMargin / CheckRotVar_right;
        float scale_time = errorMargin * hintMargin / CheckScaleVar;

        value_pos = Mathf.Lerp(-0.33f, 0.33f, pos_time);
        
        value_rot = Mathf.Lerp(-0.11f, 0.11f, rot_time_forward) + 
                    Mathf.Lerp(-0.11f, 0.11f, rot_time_up) + 
                    Mathf.Lerp(-0.11f, 0.11f, rot_time_right);

        value_scale = Mathf.Lerp(-0.33f, 0.33f, scale_time);
        
        final_value = value_pos + value_scale + value_rot;

        // Debug.Log($"{value_pos} : {value_rot} : {value_scale}");

        if (hintMeshRender == null)
            return;
        
        hintMeshRender.sharedMaterial.SetFloat("_CorrectPercentage", final_value);
    }

    IEnumerator PassPuzzle()
    {
        isFinished = true;
        // insert whatever you want when the player finishes the a puzzle

        // play the "LevelComplete" sound
        
        selfAudioSource.Play();
        
        // for now it only prints to the console that we have passed the level

        int time = 4;

        while (time > 0)
        {
            if (time % 2 == 0)
            {
                Debug.Log(time);
                particle.Play();
                levelCompleteSource.Play();
                
                // Using a the UI pop up card prefab to notify that the player completed the level
                if (levelFinishedUI)
                {
                    levelFinishedUI.SetActive(true);
                    break;
                }
                Debug.Log("Missing UI pop up card prefab");
            }

            yield return new WaitForSeconds(1);

            time--;
        }

        yield return new WaitForSeconds(2f);
        
        LevelLoader.FinishedLevel.Invoke();
    }

    private bool CheckPosition()
    {
        CheckPosVar = (puzzleSolution.position - puzzlePieces.position).magnitude;
        
        return CheckPosVar <= errorMargin;
        
    }

    private bool CheckRotation()
    {
        // basis vectors
            // transform.forward
            // transform.up
            // transform.right

        float forwardAngle = Vector3.Angle(puzzlePieces.forward, puzzleSolution.forward);
        float upAngle = Vector3.Angle(puzzlePieces.up, puzzleSolution.up);
        float rightAngle = Vector3.Angle(puzzlePieces.right, puzzleSolution.right);

        bool isForward = forwardAngle <= errorMargin;
        bool isUp = upAngle <= errorMargin;
        bool isRight = rightAngle <= errorMargin;

        CheckRotVar_forward = forwardAngle;
        CheckRotVar_up = upAngle;
        CheckRotVar_right = rightAngle;
        
        return isForward && isUp && isRight;
    }

    private bool CheckScale()
    {
        CheckScaleVar = (puzzleSolution.localScale - puzzlePieces.localScale).magnitude;
        
        return CheckScaleVar <= errorMargin;
    }
}
