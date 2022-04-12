
public enum MessageType {
    None,
    Connect,
    Disconnect,
    Chat,
    ColorChange
}

[System.Serializable]

public struct ChatMessage {
    public MessageType messageType;
    public int index;
    public string handle;
    public string content;
}
