using UnityEngine;
using Random = UnityEngine.Random;

public class OrcMovement : MonoBehaviour
{
    #region Internas

    private Rigidbody2D m_Rb;
    private Vector2 m_Movement;
    //private Vector2 m_PosicaoOrigem = new Vector2(-16, -11);
    //private Vector2 m_PosicaoFinal = new Vector2(10, 10);
    private Vector2 m_Cohesion;
    private Vector2 m_CurrentSpeed;
    private float m_PerceptionDistanceSqr;
    
    #endregion

    #region Calibração
    
    [Header("Orc Movement")]
    [Range(0f, 10f)] [SerializeField] private float speedWalk = 3f;
    [Range(1f, 5f)] [SerializeField] private float smoothDamp = 1f;

    [Header("Detection")]
    [Range(0f, 10f)] [SerializeField] private float perceptionDistance = 5f;
    
    #endregion

    #region Unity Methods
    
    private void Awake()
    {
        m_Rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        var ang = Random.Range(0, 2 * Mathf.PI);
        m_Movement.x = Mathf.Cos(ang);
        m_Movement.y = Mathf.Sin(ang);

        m_PerceptionDistanceSqr = Mathf.Pow(perceptionDistance, 2);
    }

    private void Update()
    {
        Perception();
    }

    private void FixedUpdate()
    {
        Move();
    }
    
    #endregion

    #region Methods
    
    private void Move()
    {
        m_Rb.MovePosition(m_Rb.position + m_Movement * (speedWalk * Time.fixedDeltaTime));
    }

    private void Perception()
    {
        m_Cohesion = Vector2.zero;
        int numeroDeOrcs = 0;
        var orcs = GameObject.FindGameObjectsWithTag("Orc");

        foreach (var orc in orcs)
        {
            if (orc != gameObject)
            {
                Vector2 distance = orc.transform.position - this.transform.position;

                float distSqr = Vector2.SqrMagnitude(distance);
                if (distSqr <= m_PerceptionDistanceSqr)
                {
                    m_Cohesion += distance;
                    numeroDeOrcs++;
                }
            }
        }

        if (numeroDeOrcs > 0)
        {
            m_Cohesion /= numeroDeOrcs;
            m_Cohesion = m_Cohesion.normalized;
        }

        m_Movement = Vector2.SmoothDamp(m_Movement, m_Cohesion, ref m_CurrentSpeed, smoothDamp);
        m_Movement = m_Movement.normalized;
    }
    
    #endregion

    #region Events
     
    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_Movement = Vector2.Reflect(m_Movement, collision.contacts[0].normal);
    }
    
    #endregion
}