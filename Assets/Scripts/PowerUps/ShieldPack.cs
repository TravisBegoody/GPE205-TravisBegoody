using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPack : PowerUpPickUp
{
    public override void PickUp(PlayerController player)
    {
        if (player.canShield) //If the player already has pick up dont pickup
        {
            return;
        }
        player.ShieldPower();
        Destroy(this.gameObject);
    }
}
