using UnityEngine;
using UnityEngine.InputSystem;

public class PivotSetter : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform target;         // The pivot object (used by 3DViewController)
    [SerializeField] private GameObject pivotMarker;   // Gizmo to show the pivot

    [SerializeField] private ThreeDViewerController threeDViewerController;

    private InputSystem_Actions inputActions;

    public bool isPivotSetAllowed = true;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.UI.Click.performed += ctx => TrySetPivot();
        isPivotSetAllowed = true;

    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Click.Enable();
    }

    private void OnDisable()
    {
        inputActions.UI.Click.Disable();
    }

    private void TrySetPivot()
    {
        if (!isPivotSetAllowed) return;

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            pivotMarker.gameObject.SetActive(true);
            target.transform.position = hit.point;

            // Update pivot without visual jump
        }
        else
        {
            pivotMarker.gameObject.SetActive(false);
        }
    }

}
