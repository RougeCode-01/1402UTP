using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineCameraOffset : MonoBehaviour
{
    public SpriteRenderer characterSprite; // My dude
    public CinemachineVirtualCamera cinemachineCamera;
    public Vector3 offsetWhenFlipped; // Oh, this is nasty
    public float smoothTime = 0.2f;

    private CinemachineFramingTransposer transposer;
    private Vector3 originalOffset;

    void Start()
    {
        transposer = cinemachineCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        originalOffset = transposer.m_TrackedObjectOffset;
        offsetWhenFlipped = transposer.m_TrackedObjectOffset * -1; // why did i not do this in the first place
    }

    void Update()
    {
        // I feel like there has to be a better way to do this within unity..
        // This tracks the character sprite's flip.x and changes the "Camera Offset" in cinemachine to match.
        // With a smoothtime lerp to make it feel less jank, as it looked nauseating without it.
        Vector3 targetOffset = characterSprite.flipX ? offsetWhenFlipped : originalOffset;
        Vector3 smoothedOffset = Vector3.Lerp(transposer.m_TrackedObjectOffset, targetOffset, smoothTime);
        transposer.m_TrackedObjectOffset = smoothedOffset;
    }
}
