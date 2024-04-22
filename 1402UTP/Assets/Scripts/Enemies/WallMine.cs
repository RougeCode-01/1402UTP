using UnityEngine;
using System.Collections;

public class WallMine : Enemy
{
    [Header("Movement Settings")]
    public GameObject pointA;
    public GameObject pointB;
    public float speed = 2f;

    [Header("Detonation Settings")]
    public float detonateRadius = 1.5f;
    public float detonateDelay = 1.5f;
    public float reactivateDelay = 5f;
    public float deactivateDelay = 0.5f;
    public LayerMask playerLayer;
    public string playerTag = "Player";

    [Header("Appearance Settings")]
    public Color activeColor = Color.white;
    public Color inactiveColor = Color.red;

    private Transform currentTarget;
    private float originalColliderRadius;

    private SpriteRenderer spriteRenderer;
    private bool isPulsing = false;

    public bool isActive = true;

    [Header("Components")]
    private BoxCollider2D col;
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    private ParticleSystem ps;
    private Animator anim;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = activeColor;
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        ps = GetComponentInChildren<ParticleSystem>(); 
        currentTarget = pointB.transform;
        originalColliderRadius = col.edgeRadius; // Store original collider radius
    }

    void FixedUpdate()
    {
          DetonateIfPlayerClose();
          MoveBetweenPoints();
    }

    void MoveBetweenPoints()
    {
            Debug.Log("Current Position: " + transform.position);
            Debug.Log("Target Position: " + currentTarget.position);

            transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, currentTarget.position) < 0.5f)
            {
                currentTarget = (currentTarget == pointA.transform) ? pointB.transform : pointA.transform;
                Debug.Log("Switched target.");
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
        col.edgeRadius = 2;//should add a variable
        ps.Play();
        Invoke("Deactivate", deactivateDelay);
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Reactivate()
    {
       gameObject.SetActive(true);
        // Reset position
        transform.position = pointA.transform.position;

        // Reset velocity
        rb.velocity = Vector2.zero;

        // Reactivate collider
        col.enabled = true;
        col.edgeRadius = originalColliderRadius;

        // Reset sprite color
        spriteRenderer.color = activeColor;

        // Stop particle system
        ps.Stop();
    }
}
