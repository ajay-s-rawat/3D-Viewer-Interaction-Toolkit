using UnityEngine;
using UnityEngine.InputSystem;

public class ThreeDViewerController : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, -5f);

    [Header("Orbit Settings")]
    public float orbitSpeed = 0.1f;
    public float minVerticalAngle = -20f;
    public float maxVerticalAngle = 80f;

    [Header("Zoom Settings")]
    public float zoomSpeed = 1f;
    public float minZoomDistance = 2f;
    public float maxZoomDistance = 15f;

    private float currentYaw = 0f;
    private float currentPitch = 20f;
    private float currentDistance;

    private float defaultYaw, defaultPitch, defaultDistance;

    private bool isOrbiting = false;

    private InputSystem_Actions input;

    void Awake()
    {
        input = new InputSystem_Actions();

        input.Camera.Orbit.started += ctx => isOrbiting = true;
        input.Camera.Orbit.canceled += ctx => isOrbiting = false;

        input.Camera.Look.performed += ctx =>
        {
            if (!isOrbiting) return;

            Vector2 delta = ctx.ReadValue<Vector2>();
            currentYaw += delta.x * orbitSpeed;
            currentPitch -= delta.y * orbitSpeed;
            currentPitch = Mathf.Clamp(currentPitch, minVerticalAngle, maxVerticalAngle);
            UpdateCameraPosition();
        };

        input.Camera.Zoom.performed += ctx =>
        {
            float scrollY = ctx.ReadValue<Vector2>().y;
            currentDistance -= scrollY * zoomSpeed;
            currentDistance = Mathf.Clamp(currentDistance, minZoomDistance, maxZoomDistance);
            UpdateCameraPosition();
        };

        //input.Camera.ResetView.performed += ctx => ResetView();
    }

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("3DViewerController: No target assigned.");
            enabled = false;
            return;
        }

        currentDistance = offset.magnitude;

        // Store defaults
        defaultYaw = currentYaw;
        defaultPitch = currentPitch;
        defaultDistance = currentDistance;

        UpdateCameraPosition();
    }

    void OnEnable() => input.Enable();
    void OnDisable() => input.Disable();

    private void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
        Vector3 direction = rotation * Vector3.forward;
        transform.position = target.position - direction * currentDistance;
        transform.LookAt(target);
    }

    public void ResetView()
    {
        currentYaw = defaultYaw;
        currentPitch = defaultPitch;
        currentDistance = defaultDistance;
        UpdateCameraPosition();
    }
}
