using Mirror;
using UnityEngine;

public class Chat : NetworkBehaviour
{
    readonly SyncList<ChatMessage> messages = new SyncList<ChatMessage>();

    int messageCount;

    public Player Player { get; set; }

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

    public void SendChatMessage(string message) {
        if (isClient) {
            CmdSendChatMessage(message, Player.handle);
        }
        else if (isServer) {
            Debug.Log("Can't send messages from server");
        }
    }

    [Command]
    private void CmdSendChatMessage(string message, string handle) {

        int count = messageCount;
        messageCount++;

        messages.Add(new ChatMessage { index = count, handle = handle, content = message });
    }
}
