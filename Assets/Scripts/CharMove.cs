using UnityEngine;

public class CharMove : MonoBehaviour
{
    [SerializeField] private float speed;

    private Sprite m_Player;
    private Rigidbody2D m_Rb;
    private Vector2 m_MoveVelocity;
    private Animator m_Animator;
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
        m_Rb = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
    }
    private void Update()
    {
        m_MoveVelocity.x = Input.GetAxis("Horizontal");
        m_MoveVelocity.y = Input.GetAxis("Vertical");
        Attack();
    }
    private void FixedUpdate() 
    {
        Move();
    }

    private void Move()
    {
        Vector2 move = new Vector2(m_MoveVelocity.x, m_MoveVelocity.y).normalized;
        m_Rb.MovePosition(m_Rb.position + (move * (speed * Time.fixedDeltaTime)));

        if (move != Vector2.zero)
        {
            m_Animator.SetFloat(X, move.x);
            m_Animator.SetFloat(Y, move.y);
            m_Animator.SetBool(IsIdle, false);
            m_Animator.SetBool(IsWalking, true);
        }
        else
        {
            m_Animator.SetBool(IsWalking, false);
            m_Animator.SetBool(IsIdle, true);
        }
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_Animator.SetBool(IsAttacking, true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            m_Animator.SetBool(IsAttacking, false);
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
                other.GetComponent<OrcIA>().Kill();
            }
        }
    }

    public void EnableAttackCollider() => attackCollider.enabled = true;
    public void DisableAttackCollider() => attackCollider.enabled = false;
}
