using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineCircle : MonoBehaviour
{
    public float orbitSpeed = 1.0f;
    public float orbitRadius = 5.0f;

    private LineRenderer lineRenderer;
    private Vector3 centerPosition;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        centerPosition = transform.parent.position;
    }

    private void Update()
    {
        // Calculate the position of the Orbit object in 3D space
        Vector3 orbitPosition = new Vector3(
            Mathf.Cos(Time.time * orbitSpeed) * orbitRadius,
            0,
            Mathf.Sin(Time.time * orbitSpeed) * orbitRadius
        );

        // Set the position of the Orbit object relative to the center position of the planet
        transform.position = centerPosition + orbitPosition;

        // Update the positions of the line renderer to draw a circle
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            float angle = Mathf.PI * 2 / lineRenderer.positionCount * i;
            Vector3 position = new Vector3(
                Mathf.Cos(angle) * orbitRadius,
                0,
                Mathf.Sin(angle) * orbitRadius
            );
            lineRenderer.SetPosition(i, position);
        }
    }
}