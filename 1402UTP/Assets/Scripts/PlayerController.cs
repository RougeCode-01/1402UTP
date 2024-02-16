using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 movement;
    Rigidbody2D rb;
    Collider2D col;

    #region Serialized Fields
    [SerializeField]
    int moveSpeed = 500;
    [SerializeField]
    int moveSmoothness = 1;
    [SerializeField]
    int jumpForce = 500;
    #endregion

    bool isGrounded;

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
        HorizonatalMovement();

    }

    public void HorizonatalMovement()
    {
        //rb.velocity = movement * moveSpeed * Time.fixedDeltaTime;
        //rb.velocity = new Vector2(movement.x * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(movement.x * moveSpeed * Time.fixedDeltaTime, rb.velocity.y), moveSmoothness * Time.fixedDeltaTime);//This is the same as the line above, but with smoothness
    }

    public void HandleMovementInput(Vector2 movementInput)
    {
        //movement = movementInput;
        //movement.Normalize();
        movement = movementInput;
        movement = Vector2.Lerp(movement, movementInput, moveSmoothness * Time.fixedDeltaTime);
    }
    public void HandleJumpInput()
    {
        if (col.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            isGrounded = true;
            rb.AddForce(Vector2.up * jumpForce);
        }
        else
        {
            isGrounded = false;
        }
    }

}
