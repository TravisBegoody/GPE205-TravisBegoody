using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public float Damage;
    private bool IsExploded;//Stops from blowing up too late
    // Start is called before the first frame update
    void Start()
    {
        IsExploded = false;

        StartCoroutine(Explosion());
    }
    private void OnTriggerEnter(Collider other)
    {
        CharacterController hit = other.GetComponent<CharacterController>();
        if(hit != null && !IsExploded)
        {
            hit.Damage(Damage);
        }
    }
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(1f);
        IsExploded = true;
    }
}
