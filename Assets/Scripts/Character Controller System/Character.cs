using UnityEngine;

public abstract class  Character : MonoBehaviour
{
    [Header("Character Settings")]

    [SerializeField] protected float moveSpeed = 5.0f;


  

    public virtual void Die()
    {
        
    }
}
