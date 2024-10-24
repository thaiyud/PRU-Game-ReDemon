using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour,IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject magicLaser;
    [SerializeField] private Transform magicLaserSpawnPoint;    

    private Animator animator;
    private float cooldownTimer;
    private bool isAttacking = false;

    readonly int AttackHarsh = Animator.StringToHash("Attack");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        MouseFollowWithOffset();
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
            animator.SetTrigger(AttackHarsh);
            cooldownTimer = weaponInfo.weaponCooldown;
        }
    }
    public void SpamStaffProjectileAnimeEvent()
    {
        GameObject newLaser = Instantiate(magicLaser, magicLaserSpawnPoint.position, Quaternion.identity);
        newLaser.GetComponent<MagicLaser>().UpdateLaserRange(weaponInfo.weaponRange);
    }
    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController3.Instance.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);            
        }
    }
    public bool IsCoolingDown()
    {
        return cooldownTimer > 0;
    }
}
