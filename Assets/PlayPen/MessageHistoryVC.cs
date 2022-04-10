using UnityEngine;
using UnityEngine.UI;

public class MessageHistoryVC : MonoBehaviour
{
    [SerializeField]
    private Text historyText;

    [SerializeField]
    private Network network;

    private void OnEnable() {
        network.ChatChanged += UpdateChat;
    }

    private void OnDisable() {
        network.ChatChanged -= UpdateChat;
    }

    private void UpdateChat(Chat old, Chat chat) {
        if (old != null) {
            old.OnNewMessage -= MessageReceived;
        }
        if (chat != null) {
            chat.OnNewMessage += MessageReceived;
        }
    }

    public void MessageReceived(ChatMessage e) {
        historyText.text += $"\n<b>{e.handle}</b>: {e.content}";
    }

    private void Awake() {
        historyText.text = "<i><color=\"grey\">New Conversation</color></i>";
    }
}
