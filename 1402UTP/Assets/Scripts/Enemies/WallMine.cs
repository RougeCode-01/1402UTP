using UnityEngine;
using System.Collections;

public class WallMine : Enemy
{
    public GameObject pointA;
    public GameObject pointB;
    public BoxCollider2D col;
    public Rigidbody2D rb;
    public SpriteRenderer sp;
    public ParticleSystem ps;
    public Animator anim;
    public float speed = 2f;
    private Transform currentTarget;
    public float detonateRadius = 1.5f;
    public float detonateDelay = 1.5f;
    public LayerMask playerLayer;
    public string playerTag = "Player";
    public GameObject objectToDeactivate;

    public Color activeColor = Color.white;
    public Color inactiveColor = Color.red;

    private SpriteRenderer spriteRenderer;
    private bool isPulsing = false;

    public bool isActive = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = activeColor;
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        ps = GetComponentInChildren<ParticleSystem>(); // Assuming ParticleSystem is a child object
        currentTarget = pointB.transform;
    }

    void FixedUpdate()
    {
        if (isActive)
        {
            DetonateIfPlayerClose();
            MoveBetweenPoints();
        }
        else
        {
            StartCoroutine(ReactivateAfterDelay(5f));
        }
    }

    void MoveBetweenPoints()
    {
        Vector2 direction = (currentTarget.position - transform.position).normalized;
        rb.velocity = direction * speed;

        if (Vector2.Distance(transform.position, currentTarget.position) < 0.5f)
        {
            currentTarget = (currentTarget == pointA.transform) ? pointB.transform : pointA.transform;
        }
    }

    void DetonateIfPlayerClose()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detonateRadius, playerLayer);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag(playerTag))
            {
                StartCoroutine(DetonateAfterDelay());
                break; // Exit loop after one detonation
            }
        }
    }

    IEnumerator DetonateAfterDelay()
    {
        float remainingTime = detonateDelay;
        while (remainingTime > 0)
        {
            // Pulse effect: toggle between active and inactive colors
            if (!isPulsing)
            {
                spriteRenderer.color = inactiveColor;
            }
            else
            {
                spriteRenderer.color = activeColor;
            }
            isPulsing = !isPulsing;

            yield return new WaitForSecondsRealtime(0.1f);
            remainingTime -= 0.1f;
        }
        // Set sprite color back to active after countdown
        spriteRenderer.color = activeColor;
        Debug.Log("Detonation!");
        col.edgeRadius = 3;
        ps.Play();
        Invoke("Deactivate", 1.5f);
    }

    void Deactivate()
    {
        if (objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(false);
        }
        isActive = false;
        Debug.Log("WallMine deactivated. isActive: " + isActive);
    }

    void Reactivate()
    {
        if (objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(true);
        }
        isActive = true;
        Debug.Log("WallMine reactivated. isActive: " + isActive);
    }

    IEnumerator ReactivateAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Reactivate();
    }
}
