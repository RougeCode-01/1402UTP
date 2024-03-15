using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] float loadDelay = 1.0f;
    [SerializeField] float maxFallDistance = -4.0f;
    [SerializeField] float Timer;
    public GameObject player; // Reference to the player GameObject
    PlayerController pc;
    private Vector3 initialPlayerPosition;
    
    void Start()
    {
        // Store the initial position of the player
        initialPlayerPosition = player.transform.position;
        pc = player.GetComponent<PlayerController>();

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
    public void RespawnPlayer()
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
        SceneManager.LoadScene(0); // Reloads the same scene for now
    }
}
