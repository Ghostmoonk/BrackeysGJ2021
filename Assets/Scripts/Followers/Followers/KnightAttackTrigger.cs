using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAttackTrigger : MonoBehaviour
{
    public delegate void OnEnemyAttackable(Enemy enemy);
    public OnEnemyAttackable EnemyAttackable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EnemyAttackable?.Invoke(other.GetComponent<Enemy>());
        }
    }
}
