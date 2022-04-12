using UnityEngine;
using UnityEngine.UI;

public class MessageHistoryVC : MonoBehaviour
{
    [SerializeField]
    private Text historyText;

    [SerializeField]
    private Chat chat;

    [SerializeField]
    private Network network;

    private void OnEnable() {
        chat.OnNewMessage += MessageReceived;
        network.Connected += OnConnected;
        network.Disconnected += OnDisconnected;
    }

    private void OnDisable() {
        chat.OnNewMessage -= MessageReceived;
        network.Connected -= OnConnected;
        network.Disconnected -= OnDisconnected;
    }

    private void OnConnected() {
        historyText.text = "\t<i><color=\"grey\">Connected to Server</color></i>\n";
    }

    private void OnDisconnected() {
        historyText.text += "\n\t<i><color=\"grey\">Disconnected from Server</color></i>\n";
    }

    public void MessageReceived(ChatMessage e) {

        string c = chat.ColorForPlayer(e.handle) ?? "pink";
        string m = "";
        bool isStatus = true;

        switch (e.messageType) {
            case MessageType.Chat:
                m = e.content;
                isStatus = false;
                break;

            case MessageType.ColorChange:
                c = e.content ?? c;
                m = " has updated their color";
                break;

            case MessageType.Connect:
                c = e.content ?? c;
                m = " has joined";
                break;

            case MessageType.Disconnect:
                m = " has gone away";
                break;

            default:
                break;
        }

        string h = $"<b><color=\"{c}\">{e.handle}</color></b>";

        if (isStatus) {
            historyText.text += $"\n\t<i>{h}<color=\"grey\">{m}</color></i>";
        }
        else {
            historyText.text += $"\n{h}: {m}";
        }
    }

    private void Awake() {
        historyText.text = "\t<i><color=\"grey\">New Conversation</color></i>";
    }
}
