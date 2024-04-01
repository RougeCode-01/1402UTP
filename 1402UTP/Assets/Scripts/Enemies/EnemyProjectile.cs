using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Enemy
{
    GameObject player;
    Rigidbody2D rb;
    Vector3 direction;
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Projectile spawned!");
        Physics2D.IgnoreLayerCollision(6, 6, true);
        Physics2D.IgnoreLayerCollision(3, 3, true);
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        direction = (player.transform.position - this.transform.position).normalized;
        rb.AddForce(10.0f * direction, ForceMode2D.Impulse);
        Invoke("KillSelf", 20.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>())
        {
            KillSelf();
        }
    }

    private void KillSelf()
    {
        Destroy(this.gameObject);
    }
}
