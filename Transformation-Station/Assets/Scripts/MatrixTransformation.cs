using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Vector3 = UnityEngine.Vector3;

public class MatrixTransformation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _transform;
    [SerializeField] private Transform targetTransform;
    private List<Matrix4x4> currentTransformations;
    
    private Vector3 translationVector;
    private Vector3 rotationVector;
    private Vector3 scaleVector;
    private Vector3 originalScale;

    // Overview of a matrix4x4
        // |  x  y  z  t | 
        // |  1  0  0  0 |
        // |  0  1  0  0 |
        // |  0  0  1  0 |
        // |  0  0  0  1 |
        
    // initialize and cache some data for later
    void Start()
    {
        _transform = GetComponent<Transform>();

        currentTransformations = new List<Matrix4x4>();

        scaleVector = Vector3.one;
        originalScale = _transform.localScale;
    }

    // apply all the input transformations into here in an empty matrix
    public void ApplyTransformations()
    {
        Debug.Log(getSize());
          
        
        // could just initialize it to the identity matrix
        //Matrix4x4 m = currentTransformations.Dequeue();
        
        // apply matrices from right to left
        
        // R * R * T
        // first translate
        // then rotate
        // then rotate again
        //int max = newMatrices.Count;
        //for (int i = 0; i < max; i++)
        //{
            //m = newMatrices.Dequeue() * m;
        //}

        //_transform.Rotate(m.rotation.eulerAngles, Space.World);
        //_transform.Translate(m.GetPosition(), Space.World);
    }

    public void AddMatrix()
    {
        currentTransformations.Insert(0, Matrix4x4.identity);
    }

    public int getSize()
    {
        return currentTransformations.Count;
    }

    public void Reset()
    {
        _transform.position = Matrix4x4.identity.GetPosition();
        _transform.rotation = Matrix4x4.identity.rotation;
        _transform.localScale = Matrix4x4.identity.lossyScale;
    }

    #region makeScaleTransformation

    // scale transformation
        // | sx  0   0   0 |
        // | 0   sy  0   0 |
        // | 0   0   sz  0 |
        // | 0   0   0   1 |
    public void NewScaleMatrix()
    {
        if (scaleVector == Vector3.one) return;
        
        
        Matrix4x4 m = Matrix4x4.identity;

        m.m00 = scaleVector.x;
        m.m11 = scaleVector.y;
        m.m22 = scaleVector.z;

        //newMatrices.Enqueue(m);
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

    public void makeRotationX(TMP_InputField degrees)
    {
        if (degrees.text.Length == 0) return;
        
        
        
        float radians = float.Parse(degrees.text) * Mathf.Deg2Rad;
        float s = Mathf.Sin(radians);
        float c = Mathf.Cos(radians);
        Matrix4x4 matrix = Matrix4x4.identity;

        matrix.m11 = c;
        matrix.m12 = -s;
        matrix.m21 = s;
        matrix.m22 = c;

        //newMatrices.Enqueue(matrix);
    }
    
    // here is how we rotate a matrix on the y-axis
    // | cos(theta)   0   sin(theta)   0 |
    // | 0            1   0              0 |
    // | -sin(theta)  0   cos(theta)   0 |
    // | 0            0   0              1 |
    public void makeRotationY(TMP_InputField degrees)
    {
        if (degrees.text.Length == 0) return;
        
        
        
        float radians = float.Parse(degrees.text) * Mathf.Deg2Rad;
        float s = Mathf.Sin(radians);
        float c = Mathf.Cos(radians);
        Matrix4x4 matrix = Matrix4x4.identity;

        matrix.m00 = c;
        matrix.m02 = s;
        matrix.m20 = -s;
        matrix.m22 = c;

        //newMatrices.Enqueue(matrix);
    }

    // here is how we rotate a matrix on the z-axis
        // | cos(theta) -sin(theta)  0   0 |
        // | sin(theta)  cos(theta)  0   0 |
        // | 0           0           1   0 |
        // | 0           0           0   1 |
    public void makeRotationZ(TMP_InputField degrees)
    {
        if(degrees.text.Length == 0) return;

          
        
        float s = Mathf.Sin(float.Parse(degrees.text) * Mathf.Deg2Rad);
        float c = Mathf.Cos(float.Parse(degrees.text) * Mathf.Deg2Rad);
        Matrix4x4 matrix = Matrix4x4.identity;

        matrix.m00 = c;
        matrix.m01 = -s;
        matrix.m10 = s;
        matrix.m12 = c;

        //newMatrices.Enqueue(matrix);
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
    private Matrix4x4 Translate(Vector3 t)
    {
        Matrix4x4 m = Matrix4x4.identity;

        m.m03 = t.x;
        m.m13 = t.y;
        m.m23 = t.z;

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
        
        Debug.Log("Edit has been changed");
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
