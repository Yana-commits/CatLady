using System;
using System.Collections;
using UnityEngine;

public class Enemy : NavMeshController, IDestructable
{
    [Header("Behavors")]
    [SerializeField] private TailController tailController;
    [SerializeField] private CatPicker catPicker;
    [SerializeField] private CreatureAnimator animator;
    [SerializeField] private ParticleSystem smilePrefab;
    [Header("Throw Settings")]
    [SerializeField] private Transform throwPos;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider collider;
    [Header("Enemy Settings")]
    [SerializeField] private int walkNumbers = 0;

    private int id;
    private int counter = 0;
    private bool isLookingCat = false;
    private Transform targetCat;
    private GameObject player;
    private float throwPower;
    private float bouncePower;
    private bool isThrowing = false;
    private float pushPower;
    private float bounceKoef;

    public Action OnChaseCat;

    public bool IsLookingCat { get => isLookingCat; set => isLookingCat = value; }
    public GameObject Player { get => player; set => player = value; }
    public int Id { get => id; set => id = value; }

    public void Start()
    {
        NavRandomPoint();
        catPicker.OnPickeUp += AddNewCat;
        catPicker.OnHit += Hit;
        animator.OnWalkAgain += MoveAgain;
        catPicker.Init(Owner.Enemy, Id);
    }
    private void Update()
    {
        if ((transform.position - randomPoint).magnitude < 1f && !IsLookingCat)
        {
            if (counter < walkNumbers)
            {
                NavRandomPoint();
                counter++;
            }
            else
            {
                counter = 0;
                IsLookingCat = true;
                OnChaseCat?.Invoke();
            }
        }
        else if (IsLookingCat && targetCat != null && agent.enabled == true)
        {
            agent.SetDestination(targetCat.position);
        }

        WatchTarget(transform.position - Player.transform.position);
    }
    private void FixedUpdate()
    {
        if (!IsLookingCat)
        {
            tailController.MoveTail(0.99f);
        }
        else
        {
            tailController.MoveTail(0.99f);
        }
    }

    public void EnemySettings(GameObject _player, float _throwPower, float _bouncePower, Action _OnChaseCat, float _pushPower, float _bounceKoef)
    {
        player = _player;
        throwPower = _throwPower;
        bouncePower = _bouncePower;
        OnChaseCat = _OnChaseCat;
        pushPower = _pushPower;
        bounceKoef = _bounceKoef;
    }
    public void ChaseCat(Transform _targetCat)
    {
        targetCat = _targetCat;
    }

    private void AddNewCat(GameObject cat)
    {
        tailController.AddCat(cat);
        IsLookingCat = false;
        NavRandomPoint();
    }
    public void WatchTarget(Vector3 targetDirection)
    {
        if (targetDirection.magnitude < 7 && !isThrowing && tailController.tails.Count > 1)
        {
            StartCoroutine(ThrowTimer());
        }
    }

    private IEnumerator ThrowTimer()
    {
        isThrowing = true;
        agent.Stop();
        agent.isStopped = true;
        RotateToPlayer();
        animator.Attack();
        yield return new WaitForSeconds(0.7f);

        tailController.ThrowCat(throwPower, (player.transform.position - transform.position).normalized, throwPos, bouncePower, bounceKoef);

        yield return new WaitForSeconds(0.5f);
        MoveAgain();
        yield return new WaitForSeconds(4f);
        isThrowing = false;
    }

    private void RotateToPlayer()
    {
        Vector3 targetDirection = player.transform.position - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.position - randomPoint, targetDirection, 10f, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
    public void Hit(Cat cat)
    {
        if (cat.Owner == Owner.Player || cat.OwnerId != Id)
        {
            collider.enabled = false;
            cat.GoDown();
            //agent.enabled = false;
            agent.Stop();
            agent.isStopped = true;

            agent.velocity = Vector3.zero;
            tailController.SetCatsFree();

            animator.MakePhysical(0.5f, cat.Direction,pushPower);
        }
    }
    public void MoveAgain()
    {
        collider.enabled = true;
        //agent.enabled = true;
        agent.Resume();
        agent.isStopped = false;
        NavRandomPoint();
    }
  
    public void ShowSmile()
    {
        smilePrefab.gameObject.SetActive(true);
        smilePrefab.Play();
    }

    private void OnDisable()
    {
        catPicker.OnPickeUp -= AddNewCat;
        animator.OnWalkAgain -= MoveAgain;
    }
}
