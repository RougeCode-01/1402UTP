using UnityEngine;

public class GrappleGun : MonoBehaviour
{
    public float grappleSpeed = 20f; // Speed of the grapple
    public float retractSpeed = 30f; // Speed to retract the player
    public LayerMask grappleMask; // Layer mask to filter what the grapple can attach to
    public LineRenderer ropeRenderer; // Reference to the Line Renderer component
    public float grappleCooldown = 2f; // Cooldown time between consecutive grapples

    private Rigidbody2D rb;
    private SpringJoint2D joint;
    private Vector2 grapplePoint;
    private bool isGrappling = false;
    private bool isCooldown = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component of the player
        ropeRenderer.positionCount = 2; // Set the number of points for the Line Renderer
        ropeRenderer.enabled = false; // Initially hide the rope
    }

    void Update()
    {
        if (isGrappling)
        {
            // Move the player towards the grapple point
            Vector2 grappleDirection = (grapplePoint - rb.position).normalized;
            rb.velocity = grappleDirection * grappleSpeed;

            // Update the rope renderer to show the rope
            ropeRenderer.SetPosition(0, transform.position);
            ropeRenderer.SetPosition(1, grapplePoint);
            ropeRenderer.enabled = true; // Show the rope

            // Check if player is close to grapple point to stop grappling
            if (Vector2.Distance(rb.position, grapplePoint) < 1f)
            {
                StopGrapple();
            }
        }
        else
        {
            ropeRenderer.enabled = false; // Hide the rope when not grappling
        }

        // Update cooldown
        if (isCooldown)
        {
            grappleCooldown -= Time.deltaTime;
            if (grappleCooldown <= 0)
            {
                isCooldown = false;
                grappleCooldown = 2f; // Reset cooldown time
            }
        }
    }

    public void StartOrStopGrapple()
    {
        if (!isGrappling && !isCooldown)
        {
            StartGrapple();
        }
        else
        {
            StopGrapple();
        }
    }

    public void StartGrapple()
    {
        // Shoot a raycast to find a surface to grapple onto
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.up, Mathf.Infinity, grappleMask);
        if (hit.collider != null)
        {
            // Set the grapple point to the hit point of the raycast
            grapplePoint = hit.point;

            // Create a SpringJoint2D component to attach the player to the grapple point
            joint = gameObject.AddComponent<SpringJoint2D>();
            joint.autoConfigureDistance = false;
            joint.connectedAnchor = grapplePoint;
            joint.distance = 0.1f; // Set a short initial distance to avoid glitches
            joint.frequency = 1f; // Set the frequency of the spring (controls elasticity)
            joint.dampingRatio = 0.5f; // Set the damping ratio (controls damping)
            isGrappling = true; // Set grappling state to true
            isCooldown = true; // Start cooldown
        }
    }

    public void StopGrapple()
    {
        // Remove the SpringJoint2D component to stop grappling
        if (joint != null)
        {
            Destroy(joint);
            isGrappling = false; // Set grappling state to false
        }
    }
}
