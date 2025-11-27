using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private int defaultPoolSize = 20;
    [SerializeField] private int maxPoolSize = 100;

    private IObjectPool<Projectile> pool;

    void Awake()
    {
        pool = new ObjectPool<Projectile>(
            createFunc: CreateProjectile,
            actionOnGet: OnTakeFromPool,
            actionOnRelease: OnReleaseToPool,
            actionOnDestroy: OnDestroyPooledObject,
            collectionCheck: true,
            defaultCapacity: defaultPoolSize,
            maxSize: maxPoolSize
        );
    }

    private Projectile CreateProjectile()
    {
        Projectile proj = Instantiate(projectilePrefab);
        proj.SetPool(pool);
        return proj;
    }

    private void OnTakeFromPool(Projectile proj)
    {
        proj.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(Projectile proj)
    {
        proj.gameObject.SetActive(false);
    }

    private void OnDestroyPooledObject(Projectile proj)
    {
        Destroy(proj.gameObject);
    }
    public void Fire(Vector3 position, Vector3 direction)
    {
        Projectile proj = pool.Get();
        proj.Init(position, direction);
    }
}
