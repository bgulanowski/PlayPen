using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SyncVar] public string handle;

    [SyncVar] public Color color;

    public override void OnStartClient() {
        base.OnStartClient();

    }
}
