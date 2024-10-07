using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public bool isDead { get; private set; }
    public static event Action OnPlayerDeath;
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;

    private Slider healthSlider;
    private int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;

	readonly int DEATH_HASH = Animator.StringToHash("Death");
    protected override void Awake()
    {
        base.Awake();

        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        isDead = false;
        UpdateHealthSlider();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();
        DragonAI dr = other.gameObject.GetComponent<DragonAI>();
        EnemyMovement enemyChapter5E1 = other.gameObject.GetComponent<EnemyMovement>();
        
        if (enemy)
        {
            TakeDamage(4, other.transform);
        }
        if (dr)
        {
            TakeDamage(2, other.transform);
        }
        if (enemyChapter5E1)
        {
            TakeDamage(1, other.transform);
        }
    }

    public void HealPlayer()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 1;
            UpdateHealthSlider();
        }
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage) { return; }
        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        canTakeDamage = false;
        currentHealth -= damageAmount;        
        StartCoroutine(DamageRecoveryRoutine());
        StartCoroutine(flash.FlashRoutine());
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }

	private void CheckIfPlayerDeath()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            currentHealth = 0;            
            Destroy(gameObject);
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            
            //HighScoreEntry newScore = new HighScoreEntry();
            //newScore.score=ScoreManager.Instance.Score;

            //List<HighScoreEntry> scoreList = new List<HighScoreEntry>();
            //scoreList.Add(newScore);            
            //XMLManager.instance.SaveScores(scoreList);
            OnPlayerDeath?.Invoke();
        }
    }


    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find("Health Slider").GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
