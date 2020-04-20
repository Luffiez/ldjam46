using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class Flower : MonoBehaviour, IWater
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

    [Header("Flower Sprites")]
    public SpriteRenderer spriteRenderer;
    public Sprite healthySprite;
    public Sprite damagedSprite;
    public Sprite dyingSprite;
    public Sprite deadSprite;

    [Header("Flower Sound")]
    [SerializeField]
    AudioClip Burning;
    [SerializeField]
    AudioClip RefreshingSound;
    [SerializeField]
    AudioClip Ignite;
    AudioSource AuSource;
    [SerializeField]
    float IgniteAplifier = 1;
    [SerializeField]
    float RefreshAmplifier = 1;
    [SerializeField]
    float BurnAmplifier = 1;
    float WaterTimer = 0;
    float WaterTime = 0.51f;
    public float CurrentHealth 
    {   get { return currentHealth; }  
        set 
        {
            currentHealth = value;
            UpdateHealthBar();
            UpdateFlowerSprite();
        }  
    }

    public bool IsBurning { get => isBurning; private set => isBurning = value; }

    void Start()
    {
        CurrentHealth = maxHealth;
        AuSource = GetComponent<AudioSource>();
        AuSource.clip = Burning;
        StartCoroutine(Decay());
    }

    private IEnumerator Decay()
    {
        while (true)
        {
            yield return new WaitForSeconds(decayRate);

            float amount = decayStrength;
            if (IsBurning)
            {
                if (!AuSource.isPlaying)
                {
                    if(MusicManager.Instance)
                        AuSource.volume = MusicManager.Instance.SfxVolume * BurnAmplifier;
                    AuSource.Play();
                }
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
        float percentage = CurrentHealth / maxHealth;

        healthImage.fillAmount = percentage;
    }

    void UpdateFlowerSprite()
    {
        float percentage = CurrentHealth / maxHealth;
        Sprite newState = healthySprite;

        if (percentage <= 0f)
        {
            newState = deadSprite;
        }
        else if (percentage <= 0.3f)
        {
            newState = dyingSprite;
        }
        else if (percentage <= 0.6f)
        {
            newState = damagedSprite;
        }

        spriteRenderer.sprite = newState;
    }

    public void SetOnFire()
    {
        IsBurning = true;
        fireParticles.Play();
        if (MusicManager.Instance)
            MusicManager.Instance.PlayOneShot(Ignite,IgniteAplifier);
    }

    private void ExtinguishFire()
    {
        IsBurning = false;
        fireParticles.Stop();
    }

    public void Water()
    {

        if (WaterTimer > Time.time)
            return;
        WaterTimer = Time.time + WaterTime;

        float curGain = nourishGain;
        if (IsBurning)
        {
            ExtinguishFire();
        }
        AuSource.Stop();

        if (MusicManager.Instance)
            MusicManager.Instance.PlayOneShot(RefreshingSound,RefreshAmplifier);

        CurrentHealth += curGain;
        if (CurrentHealth >= maxHealth)
        {
            CurrentHealth = maxHealth;
            nourishParticles.Play();
        }
    }
}
