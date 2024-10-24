using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : Singleton<Stamina>
{
    public int CurrentStamina { get; private set; }
    
    [SerializeField] private int timeBetweenStaminaRefresh = 1;

    private Transform staminaContainer;
    [SerializeField] private int startingStamina = 100;
    private int maxStamina;
    const string STAMINA_CONTAINER_TEXT = "Stamina Container";
    private Slider staminaSlider;

    protected override void Awake()
    {
        base.Awake();

        maxStamina = startingStamina;
        CurrentStamina = startingStamina;
    }

    private void Start()
    {
        staminaContainer = GameObject.Find(STAMINA_CONTAINER_TEXT).transform;
    }

    public void UseStamina(int staminaCost)
    {
         CurrentStamina -= staminaCost;
        UpdateStaminaSlider();
    }

    public void RefreshStamina()
    {
        if (CurrentStamina < maxStamina)
        {
            CurrentStamina += 2;
        }
        UpdateStaminaSlider();
    }

    private IEnumerator RefreshStaminaRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenStaminaRefresh);
            RefreshStamina();
        }
    }
    private void UpdateStaminaSlider()
    {
        if (staminaSlider == null)
        {
            staminaSlider = GameObject.Find("Stamina Slider").GetComponent<Slider>();
        }

        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = CurrentStamina;
        if (CurrentStamina < maxStamina)
        {
            StopAllCoroutines();
            StartCoroutine(RefreshStaminaRoutine());
        }
    }
    public void StaminaPickUp()
    {
        if (CurrentStamina < maxStamina)
        {
            CurrentStamina += 20;
            UpdateStaminaSlider();
        }
    }
}