using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public enum TargetType { Player, Enemy, Any };
    public TargetType bulletTarget;

    [SerializeField] public GameObject Shooter;
    public float Damage;
    [SerializeField] private GameObject Explosion;

    private void OnTriggerEnter(Collider other)
    {
        CharacterController hit = other.GetComponent<CharacterController>();
        BulletController bullet = other.GetComponent<BulletController>();
        if (hit != null && other.gameObject != Shooter)
        {
            switch (bulletTarget)
            {
                case TargetType.Player:
                    if (GameManager.Instance.IsPlayer(hit.transform.position))
                    {
                        Debug.Log("Hit Player");
                        goto case TargetType.Any;
                    }
                    break;
                case TargetType.Enemy:
                    if (GameManager.Instance.IsEnemy(hit.transform.position))
                    {
                        Debug.Log("Hit Enemy");
                        goto case TargetType.Any;
                    }
                    break;
                case TargetType.Any:
                    hit.Damage(Damage);
                    if (Explosion != null)
                    {
                        Instantiate(Explosion, this.transform.position, Quaternion.identity);
                    }
                    Destroy(this.gameObject);
                    break;
            }
        }
        if (bullet != null && bullet.bulletTarget != this.bulletTarget)
        {
            //Debug.Log("Hit Another Bullet");
            if (Explosion != null)
            {
                Instantiate(Explosion, this.transform.position, Quaternion.identity);
            }
            Destroy(this.gameObject);
        }
        //if it isnt this shooter or bullet target type then it doesnt hit
        else if (other.gameObject != Shooter && bullet == null && other.tag != "PowerUp")
        {
            if (Explosion != null)
            {
                Instantiate(Explosion, this.transform.position, Quaternion.identity);
            }
            Destroy(this.gameObject);
        }
    }
}