using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] float loadDelay = 999999999999999999999999999999f;
    [SerializeField] float maxFallDistance = -4.0f;
    [SerializeField] float distanceFromFlag = 0.5f;
    [SerializeField] int LevelSelect = 0;

    public GameObject player; // Reference to the player GameObject
    PlayerController pc;
    public Transform StartPoint;
    public Transform checkpoint;
    public Transform finishLine;
    AudioManager sfx;
    

    private Vector3 initialPlayerPosition;
    private bool checkpointReached = false;
    private bool cp_sfxplay = false;
    private bool finish_sfxplay = false;

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
        pc = player.GetComponent<PlayerController>();

    }
    private void Awake()
    {
        sfx = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
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
    public void RespawnPlayer()
    {
        // Reset player position to the initial position
        // yandev.jpg
        player.transform.position = initialPlayerPosition;
        player.GetComponent<SpriteRenderer>().enabled = true;
        player.GetComponent<BoxCollider2D>().enabled = true;
        player.GetComponent<GrappleGun>().enabled = true;
        player.GetComponent<LineRenderer>().enabled = true;
        player.GetComponent<Rigidbody2D>().simulated = true;
        //player.GetComponent<PlayerInputController>().enabled = true;
        pc.enabled = true;
        pc.isDead = false;
        sfx.PlaySFX(sfx.portal);
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
            if (cp_sfxplay == false)
               {
                sfx.PlaySFX(sfx.checkpoint);
                cp_sfxplay = true;
                }
        }
        if (Vector3.Distance(player.transform.position, checkpoint.position) > distanceFromFlag)
        {
            cp_sfxplay = false;
        }

            // Check if the player is close to the finish line
            if (Vector3.Distance(player.transform.position, finishLine.position) < distanceFromFlag)
        {
            // Call NextScene function after the specified load delay
            if (finish_sfxplay == false)
            {
                sfx.PlaySFX(sfx.portal);
                finish_sfxplay = true;
            }
            Debug.Log("Finish line reached. Loading next scene...");
            Invoke("NextScene", loadDelay);
        }
        if (Vector3.Distance(player.transform.position, finishLine.position) > distanceFromFlag)
        {
            finish_sfxplay = false;
        }
    }

    private void NextScene()
    {
        // Load the next level or reload the same scene
        LevelSelect++;
        SceneManager.LoadScene(LevelSelect); // Reloads the same scene for now
        Collectible.totalcoin = 0;
    }
}
