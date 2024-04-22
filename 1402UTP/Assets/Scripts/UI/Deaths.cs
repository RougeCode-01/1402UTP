using UnityEngine;
using TMPro;

public class Deaths : MonoBehaviour
{
    public TMP_Text deathCounterText;
    public float deathAmount;

    void Update()
    {
        ShowCount();
    }
    public void IncreaseDeaths()
    {
        deathAmount++;
    }
    public void ResetDeaths()
    {
        deathAmount = 0f;
    }
    void ShowCount()
    {
        if (deathCounterText != null)
        {
            deathCounterText.text = deathAmount.ToString();
        }
        else
        {
            Debug.LogWarning("Death Counter Text not assigned in Deaths script.");
        }
    }
}
