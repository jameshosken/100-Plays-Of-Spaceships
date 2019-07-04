using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileHit : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    [SerializeField] float damage = 101;
    GameObject player;

    // Start is called before the first frame update


    private void Start()
    {
        player = GameObject.Find("Player");
        Physics.IgnoreCollision(player.GetComponentInChildren<Collider>(), GetComponent<Collider>());
    }


    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.collider.gameObject;

        if(other.tag == "Shootable")
        {
            if (other.GetComponent<Health>())
            {
                other.GetComponent<Health>().LoseHealth(damage);
            }
        }

        GameObject expl = Instantiate(explosion, transform.position, Quaternion.identity);
        GameObject.Destroy(gameObject);
    }
}
