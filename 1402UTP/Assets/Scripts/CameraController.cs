using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController: MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 velocity = Vector3.zero;
    [SerializeField] [Range(0,1)] private float smoothTime;
    [SerializeField] private Vector3 positionOffset;
    [Header("Camera Limits")]
    [SerializeField] private Vector2 xLimit; // Horizontal Limit, could perhaps be tied to an object for easier level design? placing a "camera boundary" object, etc
    [SerializeField] private Vector2 yLimit; // Vertical Limit

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }


    // Start is called before the first frame update
    void Start()
    {

    }
   

    // Update is called once per frame
    private void LateUpdate()
    {
        Vector3 targetPosition = target.position+positionOffset;
        targetPosition = new Vector3(Mathf.Clamp(targetPosition.x, xLimit.x, xLimit.y), Mathf.Clamp(targetPosition.y, yLimit.x, yLimit.y), -10);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
