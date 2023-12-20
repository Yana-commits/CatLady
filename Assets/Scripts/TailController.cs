using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailController : MonoBehaviour
{
    public List<Transform> tails;

    private Transform _currentPart;
    private Transform _previousPart;

    public void MoveTail(float dir)
    {
        if (tails.Count <= 1)
        {
            return;
        }
        for (int i = 1; i < tails.Count; i++)
        {
            _currentPart = tails[i];
            _previousPart = tails[i - 1];
            var _distance = Vector3.Distance(_previousPart.position, _currentPart.position);
            Vector3 newpos = _previousPart.position;
            float time = Time.deltaTime * _distance * dir;
            if (time > 0.5f)
            { time = 0.5f; }

            _currentPart.position = Vector3.Slerp(_currentPart.position, newpos, time);
            var actualPos = _currentPart.position;
            actualPos.y = 0.3f;
            _currentPart.position = actualPos;
            _currentPart.rotation = Quaternion.Slerp(_currentPart.rotation, _previousPart.rotation, time);
        }
    }
    public void AddCat(GameObject cat)
    {
        Transform lastPart = tails[tails.Count - 1];
        tails.Add(cat.GetComponent<Transform>());
        cat.transform.position = lastPart.position;
        cat.SetActive(true);
    }
    public void ThrowCat(float throwPower, Vector3 direction, Transform throwPos,float bouncePower,float bounceKoef)
    {
        if (tails.Count > 1)
        {
            var catBall = tails[1];
            tails.Remove(catBall);
            catBall.position = throwPos.position;
            var cat = catBall.GetComponent<Cat>();
            cat.NoFreeze();
            cat.BounceSettings(direction, bouncePower, bounceKoef);
            cat.rb.AddForce((direction - Vector3.up*0.25f) * throwPower , ForceMode.Force);
        }
    }
    public void SetCatsFree()
    {
        if (tails.Count > 1)
        {
            while (tails.Count > 1) 
            {
                tails[tails.Count-1].GetComponent<Cat>().BeThrown(3f);
                tails.Remove(tails[tails.Count - 1]);
            }
        }
    }
}
