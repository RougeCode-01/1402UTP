using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    private PlayerController playerController;
    private GrappleGun grappleGun;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        grappleGun = GetComponent<GrappleGun>();
    }

    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = new PlayerInput();

            //movement input
            playerInput.PlayerMovement.Movement.performed += i => playerController.HandleMovementInput(i.ReadValue<Vector2>());

            //jump input
            playerInput.PlayerActions.Jump.performed += i => playerController.HandleJumpInput();
            playerInput.PlayerActions.Jump.canceled += i => playerController.JumpCancel();

            //dash input
            playerInput.PlayerActions.Dash.performed += i => playerController.HandleDashInput();

            //grapple input
            playerInput.PlayerActions.Grapple.performed += i => grappleGun.StartOrStopGrapple(); // Call method to start/stop grapple

            //quit
            playerInput.PlayerActions.Quit.performed += i => playerController.quitGame();
        }
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
}
