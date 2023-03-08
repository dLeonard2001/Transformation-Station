using UnityEngine;

public class DisableUI : MonoBehaviour
{
    private void Start()
    {
        DisableOnClickOutside disableOnClickOutside = FindObjectOfType<DisableOnClickOutside>();
        if (disableOnClickOutside != null)
        {
            disableOnClickOutside.onDisable.AddListener(Disable);
        }
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}