using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class PatrolEnemy : Enemy
{
    public GameObject pointA;
    public GameObject pointB;
    public Rigidbody2D rb;
    public SpriteRenderer sp;
    public Animator anim;
    public float speed = 2f;
    private Transform currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        currentTarget = pointB.transform; // Moves to Point B at first
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = currentTarget.position - transform.position;
        if(currentTarget == pointB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
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

    private void Flip()
    {
        if (sp.flipX == true)
        {
            sp.flipX = false;
        }
        else
        {
            sp.flipX=true;
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
