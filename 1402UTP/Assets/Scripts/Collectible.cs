using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    AudioManager sfx;
    public static event Action OnCollected;
    public static int totalcoin = 0;

    // Start is called before the first frame update

    private void Awake()
    {
        sfx = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        totalcoin++;
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnCollected?.Invoke();
            sfx.PlaySFX(sfx.coin);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.localRotation = Quaternion.Euler(0, Time.time * 100f, 0);
    }
}
