using UnityEngine;

public class AngleMeasurementManager : MonoBehaviour
{
    public PointSelector pointSelector;
    public AngleVisualizer angleVisualizer;
    public AngleMeasurementUI angleUI;

    private Vector3? pointA, pointB, pointC;

    public enum SelectionState { Idle, SelectVertex, SelectPoint1, SelectPoint2 }
    public SelectionState currentState = SelectionState.Idle;

    private void OnEnable()
    {
        pointSelector.OnThirdPointUpdated += HandlePointSelection;
        angleUI.OnSelectPointClicked += HandlePointSelection;
        angleUI.OnResetClicked += ResetMeasurement;
    }

    private void OnDisable()
    {
        pointSelector.OnThirdPointUpdated -= HandlePointSelection;
        angleUI.OnSelectPointClicked -= HandlePointSelection;
        angleUI.OnResetClicked -= ResetMeasurement;
    }

    public void ActivateTool()
    {
        ResetMeasurement();
        angleUI.gameObject.SetActive(true);
        //currentState = SelectionState.SelectVertex;
        //angleUI.SetPrompt("Select Vertex Point (Point B)");
    }

    private void HandlePointSelection()
    {
        Vector3 hitPoint;
        if (pointSelector.TryGetPoint(out hitPoint))
        {
            switch (currentState)
            {
                case SelectionState.SelectVertex:
                    pointB = hitPoint;
                    currentState = SelectionState.SelectPoint1;
                    angleUI.SetPrompt("Select First Terminal Point (Point A)");
                    break;
                case SelectionState.SelectPoint1:
                    pointA = hitPoint;
                    currentState = SelectionState.SelectPoint2;
                    angleUI.SetPrompt("Select Second Terminal Point (Point C)");
                    break;
                case SelectionState.SelectPoint2:
                    pointC = hitPoint;
                    FinalizeSelection();
                    break;
            }
        }
        else
        {
            Debug.Log("TryGetPoint");
        }
    }

    private void FinalizeSelection()
    {
        if (pointA.HasValue && pointB.HasValue && pointC.HasValue)
        {
            angleVisualizer.DisplayAngle(pointA.Value, pointB.Value, pointC.Value);
            angleUI.SetPrompt("");
        }
    }

    public void ResetMeasurement()
    {
        pointA = pointB = pointC = null;
        currentState = SelectionState.SelectVertex;
        angleVisualizer.Clear();
        angleUI.ResetUI();
        pointSelector.ClearAllPoints();
        angleUI.SetPrompt("Select Vertex Point (Point B)");
    }
}
