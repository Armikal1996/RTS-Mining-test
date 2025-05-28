using UnityEngine;
using UnityEngine.UIElements;

public class GameplayUIHandler : MonoBehaviour
{
    public BaseController redBase;
    public BaseController blueBase;

    private SliderInt droneSpeedSlider;
    private SliderInt droneCountSlider;
    private Label redStrengthLabel;
    private Label blueStrengthLabel;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        redStrengthLabel = root.Q<Label>("RedStrengthLabel");
        blueStrengthLabel = root.Q<Label>("BlueStrengthLabel");

        droneSpeedSlider = root.Q<SliderInt>("DronesSpeedSlider");
        droneCountSlider = root.Q<SliderInt>("NumberofDronesSlider");

        // Immediately apply the slider values to bases
        ApplySettingsToBases();

        // Listen for slider changes
        droneSpeedSlider.RegisterValueChangedCallback(evt => ApplySettingsToBases());
        droneCountSlider.RegisterValueChangedCallback(evt => ApplySettingsToBases());
    }

    private void ApplySettingsToBases()
    {
        int speed = droneSpeedSlider.value;
        int droneCount = droneCountSlider.value;

        redBase.SetDroneSettings(droneCount, speed);
        blueBase.SetDroneSettings(droneCount, speed);
    }

    public void UpdateBaseStrength()
    {
        redStrengthLabel.text = "Red Score: " + redBase.strength;
        blueStrengthLabel.text = "Blue Score: " + blueBase.strength;
    }
}
