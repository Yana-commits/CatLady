using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float speed;
   
    void LateUpdate()
    {
        Vector3 newCamPosition = new Vector3(playerTransform.position.x + offset.x, playerTransform.position.y + offset.y,
                            playerTransform.position.z + offset.z);

        transform.position = Vector3.Lerp(transform.position, newCamPosition, speed *Time.fixedDeltaTime);
    }
}
