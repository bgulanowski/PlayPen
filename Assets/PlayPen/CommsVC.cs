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

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        colorPanel.ColorChanged += OnColorChanged;
        colorPanel.gameObject.SetActive(false);

        // todo: get these from user prefs
        //handleField.text = ;
        //colorImage.color = ;

        network.PlayerChanged += OnPlayerChanged;
        network.ConnectionChanged += OnConnectionChanged;
    }

    private void OnPlayerChanged() {
        player = network.Player;
        if (player != null) {
            Debug.Log("Updating player properties");
            player.handle = handleField.text;
            player.color = colorImage.color;
        }
    }

    private void OnConnectionChanged() {
        UpdateUIState();
    }

    public void UpdateHandle(string handle) {
        if (player != null) {
            player.handle = handle;
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
        if (player != null) {
            player.color = color;
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

        string hostTitle = "Start";
        string clientTitle = "Join";
        bool isOffline = false;
        bool isHost = false;
        bool isClient = false;

        switch (network.mode) {
            case NetworkManagerMode.Offline:
                isOffline = true;
                break;

            case NetworkManagerMode.Host:
                isHost = true;
                hostTitle = "Stop";
                break;

            case NetworkManagerMode.ClientOnly:
                isClient = true;
                clientTitle = "Leave";
                break;
        }

        clientButton.interactable = !isHost;
        hostButton.interactable = !isClient;
        handleField.interactable = isOffline;
        hostButton.GetComponentInChildren<Text>().text = hostTitle;
        clientButton.GetComponentInChildren<Text>().text = clientTitle;
    }
}
