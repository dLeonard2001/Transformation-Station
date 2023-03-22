using System;
using System.Collections.Generic;
using UnityEngine;

public class MatrixTransformation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _transform;
    private List<Matrix4x4> currentTransformations;
    private List<GameObject> currentCards;

    // Overview of a matrix4x4
        // |  Xv  Yv  Zv  Tv | 
        // |  1   0   0   0  |
        // |  0   1   0   0  |
        // |  0   0   1   0  |
        // |  0   0   0   1  |
        
    // initialize and cache some data for later
    void Start()
    {
        _transform = GetComponent<Transform>();

        currentTransformations = new List<Matrix4x4>();
        currentCards = new List<GameObject>();
    }

    // apply all the input transformations into here in an empty matrix
    public void ApplyTransformations(int index)
    {
        if (GetSize() == 0)
            return;
        
        // first matrix in sequence
        Matrix4x4 m = Matrix4x4.identity;
        
        // multiply all matrices into one matrix, right to left style
        for (int i = 0; i < index + 1; i++)
        {
            // second transformation * first transformation
            m = currentTransformations[i] * m;
        }

        _transform.localScale = m.lossyScale;
        _transform.Rotate(m.rotation.eulerAngles, Space.World);
        _transform.position = m.GetPosition();
    }

    public void ResetMatrices()
    {
        for (int i = 0; i < GetSize(); i++)
        {
            currentTransformations[i] = Matrix4x4.identity;
        }
    }

    public void EditMatrix(float num, string transformation, int index)
    {
        switch (transformation)
        {
            case "Translate X":
                currentTransformations[index] = TranslateX(num);
                break;
            case "Translate Y":
                currentTransformations[index] = TranslateY(num);
                break;
            case "Translate Z":
                currentTransformations[index] = TranslateZ(num);
                break;
            case "Rotate X":
                currentTransformations[index] = makeRotationX(num);
                break;
            case "Rotate Y":
                currentTransformations[index] = makeRotationY(num);
                break;
            case "Rotate Z":
                currentTransformations[index] = makeRotationZ(num);
                break;
            case "Scale":
                // currentTransformations[index] = NewScaleMatrix(num);
                break;
        }
    }

    public void AddMatrix()
    {
        currentTransformations.Add(Matrix4x4.identity);
    }

    public void RemoveMatrix(int index)
    {
        currentTransformations.RemoveAt(index);
        currentCards.RemoveAt(index);
    }
    
    public void AddCard(GameObject newCard)
    {
        currentCards.Add(newCard);
    }

    public int GetSize()
    {
        return currentTransformations.Count;
    }
    
    public List<GameObject> GetCurrentCards()
    {
        return currentCards;
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
    private Matrix4x4 NewScaleMatrix(Vector3 vec)
    {
        Matrix4x4 m = Matrix4x4.identity;

        m.m00 = vec.x == 0 ? 1 : vec.x;
        m.m11 = vec.y == 0 ? 1 : vec.y;
        m.m22 = vec.z == 0 ? 1 : vec.z;

        return m;
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
    private Matrix4x4 TranslateX(float num)
    {
        Matrix4x4 m = Matrix4x4.identity;

        m.m03 = num;
        // m.m13 = t.y;
        // m.m23 = t.z;

        return m;
    }
    private Matrix4x4 TranslateY(float num)
    {
        Matrix4x4 m = Matrix4x4.identity;
        
        m.m13 = num;
        
        return m;
    }
    private Matrix4x4 TranslateZ(float num)
    {
        Matrix4x4 m = Matrix4x4.identity;

        m.m23 = num;

        return m;
    }

    #endregion

    
    // How scripts are communicating/depending on one another to perform object movement
    // ChangeCard.cs >>> UI_Manager.cs >>> MatrixTransformation.cs
    private void OnMouseDrag()
    {
        /*
         * Finds the UI manager in the game which has the current transformation type
         * and uses it to verify what transformation are we applying to the object using the Matrix4x4 related functions
         */
        // if (FindObjectOfType<UI_Manager>().currentTransformationType.Equals("Translate"))
        // {
        //     // Debug.Log(Input.GetAxis("Mouse X"));
        //     currentTransformations.Add(Translate(new Vector3(Input.GetAxis("Mouse X") * 0.01f, transform.position.y, transform.position.z)));
        //     ApplyTransformations(GetSize());
        // }

        if (UI_Manager.HasCardSelected())
        {
            // float direction = Input.GetAxis("Mouse X") > 0
            
            UI_Manager.UpdateCardValue(Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1));
            
        }
    }
}
