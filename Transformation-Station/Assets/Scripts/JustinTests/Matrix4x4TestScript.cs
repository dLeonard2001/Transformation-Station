using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix4x4TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    
    // mesh filter of gameObject
    private MeshFilter boxMesh;
    private Vector3[] oldVerts;
    private Vector3[] newVerts;
    
    // Vectors to multiply by the matrix
    public Vector3 translation;
    public Vector3 anglesTest;
    public Vector3 scale;
    
    public Vector3 eulerAngles;
    
    // test variables
    public float moveTest;
    public float rotationTest;
    void Start()
    {
        moveTest = 0;
        boxMesh = GetComponent<MeshFilter>();
        oldVerts = boxMesh.mesh.vertices;
        newVerts = new Vector3[oldVerts.Length];
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rotation = Quaternion.Euler(anglesTest.x, anglesTest.y, anglesTest.z);
        
        Matrix4x4 boxMatrix = Matrix4x4.TRS(translation,rotation,Vector3.one);

        int i = 0;
        
        while (i < oldVerts.Length) {
            newVerts[i] = boxMatrix.MultiplyPoint3x4(oldVerts[i]);
            i++;
        }
        boxMesh.mesh.vertices = newVerts;
        
        //movetest += Time.deltaTime;
        
        
    }
}
