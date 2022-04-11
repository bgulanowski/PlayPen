using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SyncVar] public string handle;

    [SyncVar] public Color color;

    Chat chat;

    public override void OnStartServer() {
        base.OnStartServer();
        chat = FindObjectOfType<Chat>();
    }

    public override void OnStopServer() {
        base.OnStopServer();
        chat.Disconnect(handle);
    }

    public override void OnStartLocalPlayer() {
        base.OnStartLocalPlayer();
        FindObjectOfType<Network>().Player = this;

        // make sure client is ready and we are configured
        CmdSendConnectMessage();
    }

    public void SendChatMessage(string message) {
        if (isClient) {
            CmdSendChatMessage(message);
        }
        else if (isServer) {
            Debug.Log("Can't send messages from server");
        }
    }

    [Command]
    private void CmdSendConnectMessage() {
        chat.Connect(handle, color);
    }

    [Command]
    private void CmdSendChatMessage(string message) {
        chat.Send(handle, message);
    }
}
