using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] protected GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;

    readonly int FIRE_HASH = Animator.StringToHash("Fire");
    private Animator animator;
    private float cooldownTimer;
    private bool isAttacking = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;            
            isAttacking = true;
        }
        else
        {
            isAttacking = false; // Allow attack when cooldown is finished
        }
        weaponInfo.isAttacking = isAttacking;
    }
    public void Attack()
    {
        if (cooldownTimer <= 0)
        {
            weaponInfo.isAttacking = true;
            animator.SetTrigger(FIRE_HASH);
            GameObject newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, ActiveWeapon.Instance.transform.rotation);
            newArrow.GetComponent<Projectile>().UpdateWeaponInfo(weaponInfo);
            cooldownTimer = weaponInfo.weaponCooldown;
        }

    }
    public WeaponInfo GetWeaponInfo() { return weaponInfo; }
    public bool IsCoolingDown()
    {
        return cooldownTimer > 0;
    }
}
 