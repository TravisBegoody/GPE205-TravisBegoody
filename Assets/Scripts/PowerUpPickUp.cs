using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //If player enters then activates pick up
        if(other.GetComponent<PlayerController>() != null)
        {
            PickUp(other.GetComponent<PlayerController>());
        }
    }
    public virtual void PickUp(PlayerController player)
    {
        Debug.Log("Picked Up A Power Up");
    }
}
