using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : ScriptableObject
{
    [Header("Gun Options")]
    public string gunName;
    public float cooldownTime;
    public float discardForce;
    public Vector2 originOffset;

    [Header("Bullet")]
    public GameObject bullet;
    public float bulletSpeed = 0.5f;
    public int damage = 1;
    public float impact;

    [Header("Automatic Gun Props")]
    public int iterations = 1;
    public float timeBetween = 0f;

    public virtual void Shoot(GameObject parent, Vector3 origin, Vector3 dir) { }

    public void Recoil(GameObject parent, Vector3 dir)
    {
        Rigidbody2D rb = parent.GetComponent<Rigidbody2D>();
        rb.AddForce(-dir * discardForce, ForceMode2D.Impulse);
    }

    public void InstantiateBullet(Vector3 origin, Vector3 dir)
    {
        Vector3 offsettedOrigin = origin + originOffset.y * dir + originOffset.x * -(Vector3)Vector2.Perpendicular(dir);
        Bullet newBullet = Instantiate(bullet, offsettedOrigin, Quaternion.LookRotation(Vector3.forward, dir)).GetComponent<Bullet>();
        newBullet.bulletSpeed = bulletSpeed;
        newBullet.dir = dir;
        newBullet.damage = damage;
        newBullet.impact = impact;
    }
}
