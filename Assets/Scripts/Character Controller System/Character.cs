using UnityEngine;

public abstract class  Character : MonoBehaviour
{
    [Header("Character Settings")]

    [SerializeField] protected float moveSpeed = 5.0f;


    public abstract void Attack();


    public abstract void ApplyDamage();


    public abstract void TakeDamage();
  

    public  void Die()
    {
        Debug.Log($"{gameObject.name} Died");

    }
}
