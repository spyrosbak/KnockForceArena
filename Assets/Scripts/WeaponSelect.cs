using UnityEngine;
using PurrNet;

public class WeaponSelect : NetworkBehaviour
{
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private Transform weaponSlot;
    private GameObject currentWeapon;

    protected override void OnSpawned()
    {
        base.OnSpawned();

        enabled = isOwner;
    }

    private void Update()
    {
        if (InputManager.Instance.interactAction.triggered)
        {
            if(currentWeapon)
                Destroy(currentWeapon);

            currentWeapon = Instantiate(weaponPrefab, weaponSlot.position, Quaternion.identity, transform);
        }
    }
}