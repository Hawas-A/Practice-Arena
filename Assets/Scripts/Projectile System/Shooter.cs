using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private ProjectilePool projectilePool;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 firePos = transform.position;
            Vector3 fireDir = transform.forward;  
            projectilePool.Fire(firePos, fireDir);
        }
    }
}
