using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMove : MonoBehaviour
{
    [SerializeField] private float speed;

    private Sprite _player;
    private Rigidbody2D _rb;
    private Vector2 _moveVelocity;
    private Animator _animator;
    public int playerHealth;
    public Collider2D attackCollider;
    private static readonly int X = Animator.StringToHash("X");
    private static readonly int Y = Animator.StringToHash("Y");
    private static readonly int IsIdle = Animator.StringToHash("isIdle");
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private static readonly int IsAttacking = Animator.StringToHash("isAttacking");

    private void Start() 
    {
        playerHealth = GetComponent<Health>().maxHealth;
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        _moveVelocity.x = Input.GetAxis("Horizontal");
        _moveVelocity.y = Input.GetAxis("Vertical");
        Attack();
    }
    private void FixedUpdate() 
    {
        Move();
    }

    private void Move()
    {
        Vector2 move = new Vector2(_moveVelocity.x, _moveVelocity.y).normalized;
        _rb.MovePosition(_rb.position + (move * (speed * Time.fixedDeltaTime)));

        if (move != Vector2.zero)
        {
            _animator.SetFloat(X, move.x);
            _animator.SetFloat(Y, move.y);
            _animator.SetBool(IsIdle, false);
            _animator.SetBool(IsWalking, true);
        }
        else
        {
            _animator.SetBool(IsWalking, false);
            _animator.SetBool(IsIdle, true);
        }
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetBool(IsAttacking, true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            _animator.SetBool(IsAttacking, false);
        }
    }

    private void TakeDamage(int damage)
    {
        playerHealth -= damage;
        if (playerHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Slime"))
        {
            TakeDamage(1);

            if (attackCollider.enabled)
            {
                other.GetComponent<SlimeIA>().Kill();
            }
        }
        if (other.CompareTag("Orc"))
        {
            TakeDamage(1);

            if (attackCollider.enabled)
            {
                //other.GetComponent<OrcMovement>().Kill();
            }
        }
    }

    public void EnableAttackCollider() => attackCollider.enabled = true;
    public void DisableAttackCollider() => attackCollider.enabled = false;
}
