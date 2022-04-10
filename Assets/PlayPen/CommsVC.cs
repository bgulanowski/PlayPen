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

    // fixme: this won't work -- how do we find the local player?
    //[SerializeField]
    //private Player player;

    [SerializeField]
    private Network network;

    // Start is called before the first frame update
    void Start()
    {
        colorPanel.ColorChanged += OnColorChanged;
        colorPanel.gameObject.SetActive(false);

        // todo: get these from user prefs
        //handleField.text = player.handle;
        //colorImage.color = player.color;
        colorImage.color = colorPanel.Color;
    }

    public void UpdateHandle(string handle) {
        network.Player.handle = handle;
        // save to user prefs
    }

    public void ToggleColorPanel() {
        var cp = colorPanel.gameObject;
        cp.SetActive(!cp.activeSelf);
    }

    private void OnColorChanged(Color color) {
        colorImage.color = color;
        network.Player.color = color;
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
            network.Player.handle = "Host";
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
