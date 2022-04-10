using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SyncVar] public string handle;

    [SyncVar] public Color color;

    public override void OnStartClient() {
        base.OnStartClient();

        Chat chat = FindObjectOfType<Chat>();
        if (chat != null) {
            chat.Player = this;
        }
    }
}
