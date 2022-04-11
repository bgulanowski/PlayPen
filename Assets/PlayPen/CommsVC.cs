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


    const string PlayerHandleKey = "PlayerHandle";
    const string PlayerColorKey = "PlayerColor";

    private static void SaveHandle(string handle) {
        PlayerPrefs.SetString(PlayerHandleKey, handle);
    }

    private static string LoadHandle() {
        return PlayerPrefs.GetString(PlayerHandleKey);
    }

    private static void SaveColor(Color color) {
        var s = ColorUtility.ToHtmlStringRGB(color);
        PlayerPrefs.SetString(PlayerColorKey, s);
    }

    private static Color LoadColor() {

        string s = PlayerPrefs.GetString(PlayerColorKey);
        if (s?.Length > 0) {
            if (s[0] != '#') {
                s = $"#{s}";
            }
            if (ColorUtility.TryParseHtmlString(s, out Color c)) {
                return c;
            }
        }

        return new Color(15, 15, 15);
    }


    // Start is called before the first frame update
    void Start()
    {
        colorPanel.ColorChanged += OnColorChanged;
        colorPanel.gameObject.SetActive(false);

        // todo: get these from user prefs
        handleField.text = LoadHandle();
        colorImage.color = LoadColor();

        network.PlayerChanged += OnPlayerChanged;
        network.ConnectionChanged += OnConnectionChanged;
    }

    private void OnPlayerChanged() {
        player = network.Player;
        if (player != null) {
            Debug.Log("Updating player properties");
            player.Handle = handleField.text;
            player.Color = colorImage.color;
            player.BroadcastProperties();
        }
    }

    private void OnConnectionChanged() {
        UpdateUIState();
    }

    public void HandleChanged(string _) {
        clientButton.interactable = handleField.text.Length > 0;
    }

    public void UpdateHandle(string handle) {
        if (network.mode != NetworkManagerMode.Host && handle?.Length > 0) {
            if (player != null) {
                player.Handle = handle;
                player.BroadcastProperties();
            }
            SaveHandle(handle);
            UpdateUIState();
        }
    }

    public void ToggleColorPanel() {
        var cp = colorPanel.gameObject;
        cp.SetActive(!cp.activeSelf);
    }

    private void OnColorChanged(Color color) {
        colorImage.color = color;
        if (player != null) {
            player.Color = color;
            player.BroadcastProperties();
        }
        PlayerPrefs.SetString(PlayerColorKey, ColorUtility.ToHtmlStringRGB(color));
        Debug.Log($"Saved color {color} to prefs");
    }
    public void Awake() {
        handleField.ActivateInputField();
    }

    public void ToggleHost() {
        if (network.mode == NetworkManagerMode.Host) {
            handleField.text = player.Handle;
            network.StopHost();
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

        clientButton.interactable = !isHost && handleField.text.Length > 0;
        hostButton.interactable = !isClient;
        handleField.interactable = isOffline;
        hostButton.GetComponentInChildren<Text>().text = hostTitle;
        clientButton.GetComponentInChildren<Text>().text = clientTitle;
    }
}
