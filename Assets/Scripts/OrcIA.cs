using UnityEngine;
using Random = UnityEngine.Random;

public class OrcIA : MonoBehaviour
{
//
    public GameObject player;
    public float walkSpeed = 3f;
    public float timerMin = 0.3f, timerMax = 0.8f;
    public float dist = 6f;
    public float distAttack = 1f;
    public float attackSpeed = 1f;
    public BoxCollider2D attackTrigger;
    public bool isAttacking;
//
    public enum States { Idle, Walk, Attack }
    [HideInInspector] public States state = States.Idle;
    [HideInInspector] public float distanceToPlayer = 1000f;
    [HideInInspector] public float timer;
    [HideInInspector] public Vector2 playerDirection;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;

    private void Awake() => player = GameObject.FindGameObjectWithTag("Player");

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerDirection = player.transform.position - transform.position;
    }
    private void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
        switch(state)
        {
            case States.Idle:
                animator.SetBool("isAttacking", false);
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdle", true);
                break;
            case States.Walk:
                animator.SetBool("isAttacking", false);
                animator.SetBool("isIdle", false);
                animator.SetBool("isWalking", true);
                rb.MovePosition(rb.position + (playerDirection.normalized * (walkSpeed * Time.fixedDeltaTime)));
                break;
            case States.Attack:
                animator.SetBool("isIdle", false);
                animator.SetBool("isWalking", false);
                animator.SetBool("isAttacking", true);
                rb.MovePosition(rb.position + (playerDirection.normalized * (attackSpeed * Time.fixedDeltaTime)));
                break;
        }
    }
    
    public virtual void Move()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        animator.SetFloat("X", playerDirection.x);
        animator.SetFloat("Y", playerDirection.y);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        switch(state)
        {
            case States.Idle:
                if (distanceToPlayer < dist)
                {
                    state = States.Walk;
                }
                break;
            case States.Walk:
                if (timer < 0)
                {
                    playerDirection = (player.transform.position - transform.position);
                    timer = Random.Range(timerMin, timerMax);
                    if (distanceToPlayer > dist)
                    {
                        state = States.Idle;
                    }
                    if (distanceToPlayer < distAttack)
                    {
                        playerDirection = (player.transform.position - transform.position);
                        state = States.Attack;
                        timer = (distanceToPlayer + 0.1f) / attackSpeed;
                    }
                }
                break;
            case States.Attack:
                if (timer < 0)
                {
                    print("Attacking");
                    state = States.Idle;
                    timer = 1f;
                }
                break;
        }
        timer -= Time.deltaTime;
    }

    public void Kill() => animator.SetBool("isDead", true);
    public void Destroy() => Destroy(gameObject);

    public void StartAttacking() => attackTrigger.enabled = true;
    public void StopAttacking() => attackTrigger.enabled = false;
}