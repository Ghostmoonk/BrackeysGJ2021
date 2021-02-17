using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(FighterBehavior))]
public class KnightFollower : Follower, IHealth
{
    FighterBehavior fighterBehavior;

    [Header("Health")]
    [Tooltip("Nombre de fois que le chevalier va pouvoir tanker")]
    [SerializeField]
    int maxHealth;
    int currentHealth;
    [SerializeField]
    UnityEvent OnLastHeadRemoved;

    [Header("Attack")]
    [SerializeField]
    Collider attackRange;


    public delegate void EnemyInRangeDelegate(Enemy enemy);
    public EnemyInRangeDelegate OnEnemyInRange;

    protected override void Start()
    {
        base.Start();
        fighterBehavior = (FighterBehavior)followBehavior;

        OnEnemyInRange += SetTarget;
    }

    public void UpdateHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth == 0)
        {
            OnLastHeadRemoved?.Invoke();
        }
    }

    public void SetTarget(Enemy enemy)
    {
        currentTarget = enemy.transform;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
