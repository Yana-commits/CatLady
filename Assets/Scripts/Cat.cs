using System;
using System.Collections;
using UnityEngine;

public class Cat : NavMeshController
{
    [SerializeField] private Material[] materials = new Material[2];
    [SerializeField] private SkinnedMeshRenderer meshRenderer;

    private bool isThrown = false;
    private Vector3 direction;
    private float bouncePower;
    private Owner owner = Owner.None;
    private int ownerId;
    private bool isHit = true ;
    private float bounceKoef =0.1f;
    private int id;

    [Header("Particles & Phis set")]
    public ParticleSystem trailPrefab;
    public ParticleSystem hitPrefab;
    public ParticleSystem groundTouchPrefab;
    public Rigidbody rb;
    public bool isPicked = false;
    public Action<int,int> OnPickedUp;

    public bool IsThrown { get => isThrown; set => isThrown = value; }
    public Vector3 Direction { get => direction; set => direction = value; }
    public Owner Owner { get => owner; set => owner = value; }
    public bool IsHit { get => isHit; set => isHit = value; }
    public int OwnerId { get => ownerId; set => ownerId = value; }
    public int Id { get => id; set => id = value; }

    private void Update()
    {
        if (isPicked)
            return;

        if ((transform.position - randomPoint).magnitude < 1f)
        {
            NavRandomPoint();
        }
    }

    public void SetCat()
    {
        meshRenderer.material = materials[0];
        NavRandomPoint();
    }
    public void BounceSettings(Vector3 _direction, float _bouncePower, float _bounceKoef)
    {
        direction = _direction;
        bouncePower = _bouncePower;
        bounceKoef = _bounceKoef;
        ShowEffect(trailPrefab);
    }
    public void BePickedUp(Owner _owner,int _ownerId)
    {
        isPicked = true;
        gameObject.SetActive(false);
        //rb.constraints = RigidbodyConstraints.FreezeAll;
        agent.enabled = false;
        meshRenderer.material = materials[1];
        Owner = _owner;
        OwnerId = _ownerId;
        OnPickedUp?.Invoke(Id, OwnerId);
    }
    public void BeThrown(float noPickedTime)
    {
        StartCoroutine(ThrowCat(noPickedTime));
    }

    private IEnumerator ThrowCat(float noPickedTime)
    {
        agent.enabled = true;
        Owner = Owner.None;
        StopEffect(trailPrefab);
        meshRenderer.material = materials[0];
        NavRandomPoint();
        yield return new WaitForSeconds(noPickedTime);

        isPicked = false;
        IsThrown = false;
    }
    public void NoFreeze()
    {
        //rb.constraints = RigidbodyConstraints.None;
        //rb.constraints = RigidbodyConstraints.FreezeRotation;
        IsThrown = true;
    }
    public void Bounce()
    {
        if (IsHit == true)
        {
            rb.AddForce((direction + Vector3.up * bounceKoef) * bouncePower, ForceMode.Force);
        }
        else
        {
            IsThrown = false;
            BeThrown(0.5f);
        }
    }
    public void GoDown()
    {
        ShowEffect(hitPrefab);
        IsHit = false;
        rb.AddForce(-Vector3.up*1000);
        StopEffect(hitPrefab);
    }

    public void ShowEffect(ParticleSystem particles)
    {
        particles.gameObject.SetActive(true);
        particles.Play();
    }
    public void StopEffect(ParticleSystem particles)
    {
        particles.Stop();
        particles.gameObject.SetActive(false);
    }
}

public enum Owner
{
    None,
    Player,
    Enemy
}