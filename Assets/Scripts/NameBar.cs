using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameBar : MonoBehaviour
{
    [SerializeField] private GameObject body = null;

    void LateUpdate()
    {
        transform.position = new Vector3(body.transform.position.x, transform.position.y, body.transform.position.z);
        transform.forward = -(Camera.main.transform.position - transform.position);
    }
}
