using UnityEngine;

public class SlimeFujao : SlimeIA
{
    public int speed;
    public override void Move()
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