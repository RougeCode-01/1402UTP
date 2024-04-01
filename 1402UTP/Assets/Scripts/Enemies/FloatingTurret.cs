using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTurret : Enemy
{
    [SerializeField]
    EnemyProjectile projectile;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Player has entered turret radius!");
            StartCoroutine(FireAtPlayer());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Player has exited turret radius!");
            StopAllCoroutines();
        }
    }

    IEnumerator FireAtPlayer()
    {
        while(true)
        {
            Instantiate(projectile, this.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(2f);
        }
    }
}
