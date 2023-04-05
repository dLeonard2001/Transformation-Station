using UnityEngine;

public class ObjectChecker : MonoBehaviour
{
    public Transform targetObject;

    public float positionThreshold = 0.2f;
    public float rotationThreshold = 0.2f;

    private void Update()
    {
        float positionDistance = Vector3.Distance(transform.position, targetObject.position);
        float rotationDistance = Quaternion.Angle(transform.rotation, targetObject.rotation);

        if (positionDistance <= positionThreshold && rotationDistance <= rotationThreshold)
        {
            Debug.Log("Object is in the same position and rotation as the target object!");
        }
    }
}