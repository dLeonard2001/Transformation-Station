using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
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
        // Debug.Log(getSize());

        Matrix4x4 m = Matrix4x4.identity;


        for (int i = getSize() - 1; i >= 0; i--)
        {
            m = currentTransformations[i] * m;
        }

        _transform.Rotate(m.rotation.eulerAngles, Space.World);
        _transform.Translate(m.GetPosition(), Space.World);
    }

    public void EditMatrix(Vector3 vec, string transformation, int index)
    {
        switch (transformation)
        {
            case "Translate":
                currentTransformations[index] = Translate(vec);
                break;
            case "Rotate":
                
                if (vec.x == 0 && vec.z == 0)
                {
                    currentTransformations[index] = makeRotationY(vec.y);
                }else if (vec.x == 0 && vec.y == 0)
                {
                    currentTransformations[index] = makeRotationZ(vec.z);
                }else if (vec.y == 0 && vec.z == 0)
                {
                    currentTransformations[index] = makeRotationX(vec.x);
                }
                break;
            case "Scale":
                Debug.Log("scale edit");
                break;
        }
    }

    public void AddMatrix()
    {
        currentTransformations.Insert(0, Matrix4x4.identity);
    }
    
    public void DeleteMatrix()
    {
        currentTransformations.RemoveAt(currentTransformations.Count-1);
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

    public Matrix4x4 makeRotationX(float degrees)
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
    public Matrix4x4 makeRotationY(float degrees)
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
    public Matrix4x4 makeRotationZ(float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float s = Mathf.Sin(radians);
        float c = Mathf.Cos(radians);
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

    #endregion
    
}
