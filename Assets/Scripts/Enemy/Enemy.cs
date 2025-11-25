using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    private NavMeshAgent navMeshAgent;

    public event Action<Enemy> OnDeath;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void Init()
    {
        gameObject.SetActive(true);
    }

    public override void ApplyDamage()
    {
        throw new System.NotImplementedException();
    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void TakeDamage()
    {
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        OnDeath?.Invoke(this);
        gameObject.SetActive(false);    
    }
}
