using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    private IObjectPool<Projectile> projectilePool;
    private static Dictionary<Projectile, ObjectPool<Projectile>> poolDictionary = new Dictionary<Projectile, ObjectPool<Projectile>>();

    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 3f;

    private float lifeTimer;
    private Vector3 direction;

    private Character shooterCharacter;
    private Action<Collider> onHitAction;
    public void SetPool(IObjectPool<Projectile> objectPool) => projectilePool = objectPool;

    public void Init(Vector3 startPos, Vector3 dir, Character shooter, Action<Collider> onHit)
    {
        transform.position = startPos;
        direction = dir;
        lifeTimer = lifeTime;
        shooterCharacter = shooter;
        onHitAction = onHit;
    }


    private void OnEnable()
    {
        lifeTimer = lifeTime;
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0)
        {
            ReleaseSelf();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == shooterCharacter) return;
        onHitAction?.Invoke(other);
        ReleaseSelf();
    }
    private void ReleaseSelf()
    {
        if (gameObject.activeSelf) projectilePool.Release(this);
    }

    public static void Fire(Projectile prefab, Vector3 position, Vector3 direction, Character shooter, Action<Collider> onHitCallback)
    {
      
        if (!poolDictionary.ContainsKey(prefab))
        {
            poolDictionary[prefab] = new ObjectPool<Projectile>(
                createFunc: () =>
                {
                    Projectile p = Instantiate(prefab);
                    p.SetPool(poolDictionary[prefab]);
                    return p;
                },
                actionOnGet: p => p.gameObject.SetActive(true),
                actionOnRelease: p => p.gameObject.SetActive(false),
                actionOnDestroy: p => Destroy(p.gameObject),
                defaultCapacity: 20,
                maxSize: 100
            );
        }

        Projectile instance = poolDictionary[prefab].Get();
        instance.Init(position, direction, shooter, onHitCallback);
    }
}