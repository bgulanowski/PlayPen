using UnityEngine;
using UnityEngine.UI;

public class MessageHistoryVC : MonoBehaviour
{
    [SerializeField]
    private Text historyText;

    [SerializeField]
    private Chat chat;

    private void OnEnable() {
        chat.OnNewMessage += MessageReceived;
    }

    private void OnDisable() {
        chat.OnNewMessage -= MessageReceived;
    }

    public void MessageReceived(ChatMessage e) {
        historyText.text += $"\n<b>{e.handle}</b>: {e.content}";
    }

    private void Awake() {
        historyText.text = "<i><color=\"grey\">New Conversation</color></i>";
    }
}
