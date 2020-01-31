using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColourPicker : MonoBehaviour {

    [Header("Hue:")]
    public Image hueImage;
    public Slider hueSlider;
    public InputField hueInputField;
    [Header("Saturation:")]
    public Image saturationImage;
    public Slider saturationSlider;
    public InputField saturationInputField;
    [Header("Lightness Value:")]
    public Image valueLightnessImage;
    public Slider valueLightnessSlider;
    public InputField valueInputField;
    [Header("Settings:")]
    public int multiply = 540;
	public Material carMat;
    [SerializeField] private string metallicKey = "_Metallic";
    [SerializeField] private string smoothnessKey = "_Glossiness";
    [SerializeField] private string colourKey = "_BaseColor";

    private float saturation;
    private GameObject currentVehicle;
    private float valueLightness;
    private float hue;
    private Renderer carRenderer;
    private Shader currentShader;

    private void Start () {
        SetSaturationBarColour();

        hue = hueSlider.value;
        saturation = saturationSlider.value;
        valueLightness = valueLightnessSlider.value;
    }

    public void SetSaturationBarColour () {
        int xCord = (int)(hueSlider.value * multiply);
        saturationImage.color = hueImage.sprite.texture.GetPixel(xCord, 0);
    }

    /*private int SaturationBarColour () {
        int xCord = (int)(hueSlider.value * multiply);
        saturationImage.color = hueImage.sprite.texture.GetPixel(xCord, 0);
        return xCord;
    }*/

    public void SetHue (float _hue) {
        hue = _hue;
        hueInputField.text = hue.ToString();
    }

    public void SetSaturation (float _value) {
        saturation = _value;
        saturationInputField.text = saturation.ToString();
    }

    public void SetValueLightness (float _value) {
        valueLightness = _value;
        valueInputField.text = valueLightness.ToString();
    }

    public void SetColour () {
        //carMat.color = Color.HSVToRGB(hue, saturation, valueLightness, true);
        carMat.SetColor(colourKey, Color.HSVToRGB(hue, saturation, valueLightness, true));
    }

    public void ChangeMetalness (float _value) {
        carMat.SetFloat(metallicKey, _value);
    }

    public void ChangeSmoothness (float _value) {
        carMat.SetFloat(smoothnessKey, _value);
    }

    public void UpdateHueSlider () {
        hueSlider.value = float.Parse(hueInputField.text);
    }

    public void UpdateSaturationSlider(float _value) {
        saturationSlider.value = float.Parse(saturationInputField.text);
    }

    public void UpdateValueLightnessSlider(float _value) {
        valueLightnessSlider.value = float.Parse(valueInputField.text);
    }

    public void SetColourByHex (string hex) {
        
    }

}
