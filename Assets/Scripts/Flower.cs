using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class Flower : MonoBehaviour,IWater
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

    public bool IsBurning { get => isBurning; private set => isBurning = value; }

    void Start()
    {
        CurrentHealth = maxHealth;
        StartCoroutine(Decay());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Water();

        if (Input.GetKeyDown(KeyCode.LeftControl))
            SetOnFire();
    }

    private IEnumerator Decay()
    {
        while (true)
        {
            yield return new WaitForSeconds(decayRate);

            float amount = decayStrength;
            if (IsBurning)
            {
                amount *= decayMultiplier;
            }

            CurrentHealth -= amount;

            if (CurrentHealth <= 0)
            {
                currentHealth = 0;
                Die();
            }
            else
            {
                // TODO: Display Decay Effect/Text
                GameHandler.Instance.AddScore((int)currentHealth);
            }
        }
    }

    void Die()
    {
        if (IsBurning)
        {
            Debug.Log($"Game Over. {gameObject.name} was set ablaze!");
        }
        else
        {
            Debug.Log($"Game Over. {gameObject.name} just wanted a sip of water!");
        }
        GameHandler.Instance.StartGameOver();
    }

    private void UpdateHealthBar()
    {
        healthImage.fillAmount = CurrentHealth / maxHealth;
    }

    public void SetOnFire()
    {
        IsBurning = true;
        fireParticles.Play();
        // TODO: Add Particles for setting flower on fire?

    }

    private void ExtinguishFire()
    {
        IsBurning = false;
        fireParticles.Stop();
        // TODO: Add Particles for stopping the fire?
    }

    public void Water()
    {
        float curGain = nourishGain;
        if (IsBurning)
        {
            ExtinguishFire();
            curGain /= 2;
        }

        CurrentHealth += curGain;
        if (CurrentHealth >= maxHealth)
        {
            CurrentHealth = maxHealth;
            nourishParticles.Play();
        }
        // TODO: Display Nourish Text(?)
    }
}
