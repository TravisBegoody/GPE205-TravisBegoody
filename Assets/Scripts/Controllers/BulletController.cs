using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject Shooter;
    public float Damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterController hit = other.GetComponent<CharacterController>();
        if(hit != null && other.gameObject != Shooter)
        {
            Debug.Log("Hit Creature");
            hit.Damage(Damage);


            Destroy(this.gameObject);
            //Maybe add explosion particle effect
        }
        if(other.gameObject != Shooter)
        {
            Destroy(this.gameObject);
        }
    }
}
