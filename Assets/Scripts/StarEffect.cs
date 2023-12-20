using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem confettiPrefab;
    [SerializeField] private Transform headPos;

    private void Awake()
    {
        confettiPrefab.Stop();
    }
    private void Update()
    {
        confettiPrefab.gameObject.transform.position = new Vector3(headPos.position.x, 1f, headPos.position.z);
    }
    public void ShowEffect()
    {
        
        confettiPrefab.Play();
    }

    public void HideEffect()
    {
        confettiPrefab.Stop();
        confettiPrefab.transform.position = headPos.position;
    }
}
