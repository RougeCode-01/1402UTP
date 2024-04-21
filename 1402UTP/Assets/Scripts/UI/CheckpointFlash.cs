using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckpointFlash : MonoBehaviour
{
    public TextMeshProUGUI checkpointIndicator;
    public TextMeshProUGUI finishIndicator;
    public float flashDuration = 1.0f;
    public Color inviscolor = Color.white;
    public Color viscolor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        inviscolor.a = 0;
        checkpointIndicator.color = inviscolor;
        finishIndicator.color = inviscolor;
    }

    // Update is called once per frame
    public void CallCheckpointFlash()
    {
        StartCoroutine(Flash());
    }
    public void CallFinishFlash()
    {
        StartCoroutine(FinishFlash());
    }


    private IEnumerator Flash()
    {
        checkpointIndicator.color = viscolor;
        yield return new WaitForSeconds(flashDuration);
        checkpointIndicator.color = inviscolor;
    }
    private IEnumerator FinishFlash()
    {
        finishIndicator.color = viscolor;
        yield return new WaitForSeconds(flashDuration);
        finishIndicator.color = inviscolor;
    }
}
