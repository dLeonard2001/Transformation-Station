using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MatrixTransformation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _transform;
    private List<Matrix4x4> currentTransformations;
    private List<GameObject> currentCards;
    
    // keeps track of all transformations
    public Matrix4x4 totalTransformations;

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

        totalTransformations = Matrix4x4.identity;
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
        _transform.rotation = Quaternion.Euler(m.rotation.eulerAngles);
        _transform.position = m.GetPosition();
    }


    // edit any matrix at a certain index 
        // (Not very scalable/readability later on because this function will get messy the more transformations we 
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

    // add a matrix to the end of the list for this object
    public void AddMatrix()
    {
        currentTransformations.Add(Matrix4x4.identity);
    }
    
    public void DeleteMatrix()
    {
        currentTransformations.RemoveAt(currentTransformations.Count-1);
    }

    // remove a matrix at a certain index within the list
    public void RemoveMatrix(int index)
    {
        currentTransformations.RemoveAt(index);
        currentCards.RemoveAt(index);
    }
    
    // adds a card to object's current card
    public void AddCard(GameObject newCard)
    {
        currentCards.Add(newCard);
    }

    // returns the size of transformations
    public int GetSize()
    {
        return currentTransformations.Count;
    }
    
    // returns the current cards correlated with this object
    public List<GameObject> GetCurrentCards()
    {
        return currentCards;
    }

    // simple reset function (doesn't account for the origin)
    public void Reset()
    {
        _transform.position = Matrix4x4.identity.GetPosition();
        _transform.rotation = Matrix4x4.identity.rotation;
        _transform.localScale = Matrix4x4.identity.lossyScale;
        
        // reset the total
        totalTransformations = Matrix4x4.identity;
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

        // apply to totals
        totalTransformations[0, 0] *= m.m00;
        totalTransformations[1, 1] *= m.m11;
        totalTransformations[2, 2] *= m.m22;

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
        
        // apply to totals
        totalTransformations[1, 1] += matrix.m11;
        totalTransformations[1, 2] += matrix.m12;
        totalTransformations[2, 1] += matrix.m21;
        totalTransformations[2, 2] += matrix.m22;

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
        
        // apply to totals
        totalTransformations[0, 0] += matrix.m00;
        totalTransformations[0, 2] += matrix.m02;
        totalTransformations[2, 0] += matrix.m20;
        totalTransformations[2, 2] += matrix.m22;

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
        matrix.m11 = c;
        
        // apply to totals
        totalTransformations[0, 0] += matrix.m00;
        totalTransformations[0, 1] += matrix.m01;
        totalTransformations[1, 0] += matrix.m10;
        totalTransformations[1, 1] += matrix.m11;

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
        
        // apply to totals
        totalTransformations[0, 3] += m.m03;
        totalTransformations[1, 3] += m.m13;
        totalTransformations[2, 3] += m.m23;

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

    
    // move an object according to the mouse input, while the object is selected
    private void OnMouseDrag()
    {
        // must have a card selected to edit
            // also cannot have zero transformations
        if (UI_Manager.HasCardSelected() && GetSize() != 0)
        {
            // gets the current input
            float inputVal = DirectionalInput();
            
            // apply the current input
            UI_Manager.UpdateCardValue(Mathf.Clamp(inputVal, -1, 1));
        }
    }

    // get directional input depending on the type of transformation
    private float DirectionalInput()
    {
        float num = 0;

        switch (UI_Manager.TransformationType().Split(' ')[0])
        {
            case "Translate":
                num = UI_Manager.CardDirection() == 'X' ? Input.GetAxis("Mouse X") : Input.GetAxis("Mouse Y");
                break;
            case "Rotate":
                num = UI_Manager.CardDirection() == 'X' ? Input.GetAxis("Mouse Y") : Input.GetAxis("Mouse X");
                break;
            case "Scale":
                num = UI_Manager.CardDirection() == 'X' ? Input.GetAxis("Mouse X") : Input.GetAxis("Mouse Y");
                break;
        }

        return num;
    }

    /*private void AdjustTotal(Matrix4x4 m)
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                totalTransformations[i, j] += m[i, j];
            }
        }
        totalTransformations[3, 3] = 1;
    }*/
    

    public Matrix4x4 GetTotal()
    {
        return totalTransformations;
    }

    public void ResetTotal()
    {
        totalTransformations = Matrix4x4.identity;
    }
}
