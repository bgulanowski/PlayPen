using UnityEngine;
using Mirror;
using System;

public class Network : NetworkManager
{
    public Player Player {
        get => _player;
        set {
            if (value == null && _player != null) {
                chat.Disconnected(_player);
            }
            _player = value;
            PlayerReady?.Invoke();
            if(_player != null) {
                chat.Connected(_player);
            }
        }
    }

    private Player _player;

    public event Action PlayerReady;

    [SerializeField]
    private Chat chat;

    public override void OnClientDisconnect() {
        base.OnClientDisconnect();
        Player = null;
    }
}
