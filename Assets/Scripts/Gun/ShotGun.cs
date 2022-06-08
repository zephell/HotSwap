using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Guns/ShotGun")]
public class ShotGun : Gun
{
    [Header("Shot Gun Props")]
    public float range;
    public float distance = 0.5f;
    public int bulletCount = 1;
    public override void Shoot(GameObject parent, Vector3 origin, Vector3 dir)
    {
        Recoil(parent, dir);

        float min = -range / 2;
        float max = range / 2;

        for (int i = 0; i < bulletCount; i++)
        {
            float randomDeltaDegAngle = Random.Range(min, max);
            float curDegAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float randomDegreeAngle = curDegAngle + randomDeltaDegAngle;
            float randomAngle = randomDegreeAngle * Mathf.Deg2Rad;
            Vector3 randomDir = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0).normalized;

            InstantiateBullet(origin + distance * randomDir, randomDir);
        }
    }
}
