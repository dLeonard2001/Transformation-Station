using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class MatrixTransformation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _transform;
    [SerializeField] private Transform targetTransform;
    private Vector3 translationVector;
    private Vector3 scaleVector;
    private Vector3 originalScale;

    private Matrix4x4 newMatrix;

    private bool cr_active;

    // ===================================== NOTES =====================================
    // General order for matrix transformations are
    // 1. scaling
    // 2. rotations
    // 3. translation
    // there is a specific order because
    // the order in which the transformations are done can affect the result
    // ===================================== NOTES =====================================

    // cache some data for later
    void Start()
    {
        _transform = GetComponent<Transform>();

        scaleVector = Vector3.one;
        originalScale = _transform.localScale;
    }
    
    // a poor way to determine if we have won/beat the current level
    private void Update()
    {
        if ((targetTransform.position - _transform.position).magnitude < 0.5f && !cr_active)
        {
            StartCoroutine(Win());
        }
    }

    private IEnumerator Win()
    {

        cr_active = true;
        _animator.CrossFade("winAnimation", 0f, 0);

        yield return new WaitForSeconds(3f);

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        cr_active = false;
    }
    
    // reset our player's transformations to the beginning of the level
    public void ResetPlayer()
    {
        StopAllCoroutines();
        
        newMatrix = Matrix4x4.identity;

        _transform.localScale = originalScale;
        _transform.localRotation = Quaternion.LookRotation(newMatrix.GetColumn(2), newMatrix.GetColumn(1));
        _transform.position = newMatrix.GetPosition();
    }
    
    

    #region OrderOfOperations
    
    // apply our scale input
    public void ApplyScale()
    {
        newMatrix = _transform.localToWorldMatrix;
        newMatrix *= Scale(scaleVector);

        _transform.localScale = newMatrix.lossyScale;
    }

    // apply x rotation input
    public void ApplyRotationX(TMP_InputField input)
    {
        if(input.text.Length == 0) return;
        
        newMatrix = _transform.localToWorldMatrix;
        newMatrix *= makeRotationX(float.Parse(input.text));

        _transform.rotation = Quaternion.LookRotation(newMatrix.GetColumn(2), newMatrix.GetColumn(1));
    }
    
    // apply y rotation input
    public void ApplyRotationY(TMP_InputField input)
    {
        if(input.text.Length == 0) return;
        
        newMatrix = _transform.localToWorldMatrix;
        newMatrix *= makeRotationY(float.Parse(input.text));

        _transform.rotation = Quaternion.LookRotation(newMatrix.GetColumn(2), newMatrix.GetColumn(1));
    }
    
    // apply z rotation input
    public void ApplyRotationZ(TMP_InputField input)
    {
        if(input.text.Length == 0) return;
        
        newMatrix = _transform.localToWorldMatrix;
        newMatrix *= makeRotationZ(float.Parse(input.text));

        _transform.rotation = Quaternion.LookRotation(newMatrix.GetColumn(2), newMatrix.GetColumn(1));
    }
    
    // apply translation input
    public void ApplyTranslation()
    {
        newMatrix = _transform.localToWorldMatrix;
        newMatrix *= Translate(translationVector);

        _transform.position = newMatrix.GetPosition();
    }
    
    #endregion
    
    #region makeScaleTransformation

    // scale transformation
        // | sx  0   0   0 |
        // | 0   sy  0   0 |
        // | 0   0   sz  0 |
        // | 0   0   0   1 |
    private Matrix4x4 Scale(Vector3 scaleVec)
    {
        Matrix4x4 m = Matrix4x4.identity;

        m.m00 = scaleVec.x;
        m.m11 = scaleVec.y;
        m.m22 = scaleVec.z;

        return m;
    }
    
    public void SetXScale(TMP_InputField info)
    {
        if (info.text.Length == 0)
            scaleVector.x = 1;
        else
            scaleVector.x = float.Parse(info.text);
    }

    public void SetYScale(TMP_InputField info)
    {

        if (info.text.Length == 0)
            scaleVector.y = 1;
        else
            scaleVector.y = float.Parse(info.text);
    }

    public void SetZScale(TMP_InputField info)
    {
        if (info.text.Length == 0)
            scaleVector.z = 1;
        else
            scaleVector.z = float.Parse(info.text);
    }
    
    #endregion
    
    #region MakeRotationTransformation

    // here is how we rotate a matrix on the x-axis
    // | 1    0           0           0 |
    // | 0    cos(theta)    -sin(theta)   0 |
    // | 0    sin(theta)    cos(theta)    0 |
    // | 0    0           0           1 |
    // WHEN ROTATING YOU MUST APPLY A SPECIFIC ORDER
    // apply the x then y then z
    // if you don't apply rotations in this order, you will get different results than you think

    private Matrix4x4 makeRotationX(float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float s = Mathf.Sin(radians);
        float c = Mathf.Cos(radians);
        Matrix4x4 matrix = Matrix4x4.identity;

        matrix.m11 = c;
        matrix.m12 = -s;
        matrix.m21 = s;
        matrix.m22 = c;

        return matrix;
    }

    // here is how we rotate a matrix on the y-axis
    // | cos(theta)   0   sin(theta)   0 |
    // | 0            1   0              0 |
    // | -sin(theta)  0   cos(theta)   0 |
    // | 0            0   0              1 |
    private Matrix4x4 makeRotationY(float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float s = Mathf.Sin(radians);
        float c = Mathf.Cos(radians);
        Matrix4x4 matrix = Matrix4x4.identity;

        matrix.m00 = c;
        matrix.m02 = s;
        matrix.m20 = -s;
        matrix.m22 = c;

        return matrix;
    }

    // here is how we rotate a matrix on the z-axis
    // | cos(theta) -sin(theta)  0   0 |
        // | sin(theta)  cos(theta)  0   0 |
        // | 0           0           1   0 |
        // | 0           0           0   1 |
    private Matrix4x4 makeRotationZ(float degrees)
    {
            float s = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float c = Mathf.Cos(degrees * Mathf.Deg2Rad);
            Matrix4x4 matrix = Matrix4x4.identity;

            matrix.m00 = c;
            matrix.m01 = -s;
            matrix.m10 = s;
            matrix.m12 = c;

            return matrix;
    }

    #endregion
    
    #region makeTranslationTransformation

    // translation for a matrix4x4
    // where {tx, ty, tz} are our elements to change in our matrix4x4
    // | 1  0  0  tx |
    // | 0  1  0  ty |
    // | 0  0  1  tz |
    // | 0  0  0  1  |

    // we perform translation this way because m03, m13, m23 are the x y z coordinates in world space
    // and m33 represents a scaling factor

    // translation transformation
    private Matrix4x4 Translate(Vector3 direction)
    {
        Matrix4x4 m = Matrix4x4.identity;

        m.m03 = direction.x;
        m.m13 = direction.y;
        m.m23 = direction.z;

        // Debug.Log(newMatrix.MultiplyPoint3x4(Vector3.zero));
        // _transform.position = newMatrix.MultiplyPoint3x4(Vector3.zero);
        return m;
    }
    
    public void SetXPosition(TMP_InputField info)
    {
        if (info.text.Length == 0)
            translationVector.x = 0;
        else
            translationVector.x = float.Parse(info.text);
    }

    public void SetYPosition(TMP_InputField info)
    {

        if (info.text.Length == 0)
            translationVector.y = 0;
        else
            translationVector.y = float.Parse(info.text);
    }

    public void SetZPosition(TMP_InputField info)
    {

        if (info.text.Length == 0)
            translationVector.z = 0;
        else
            translationVector.z = float.Parse(info.text);
    }
    
    #endregion

}
