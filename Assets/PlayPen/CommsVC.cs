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
    private Network network;

    private bool playerReady;

    // Start is called before the first frame update
    void Start()
    {
        colorPanel.ColorChanged += OnColorChanged;
        colorPanel.gameObject.SetActive(false);

        // todo: get these from user prefs
        //handleField.text = ;
        //colorImage.color = ;

        network.PlayerChanged += OnPlayerChanged;
    }

    private void OnPlayerChanged() {
        playerReady = network.Player != null;
        Debug.Log("Player changed");
        if (playerReady) {
            Debug.Log("Updating player properties");
            network.Player.handle = handleField.text;
            network.Player.color = colorImage.color;
        }
        UpdateUIState();
    }

    public void UpdateHandle(string handle) {
        if (playerReady) {
            network.Player.handle = handle;
        }
        UpdateUIState();
        // todo: save to user prefs - on connection?
    }

    public void ToggleColorPanel() {
        var cp = colorPanel.gameObject;
        cp.SetActive(!cp.activeSelf);
    }

    private void OnColorChanged(Color color) {
        colorImage.color = color;
        if (playerReady) {
            network.Player.color = color;
        }
        // todo: save to user prefs
    }
    public void Awake() {
        handleField.ActivateInputField();
    }

    public void ToggleHost() {
        if (network.mode == NetworkManagerMode.Host) {
            network.StopHost();
            // todo: update player handle from user prefs
        }
        else if (network.mode == NetworkManagerMode.Offline) {
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
