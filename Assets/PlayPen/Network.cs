using UnityEngine;
using Mirror;
using System;

public class Network : NetworkManager
{
    public Player Player {
        get => _player;
        set {
            _player = value;
            PlayerChanged?.Invoke();
        }
    }

    private Player _player;

    public event Action PlayerChanged;

    [SerializeField]
    private Chat chat;

    public override void OnClientDisconnect() {
        Player = null;
        base.OnClientDisconnect();
    }
}
