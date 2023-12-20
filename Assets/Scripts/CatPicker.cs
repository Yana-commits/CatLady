using System;
using UnityEngine;

public class CatPicker : MonoBehaviour
{
    private Owner owner = Owner.None;
    private int id = 100;

    public Action<GameObject> OnPickeUp;
    public Action<Cat> OnHit;
    public Action OnBorder;

    public Owner Owner { get => owner; set => owner = value; }
  
    public void Init(Owner _owner, int _id)
    {
        owner = _owner;
        id = _id;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Cat>(out Cat cat))
        {
            if (!cat.isPicked)
            {
                cat.BePickedUp(Owner,id);
                OnPickeUp?.Invoke(cat.gameObject);
            }
            else if (cat.IsThrown)
            {
                OnHit?.Invoke(cat);
            }
        }
        else if (other.TryGetComponent<Border>(out Border border))
        {
            OnBorder?.Invoke();
        }
    }
}
