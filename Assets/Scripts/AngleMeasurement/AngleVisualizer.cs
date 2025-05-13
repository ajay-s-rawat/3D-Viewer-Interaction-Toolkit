using UnityEngine;
using TMPro;

public class AngleVisualizer : MonoBehaviour
{
    public LineRenderer lineAB;
    public LineRenderer lineCB;
    public ArcMeshGenerator arcMeshGen;
    public TextMeshProUGUI angleText;

    public void DisplayAngle(Vector3 A, Vector3 B, Vector3 C)
    {
        Vector3 dirAB = (A - B).normalized;
        Vector3 dirCB = (C - B).normalized;

        lineAB.positionCount = 2;
        lineCB.positionCount = 2;
        lineAB.SetPositions(new Vector3[] { B, A });
        lineCB.SetPositions(new Vector3[] { B, C });

        float angle = Vector3.Angle(dirAB, dirCB);
        arcMeshGen.GenerateArc(B, dirAB, dirCB);

        angleText.text = $"{angle:F1}°";
        //angleText.transform.position = B + (dirAB + dirCB).normalized * 0.2f;
    }

    public void Clear()
    {
        lineAB.positionCount = 0;
        lineCB.positionCount = 0;
        arcMeshGen.ClearArc();
        angleText.text = "";
    }
}
