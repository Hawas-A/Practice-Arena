using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    private IObjectPool<Projectile> pool;

    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 3f;

    private float lifeTimer;
    private Vector3 direction;

    private Character shooterCharacter;
    public void SetPool(IObjectPool<Projectile> objectPool)
    {
        pool = objectPool; 
    }

    public void Init(Vector3 startPos, Vector3 dir, Character shooter)
    {
        transform.position = startPos;
        direction = dir;
        lifeTimer = lifeTime;
        shooterCharacter = shooter;
    }


    private void OnEnable()
    {
        lifeTimer = lifeTime;
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        lifeTimer -= Time.deltaTime;

        if(lifeTimer <= 0 )
        {
            pool.Release(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == shooterCharacter)
        {
            return;
        }
        pool.Release(this);
    }

}
