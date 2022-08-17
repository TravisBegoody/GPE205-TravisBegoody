using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTankData", menuName = "TankClasses/TankData")]
public class TankData : ScriptableObject
{
    public float Health;

    //How fast the tanks moves with VerticalInput
    public float MoveSpeed;
    //How fast the tank turns with HorizontalInput
    public float TurnSpeed;
    //How far the tank can see
    public float SightRange;
    // In Degrees
    public float SightFOV;
    //How far the tank can hear
    public float HearRange;

    //Projectile used by tank
    public GameObject Projectile;
    //How fast each projectile travels
    public float ProjectileSpeed;
    //Rate at which a tank fires
    public float ProjectileFirerate;
}
