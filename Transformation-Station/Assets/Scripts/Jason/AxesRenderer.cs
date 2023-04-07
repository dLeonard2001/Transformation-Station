using UnityEngine;

public class AxesRenderer : MonoBehaviour
{
    public float lineThickness = 0.1f;
    public float axisLength = 1f;

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        
        lineRenderer.startWidth = lineRenderer.endWidth = lineThickness;
        
        lineRenderer.transform.localPosition = transform.position;
    }

    private void Update()
    {
        var position = transform.position;
        
        lineRenderer.SetPosition(0, position - (Vector3.right * axisLength / 2f));
        lineRenderer.SetPosition(1, position + (Vector3.right * axisLength / 2f));
        lineRenderer.SetPosition(2, position);

        lineRenderer.SetPosition(3, position - (Vector3.up * axisLength / 2f));
        lineRenderer.SetPosition(4, position + (Vector3.up * axisLength / 2f));
        lineRenderer.SetPosition(5, position);
        
        lineRenderer.SetPosition(6, position - (Vector3.forward * axisLength / 2f));
        lineRenderer.SetPosition(7, position + (Vector3.forward * axisLength / 2f));
        lineRenderer.SetPosition(8, position);
    }
}