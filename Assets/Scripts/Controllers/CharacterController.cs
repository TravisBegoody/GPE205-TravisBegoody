using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public TankData TankStats;

    public Rigidbody rb;
    //Spot that the bullet fires from
    public Transform barrel;

    public float currentHealth;

    //When the last shot was shot off
    protected float lastShot;
    protected bool IsShooting;

    // Start is called before the first frame update
    void Start()
    {
        StartUp();
    }
    //Used to allow easy start up for all controllers that derive from this one
    protected void StartUp()
    {
        rb = GetComponent<Rigidbody>();

        //Sets currHealth to statHealth
        currentHealth = TankStats.Health;

    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }
    /// <summary>
    /// Allows the tank to shoot a bullet
    /// </summary>
    protected void Shoot()
    {
        lastShot += Time.deltaTime;

        //Checks to make sure that is currently shooting and last shot is greater than firerate
        if (IsShooting && lastShot >= TankStats.ProjectileFirerate)
        {   //Fires bullet
            GameObject bullet;

            //Shoots from barrel unless doesn't exist shoots from gameobject
            if (barrel != null)
            { bullet = Instantiate(TankStats.Projectile, barrel.position, transform.rotation); }
            else
            { bullet = Instantiate(TankStats.Projectile, this.transform.position, transform.rotation); }
            
            
            bullet.GetComponent<Rigidbody>().velocity = transform.forward * TankStats.ProjectileSpeed;
            bullet.GetComponent<BulletController>().Shooter = this.gameObject;

            lastShot = 0f;
        }
    }
    //When character takes damage, applies damage and if health reaches 0 they die
    public void Damage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
        { Death(); }
    }
    //Sets up death for children classes
    public virtual void Death()
    {
        gameObject.SetActive(false);
    }
}
