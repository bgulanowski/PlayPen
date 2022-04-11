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

        string c = chat.ColorForPlayer(e.handle);
        string s = $"<b><color=\"#{c}\">{e.handle}</color></b>";

        switch (e.messageType) {
            case MessageType.Chat:
                s = $"\n{s}: {e.content}";
                break;
            case MessageType.Connect:
                s = $"\n\t<i>{s} <color=\"grey\">has joined.</color></i>";
                break;

            case MessageType.Disconnect:
                s = $"\n\t<i>{s} <color=\"grey\">has left.</color></i>";
                break;

            default:
                break;
        }
        historyText.text += s;
    }

    private void Awake() {
        historyText.text = "\t<i><color=\"grey\">New Conversation</color></i>";
    }
}
