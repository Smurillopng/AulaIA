using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcMovement : MonoBehaviour
{
    #region Internas
    Rigidbody2D _rb;

    Vector2 _movement;
    
    Vector2 _cohesion;
    Vector2 _currentSpeed;
    Vector2 _alignment;
    Vector2 _separation;

    float _perceptionDistanceSqr;
    float _separationDistanceSqr;
    #endregion

    #region Calibracao
    [Header("Position")]
    [SerializeField] Vector2 positionOrigin = new Vector2(-16, -11);
    [SerializeField] Vector2 positionFinal = new Vector2(10, 10);

    [Header("Boss Movement")]
    [Range(0f, 10f)] [SerializeField] private float speedWalk = 3f;
    [Range(1f, 5f)] [SerializeField] private float smoothDamp = 1f;

    [Header("Detection")]
    [Range(0f, 10f)] [SerializeField] private float perceptionDistance = 5f;
    [Range(0, 360)] [SerializeField] private int fovAngle = 270;
    [Range(0f, 10f)] [SerializeField] private float separationDistance = 5f;

    [Header("Weights")]
    [Range(0f, 1f)] [SerializeField] float cohesionWeight;
    [Range(0f, 1f)] [SerializeField] float alignmentWeight;
    [Range(0f, 1f)] [SerializeField] float separationWeight;

    [Header("Initial Angles")]
    [Range(0f, 360f)] [SerializeField] float angMean = 0f;
    [Range(0f, 360f)] [SerializeField] float angDelta = 360f;
    #endregion

    #region Unity Methods
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector3(positionOrigin.x, positionOrigin.y, 0), new Vector3(positionFinal.x, positionFinal.y, 0));
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        var angMeanRad = angMean * Mathf.PI / 180;
        var angDeltaRad = angDelta * Mathf.PI / 180;
        var angMin = angMeanRad - angDeltaRad / 2;
        var angMax = angMeanRad + angDeltaRad / 2;
        var ang = Random.Range(angMin, angMax);
        _movement.x = Mathf.Cos(ang);
        _movement.y = Mathf.Sin(ang);

        _perceptionDistanceSqr = Mathf.Pow(perceptionDistance, 2);
        _separationDistanceSqr = Mathf.Pow(separationDistance, 2);
    }

    private void Update()
    {
        Reflect();
        Perception();
    }

    void FixedUpdate()
    {
        Move();
    }
    #endregion

    #region Methods
    private void Move()
    {
        _rb.MovePosition(_rb.position + _movement * speedWalk * Time.fixedDeltaTime);
    }

    private void Reflect()
    {
        var nextX = _rb.position.x + _movement.x * speedWalk * Time.fixedDeltaTime;
        var nextY = _rb.position.y + _movement.y * speedWalk * Time.fixedDeltaTime;

        if (nextX <= positionOrigin.x || nextX >= positionFinal.x)
        {
            _movement.x = -_movement.x;
        }

        if (nextY <= positionOrigin.y || nextY >= positionFinal.y)
        {
            _movement.y = -_movement.y;
        }
    }

    private void Perception()
    {
        _cohesion = Vector2.zero;
        _alignment = Vector2.zero;
        _separation = Vector2.zero;

        int numberBosses = 0;
        var bosses = GameObject.FindGameObjectsWithTag("Orc");

        foreach (var boss in bosses)
        {
            if (boss != this)
            {
                Vector2 distance = boss.transform.position - this.transform.position;

                float distSqr = Vector2.SqrMagnitude(distance);
                if (distSqr <= _perceptionDistanceSqr)
                {
                    var angle = Vector2.Angle(_movement, distance);
                    if (angle < fovAngle / 2)
                    {
                        _cohesion += distance;
                        _alignment += boss.GetComponent<OrcMovement>()._movement;

                        numberBosses++;
                    }

                    if (distSqr <= _separationDistanceSqr)
                    {
                        _separation -= distance.normalized * separationDistance;
                    }
                }
            }
        }

        if (numberBosses > 0)
        {
            _cohesion /= numberBosses;

            _alignment /= numberBosses;
            _alignment *= speedWalk;

            _separation /= numberBosses;

            Vector2 moveVector = ( (_cohesion * cohesionWeight) + (_alignment * alignmentWeight) + (_separation * separationWeight) ) / 3;
            moveVector /= (cohesionWeight + alignmentWeight + separationWeight);
            moveVector = moveVector.normalized;
            //_cohesion = _cohesion.normalized;

            _movement = Vector2.SmoothDamp(_movement, moveVector, ref _currentSpeed, smoothDamp);
            _movement = _movement.normalized;
        }
    }
    #endregion

    #region Events
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    _movement = Vector2.Reflect(_movement, collision.GetContact(0).normal);
    //}
    #endregion
}