using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YKillZone : MonoBehaviour {

    public delegate void KillZone();
    public static KillZone OnKillZone;

    private void OnTriggerEnter(Collider other) {
        PlayerController pc = other.gameObject.GetComponent<PlayerController>();
        if (pc != null)
            pc.Respawn();
    }
}
