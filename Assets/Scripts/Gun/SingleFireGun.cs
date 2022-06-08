using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Guns/SingleFireGun")]
public class SingleFireGun : Gun
{
    public override void Shoot(GameObject parent, Vector3 origin, Vector3 dir)
    {
        Recoil(parent, dir);

        InstantiateBullet(origin, dir);
    }
}
