using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
    float HorizontalInput;
    float VerticalInput;

    // Start is called before the first frame update
    void Start()
    {
        StartUp();
        GameManager.Instance.AddPlayer(this);

        lastShot = TankStats.ProjectileFirerate;
        ///Should as some point create a pool for itself to hold all of its
        ///bullets which will be deactivated and activated when shooting
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        Movement();
        Shoot();
    }
    void PlayerInput()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");

        if(Input.GetKeyDown(KeyCode.Space))
        {
            IsShooting = true;
        } else
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
    public override void Death()
    {
        GameManager.Instance.RemovePlayer(this);

        gameObject.SetActive(false);
    }
}
