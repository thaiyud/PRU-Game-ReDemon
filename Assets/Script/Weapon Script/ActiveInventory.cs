using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInventory : Singleton<ActiveInventory>
{
    private int activeSlotIndexNum = 0;

    private PlayerControls playerControls;


    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
    }

    private void Start()
    {
        playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
        ToggleActiveHighlight(0);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    public void EquipStartingWeapon()
    {
        ToggleActiveHighlight(0);
    }
    private void ToggleActiveSlot(int numValue)
    {
        ToggleActiveHighlight(numValue - 1);
    }

    private void ToggleActiveHighlight(int indexNum)
    {
        activeSlotIndexNum = indexNum;

        foreach (Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);
        ChangeActiveWeapon();
    }
    private MonoBehaviour previousWeapon;
    private void ChangeActiveWeapon()
    {
        // Disable the current active weapon, but keep it alive to preserve its state
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            ActiveWeapon.Instance.CurrentActiveWeapon.gameObject.SetActive(false);
            previousWeapon = ActiveWeapon.Instance.CurrentActiveWeapon; // Store the current weapon
        }

        // Check if the new slot has a valid weapon
        var newWeaponInfo = transform.GetChild(activeSlotIndexNum).GetComponentInChildren<InventorySlot>().GetWeaponInfo();
        if (newWeaponInfo == null)
        {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        // If the weapon in this slot is the same as the previous weapon, simply re-enable it
        if (previousWeapon != null && previousWeapon.GetComponent<IWeapon>().GetWeaponInfo() == newWeaponInfo)
        {
            previousWeapon.gameObject.SetActive(true);  // Reactivate the stored weapon
            ActiveWeapon.Instance.NewWeapon(previousWeapon);
            return;
        }

        // If it's a different weapon, instantiate it
        GameObject weaponToSpawn = newWeaponInfo.weaponPrefab;
        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position, Quaternion.identity);

        // Set up the new weapon
        newWeapon.transform.parent = ActiveWeapon.Instance.transform;
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);

        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}
