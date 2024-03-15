using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    Vector2 movementInput;
    Rigidbody2D rb;
    Collider2D col;
    SpriteRenderer sp;
    TrailRenderer tr;
    GameManager gm;

    #region Serialized Fields
    [SerializeField]
    int moveSpeed = 500;
    [SerializeField]
    float moveSmoothness = 0.5f;
    [SerializeField]
    int jumpForce = 500, defaultJumpForce;
    [SerializeField]
    int maxAirActions = 1, airActions; //DoubleJump variable, serializable in case we ever want to give the player multiple for whatever reason
    [SerializeField]
    float dashForce = 50f;
    [SerializeField]
    float fallMultiplier = 15f;
    [SerializeField]
    float playerGravity = 1f;
    float playerGravityDefault;
    [SerializeField]
    float rateOfGravityChange = 0.05f;
    [SerializeField]
    float trailFXDelay = 2;
    [SerializeField]
    float maxFallVelocity = -20f; // Maximum fall velocity
    [SerializeField]
    float accelerationRate = 0.5f; // Rate of acceleration
    #endregion
    #region Private/Protected Variables
    private float directionMultiplier;
    #endregion

    bool isGrounded;
    bool isJumping = false;
    bool isDashing = false; // added a dash bool
    bool facingDirection = true; //false = left, true = right

    /*
    ====================================
    TO-DO - PlayerController
    ====================================
    ==CONSTANT TOP PRIORITIES:
    -Adjust basic movement until it feels GOOD
    -Adjust camera options until it feels GOOD

    ==LOW PRIORITY:
    -idk
    -add visual effects (fix dash trail, add step dust)
    -sound effects?
    -wow, i hate that dash code

    */

    // Start is called before the first frame update
    void Start()
    {
        tr.enabled = false; //Disables dash trail on start
    }

    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        tr = GetComponent<TrailRenderer>(); // added trail renderer
        gm = FindObjectOfType<GameManager>();
        defaultJumpForce = jumpForce;
        playerGravityDefault = playerGravity;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(HorizontalMovement(), VerticalMovement()); //Movement system imported from Lecture 6
        isGrounded = GroundCheck();
        if (isGrounded) //If grounded, reset air actions and jump force
        {
            jumpForce = defaultJumpForce;
            airActions = maxAirActions;
            playerGravity = playerGravityDefault;
        }
        else if (!isGrounded)
        {
            ApplyGravity();
        }
        //Debug.Log("ISGROUNDED = " + isGrounded);
        UpdatePlayerDirection();
        directionMultiplier = facingDirection ? 1f : -1f; //float multiplier based on player's facing direction - used for walljump/dash thrusts
    }

    public void UpdatePlayerDirection()
    {
        if (!facingDirection) //if player is not facing right then flip, otherwise don't
            sp.flipX = true;
        else
            sp.flipX = false;
    }

    public void HandleMovementInput(Vector2 movement)
    {
        movementInput.x = Mathf.Lerp(movementInput.x, movement.x * moveSpeed, accelerationRate * Time.fixedDeltaTime); // Apply acceleration
        if (movement.x == 1)            //player presses right, face right
            facingDirection = true;
        else if (movement.x == -1)      //player press left, face left
            facingDirection = false;
    }

    private float HorizontalMovement()
    {
        return Mathf.Lerp(rb.velocity.x, movementInput.x, moveSmoothness);
    }

    public void HandleJumpInput()
    {
        if (WallCheck() && !isGrounded) // Check if the player is in contact with a wall
        {
            // Jump from wall
            WallJump();
        }
        else if (isGrounded || airActions > 0) //check for walljump
        {
            // Normal jump if grounded or having jumps left
            Jump();
            jumpForce = jumpForce * (2 / 3);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Perform jump
        isGrounded = false; // Update grounded status
        isJumping = true; // Update jumping status
        airActions--; // Decrease jump count if applicable
        playerGravity = playerGravityDefault;
    }

    private void WallJump()
    {
        // Apply force for the wall jump
        Vector2 jumpForceVector = new Vector2(-directionMultiplier * moveSpeed, defaultJumpForce * 2 / 3);
        rb.velocity = jumpForceVector;
        rb.AddForce(jumpForceVector, ForceMode2D.Impulse); // Apply impulse force
        isJumping = true; // Update jumping status
        airActions = maxAirActions;
        playerGravity = playerGravityDefault;
    }

    public void JumpCancel()
    {
        if (rb.velocity.y > 0) //makes sure the player isn't cancelling their momentum while already falling
        {
            isJumping = false;
            StartCoroutine(JumpCanceler());
        }
    }

    IEnumerator JumpCanceler() //smooth jump cancel
    {
        float currentY = rb.velocity.y;
        float jumpCancelSmooth = Mathf.Lerp(currentY, Mathf.Abs(0.0f), 0.25f);  //lerps from current y value to 0 at a rate of .25,
        rb.velocity = new Vector2(rb.velocity.x, jumpCancelSmooth);             //then sets the current y value to that lerped value.
        if (currentY == 0.0f)                                                       //(this should mean it takes ~4 fixedupdates to set it to 0? i think)
            StopCoroutine(JumpCanceler());                                      //once the player's y velocity is 0, 
        yield return new WaitForFixedUpdate();
    }

    IEnumerator Dashtrail()
    {
        // This is cheap and dirty. Buggy. Best implementation could maybe check if player is grounded before turning off the trail, giving a "speed jump" effect.
        if (isDashing)
        {
            tr.enabled = true;
            yield return new WaitForSeconds(tr.time * trailFXDelay);
            tr.enabled = false;
            isDashing = false;
        }
        else
        {
            tr.enabled = false;
        }
    }

    public void HandleDashInput()
    {
        if (isGrounded)
        {
            isDashing = true;
            StartCoroutine(Dashtrail());
            Vector2 dashVector = new Vector2((dashForce * 2 / 3) * directionMultiplier, 0f);
            rb.AddForce(dashVector, ForceMode2D.Impulse);
        }
        else if (!isGrounded && airActions > 0)
        {
            isDashing = true;
            StartCoroutine(Dashtrail());
            Vector2 dashVector = new Vector2(dashForce * directionMultiplier, 10f);
            rb.AddForce(dashVector, ForceMode2D.Impulse);
            isJumping = false;
            StartCoroutine(JumpCanceler());
            airActions--;
            playerGravity = playerGravityDefault;
        }
    }

    private float VerticalMovement()
    {
        float jumpVelocity = rb.velocity.y;
        if (jumpVelocity < 0.2f)
        {
            jumpVelocity += Physics2D.gravity.y * (fallMultiplier) * Time.fixedDeltaTime;
        }
        else if (jumpVelocity > 0 && !isJumping)
        {
            jumpVelocity += Physics2D.gravity.y * (fallMultiplier * 2) * Time.fixedDeltaTime;
        }

        // Clamp fall velocity
        jumpVelocity = Mathf.Clamp(jumpVelocity, maxFallVelocity, float.MaxValue);

        return jumpVelocity;
    }

    private void ApplyGravity()
    {
        playerGravity = playerGravity - rateOfGravityChange;
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + playerGravity);
    }

    bool GroundCheck() //magic groundcheck raycast from the lecture. we can repurpose this for walljump colliders if we place two on the sides of the player!
    {
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        float extraHeight = .05f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, extraHeight, LayerMask.GetMask("Ground"));
        #region Debug BoxCast Visual
        Color rayColor;
        if (raycastHit.collider)
        {
            rayColor = Color.red;
        }
        else
            rayColor = Color.green;
        Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, boxCollider2D.bounds.extents.y + extraHeight), Vector2.right * 2f * (boxCollider2D.bounds.extents.x), rayColor);
        Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, boxCollider2D.bounds.extents.y - extraHeight), Vector2.right * 2f * (boxCollider2D.bounds.extents.x), rayColor);
        #endregion
        return raycastHit.collider != null;
    }

    bool WallCheck()
    {
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        float extraWidth = 0.05f; // Adjust this value as needed for better detection

        // Calculate the size of the overlap box
        Vector2 boxSize = new Vector2(extraWidth * 2f, boxCollider2D.bounds.size.y);

        // Calculate the center position of the overlap box on the left and right sides of the player
        Vector2 leftBoxCenter = (Vector2)transform.position + Vector2.left * (boxCollider2D.bounds.extents.x + extraWidth);
        Vector2 rightBoxCenter = (Vector2)transform.position + Vector2.right * (boxCollider2D.bounds.extents.x + extraWidth);

        // Check for overlaps with colliders on the left and right sides
        Collider2D leftCollider = Physics2D.OverlapBox(leftBoxCenter, boxSize, 0f, LayerMask.GetMask("Ground"));
        Collider2D rightCollider = Physics2D.OverlapBox(rightBoxCenter, boxSize, 0f, LayerMask.GetMask("Ground"));

        // Return true if either side has a collider, indicating a wall contact
        return leftCollider != null || rightCollider != null;
    }
    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            gm.Invoke("RespawnPlayer", 0.1f);
        }
    }*/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            gm.Invoke("RespawnPlayer", 0.1f);
        }
    }
}
