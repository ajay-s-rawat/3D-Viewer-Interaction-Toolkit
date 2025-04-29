using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class AngleMeasurementTool : MonoBehaviour
{
    [Header("Settings")]
    public GameObject pointMarkerPrefab; // small sphere marker
    public LineRenderer lineRendererPrefab; // prefab with LineRenderer attached
    public TextMeshProUGUI angleText;

    private List<GameObject> selectedMarkers = new List<GameObject>();
    private List<LineRenderer> lines = new List<LineRenderer>();

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        angleText.text = "";
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TrySelectPoint();
        }
    }

    void TrySelectPoint()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (selectedMarkers.Count >= 3)
                return;

            GameObject marker = Instantiate(pointMarkerPrefab, hit.point, Quaternion.identity);
            selectedMarkers.Add(marker);

            if (selectedMarkers.Count == 3)
            {
                DrawLines();
                CalculateAndDisplayAngle();
            }
        }
    }

    void DrawLines()
    {
        // Line from Point B to A
        LineRenderer line1 = Instantiate(lineRendererPrefab);
        line1.positionCount = 2;
        line1.SetPosition(0, selectedMarkers[1].transform.position);
        line1.SetPosition(1, selectedMarkers[0].transform.position);
        lines.Add(line1);

        // Line from Point B to C
        LineRenderer line2 = Instantiate(lineRendererPrefab);
        line2.positionCount = 2;
        line2.SetPosition(0, selectedMarkers[1].transform.position);
        line2.SetPosition(1, selectedMarkers[2].transform.position);
        lines.Add(line2);
    }

    void CalculateAndDisplayAngle()
    {
        Vector3 a = selectedMarkers[0].transform.position;
        Vector3 b = selectedMarkers[1].transform.position;
        Vector3 c = selectedMarkers[2].transform.position;

        Vector3 dir1 = (a - b).normalized;
        Vector3 dir2 = (c - b).normalized;

        float angle = Vector3.Angle(dir1, dir2);

        angleText.text = angle.ToString("F1") + "°";

        // TODO: Call function to draw arc between dir1 and dir2
    }

    public void ClearSelection()
    {
        foreach (var marker in selectedMarkers)
        {
            Destroy(marker);
        }
        selectedMarkers.Clear();

        foreach (var line in lines)
        {
            Destroy(line.gameObject);
        }
        lines.Clear();

        angleText.text = "";
        // TODO: Clear arc drawing as well
    }
}
