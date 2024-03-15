using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] float loadDelay = 0.01f;
    [SerializeField] float maxFallDistance = -4.0f;
    [SerializeField] float distanceFromFlag = 0.5f;
    [SerializeField] int LevelSelect = 0;

    public GameObject player; // Reference to the player GameObject
    public Transform StartPoint;
    public Transform checkpoint;
    public Transform finishLine;

    private Vector3 initialPlayerPosition;
    private bool checkpointReached = false;

    void Start()
    {
        // Set the player's initial position to the start point
        if (StartPoint != null)
        {
            player.transform.position = StartPoint.position;
        }
        else
        {
            Debug.LogWarning("StartPoint is not assigned. Player may start at default position.");
        }

        // Store the initial position of the player
        initialPlayerPosition = player.transform.position;
    }

    void Update()
    {
        // Check if the player's Y position is below the maximum fall distance
        if (player.transform.position.y < maxFallDistance)
        {
            RespawnPlayer(); // Respawn the player
        }

        // Check for checkpoint and finish line collisions
        CheckForCollisions();
    }

    // Respawn the player
    private void RespawnPlayer()
    {
        // Reset player position to the initial position or the last checkpoint if reached
        if (checkpointReached)
        {
            player.transform.position = checkpoint.position;
        }
        else
        {
            player.transform.position = initialPlayerPosition;
        }
    }

    // Check for collisions with checkpoint and finish line
    private void CheckForCollisions()
    {
        // Check if the player is close to the checkpoint
        if (Vector3.Distance(player.transform.position, checkpoint.position) < distanceFromFlag)
        {
            checkpointReached = true;
            Debug.Log("Checkpoint reached. Checkpoint spawn point updated to: " + checkpoint.position);
        }

        // Check if the player is close to the finish line
        if (Vector3.Distance(player.transform.position, finishLine.position) < distanceFromFlag)
        {
            // Call NextScene function after the specified load delay
            Debug.Log("Finish line reached. Loading next scene...");
            Invoke("NextScene", loadDelay);
        }
    }

    private void NextScene()
    {
        // Load the next level or reload the same scene
        SceneManager.LoadScene(LevelSelect); // Reloads the same scene for now
    }
}
