using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A free 360° orbit camera that supports zoom and reset,
/// using Unity's Input System with clean separation of input and logic.
/// </summary>
public class ThreeDViewerController : MonoBehaviour
{
    #region === Serialized Fields ===

    [Header("Target Settings")]
    [Tooltip("The target the camera will orbit around.")]
    public Transform target;

    [Header("Default View Settings")]
    [Tooltip("Default distance from the target.")]
    public float defaultDistance = 5f;

    [Header("Orbit Settings")]
    [Tooltip("Speed multiplier for orbiting via mouse movement.")]
    public float orbitSpeed = 0.2f;

    [Header("Zoom Settings")]
    [Tooltip("Speed multiplier for scroll wheel zooming.")]
    public float zoomSpeed = 1f;

    [Tooltip("Minimum distance to the target.")]
    public float minZoomDistance = 2f;

    [Tooltip("Maximum distance to the target.")]
    public float maxZoomDistance = 15f;

    [Header("Pan Settings")]
    [Tooltip("Speed multiplier for panning the view.")]
    public float panSpeed = 0.01f;


    #endregion

    #region === Private Fields ===

    private float yaw = 0f;       // Horizontal rotation (left/right)
    private float pitch = 0f;     // Vertical rotation (up/down)
    private float distance;       // Current zoom distance

    private float defaultYaw;
    private float defaultPitch;
    private float defaultZoom;

    private bool isOrbiting = false;
    private bool orbitInputEnabled = true;

    private bool isPanning = false;
    Vector3 panOffset = Vector3.zero;
    private bool panInputEnabled = true;

    private InputSystem_Actions input;

    #endregion

    #region === Unity Events ===

    private void Awake()
    {
        InitializeInput();
    }

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("ThreeDViewerController: No target assigned.");
            enabled = false;
            return;
        }

        // Set initial zoom and view angles
        distance = defaultDistance;
        defaultYaw = yaw;
        defaultPitch = pitch;
        defaultZoom = distance;

        OrbitPosition();
    }

    private void OnEnable() => input?.Enable();
    private void OnDisable() => input?.Disable();

    #endregion

    #region === Input Setup ===

    /// <summary>
    /// Initializes the Unity Input System bindings.
    /// </summary>
    private void InitializeInput()
    {
        input = new InputSystem_Actions();

        input.Camera.Orbit.started += _ => isOrbiting = true;
        input.Camera.Orbit.canceled += _ => isOrbiting = false;

        input.Camera.Look.performed += ctx =>
        {
            if (!isOrbiting || !orbitInputEnabled) return;

            Vector2 delta = ctx.ReadValue<Vector2>();
            if(delta.magnitude < 1) return;

            yaw += delta.x * orbitSpeed;
            pitch -= delta.y * orbitSpeed;

            // Optional: limit pitch slightly to avoid gimbal flip
            //pitch = Mathf.Clamp(pitch, -89f, 89f);

            OrbitPosition();
        };

        input.Camera.Zoom.performed += ctx =>
        {
            float scrollY = ctx.ReadValue<Vector2>().y;
            distance -= scrollY * zoomSpeed;
            distance = Mathf.Clamp(distance, minZoomDistance, maxZoomDistance);
            OrbitPosition();
        };

        input.Camera.Pan.started += _ => { if (panInputEnabled) isPanning = true; };
        input.Camera.Pan.canceled += _ => isPanning = false;

        input.Camera.Look.performed += ctx =>
        {
            if (!isPanning || !panInputEnabled) return;

            Vector2 delta = ctx.ReadValue<Vector2>();
            Vector3 right = transform.right;
            Vector3 up = transform.up;

            // Invert Y to match typical camera panning behavior
            Vector3 move = (-right * delta.x + -up * delta.y) * panSpeed;

            
            panOffset += move;

            MovePosition(panOffset); // Apply panOffset to the camera
        };
    }

    #endregion

    #region === Camera Logic ===

    /// <summary>
    /// Updates the camera's position and rotation based on current yaw, pitch, and zoom distance.
    /// </summary>
    private void OrbitPosition()
    {
        // Create a rotation from yaw and pitch
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Determine direction vector based on rotation
        Vector3 direction = rotation * Vector3.forward;

        // Place camera at correct position around target
        transform.position = target.position - direction * distance;

        // Align the camera to look at the target
        transform.rotation = rotation;
    }

    private void MovePosition(Vector3 panOffset)
    {
        Vector3 direction = transform.forward;
        transform.position = target.position - direction * distance + panOffset;
    }

    /// <summary>
    /// Resets the orbit camera to its default view.
    /// </summary>
    public void ResetView()
    {
        yaw = defaultYaw;
        pitch = defaultPitch;
        distance = defaultZoom;
        OrbitPosition();
    }

    public void EnableOrbit(bool enable)
    {
        orbitInputEnabled = enable;
        isOrbiting = false; // Ensure orbit immediately stops
    }

    public void EnablePan(bool enable)
    {
        panInputEnabled = enable;
        isPanning = false;  // Ensure panning immediately stops
        panOffset = Vector3.zero;
    }

    #endregion
}
