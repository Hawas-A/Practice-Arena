using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Items/Weapon")]
public class Weapon : Item
{
    [Header("Weapon Settings")]
    [field:SerializeField] public int MaxAmmo {get; private set;} = 30;
    [field:SerializeField] public float FireRate {get; private set;} = 0.2f;
    [field:SerializeField] public float Damage {get; private set;} = 20f;

    
  
      public int AmmoCount { get; private set; }

  

    private void OnEnable()
    {
        AmmoCount = MaxAmmo;   
    }

    public override void Use()
    {
        if(!CanFire())
        {
            return;
        }
        Fire();
    }

    public bool CanFire()
    {
        return AmmoCount > 0; 
    }

    public void Fire()
    {

        AmmoCount--;

    }

    public void Reload()
    {
        AmmoCount = MaxAmmo;
    }
}
