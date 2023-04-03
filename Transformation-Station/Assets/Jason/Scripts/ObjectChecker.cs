using UnityEngine;

public class ObjectChecker : MonoBehaviour
{
    public Transform targetObject; // the identical object the player needs to move and rotate to match

    public float positionThreshold = 0.2f; // how close the object needs to be in position to be considered a match
    public float rotationThreshold = 0.2f; // how close the object needs to be in rotation to be considered a match

    private void Update()
    {
        float positionDistance = Vector3.Distance(transform.position, targetObject.position);
        float rotationDistance = Quaternion.Angle(transform.rotation, targetObject.rotation);

        if (positionDistance <= positionThreshold && rotationDistance <= rotationThreshold)
        {
            Debug.Log("Object is in the same position and rotation as the target object!");
            // code to trigger success condition here
        }
    }
}