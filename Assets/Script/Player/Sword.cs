using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour,IWeapon
{
    public static Sword Instance;
    //public bool isAttacking = false;
    public bool isInSlipperyZone = false;

    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private WeaponInfo weaponInfo;

    private Transform weaponCollider;
    private Animator myAnimator;
    private float cooldownTimer;
    private bool isAttacking = false;

    private GameObject slashAnim;

    private void Awake()
    {
        Instance = this;
        myAnimator = GetComponent<Animator>();        
    }
    private void Start()
    {
        weaponCollider=PlayerController3.Instance.GetWeaponCollider();
        slashAnimSpawnPoint = GameObject.Find("SlashSpawnPoint").transform;
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
    public WeaponInfo GetWeaponInfo() {return weaponInfo;}

    public void Attack()
    {
        if (cooldownTimer <= 0)
        {
            weaponInfo.isAttacking = true;
            isAttacking = true;
            if (myAnimator != null && myAnimator.isActiveAndEnabled)
            {
                myAnimator.SetTrigger("Attack");
            }
            if (slashAnimSpawnPoint != null)
            {
                slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
                slashAnim.transform.parent = this.transform.parent;
            }
            if (weaponCollider != null)
            {
                weaponCollider.gameObject.SetActive(true);
            }
            cooldownTimer = weaponInfo.weaponCooldown;
        }
    }




    public void SwingUpFlipAnimEvent()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if (PlayerController3.Instance.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    public void DoneAttackingAnimEvent()
    {
        weaponCollider.gameObject.SetActive(false);
        weaponInfo.isAttacking = false;
        isAttacking = false;
    }


    public void SwingDownFlipAnimEvent()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (PlayerController3.Instance.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController3.Instance.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    public bool IsCoolingDown()
    {
        return cooldownTimer > 0;
    }
}
