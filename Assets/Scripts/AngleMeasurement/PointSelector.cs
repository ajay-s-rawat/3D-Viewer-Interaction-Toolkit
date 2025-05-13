using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PointSelector : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject pointPreviewPrefab;

    private readonly List<GameObject> pointInstances = new();
    private GameObject currentPointInstance;

    public event Action OnThirdPointUpdated;
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        // Initialize Input Actions from your Input Action Asset
        inputActions = new InputSystem_Actions();

        // Bind the PointSelect action to a method
        inputActions.UI.Click.performed += ctx => SelectPoint(); // This binds the action to SelectPoint method
    }
    private void OnEnable()
    {
        // Enable the UI action map
        inputActions.UI.Enable();
        // Ensure the specific action is enabled
        inputActions.UI.Click.Enable();
    }

    private void OnDisable()
    {
        // Disable the specific action when you no longer need it
        inputActions.UI.Click.Disable();
    }

    public void SelectPoint()
    {
        if (mainCamera == null)
        {
            Debug.LogWarning("Main Camera is not assigned.");
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (currentPointInstance == null)
            {
                currentPointInstance = Instantiate(pointPreviewPrefab, hit.point, Quaternion.identity);
                pointInstances.Add(currentPointInstance);
            }
            else
            {
                currentPointInstance.transform.position = hit.point;
                if (pointInstances.Count == 3) OnThirdPointUpdated?.Invoke();
            }
        }
    }

    public bool TryGetPoint(out Vector3 hitPoint)
    {
        hitPoint = Vector3.zero;

        if (currentPointInstance == null)
            return false;

        hitPoint = currentPointInstance.transform.position;

        // Clear after use if you're following single-use pattern
        if(pointInstances.Count < 3) currentPointInstance = null;

        return true;
    }

    public void ClearAllPoints()
    {
        foreach (var point in pointInstances)
        {
            if (point != null)
                Destroy(point);
        }

        pointInstances.Clear();
        currentPointInstance = null;
    }

    // Method to disable PointSelect action explicitly when needed
    public void DisablePointSelectAction()
    {
        inputActions.UI.Click.Disable();
    }

    // Method to enable PointSelect action explicitly when needed
    public void EnablePointSelectAction()
    {
        inputActions.UI.Click.Enable();
    }
}
