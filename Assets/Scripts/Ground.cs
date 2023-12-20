using System.Collections;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private int counter = 0;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Cat>(out Cat cat))
        {
            if (cat.IsThrown)
            {
                if (counter == 0)
                {
                    cat.Bounce();
                    counter++;

                    if (cat.IsHit == false)
                        counter = 0;
                    cat.IsHit = true;
                    cat.BeThrown(0.5f);
                }
                else
                {
                    //cat.IsThrown = false;
                    cat.BeThrown(0.5f);
                    counter = 0;
                }
                cat.ShowEffect(cat.groundTouchPrefab);

                StartCoroutine(HideEffect(cat));
            }
        }
    }
    private IEnumerator HideEffect(Cat cat)
    {
        yield return new WaitForSeconds(0.5f);
        cat.StopEffect(cat.groundTouchPrefab);
    }
}
