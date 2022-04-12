using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    public string Handle {
        get => _handle;
        set => localHandle = value;        
    }
    public Color Color {
        get => _color;
        set => localColor = value;
    }

    [SyncVar] private string _handle;
    [SyncVar] private Color _color;

    private string localHandle;
    private Color localColor;

    Chat chat;

    public override void OnStartServer() {
        base.OnStartServer();
        chat = FindObjectOfType<Chat>();
    }

    public override void OnStopServer() {
        base.OnStopServer();
        chat.Disconnect(_handle);
    }

    public override void OnStartAuthority() {
        base.OnStartAuthority();
        FindObjectOfType<Network>().Player = this;
        CmdSendConnectMessage(localHandle, localColor);
    }

    public void BroadcastProperties() {
        CmdUpdateProperties(localHandle, localColor);
    }

    public void SendChatMessage(string message) {
        CmdSendChatMessage(message);
    }

    [Command]
    private void CmdUpdateProperties(string handle, Color color) {
        _handle = handle;
        _color = color;
        chat.SetColorForPlayer(_handle, _color);
    }

    [Command]
    private void CmdSendConnectMessage(string handle, Color color) {
        chat.Connect(handle, color);
    }

    [Command]
    private void CmdSendChatMessage(string message) {
        chat.Send(_handle, message);
    }
}
