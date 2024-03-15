using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] float loadDelay = 1.0f;
    [SerializeField] float maxFallDistance = -4.0f;
    [SerializeField] int LevelSelect = 0;
    public GameObject player; // Reference to the player GameObject
    public Collider2D finishLineCol;
    public Collider2D checkpointCol;
    private Vector3 initialPlayerPosition;

    /*
    TODO
    Check if the player collides with the checkpoint and set the respawn to the checkpoint
    Chack if the player collides with the finish line and if they do send them to the next level
    */
    
    void Start()
    {
        // Store the initial position of the player
        initialPlayerPosition = player.transform.position;
    }

    void Update()
    {
        // Check if the player's Y position is below 0.00
        if (player.transform.position.y < maxFallDistance)
        {
            RespawnPlayer(); // Respawn the player
        }
    }

    // Respawn the player
    private void RespawnPlayer()
    {
        // Reset player position to the initial position
        player.transform.position = initialPlayerPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Invoke("NextScene", loadDelay);
        }
    }

    private void NextScene()
    {
        SceneManager.LoadScene(LevelSelect); // Reloads the same scene for now
    }
}
