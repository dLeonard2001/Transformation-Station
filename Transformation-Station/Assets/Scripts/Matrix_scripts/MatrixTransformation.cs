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

    private Matrix4x4 origin;
    
    // keeps track of all transformations
    public Matrix4x4 totalTransformations;

	// keeps track of all subvalues
	public Matrix4x4 subtotalTransformations;

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
		    subtotalTransformations = Matrix4x4.identity;

        origin = transform.localToWorldMatrix;
    }

    // apply all the input transformations into here in an empty matrix
    public void ApplyTransformations(int index)
    {
        if (GetSize() == 0)
            return;
        
        // first matrix in sequence
        Matrix4x4 m = origin;

        // multiply all matrices into one matrix, right to left style
        for (int i = 0; i < index + 1; i++)
        {
            // second transformation * first transformation
            m = currentTransformations[i] * m;
        }

        // set total values
        totalTransformations = m;

        _transform.localScale = GetMatrixScale(m);
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
            case "Scale X":
                currentTransformations[index] = ScaleX(num);
                break;
            case "Scale Y":
                currentTransformations[index] = ScaleY(num);
                break;
            case "Scale Z":
                currentTransformations[index] = ScaleZ(num);
                break;
        }
    }

    // add a matrix to the list of matrices
    public void AddMatrix()
    {
        currentTransformations.Add(Matrix4x4.identity);
    }

    public void DeleteMatrix()
    {
        currentTransformations.RemoveAt(GetSize() - 1);
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
        _transform.position = origin.GetPosition();
        _transform.rotation = origin.rotation;
        _transform.localScale = GetMatrixScale(origin);
        
        // reset the total
        totalTransformations = Matrix4x4.identity;
    }

    #region makeScaleTransformation

    private Vector3 GetMatrixScale(Matrix4x4 m)
    {
        return m.lossyScale;
    }

    // scale transformation
        // | sx  0   0   0 |
        // | 0   sy  0   0 |
        // | 0   0   sz  0 |
        // | 0   0   0   1 |
    private Matrix4x4 ScaleX(float num)
    {
        Matrix4x4 m = Matrix4x4.identity;

        m.m00 = num == 0 ? 1 : num;

        return m;
    }
    
    private Matrix4x4 ScaleY(float num)
    {
        Matrix4x4 m = Matrix4x4.identity;
        
        m.m11 = num == 0 ? 1 : num;

        return m;
    }
    
    private Matrix4x4 ScaleZ(float num)
    {
        Matrix4x4 m = Matrix4x4.identity;
        
        m.m22 = num == 0 ? 1 : num;

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
        matrix.m11 = c;

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

    
    // move an object according to the mouse input, while the object is selected
    private void OnMouseDrag()
    {
        // must have a card selected to edit
            // also cannot have zero transformations
        if (UI_Manager.HasCardSelected() && GetSize() != 0)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            // gets the current input
            float inputVal = DirectionalInput();
            
            // apply the current input
            UI_Manager.UpdateCardValue(Mathf.Clamp(inputVal, -1, 1));
        }
    }

    private void OnMouseUp()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
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

    public Matrix4x4 GetTotal()
    {
        //return totalTransformations;
		Matrix4x4 m = Matrix4x4.identity;

		for (int i = 0; i < currentCards.Count; i++)
        {
            // second transformation * first transformation
            m = currentTransformations[i] * m;
        }

		return m;
    }

    public void ResetTotal()
    {
        totalTransformations = Matrix4x4.identity;
    }

	public Matrix4x4 GetSubtotal(int cardNumber)
	{
		Matrix4x4 m = Matrix4x4.identity;

        // multiply all matrices into one matrix, right to left style
        for (int i = 0; i < cardNumber + 1; i++)
        {
            // second transformation * first transformation
            m = currentTransformations[i] * m;
        }

		return m;
	}
}
