using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    public Camera targetCamera;
    public Transform modelRoot;
    public Button topCenterButton, centerButton, bottomCenterButton, leftCenterButton, rightCenterButton;

    public float panOffset = 2f; // how far to pan in each direction
    public float zoomOutMultiplier = 1.5f; // how far back camera should be from model

    private Bounds modelBounds;
    private Vector3 baseCameraPosition;
    private Quaternion baseCameraRotation;

    void Start()
    {
        CalculateModelBounds();

        // Store initial position and rotation
        baseCameraRotation = targetCamera.transform.rotation;
        baseCameraPosition = CalculateBaseCameraPosition();

        topCenterButton.onClick.AddListener(() => PanCamera(Vector3.up));
        bottomCenterButton.onClick.AddListener(() => PanCamera(Vector3.down));
        leftCenterButton.onClick.AddListener(() => PanCamera(Vector3.left));
        rightCenterButton.onClick.AddListener(() => PanCamera(Vector3.right));
        centerButton.onClick.AddListener(() => ResetCamera());
    }

    void CalculateModelBounds()
    {
        Renderer[] renderers = modelRoot.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return;

        modelBounds = renderers[0].bounds;
        foreach (Renderer r in renderers)
        {
            modelBounds.Encapsulate(r.bounds);
        }
    }

    Vector3 CalculateBaseCameraPosition()
    {
        Vector3 center = modelBounds.center;
        float modelSize = modelBounds.extents.magnitude;
        Vector3 camDir = -targetCamera.transform.forward.normalized;
        return center + camDir * modelSize * zoomOutMultiplier;
    }

    void PanCamera(Vector3 direction)
    {
        // Use base camera orientation for consistent movement
        Vector3 right = targetCamera.transform.right;
        Vector3 up = targetCamera.transform.up;

        Vector3 panOffsetVector = direction.x * right + direction.y * up;
        Vector3 newPosition = baseCameraPosition + panOffsetVector * panOffset;

        targetCamera.transform.position = newPosition;
        targetCamera.transform.rotation = baseCameraRotation; // lock rotation
    }

    void ResetCamera()
    {
        targetCamera.transform.position = baseCameraPosition;
        targetCamera.transform.rotation = baseCameraRotation;
    }
}
