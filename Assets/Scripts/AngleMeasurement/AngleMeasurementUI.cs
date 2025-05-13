using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class AngleMeasurementUI : MonoBehaviour
{
    public TextMeshProUGUI promptText;
    public Button selectPointButton;
    public Button resetButton;

    public event Action OnSelectPointClicked;
    public event Action OnResetClicked;

    private void Awake()
    {
        selectPointButton.onClick.AddListener(() => OnSelectPointClicked?.Invoke());
        resetButton.onClick.AddListener(() => OnResetClicked?.Invoke());
    }

    public void SetPrompt(string text)
    {
        promptText.text = text;
    }

    public void ResetUI()
    {
        promptText.text = "Angle Tool Ready";
    }
}
