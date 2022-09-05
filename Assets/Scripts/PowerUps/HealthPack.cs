using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : PowerUpPickUp
{
    public float HealthRestored;
    public override void PickUp(PlayerController player)
    {
        player.Heal(HealthRestored);

        Destroy(this.gameObject);
    }
}
