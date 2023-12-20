using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody[] allRigidbodies;
    [SerializeField] private StarEffect starEffect;
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject parent =null;

    private bool isFallen = false;

    public Action OnWalkAgain;

    private void Awake()
    {
        for (int i = 0; i < allRigidbodies.Length; i++)
        {
            allRigidbodies[i].isKinematic = true;
        }
    }
    public void Move(float value)
    {
        animator.SetFloat("Speed", value);
    }
    public void Attack()
    {
        animator.SetTrigger("Attack");
    }
    public void MakePhysical(float walkAgainTime,Vector3 pushDirection,float pushPower)
    {
        if (isFallen == false)
        {
            animator.enabled = false;

            for (int i = 0; i < allRigidbodies.Length; i++)
            {
                allRigidbodies[i].isKinematic = false;
            }
            body.GetComponent<Rigidbody>().AddForce(pushDirection * pushPower, ForceMode.Impulse);

            StartCoroutine(Up(walkAgainTime));
            isFallen = true;
        }
      
    }
    private IEnumerator Up(float walkAgainTime)
    {
       
        yield return new WaitForSeconds(0.5f);
        starEffect.ShowEffect();
        yield return new WaitForSeconds(2.5f);
        starEffect.HideEffect();
        yield return new WaitForSeconds(1f);

        parent.transform.position = body.transform.position;
        body.transform.position = parent.transform.position;
        parent.transform.position = new Vector3(parent.transform.position.x, 1, parent.transform.position.z);

        animator.enabled = true;
        animator.SetTrigger("Up");

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < allRigidbodies.Length; i++)
        {
            allRigidbodies[i].isKinematic = true;
        }

        yield return new WaitForSeconds(walkAgainTime);
        OnWalkAgain?.Invoke();
        isFallen = false;
    }
    public void Idle()
    {
        animator.SetTrigger("Stop");
    }
    public void Win()
    {
        animator.SetTrigger("Win");
    }
}
