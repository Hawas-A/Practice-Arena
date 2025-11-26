using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public List<Weapon> EquippedWeapons = new List<Weapon>();

    private int activeWeaponIndex = 0;

    private int currentAmmo = 0;
    private float lastFireTime = -999f;

    private Weapon ActiveWeapon => EquippedWeapons.Count > 0 ? EquippedWeapons[activeWeaponIndex] : null;

    public void EquipWeapon(Weapon weapon)
    {
        if (weapon == null || EquippedWeapons.Contains(weapon)) return;

        EquippedWeapons.Add(weapon);

        if (EquippedWeapons.Count == 1)
        {
            activeWeaponIndex = 0;
            currentAmmo = weapon.MaxAmmo;
            lastFireTime = -999f;
        }
    }

    public void UnequipWeapon(Weapon weapon)
    {
        if (!EquippedWeapons.Contains(weapon)) return;

        EquippedWeapons.Remove(weapon);

        if (activeWeaponIndex >= EquippedWeapons.Count)
            activeWeaponIndex = EquippedWeapons.Count - 1;

        if (ActiveWeapon != null)
        {
            currentAmmo = ActiveWeapon.MaxAmmo;
            lastFireTime = -999f;
        }
        else
        {
            currentAmmo = 0;
            lastFireTime = -999f;
        }
    }

    public void SwitchWeapon(bool next)
    {
        if (EquippedWeapons.Count == 0) return;

        activeWeaponIndex += next ? 1 : -1;
        if (activeWeaponIndex >= EquippedWeapons.Count) activeWeaponIndex = 0;
        if (activeWeaponIndex < 0) activeWeaponIndex = EquippedWeapons.Count - 1;

        currentAmmo = ActiveWeapon.MaxAmmo;
        lastFireTime = -999f;
    }

    public void SwitchToWeapon(int index)
    {
        if (index < 0 || index >= EquippedWeapons.Count) return;

        activeWeaponIndex = index;

        currentAmmo = ActiveWeapon.MaxAmmo;
        lastFireTime = -999f;
    }

    public void TryFireActiveWeapon()
    {
        if (ActiveWeapon == null) return;

        if (currentAmmo == 0)
        {
            ReloadActiveWeapon();
            return;
        }

        if (Time.time < lastFireTime + ActiveWeapon.FireRate)
            return;

        ActiveWeapon.Fire();
        currentAmmo--;
        lastFireTime = Time.time;

    }

    
    public void ReloadActiveWeapon()
    {
        if (ActiveWeapon == null) return;

        currentAmmo = ActiveWeapon.MaxAmmo;
        lastFireTime = -999f;

        ActiveWeapon.Reload();
    }

    
}
