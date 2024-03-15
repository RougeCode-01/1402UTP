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
    private Vector3 initialPlayerPosition;
    private Vector3 checkpoinPosition;


    
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
