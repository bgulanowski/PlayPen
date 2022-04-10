using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CommsVC : MonoBehaviour
{
    [SerializeField]
    private InputField handleField;

    [SerializeField]
    private Image colorImage;

    [SerializeField]
    private Button hostButton;

    [SerializeField]
    private Button clientButton;

    [SerializeField]
    private ColourPicker colorPanel;

    [SerializeField]
    private Player player;

    [SerializeField]
    private NetworkManager network;

    // Start is called before the first frame update
    void Start()
    {
        colorPanel.ColorChanged += OnColorChanged;
        colorPanel.gameObject.SetActive(false);

        handleField.text = player.handle;
        colorImage.color = player.color;
    }

    public void UpdateHandle(string handle) {
        player.handle = handle;
    }

    public void ToggleColorPanel() {
        var cp = colorPanel.gameObject;
        cp.SetActive(!cp.activeSelf);
    }

    private void OnColorChanged(Color color) {
        colorImage.color = color;
        player.color = color;
    }
    public void Awake() {
        handleField.ActivateInputField();
    }

    public void ToggleHost() {
        if (network.mode == NetworkManagerMode.Host) {
            network.StopHost();
        }
        else if (network.mode == NetworkManagerMode.Offline) {
            player.handle = "Host";
            handleField.text = "Host";
            network.StartHost();
        }
        UpdateUIState();
    }

    public void ToggleConnection() {
        if (network.mode == NetworkManagerMode.ClientOnly) {
            network.StopClient();
        }
        else if (network.mode == NetworkManagerMode.Offline) {
            network.StartClient();
        }
        UpdateUIState();
    }

    private void UpdateUIState() {

        var mode = network.mode;
        bool isOffline = mode == NetworkManagerMode.Offline;
        bool isHost = mode == NetworkManagerMode.Host;
        bool hasName = handleField.text.Length > 0;

        hostButton.GetComponentInChildren<Text>().text = isHost ? "Stop" : "Start";
        clientButton.interactable = isOffline && hasName;
        handleField.interactable = isOffline;
    }
}
