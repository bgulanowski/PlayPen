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

    public Chat chat;

    private void OnEnable() {
        network.ChatChanged += UpdateChat;
    }

    private void OnDisable() {
        network.ChatChanged -= UpdateChat;
    }

    private void UpdateChat(Chat _, Chat chat) {
        this.chat = chat;
    }

    public void MessageChanged(string message) {
        sendButton.interactable = chat != null && message.Length > 0;
    }

    public void MessageComplete(string message) {
        if (chat != null && message.Length > 0) {
            Send();
            messageField.ActivateInputField();
        }
    }

    public void Send() {
        chat.SendChatMessage(messageField.text);
        messageField.text = "";
    }
}
