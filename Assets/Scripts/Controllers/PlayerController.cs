using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
    [SerializeField] private TankHUD HUD;

    float HorizontalInput;
    float VerticalInput;

    public bool canShield; //Powerup 2
    bool isInvincible; //Powerup 2 invincibility
    public float shieldTime;

    public bool canShotgun; //Powerup 3
    public float shotgunTime;

    public bool canRapid; //Powerup 4
    public float rapidTime;

    bool canMove;
    // Start is called before the first frame update
    void Start()
    {
        StartUp();
        GameManager.Instance.AddPlayer(this);

        lastShot = TankStats.ProjectileFirerate;
        ///Should as some point create a pool for itself to hold all of its
        ///bullets which will be deactivated and activated when shooting
        ///
        canMove = true;

        StartCoroutine(Shotgun());
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            PlayerInput();
        }
        Movement();


        if (canRapid)//Triples the firerate and turns off manual shooting
        {
            IsShooting = true;
            lastShot += Time.deltaTime * 2f;
        }
        if (canShotgun) //Fires two more bullets
        {
            float temp = lastShot;
            Shoot(-15f);
            lastShot = temp;
            Shoot();
            lastShot = temp;
            Shoot(15f);
        }
        Shoot();
    }
    void PlayerInput()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            IsShooting = true;
        }
        else
        {
            IsShooting = false;
        }
    }
    void Movement()
    {
        //Rotates tanks to turn 
        Vector3 rotate = new Vector3(0f, HorizontalInput, 0f) * TankStats.TurnSpeed * Time.deltaTime;

        transform.Rotate(rotate);

        //Allows tank to face direction locally to always move forward
        rb.velocity = (VerticalInput * transform.forward) * TankStats.MoveSpeed;
    }
    public override void Damage(float damage)
    {
        if (canShield) //if picked up the shield powerup then it auto activates and shields for 4.5 seconds
        {
            StartCoroutine(Invincible());
            canShield = false;
        }
        if (isInvincible)
        {
            return; //Doesn't do damage
        }
        currentHealth -= damage;
        HUD.UpdateHealthBar(TankStats.Health, currentHealth);

        if (currentHealth <= 0f)
        { Death(); }
    }
    public override void Heal(float heal)
    {
        base.Heal(heal);

        HUD.UpdateHealthBar(TankStats.Health, currentHealth);
    }
    public override void Death()
    {
        GameManager.Instance.RemovePlayer(this);

        gameObject.SetActive(false);
        GameManager.Instance.PlayerDeath();
    }
    public void ChangeInput(bool movable)
    {
        canMove = movable;
    }
    public void ShieldPower()
    {
        canShield = true;
    }
    public void ShotgunPower()
    {
        StartCoroutine(Shotgun());
    }
    public void RapidPower()
    {
        StartCoroutine(Rapid());
    }
    IEnumerator Invincible()
    {
        isInvincible = true;
        Debug.Log("wait for" + shieldTime);
        yield return new WaitForSeconds(shieldTime);
        Debug.Log("finished invincible");
        isInvincible = false;
    }
    public IEnumerator Shotgun()
    {
        canShotgun = true;
        Debug.Log("wait for" + shotgunTime);
        yield return new WaitForSeconds(shotgunTime);
        Debug.Log("finished shotgun");
        canShotgun = false;
    }
    public IEnumerator Rapid()
    {
        canRapid = true;
        Debug.Log("wait for" + rapidTime);
        yield return new WaitForSeconds(rapidTime);
        Debug.Log("finished rapid");
        canRapid = false;
    }
}