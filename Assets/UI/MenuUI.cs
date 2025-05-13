using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [Header("Buttons")]
    public Button button3DView;
    public Button buttonMeasureAngle;

    [SerializeField] private AngleMeasurementManager angleMeasurementManager;
    [SerializeField] private ThreeDViewerController threeDViewerController;
    [SerializeField] private PivotSetter pivotSetter;


    private void Awake()
    {
        // Hook up click events
        button3DView.onClick.AddListener(OnClick3DViewButton);
        buttonMeasureAngle.onClick.AddListener(OnClickActivateAngleMeasure);
    }

    public void OnClick3DViewButton()
    {
        pivotSetter.isPivotSetAllowed = true;
        threeDViewerController.EnableOrbit(true);
        threeDViewerController.EnablePan(false);
        threeDViewerController.ResetView();
        angleMeasurementManager.gameObject.SetActive(false);
        angleMeasurementManager.ResetMeasurement();
    }

    public void OnClickActivateAngleMeasure()
    {
        pivotSetter.isPivotSetAllowed = false;
        threeDViewerController.EnableOrbit(false);
        threeDViewerController.EnablePan(true);
        threeDViewerController.ResetView();
        angleMeasurementManager.gameObject.SetActive(true);
        angleMeasurementManager.ActivateTool();
        
    }
     
}
