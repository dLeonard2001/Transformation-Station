using UnityEngine;

public class ModifyEmission : MonoBehaviour
{
    public Color startColor;
    public Color endColor;
    public float emissionSpeed = 1f;

    private Renderer myRenderer;

    private void Start()
    {
        myRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        // Calculate the emission color based on the PingPong value
        float pingPong = Mathf.PingPong(Time.time * emissionSpeed, 1f);
        Color emissionColor = Color.Lerp(startColor, endColor, pingPong);

        // Get the current material
        Material mat = myRenderer.material;

        // Set the emission color of the material
        mat.SetColor("_EmissionColor", emissionColor);

        // Enable emission
        mat.EnableKeyword("_EMISSION");
    }
}
