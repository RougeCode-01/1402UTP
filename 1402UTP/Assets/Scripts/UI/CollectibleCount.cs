using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleCount : MonoBehaviour
{
    TMPro.TMP_Text text;
    int count = 0;
    // Start is called before the first frame update

    public void Start()
    {
        UpdateCount();
    }
    private void OnEnable()
    {
        Collectible.OnCollected += OnCollectibleCollected;
    }
    private void OnDisable() 
    { 
        Collectible.OnCollected -= OnCollectibleCollected;
    }
    private void Awake()
    {
        text = GetComponent<TMPro.TMP_Text>();
    }

    void OnCollectibleCollected()
    {
        count++;
        UpdateCount();
    }

    void UpdateCount()
    {
        text.text = $"{count} / {Collectible.totalcoin}";
    }
}
