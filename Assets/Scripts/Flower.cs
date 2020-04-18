using UnityEngine.UI;
using UnityEngine;

public class Flower : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth;
    private float currentHealth;
    [SerializeField] private Image healthImage;

    [Header("Decay Settings")]
    [Tooltip("The tick-rate for decay in seconds.")]
    [SerializeField] private float decayRate = 1f;
    [Tooltip("The strenght of each dacay tick.")]
    [SerializeField] private float decayStrength = 1f;
  

    [Header("Nourish Settings")]
    [Tooltip("The amount gained when watered by player.")]
    [SerializeField] private int nourishGain = 5;
    [SerializeField] private ParticleSystem nourishParticles;

    [Header("Burn Settings")]
    private bool isBurning = false;
    [SerializeField] private ParticleSystem fireParticles;
    [Tooltip("The multiplier for decay strenght when on fire.")]
    [SerializeField] private float decayMultiplier = 3f;

    public float CurrentHealth 
    {   get { return currentHealth; }  
        set 
        {
            currentHealth = value;
            UpdateHealthBar();
        }  
    }

    void Start()
    {
        CurrentHealth = maxHealth;
        InvokeRepeating("Decay", 0, decayMultiplier);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            WaterPlant();

        if (Input.GetKeyDown(KeyCode.LeftControl))
            SetOnFire();
    }

    private void Decay()
    {
        float amount = decayStrength;
        if (isBurning)
        {
            amount *= decayMultiplier;
        }

        CurrentHealth -= amount;

        if(CurrentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            // TODO: Display Decay Effect/Text
        }
    }

    void Die()
    {
        if (isBurning)
        {
            Debug.Log($"Game Over. {gameObject.name} was set ablaze!");
        }
        else
        {
            Debug.Log($"Game Over. {gameObject.name} just wanted a sip of water!");
        }
    }

    private void UpdateHealthBar()
    {
        healthImage.fillAmount = CurrentHealth / maxHealth;
    }

    public void SetOnFire()
    {
        isBurning = true;
        fireParticles.Play();
        // TODO: Add Particles for setting flower on fire?

    }

    private void ExtinguishFire()
    {
        isBurning = false;
        fireParticles.Stop();
        // TODO: Add Particles for stopping the fire?
    }

    public void WaterPlant()
    {
        if(isBurning)
        {
            ExtinguishFire();
        }

        CurrentHealth += nourishGain;
        if(CurrentHealth >= maxHealth)
        {
            CurrentHealth = maxHealth;
            nourishParticles.Play();   
        }
        // TODO: Display Nourish Text(?)
    }
}
