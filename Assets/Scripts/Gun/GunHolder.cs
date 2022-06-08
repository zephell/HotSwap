using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class GunHolder : MonoBehaviour
{
    [SerializeField] private Gun gun;

    [SerializeField] private AnimationCurve curve;
    [SerializeField] private Transform square;

    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private float lastCooldownTime;
    private void Update()
    {
        if (Input.GetMouseButton(0) && lastCooldownTime + gun.cooldownTime < Time.time)
        {
            ShootRepetition(gun.iterations, gun.timeBetween);
            Anim();
            lastCooldownTime = Time.time;
        }
        square.eulerAngles = new Vector3(square.rotation.x, square.eulerAngles.y, Time.time * 200);
    }

    private async void Anim()
    {
        float time = 1;
        while (time - 1 < curve.keys[curve.length - 1].time)
        {
            square.localScale = (1 + curve.Evaluate(time - 1)) * Vector2.one;
            time += .1f;
            await Task.Delay(1);
        }
    }

    private async void ShootRepetition(int iterations, float timeBetween)
    {
        for (int i = 0; i < iterations; i++)
        {
            Vector3 dir = ((Vector2)cam.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).normalized;
            gun.Shoot(gameObject, transform.position, dir);
            await Task.Delay(TimeSpan.FromSeconds(timeBetween));
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 dir = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
        Gizmos.DrawRay(transform.position, dir);
    }
}
