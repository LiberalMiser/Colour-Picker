using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColourPicker : MonoBehaviour {

    [SerializeField] private Image saturationImage;
    [SerializeField] private Material targetMaterial;
    [SerializeField] private string metallicKey = "_Metallic";
    [SerializeField] private string smoothnessKey = "_Glossiness";
    [SerializeField] private string colourKey = "_BaseColor";
    [SerializeField] private Slider hueSlider, valueLightnessSlider, saturationSlider, metallicSlider, smoothnessSlider;
    [SerializeField] private bool useHDR = true;

    private float hue, saturation, colourValue;

    private void Awake() {
        InitializeColourSliderPositions();
        InitializeSmoothnessSliderPosition();
        InitializeMetallicSliderPosition();
    }

    public void SetColour() {
        Color color = Color.HSVToRGB(hue, saturation, colourValue, useHDR);
        targetMaterial.SetColor(colourKey, color);
        saturationImage.color = color;
    }

    public void SetHue(float val) {
        hue = val;
    }

    public void SetSaturation(float val) {
        saturation = val;
    }

    public void SetColourValue(float val) {
        colourValue = val;
    }

    public void SetMetallicValue(float val) {
        targetMaterial.SetFloat(metallicKey, val);
    }

    public void SetSmoothnessValue(float val) {
        targetMaterial.SetFloat(smoothnessKey, val);
    }

    private void InitializeColourSliderPositions() {
        Color materialStartColour = targetMaterial.GetColor(colourKey);
        float h, s, v;

        Color.RGBToHSV(materialStartColour, out h, out s, out v);

        hueSlider.value = h;
        saturationSlider.value = s;
        valueLightnessSlider.value = v;
    }

    private void InitializeSmoothnessSliderPosition() {
        smoothnessSlider.value = targetMaterial.GetFloat(smoothnessKey);
    }

    private void InitializeMetallicSliderPosition() {
        metallicSlider.value = targetMaterial.GetFloat(metallicKey);
    }

}
