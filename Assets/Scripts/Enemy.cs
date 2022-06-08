using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class Enemy : Character
{
    [Header("Enemy")]
    [SerializeField] Transform convas;
    [SerializeField] Vector2 offset;
    [SerializeField] float attackRange;
    [SerializeField] Gun currentGun;

    Vector2 moveDir;
    Vector2 rotateDir;

    Player target;
    Vector3 lastTargetPos;
    Vector3 lastDirToTarget;
    public override void Awake()
    {
        base.Awake();
        lastTargetPos = transform.position;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        convas.position = (Vector2)transform.position + offset;
        if (target != null)
        {
            Vector3 dir = target.transform.position - (Vector3)rb.position;
            RaycastHit2D lineOfSight = Physics2D.Raycast(rb.position, dir.normalized, dir.magnitude, wall);
            bool targetInLineOfSight = !lineOfSight;
            lastDirToTarget = lastTargetPos - (Vector3)rb.position;
            if (targetInLineOfSight)
            {
                lastTargetPos = target.transform.position;
                lastDirToTarget = lastTargetPos - (Vector3)rb.position;
                if (lastDirToTarget.magnitude < attackRange)
                {
                    Attack(lastDirToTarget);
                }
                Debug.DrawRay(rb.position, lastDirToTarget, Color.green);
            }
            else
            {
                Debug.DrawRay(rb.position, lastDirToTarget, Color.red);
            }
            if (lastDirToTarget.magnitude < 0.1f)
            {
                moveDir = Vector2.zero;
            }
            else
            {
                moveDir = lastDirToTarget;
                rotateDir = lastDirToTarget;
            }
        }
        else if(Player.Instance != null)
        {
            target = Player.Instance;
        }

        Move(moveDir);
        Rotate(rotateDir);

    }
    float lastCooldownTime;
    private void Attack(Vector2 dir)
    {
        if (lastCooldownTime + currentGun.cooldownTime < Time.time)
        {
            ShootRepetition(currentGun.iterations, currentGun.timeBetween, dir);
            lastCooldownTime = Time.time;
        }
    }

    private async void ShootRepetition(int iterations, float timeBetween, Vector3 dir)
    {
        for (int i = 0; i < iterations; i++)
        {
            currentGun.Shoot(gameObject, transform.position, dir.normalized);
            await Task.Delay(TimeSpan.FromSeconds(timeBetween));
        }
    }
}
