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


 

    public override void Die()
    {
        gameObject.SetActive(false);
        OnDeath?.Invoke(this);
    }
}
