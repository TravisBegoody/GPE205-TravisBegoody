using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTankData", menuName = "TankClasses/TankData")]
public class TankData : ScriptableObject
{
    public float Health;

    public float MoveSpeed;
    public float TurnSpeed;

    public GameObject Projectile;
    public float ProjectileSpeed;
    public float ProjectileFirerate;
}
