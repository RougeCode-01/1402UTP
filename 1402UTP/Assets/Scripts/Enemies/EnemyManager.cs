using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    public WallMine wallMine;
    public float reactivationDelay = 5f;

    void Start()
    {
        // Start the coroutine to reactivate the WallMine after delay
        StartCoroutine(ReactivateWallMineAfterDelay());
    }

    IEnumerator ReactivateWallMineAfterDelay()
    {
        yield return new WaitForSecondsRealtime(reactivationDelay);
        ReactivateWallMine();
    }

    void ReactivateWallMine()
    {
        wallMine.gameObject.SetActive(true);
        wallMine.isActive = true;
        Debug.Log("WallMine reactivated. isActive: " + wallMine.isActive);
    }
}
