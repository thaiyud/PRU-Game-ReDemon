using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerController3 : Singleton<PlayerController3>
{
    public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }
    public VisualEffect vfxRenderer;
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private float dashSpeed;
    [SerializeField] private TrailRenderer myTrailRenderer;
    [SerializeField] public float slipperyZoneMultiplier = 3f;
    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;
    private Knockback knockback;
    private bool isDashing;
    private float startingMoveSpeed;
    private bool facingLeft = false;
    private bool isInSlipperyZone = false;
    private Sword swordInstance;

    private int slipperyZoneCounter = 0;


    protected override void Awake() 
    {
        base.Awake();
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
        swordInstance = Sword.Instance;
        // vfxRenderer = GetComponent<VisualEffect>();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        playerControls.Combat.Dash.performed += _ => Dash();
        startingMoveSpeed = moveSpeed;
        ActiveInventory.Instance.EquipStartingWeapon();
        Timer.Instance.BeginTimer();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        PlayerHealth.OnPlayerDeath += DisablePlayerMovement;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= DisablePlayerMovement;
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
        if (vfxRenderer != null)
        {
            vfxRenderer.SetVector3("ColliderPos", transform.position);
        }
    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Movement.ReadValue<Vector2>();
        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        if (!(rb.bodyType == RigidbodyType2D.Dynamic) || knockback.GettingKnockedBack || PlayerHealth.Instance.isDead)
        {
            return;
        }

        float currentMoveSpeed = isInSlipperyZone ? moveSpeed * slipperyZoneMultiplier : moveSpeed;
        rb.velocity = new Vector2(movement.x * currentMoveSpeed, movement.y * currentMoveSpeed);
    }




    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRender.flipX = true;
            FacingLeft = true;
        }
        else
        {
            mySpriteRender.flipX = false;
            FacingLeft = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SlipperyZone"))
        {
            slipperyZoneCounter++;
            isInSlipperyZone = true;
            Sword.Instance.isInSlipperyZone = true;
            playerControls.Combat.Dash.Disable();
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("SlipperyZone"))
        {
            slipperyZoneCounter--;
            if (slipperyZoneCounter <= 0)
            {
                isInSlipperyZone = false;
                Sword.Instance.isInSlipperyZone = false;
                playerControls.Combat.Dash.Enable();
            }
        }
    }



    private void DisablePlayerMovement()
    {
        myAnimator.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
    }
    private void EnablePlayerMovement()
    {
        myAnimator.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void Dash()
    {
        if (!isDashing && Stamina.Instance.CurrentStamina > 0)
        {
            Stamina.Instance.UseStamina(10);
            isDashing = true;
            moveSpeed *= dashSpeed;
            myTrailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine()
    {
        float dashTime = .2f;
        float dashCD = 1f;
        yield return new WaitForSeconds(dashTime);
        moveSpeed = startingMoveSpeed;
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }
}
