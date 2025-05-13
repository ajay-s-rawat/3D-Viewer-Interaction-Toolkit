using UnityEngine;
using UnityEngine.UI;

public class ViewModeToggleController : MonoBehaviour
{
    [Header("Buttons")]
    public Button button3DView;
    public Button buttonMeasureAngle;

    [Header("UI Screens")]
    public GameObject screen3DView;
    public GameObject screenMeasureAngle;

    void Start()
    {
        // Hook up click events
        button3DView.onClick.AddListener(Activate3DView);
        buttonMeasureAngle.onClick.AddListener(ActivateAngleMeasure);

        // Set default state
        Activate3DView();
    }

    public void Activate3DView()
    {
        screen3DView.SetActive(true);
        screenMeasureAngle.SetActive(false);

        // Optionally highlight selected button or disable
        button3DView.interactable = false;
        buttonMeasureAngle.interactable = true;
    }

    public void ActivateAngleMeasure()
    {
        screen3DView.SetActive(false);
        screenMeasureAngle.SetActive(true);

        button3DView.interactable = true;
        buttonMeasureAngle.interactable = false;
    }

    void OnDestroy()
    {
        button3DView.onClick.RemoveListener(Activate3DView);
        buttonMeasureAngle.onClick.RemoveListener(ActivateAngleMeasure);
    }
}
