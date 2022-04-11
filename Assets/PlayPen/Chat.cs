using Mirror;
using UnityEngine;

public class Chat : NetworkBehaviour {

    readonly SyncList<ChatMessage> messages = new SyncList<ChatMessage>();

    readonly SyncDictionary<string, string> playerColors = new SyncDictionary<string, string>();

    int messageCount;

    public override void OnStartClient() {

        messages.Callback += MessagesChanged;

        if (OnNewMessage != null) {
            foreach (var message in messages) {
                OnNewMessage(message);
            }
        }
    }

    public event System.Action<ChatMessage> OnNewMessage;

    private void MessagesChanged(SyncList<ChatMessage>.Operation op, int itemIndex, ChatMessage oldItem, ChatMessage newItem) {
        if (op == SyncList<ChatMessage>.Operation.OP_ADD) {
            OnNewMessage?.Invoke(newItem);
        }
    }

    public string ColorForPlayer(string handle) {
        if (playerColors.TryGetValue(handle, out string value)) {
            return value;
        }
        return "#000000";
    }

    public void Send(string handle, string content) {

        ChatMessage message = new ChatMessage {
            index = IncrementCount(),
            messageType = MessageType.Chat,
            handle = handle,
            content = content
        };
        messages.Add(message);
    }

    public void Connected(Player player) {

        playerColors[player.handle] = ColorUtility.ToHtmlStringRGB(player.color);

        ChatMessage message = new ChatMessage {
            index = IncrementCount(),
            messageType = MessageType.Connect,
            handle = player.handle
        };
        messages.Add(message);
    }

    public void Disconnected(Player player) {

        ChatMessage message = new ChatMessage {
            index = IncrementCount(),
            messageType = MessageType.Connect,
            handle = player.handle
        };
        messages.Add(message);
    }

    private int IncrementCount() {
        int count = messageCount;
        ++messageCount;
        return count;
    }
}
