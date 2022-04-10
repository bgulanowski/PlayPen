using UnityEngine;
using Mirror;

public class Network : NetworkManager
{
    public Player Player { get; set; }

    [SerializeField]
    private Chat chat;

    public override void OnClientConnect() {
        base.OnClientConnect();

        var conn = NetworkClient.connection as LocalConnectionToClient;
        Player = conn.identity.GetComponent<Player>();
    }
}
