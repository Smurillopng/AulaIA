using UnityEngine;

public class SlimeFujao : SlimeIA
{
    public int speed;
    private static readonly int Y = Animator.StringToHash("Y");
    private static readonly int X = Animator.StringToHash("X");

    protected override void Move()
    {
        var position = transform.position;
        distanceToPlayer = Vector2.Distance(position, player.transform.position);
        animator.SetFloat(X, playerDirection.x);
        animator.SetFloat(Y, playerDirection.y);
        position = new Vector3(position.x, position.y, 0);
        transform.position = position;

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
                    playerDirection = -playerDirection;
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
}