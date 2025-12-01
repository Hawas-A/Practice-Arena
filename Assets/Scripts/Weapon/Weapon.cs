using UnityEngine;

public class Weapon : Item
{
    [Header("Weapon Settings")]
    [field: SerializeField] public int MaxAmmo { get; private set; } = 30;
    [field: SerializeField] public float FireRate { get; private set; } = 0.2f;
    [field: SerializeField] public float Damage { get; private set; } = 20f;

    public override void Use()
    {
        Fire();
    }

    public bool CanFire()
    {
        return true;
    }

    public void Fire()
    {
    }

    public void Reload()
    {
    }
}
