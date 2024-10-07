using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; private set; }

    private PlayerControls playerControls;
    private float timeBetweenAttacks;

    private bool attackButtonDown;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();     
    }

    private void Update()
    {
        Attack();
    }
    private void Attack()
    {
        IWeapon weapon = CurrentActiveWeapon as IWeapon;
        if (weapon == null) return;

        if (attackButtonDown && !weapon.IsCoolingDown())
        {
            var manaCost = weapon.GetWeaponInfo().weaponStaminaCost;
            if (Stamina.Instance.CurrentStamina >= manaCost)
            {
                weapon.Attack();
                Stamina.Instance.UseStamina(manaCost);
            }
        }
    }
    public void NewWeapon(MonoBehaviour newWeapon)
    {
        if (CurrentActiveWeapon != null)
        {            
            CurrentActiveWeapon.enabled = false;
        }

        CurrentActiveWeapon = newWeapon;
        CurrentActiveWeapon.enabled = true;
    }
    public void WeaponNull()
    {
        CurrentActiveWeapon = null;
    }
    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    
}
