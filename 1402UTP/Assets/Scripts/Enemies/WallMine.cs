using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMine : Enemy
{
    public GameObject pointA;
    public GameObject pointB;
    public Rigidbody2D rb;
    public SpriteRenderer sp;
    public Animator anim;
    public float speed = 2f;
    private Transform currentTarget;
    public float detonateRadius = 1.5f; // Radius for detecting player proximity
    public float detonateDelay = 1.5f; // Delay before detonation after detecting player
    public LayerMask playerLayer; // Layer mask for the player
    public string playerTag = "Player"; // Tag of the player object

    private bool playerDetected = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        currentTarget = pointB.transform; // Moves to Point B at first
    }

    void Update()
    {
        DetonateIfPlayerClose();
        if (!playerDetected)
        {
            MoveBetweenPoints();
        }
        else
        {
            DetonateIfPlayerClose();
        }
    }

    void MoveBetweenPoints()
    {
        Vector2 point = currentTarget.position - transform.position;
        if (currentTarget == pointB.transform)
        {
            rb.velocity = new Vector2(0, -speed);
        }
        else
        {
            rb.velocity = new Vector2(0, speed);
        }

        // Check if close to the target point
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.5f && currentTarget == pointB.transform)
        {
            Flip();
            currentTarget = pointA.transform;
        }
        // Check if close to the target point
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.5f && currentTarget == pointA.transform)
        {
            Flip();
            currentTarget = pointB.transform;
        }
    }

    void DetonateIfPlayerClose()
    {
        Debug.Log("Checking for player...");

        // Perform a circle cast around the mine to detect the player
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, detonateRadius, Vector2.zero, 0f, playerLayer);

        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, (hit.point - (Vector2)transform.position), Color.red);
        }

        if (hit.collider != null && hit.collider.CompareTag(playerTag))
        {
            // Player detected, start detonation countdown
            playerDetected = true;
            StartCoroutine(DetonateAfterDelay());
            Debug.Log("Player detected by mine!");
        }
        else
        {
            Debug.Log("Player not detected.");
        }
    }

    //start countdown and once its done do damage/destroy self
    IEnumerator DetonateAfterDelay()
    {
        float remainingTime = detonateDelay;
        while (remainingTime > 0)
        {
            Debug.Log("Detonation Countdown: " + remainingTime.ToString("F1"));
            yield return new WaitForSeconds(0.1f);
            remainingTime -= 0.1f;
        }

        Debug.Log("Detonation!");
        Destroy(gameObject); 
    }

    private void Flip()
    {
        sp.flipY = !sp.flipY;
    }
}
