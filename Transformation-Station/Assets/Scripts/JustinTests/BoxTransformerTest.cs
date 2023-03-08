using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoxTransformerTest : MonoBehaviour
{
    // target to reach
	public GameObject target;

	// targets to reach to win the game
	private float targetX;
	private float targetZ;
	public float targetRotation;

	// Holds the input fields
    public GameObject[] inputField;

	// stores the types of transformations being performed
	private int[] transformTypes = new int[4];

	// stores the value from the input fields
	private float[] transformValues = new float[4];
	private float[] transformValuesOld = new float[4];

	// vectors for the transformation
	public Vector3 angles;
	private Vector3 translation;
	private Vector3 scale;

	// properties of the box being transformed
	private MeshFilter boxMesh;
    private Vector3[] oldVerts;
    private Vector3[] newVerts;
	private Vector3[] originalVerts;

	

    // Start is called before the first frame update
    void Start()
    {
		// set initial values to zero
		for (int i = 0; i < 4; i++)
		{
			transformTypes[i] = 0;
			transformValues[i] = 0;
			transformValuesOld[i] = 0;
		}

		// set initial values for mesh
		boxMesh = GetComponent<MeshFilter>();
        oldVerts = boxMesh.mesh.vertices;
		originalVerts = boxMesh.mesh.vertices;
        newVerts = new Vector3[oldVerts.Length];

		// set initial vectors to standard
		angles = Vector3.zero;
		translation = Vector3.zero;
		scale = Vector3.one;

		// set target values
		targetX = target.transform.position.x;
		targetZ = target.transform.position.z;
		targetRotation = target.transform.rotation.eulerAngles.y;
		Debug.Log(targetRotation);
    }

    // Update is called once per frame
    void Update()
    {
		angles = Vector3.zero;
		translation = Vector3.zero;
		scale = Vector3.one;

		oldVerts = originalVerts;

		// for each input box
		for (int i = 3; i >= 0; i--)
		{
			transformValues[i] = float.Parse(inputField[i].GetComponent<TMP_InputField>().text);

			//check the type of transfrom from transformTypes[i]
			/*
			0 = Translation(X)
			1 = Translation(Y)
			2 = Rotation
			3 = Scale(X)
			4 = Scale(Y)
			*/
			
			// translate X
			if (transformTypes[i] == 0)
			{
				translation.x += (transformValues[i] * Mathf.Cos(angles.y));
				translation.z += (transformValues[i] * Mathf.Sin(angles.y));
			}

			// translate "Y"
			else if (transformTypes[i] == 1)
			{
				translation.x += (transformValues[i] * Mathf.Sin(angles.y));
				translation.z += (transformValues[i] * Mathf.Cos(angles.y));
			}

			// rotate
			else if (transformTypes[i] == 2)
			{
				angles.y += transformValues[i];
			}

			// scale X
			else if (transformTypes[i] == 3)
			{
				scale.x += transformValues[i];
			}

			// Scale "Y"
			else if (transformTypes[i] == 4)
			{
				scale.z += transformValues[i];
			}
			
			// have the current and old values match
			if (transformValues[i] != transformValuesOld[i])
			{
				transformValuesOld[i] += transformValues[i];
			}
		}
		// check targets
		/*Debug.Log(targetZ + " | " + translation.z);
		Debug.Log(targetX + " | " + translation.x);
		Debug.Log(targetRotation + " | " + angles.y);*/
		if (targetX == translation.x && targetZ == translation.z && targetRotation == angles.y)
		{
			win();
		}

		// get quaternion for matrix4x4TRS
		Quaternion rotation = Quaternion.Euler(angles.x, angles.y, angles.z);

		// perform the matrix4x4TRS
		Matrix4x4 boxMatrix = Matrix4x4.TRS(translation,rotation,scale);

		// plug in the new vertices for the transformation
		for (int j = 0; j < oldVerts.Length; j++)
		{
			newVerts[j] = boxMatrix.MultiplyPoint3x4(oldVerts[j]);
		}
		// apply the new vertices
       	boxMesh.mesh.vertices = newVerts;
    }
    
	// gets the value from the four seperate dropdownmenus
    public void HandleDropdownData1(int val)
    {
		transformTypes[0] = val;
    }
	
	public void HandleDropdownData2(int val)
    {
        transformTypes[1] = val;
    }

	public void HandleDropdownData3(int val)
    {
        transformTypes[2] = val;
    }

	public void HandleDropdownData4(int val)
    {
        transformTypes[3] = val;
    }

	public void Reset()
	{
		// reset to base values
		angles = Vector3.zero;
		translation = Vector3.zero;
		scale = Vector3.one;

		// get quaternion for matrix4x4TRS
		Quaternion rotation = Quaternion.Euler(angles.x, angles.y, angles.z);

		// perform the matrix4x4TRS
		Matrix4x4 boxMatrix = Matrix4x4.TRS(translation,rotation,scale);			

		// plug in the new vertices for the transformation
		for (int j = 0; j < oldVerts.Length; j++)
		{
			newVerts[j] = boxMatrix.MultiplyPoint3x4(originalVerts[j]);
		}
		// apply the new vertices
       	boxMesh.mesh.vertices = newVerts;
	}

	public void win()
	{
		Debug.Log("Winner!");
	}
}
