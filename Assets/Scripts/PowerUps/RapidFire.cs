using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidFire : PowerUpPickUp
{
    public override void PickUp(PlayerController player)
    {
        if (player.canRapid) //If the player already has pick up dont pickup
        {
            return;
        }
        player.RapidPower();
        Destroy(this.gameObject);
    }
}
