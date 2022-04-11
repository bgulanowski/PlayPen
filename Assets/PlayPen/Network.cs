using UnityEngine;
using Mirror;
using System;

public class Network : NetworkManager
{
    public Player Player {
        get => _player;
        set {
            _player = value;
            PlayerReady?.Invoke();
        }
    }

    private Player _player;

    public event Action PlayerReady;

    [SerializeField]
    private Chat chat;

    public override void OnClientDisconnect() {
        Player = null;
        base.OnClientDisconnect();
    }
}
