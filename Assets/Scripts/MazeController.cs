using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MazeUIController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Dropdown _algorithmDropdown;
    [SerializeField] private TMP_InputField _widthInput;
    [SerializeField] private TMP_InputField _heightInput;
    [SerializeField] private Slider _delaySlider;
    [SerializeField] private TextMeshProUGUI _delayValueText;
    [SerializeField] private Button _generateButton;
    [SerializeField] private MazeGenerator _mazeGenerator;

    private void Awake()
    {
        _algorithmDropdown.onValueChanged.AddListener(OnAlgorithmChanged);
        _widthInput.onEndEdit.AddListener(OnWidthChanged);
        _heightInput.onEndEdit.AddListener(OnHeightChanged);
        _delaySlider.onValueChanged.AddListener(OnDelayChanged);
        _generateButton.onClick.AddListener(OnGenerateClicked);

        
    }

    private void Start()
    {
        // Initialize UI with current generator settings
        _algorithmDropdown.value = (int)_mazeGenerator.Algorithm;
        _widthInput.text = _mazeGenerator.Width.ToString();
        _heightInput.text = _mazeGenerator.Height.ToString();
        _delaySlider.value = _mazeGenerator.GenerationDelay;
        UpdateDelayText(_mazeGenerator.GenerationDelay);
    }

    private void OnGenerateClicked() => _mazeGenerator.GenerateMaze();

    private void OnAlgorithmChanged(int idx) => _mazeGenerator.Algorithm = (MazeAlgorithm)idx;

    private void OnWidthChanged(string text) { 
        if (int.TryParse(text, out int val)) 
            _mazeGenerator.Width = val; 
    }

    private void OnHeightChanged(string text) {
        if (int.TryParse(text, out int val)) 
            _mazeGenerator.Height = val;
    }

    private void OnDelayChanged(float val) { 
        _mazeGenerator.GenerationDelay = val; 
        UpdateDelayText(val);
    }

    private void UpdateDelayText(float val) { 
        if (_delayValueText != null) 
            _delayValueText.text = val.ToString("F2") + "s"; 
    }
}
