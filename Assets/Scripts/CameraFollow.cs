using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);
    [SerializeField] private float smoothTime = 5f;
    [SerializeField] private AnimationCurve mouseSenseCurve;
    [SerializeField] private float mouseSense = 0.005f;

    Camera cam;

    Controls controls;
    Vector2 mousePos;
    private void Awake()
    {
        cam = Camera.main;

        controls = new Controls();
        controls.Enable();
    }

    private void FixedUpdate()
    {
        Vector3 resolution = new Vector3(cam.scaledPixelWidth, cam.scaledPixelHeight, 0) / 2;
        Vector3 fixedMousePos = new Vector3(Mathf.Clamp(mousePos.x - resolution.x, -resolution.x, resolution.x), Mathf.Clamp(mousePos.y - resolution.y, -resolution.y, resolution.y), 0);
        Vector3 targetPos = target.position + offset + (mouseSense * mouseSenseCurve.Evaluate(Vector3.Distance(fixedMousePos, Vector3.zero) / resolution.x) * fixedMousePos);
        Vector3 pos = transform.position;
        pos = Vector3.Lerp(pos, targetPos, smoothTime * Time.fixedDeltaTime);
        transform.position = pos;
    }

    private void Update()
    {
        mousePos = controls.Gameplay.Pointer.ReadValue<Vector2>();
    }
}
