using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 movement;
    Rigidbody2D rb;
    int moveSpeed = 500;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.velocity = movement * moveSpeed * Time.fixedDeltaTime;
    }

    public void HandleMovementInput(Vector2 movementInput)
    {
        movement = movementInput;
        movement.Normalize();
    }
    public void HandleJumpInput()
    {

    }
}
