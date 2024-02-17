using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 movementInput;
    Rigidbody2D rb;
    Collider2D col;

    #region Serialized Fields
    [SerializeField]
    int moveSpeed = 500;
    [SerializeField]
    float moveSmoothness = 0.5f;
    [SerializeField]
    int jumpForce = 500;
    [SerializeField]
    int maxJumps = 1, jumps; //DoubleJump variable, serializable in case we ever want to give the player multiple for whatever reason
    [SerializeField]
    float playerGravity = 15f;
    #endregion
    #region Private/Protected Variables
    //nothing here yet
    #endregion

    bool isGrounded;
    bool isJumping = false;

    /*
    ====================================
    TO-DO - PlayerController
    ====================================
    ==CONSTANT TOP PRIORITIES:
    -Adjust basic movement until it feels GOOD
    -Adjust camera options until it feels GOOD

    ==IMPORTANT BUT NOT AS IMPORTANT I GUESS:
    -Add dashing, airdashing 
    -Add walljumps, wall collision check
    -Grappling hook 

    */

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(HorizontalMovement(), VerticalMovement()); //Movement system imported from Lecture 6
        isGrounded = GroundCheck();
        if(isGrounded) //If grounded, give the player back their doublejump
        {
            jumps = maxJumps;
        }
        //Debug.Log("ISGROUNDED = " + isGrounded);

    }
    public void HandleMovementInput(Vector2 movement)
    {
        movementInput.x = movement.x;
    }
    private float HorizontalMovement()
    {
        return Mathf.Lerp(rb.velocity.x, movementInput.x * moveSpeed, moveSmoothness);
    }
    public void HandleJumpInput()
    {
        if (isGrounded || jumps > 0) //check for doublejump
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); //this could be way better! the player's ascent is really slow, experiment with making it snappy like the descent!
            isGrounded = false;
            isJumping = true;
            jumps --;
        }
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

    private float VerticalMovement()
    {
        float jumpVelocity = rb.velocity.y;
        if (jumpVelocity < 0.2f)
        {
            jumpVelocity += Physics2D.gravity.y * (playerGravity) * Time.fixedDeltaTime;
        }
        else if (jumpVelocity > 0 && !isJumping)
        {
            jumpVelocity += Physics2D.gravity.y * (playerGravity * 2) * Time.fixedDeltaTime;
        }
        return jumpVelocity;
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

}