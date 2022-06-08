using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class Character : MonoBehaviour, IDamageable
{
    [SerializeField] public LayerMask wall;
    [Header("Movement")]
    [SerializeField] private float speed = 40f;
    [SerializeField] private float damping = 4f;

    [Header("Health")]
    public float maxHealth = 100f;
    public float health = 100f;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject objectToDestroy;

    [HideInInspector] public Rigidbody2D rb;
    Color spriteColor;
    SpriteRenderer sprite;

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        spriteColor = sprite.color;
        healthSlider.maxValue = maxHealth;
    }

    public virtual void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
    }
    float previousHealth = 0;
    public virtual void FixedUpdate()
    {
        if (health != previousHealth)
        {
            OnHealthUpdated();
        }
        previousHealth = health;
    }
    public void Move(Vector2 dir)
    {
        rb.AddForce(speed * dir.normalized);

        rb.AddForce(damping * -rb.velocity);
    }
    public void Rotate(Vector2 dir)
    {
        Quaternion targetRot = Quaternion.LookRotation(Vector3.forward, dir.normalized);
        rb.MoveRotation(targetRot);
    }
    private void OnHealthUpdated()
    {
        healthSlider.value = health;
    }
    public virtual void Damage(float damage, Vector2 impact)
    {
        health -= damage;
        rb.AddForce(impact, ForceMode2D.Impulse);
        Blink(sprite, Color.white, spriteColor);
        if (health < 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(objectToDestroy);
    }

    public async void Blink(SpriteRenderer sprite, Color blinkColor, Color spriteColor)
    {
        sprite.color = blinkColor;
        await Task.Delay(180);
        sprite.color = spriteColor;
    }
}
