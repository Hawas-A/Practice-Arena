using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    public bool IsAlive { get; private set; } = true;

    public Action<float, float> OnHealthChanged; 
    public Action OnHealthDepleted;

    public void Initialize(float maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        IsAlive = true;

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    public void TakeDamage(float amount)
    {
        if (!IsAlive) return;
        if (amount <= 0f) return;

        CurrentHealth -= amount;

        
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth); // Using clamp to limit the values between current and maxHealth

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (!IsAlive) return;
        if (amount <= 0f) return;

        CurrentHealth += amount;

        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    private void Die()
    {
        if (!IsAlive) return;

        IsAlive = false;

        OnHealthDepleted?.Invoke();
    }
}
