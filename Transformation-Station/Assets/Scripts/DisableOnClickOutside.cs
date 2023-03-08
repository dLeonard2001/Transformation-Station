using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DisableOnClickOutside : MonoBehaviour
{
    public UnityEvent onDisable;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            RaycastHit hitInfo;
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit) return;

            onDisable.Invoke();
        }
    }
}