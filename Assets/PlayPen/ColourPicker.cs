using System;
using UnityEngine;
using UnityEngine.UI;

public class ColourPicker : MonoBehaviour
{
    [SerializeField]
    Slider redSlider;

    [SerializeField]
    Slider greenSlider;

    [SerializeField]
    Slider blueSlider;

    [SerializeField]
    InputField redField;

    [SerializeField]
    InputField greenField;

    [SerializeField]
    InputField blueField;

    [SerializeField]
    InputField hexField;

    [SerializeField]
    Image colourImage;

    public event Action<Color> ColorChanged;

    public Color Color {
        get => _color;
        set {
            _color = value;
            ColorChanged?.Invoke(_color);
        }
    }

    public float Red {
        get => _color.r * 255f;
        set {
            _color.r = value / 255f;
            ColorChanged?.Invoke(_color);
        }
    }

    public string RedString {
        get => ((int)Red).ToString();
        set {
            Red = int.Parse(value);
            ColorChanged?.Invoke(_color);
        }
    }

    public float Green {
        get => _color.g * 255f;
        set {
            _color.g = value / 255f;
            ColorChanged?.Invoke(_color);
        }
    }

    public string GreenString {
        get => ((int)Green).ToString();
        set {
            Green = int.Parse(value);
            ColorChanged?.Invoke(_color);
        }
    }

    public float Blue {
        get => _color.b * 255f;
        set {
            _color.b = value / 255f;
            ColorChanged?.Invoke(_color);
        }
    }

    public string BlueString {
        get => ((int)Blue).ToString();
        set {
            Blue = int.Parse(value);
            ColorChanged?.Invoke(_color);
        }
    }

    public string ColorHexString {
        get => ColorUtility.ToHtmlStringRGB(_color);
        set {
            if (ColorUtility.TryParseHtmlString(value, out Color c)) {
                _color = c;
                ColorChanged?.Invoke(_color);
            }
        }
    }

    private Color _color;

    // Start is called before the first frame update
    void Start()
    {
        Color = colourImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateColor(Color color) {
        Color = color;
        UpdateColorControls();
        UpdateComponentControls();
    }

    public void UpdateRedFloat(float value) {
        Red = value;
        redField.text = RedString;
        UpdateColorControls();
    }

    public void UpdateGreenFloat(float value) {
        Green = value;
        greenField.text = GreenString;
        UpdateColorControls();
    }

    public void UpdateBlueFloat(float value) {
        Blue = value;
        blueField.text = BlueString;
        UpdateColorControls();
    }

    public void UpdateRedText(string value) {
        RedString = value;
        redSlider.value = Red;
        UpdateComponentControls();
    }

    public void UpdateGreenText(string value) {
        GreenString = value;
        greenSlider.value = Green;
        UpdateColorControls();
    }

    public void UpdateBlueText(string value) {
        BlueString = value;
        blueSlider.value = Blue;
        UpdateColorControls();
    }

    public void UpdateHex(string value) {
        ColorHexString = value;
        UpdateComponentControls();
    }

    void UpdateComponentControls() {
        redSlider.value = Red;
        greenSlider.value = Green;
        blueSlider.value = Blue;
        redField.text = RedString;
        greenField.text = GreenString;
        blueField.text = BlueString;
    }

    void UpdateColorControls() {
        colourImage.color = Color;
        hexField.text = ColorHexString;
    }
}
