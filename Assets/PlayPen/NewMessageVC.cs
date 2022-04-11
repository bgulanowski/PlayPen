using UnityEngine;
using UnityEngine.UI;

public class NewMessageVC : MonoBehaviour
{
    [SerializeField]
    private Button sendButton;

    [SerializeField]
    private InputField messageField;

    [SerializeField]
    private Network network;

    public Player Player { get; set; }

    private void OnEnable() {
        network.PlayerChanged += OnPlayerChanged;
    }

    private void OnDisable() {
        network.PlayerChanged -= OnPlayerChanged;
    }

    private void OnPlayerChanged() {
        Player = network.Player;
    }

    public void MessageChanged(string message) {
        sendButton.interactable = Player != null && message.Length > 0;
    }

    public void MessageComplete(string message) {
        if (Player != null && message.Length > 0) {
            Player.SendChatMessage(messageField.text);
            messageField.text = "";
            messageField.ActivateInputField();
        }
    }
}
