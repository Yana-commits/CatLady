using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadyController : MonoBehaviour, IDestructable
{
    [Header("Move Controller")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider collider;
    [SerializeField] private float speed = 5;
    [SerializeField] private float turnSpeed = 360;
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private DynamicJoystick _joystick;
    [Header("Behavors")]
    [SerializeField] private TailController tailController;
    [SerializeField] private CatPicker catPicker;
    [SerializeField] private CreatureAnimator animator;
    [Header("Throw Settings")]
    [SerializeField] private float throwPower;
    [SerializeField] private float bouncePowerUp;
    [SerializeField] private float bounceKoefDown = 0.1f;
    [SerializeField] private float pushPower;
    [SerializeField] private Transform throwPos;

    private Vector3 input;
    private List<Enemy> enemies = new List<Enemy>();
    private bool isMooving = true;

    public List<Enemy> Enemies { get => enemies; set => enemies = value; }

    public Action<int> OnHit;
    private void Start()
    {
        catPicker.OnPickeUp += AddNewCat;
        _joystick.OnPointreUpAction += Throw;
        catPicker.OnHit += Hit;
        catPicker.Init(Owner.Player, 100);
        catPicker.OnBorder += Borders;
        animator.OnWalkAgain += WalkAgain;
    }
    void Update()
    {
        if (isMooving)
        {
            GatherInput();
            Look();
            //Move();
        }
        else
        {
            input = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        if (isMooving)
            Move();
    }

    #region Move region
    private void GatherInput()
    {
        input = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
    }
    private void Look()
    {
        if (input != Vector3.zero)
        {
            /*var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 0, 0));
​
            var skewedInput = matrix.MultiplyPoint3x4(input);
​
            var relative = (transform.position + skewedInput) - transform.position;
            var rot = Quaternion.LookRotation(relative, Vector3.up);
​
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);*/
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, input, 10 * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }
    private void Move()
    {
        //rb.MovePosition(transform.position + (transform.forward * input.magnitude) * speed * Time.deltaTime);
        transform.Translate(input * Time.deltaTime * speed, Space.World);
        animator.Move(input.magnitude);
        tailController.MoveTail(input.magnitude);
    }
    private void Borders()
    {
        rb.AddForce(-input  * 100);
        StartCoroutine(BordersReaction());
    }
    private IEnumerator BordersReaction()
    {
        isMooving = false;
        yield return new WaitForSeconds(0.25f);
        isMooving = true;
    }
    #endregion
    private void AddNewCat(GameObject cat)
    {
        tailController.AddCat(cat);
    }

    #region Throw methods
    private void Throw()
    {
        if (tailController.tails.Count > 1 && isMooving)
        {
            StartCoroutine(ThrowCat(input.normalized));
        }
    }

    private IEnumerator ThrowCat(Vector3 direction)
    {
        animator.Attack();
        yield return new WaitForSeconds(0.7f);

        var dir = FindTarget(direction);
        tailController.ThrowCat(throwPower, dir, throwPos, bouncePowerUp,bounceKoefDown);
    }
    private Vector3 FindTarget(Vector3 direction)
    {
        var dir = direction;
        for (var i = 0; i < enemies.Count; i++)
        {
            if ((transform.position - enemies[i].gameObject.transform.position).magnitude < 15)
            {
                var angle = Vector3.Angle(transform.forward, transform.position - enemies[i].gameObject.transform.position);
                if (angle < 15 || angle > 165)
                {
                    dir = (transform.position - enemies[i].gameObject.transform.position) * -0.25f;
                    break;
                }
            }

        }
        return dir;
    }

    #endregion

    public void Hit(Cat cat)
    {
        if (cat.Owner == Owner.Enemy)
        {
            OnHit?.Invoke(cat.OwnerId);
            collider.enabled = false;
            cat.GoDown();
            isMooving = false;
            tailController.SetCatsFree();
            animator.MakePhysical(2f, cat.Direction,pushPower);
        }
    }
    private void WalkAgain()
    {
        collider.enabled = true;
        isMooving = true;
    }
    public void GameOver()
    {
        isMooving = false;
        animator.Win();
    }
    
    private void OnDisable()
    {
        catPicker.OnPickeUp -= AddNewCat;
        _joystick.OnPointreUpAction -= Throw;
        catPicker.OnHit -= Hit;
        catPicker.OnBorder -= Borders;
        animator.OnWalkAgain -= WalkAgain;
    }
}
