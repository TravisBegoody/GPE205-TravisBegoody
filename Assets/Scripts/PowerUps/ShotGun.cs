using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : PowerUpPickUp
{
    public override void PickUp(PlayerController player)
    {
        if(player.canShotgun) //If the player already has pick up dont pickup
        {
            return;
        }
        player.ShotgunPower();
        Destroy(this.gameObject);
    }
}
