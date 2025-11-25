using UnityEngine;

public class WeaponTester : MonoBehaviour
{
    public WeaponSystem weaponSystem;

    void Update()
    {
        if (weaponSystem == null) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            weaponSystem.TryFireActiveWeapon();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            weaponSystem.ReloadActiveWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            weaponSystem.SwitchWeapon(true);
            Debug.Log("Switched weapon");
        }

        
    }
}
