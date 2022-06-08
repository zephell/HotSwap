using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class Player : Character
{
    public static Player Instance;

    [Header("Player")]
    [SerializeField] private List<Gun> guns;
    Gun currentGun;
    int equippedSlot;

    //Controls
    Controls controls;
    int switchInput;
    bool primaryAttack;
    Vector2 mousePos;
    Vector2 moveInput;

    //Refs
    Camera cam;

    public override void Awake()
    {
        base.Awake();

        Instance = this;

        controls = new Controls();
        controls.Enable();

        currentGun = guns[0];

        cam = Camera.main;
    }

    private float lastCooldownTime = 0;
    public override void Update()
    {
        base.Update();
        SetupControls();

        if (switchInput != 0)
        {
            equippedSlot += Mathf.Clamp(switchInput, -1, 1);
            equippedSlot = Mathf.Clamp(equippedSlot, 0, guns.Count - 1);
            currentGun = guns[equippedSlot];
        }

        if (primaryAttack && lastCooldownTime + currentGun.cooldownTime < Time.time)
        {
            ShootRepetition(currentGun.iterations, currentGun.timeBetween);
            lastCooldownTime = Time.time;
        }
    }

    private async void ShootRepetition(int iterations, float timeBetween)
    {
        for (int i = 0; i < iterations; i++)
        {
            Vector3 dir = ((Vector2)cam.ScreenToWorldPoint(mousePos) - (Vector2)transform.position).normalized;
            currentGun.Shoot(gameObject, transform.position, dir);
            await Task.Delay(TimeSpan.FromSeconds(timeBetween));
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        Move(moveInput);

        Vector2 targetDir = cam.ScreenToWorldPoint(mousePos) - transform.position;
        Rotate(targetDir);
    }

    private void SetupControls()
    {
        moveInput = controls.Gameplay.Movement.ReadValue<Vector2>();
        mousePos = controls.Gameplay.Pointer.ReadValue<Vector2>();

        switchInput = (int)controls.Gameplay.SwitchWeapon.ReadValue<float>();

        primaryAttack = controls.Gameplay.PrimaryAttack.inProgress;
    }

    /*private void OnDrawGizmos()
    {
        Vector3 dir = (Vector2)cam.ScreenToWorldPoint(mousePos) - (Vector2)transform.position;
        Gizmos.DrawRay(transform.position, dir);
    }*/
}
