
public enum MessageType {
    None,
    Chat,
    Connect,
    Disconnect
}

[System.Serializable]

public struct ChatMessage {
    public MessageType messageType;
    public int index;
    public string handle;
    public string content;
}
