using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSystem : MonoBehaviour, IDamageable
{
    public float maxHealth;
    public float health;
    [SerializeField] private Slider slider;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        slider.maxValue = maxHealth;
    }

    private void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    float previousHealth = 0;
    private void FixedUpdate()
    {
        if (health != previousHealth)
        {
            OnHealthUpdated();
        }
        previousHealth = health;
    }

    private void OnHealthUpdated()
    {
        slider.value = health;
    }

    public void Damage(float damageAmount, Vector2 damageDir)
    {
        health -= damageAmount;
        rb.AddForce(damageDir);
    }

    public void Damage(float damageAmount)
    {
        health -= damageAmount;
    }
}
