using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStartTrigger : MonoBehaviour
{
    private BossFlags _BossFlags;

    private void Start() {
        _BossFlags = GameObject.Find("BossHead").GetComponent<BossFlags>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        _BossFlags.BossStarted = true;
    }
}
