using UnityEngine;
using Mirror;
using System;

public class Network : NetworkManager
{
    public Player Player {
        get => _player;
        set {
            if (value != _player) {
                _player = value;
                PlayerChanged?.Invoke();
            }
        }
    }

    private Player _player;

    public event Action PlayerChanged;

    public event Action Connected;

    public event Action Disconnected;

    [SerializeField]
    private Chat chat;

    public override void OnClientConnect() {
        base.OnClientConnect();
        Connected?.Invoke();
    }

    public override void OnClientDisconnect() {
        base.OnClientDisconnect();
        Disconnected?.Invoke();
        Player = null;
    }
}
