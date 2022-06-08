using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletWidth;

    [HideInInspector] public float bulletSpeed;
    [HideInInspector] public float damage;
    [HideInInspector] public float impact;
    [HideInInspector] public Vector2 dir;

    bool active;

    Vector3 previousPos;
    private void Awake()
    {
        active = true;
        previousPos = transform.position;
    }

    private void Update()
    {
        Vector3 dir = transform.position - previousPos;
        Debug.DrawRay(previousPos, dir, Color.red);

        RaycastHit2D hit = Physics2D.CircleCast(previousPos, bulletWidth, dir.normalized, dir.magnitude);
        if (hit)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Destroy(gameObject);
            }
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.Damage(damage, dir * impact);
                Destroy(gameObject);
            }
        }

        previousPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (!active) return;

        transform.position = transform.position + (Vector3)(bulletSpeed * dir);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
    }
}
