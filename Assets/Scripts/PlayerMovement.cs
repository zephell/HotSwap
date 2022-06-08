using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float counterMovementMultiplier;

    Rigidbody2D rb;

    Vector2 moveInput;
    Camera cam;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    private void Update()
    {
        Inputs();
    }

    private void FixedUpdate()
    {
        Vector2 vel = new Vector2(moveInput.x, moveInput.y);

        rb.AddForce(speed * moveInput);

        rb.AddForce(speed * counterMovementMultiplier * -rb.velocity);

        Vector3 target = cam.ScreenToWorldPoint(Input.mousePosition);
        Quaternion targetRot = Quaternion.LookRotation(Vector3.forward, target - transform.position);
        rb.MoveRotation(targetRot);
    }

    private void Inputs()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = moveInput.normalized;
    }
}
