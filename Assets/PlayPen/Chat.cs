using Mirror;
using UnityEngine;

public class Chat : NetworkBehaviour {

    readonly SyncList<ChatMessage> messages = new SyncList<ChatMessage>();

    readonly SyncDictionary<string, string> playerColors = new SyncDictionary<string, string>();

    int messageCount;

    private void OnEnable() {
        messages.Callback += MessagesChanged;
    }

    private void OnDisable() {
        messages.Callback -= MessagesChanged;
    }

    public override void OnStartClient() {
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
        return "0F0F0F";
    }

    public void SetColorForPlayer(string handle, Color color) {
        string hex = ColorUtility.ToHtmlStringRGB(color);
        playerColors[handle] = hex;
    }

    public void ClearColorForPlayer(string handle) {
        if (handle != null) {
            playerColors.Remove(handle);
        }
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

    public void Connect(string handle, Color color) {

        SetColorForPlayer(handle, color);

        ChatMessage message = new ChatMessage {
            index = IncrementCount(),
            messageType = MessageType.Connect,
            handle = handle,
            content = playerColors[handle]
        };
        messages.Add(message);
    }

    public void Disconnect(string handle) {

        ClearColorForPlayer(handle);

        ChatMessage message = new ChatMessage {
            index = IncrementCount(),
            messageType = MessageType.Disconnect,
            handle = handle
        };
        messages.Add(message);
    }

    private int IncrementCount() {
        int count = messageCount;
        ++messageCount;
        return count;
    }
}
