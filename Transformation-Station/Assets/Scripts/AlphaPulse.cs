using UnityEngine;
using UnityEngine.UI;

public class AlphaPulse : MonoBehaviour
{
    public float speed = 1f; // The speed of the pulse effect
    public float minAlpha = 100f; // The minimum alpha value
    public float maxAlpha = 200f; // The maximum alpha value

    private Image image; // Reference to the Image component
    private float currentAlpha; // The current alpha value

    private void Start()
    {
        image = GetComponent<Image>(); // Get the Image component
        currentAlpha = maxAlpha; // Start with the maximum alpha value
    }

    private void Update()
    {
        currentAlpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(Time.time * speed, 1f));
        image.color = new Color(image.color.r, image.color.g, image.color.b, currentAlpha / 255f);
    }
}