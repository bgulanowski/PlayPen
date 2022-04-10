using Mirror;

public class Network : NetworkManager
{
    public Chat Chat {
        get => _chat;
        set {
            var oldChat = _chat;
            _chat = value;
            ChatChanged?.Invoke(oldChat, _chat);
        }
    }

    private Chat _chat;

    public event System.Action<Chat, Chat> ChatChanged;
}
