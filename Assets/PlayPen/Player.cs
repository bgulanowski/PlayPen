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

    public override void OnStartLocalPlayer() {
        base.OnStartLocalPlayer();
        FindObjectOfType<Network>().Player = this;
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
    private void CmdSendChatMessage(string message) {
        chat.Send(handle, message);
    }
}
