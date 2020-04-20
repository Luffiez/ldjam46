using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class WaterCan : MonoBehaviour
{ 
    Vector2 ShootDirection;
    [Tooltip("How the size of the radius when watering")]
    [SerializeField]
    float WaterRadius = 1.2f;
    [Tooltip("The maximum ammo the watercan can have, also refils to this value")]
    [SerializeField]
    int MaxAmmo;
    int Ammo = 10;
    [Tooltip("The time it takes to water")]
    [SerializeField]
    float WateringTime = 0.5f;
    float WaterTimer =0.0f;
    [Tooltip("The layers that the watercan can water")]
    [SerializeField]
    LayerMask WateringLayer;
    [Tooltip("The layers that the watercan can get water from")]
    [SerializeField]
    LayerMask RefilLayer;
    [SerializeField]
    ParticleSystem waterParticles;
    //GameObject WaterParticlePrefab;
    //ParticleSystem WaterParticle;
    bool GameOver = false;
    [Header("Audio")]
    [SerializeField]
    AudioClip WateringClip;
    [SerializeField]
    AudioClip RefilingClip;
    [SerializeField]
    AudioClip EmptyClip;
    AudioSource ASource;
    [SerializeField]
    private Animator emptyAnim;
    [SerializeField]
    TMP_Text refillText;

    PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        ASource = GetComponent<AudioSource>();
        GameHandler.Instance.SetAmmoText(Ammo);
        GameHandler.Instance.GameOver.AddListener(OnGameOver);
    }

    private void Update()
    {
        if (WaterTimer < Time.time)
            ASource.Stop();
    }

    public void GetShotDir(InputAction.CallbackContext context)
    {
        Vector2 tmpVec = context.ReadValue<Vector2>();
        if (tmpVec == Vector2.zero)
            return;
        ShootDirection = tmpVec;
    }

    public void OnWater(InputAction.CallbackContext context )
    {
        
        if ( context.phase == InputActionPhase.Canceled || context.phase == InputActionPhase.Started || WaterTimer > Time.time||GameOver)
            return;
        Vector2 WaterPosition = (Vector2)transform.position + ShootDirection;
        Collider2D hit2D = Physics2D.OverlapCircle(transform.position, WaterRadius, RefilLayer);
        if (hit2D != null && Ammo != MaxAmmo)
        {
            // Debug.Log("Refill");
            Ammo = MaxAmmo;
            ASource.clip = RefilingClip;
            if (MusicManager.Instance)
                ASource.volume = MusicManager.Instance.SfxVolume;
            ASource.Play();
            refillText.text = "Refilled!";
            emptyAnim.SetTrigger("Empty");
            GameHandler.Instance.SetAmmoText(Ammo);
            WaterTimer = Time.time + WateringTime;
            return;
        }
        if (Ammo <= 0)
        {
            if (MusicManager.Instance)
                MusicManager.Instance.PlayOneShot(EmptyClip);
            refillText.text = "Emtpy";
            emptyAnim.SetTrigger("Empty");
            WaterTimer = Time.time + 0.5f;
            return;
        }
        Ammo--;
        ASource.clip = WateringClip;
        if (MusicManager.Instance)
            ASource.volume = MusicManager.Instance.SfxVolume;
        ASource.Play();
        GameHandler.Instance.SetAmmoText(Ammo);

        float rotationZ = Mathf.Atan2(ShootDirection.y, ShootDirection.x) * Mathf.Rad2Deg;
        waterParticles.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);

        waterParticles.Play();
        WaterTimer = Time.time + WateringTime;
        Collider2D [] hits2D = Physics2D.OverlapCircleAll(WaterPosition, WaterRadius,WateringLayer);
        if (hits2D.Length > 0)
        {
            for (int i = 0; i < hits2D.Length; i++)
            {
                IWater iWater = hits2D[i].GetComponent<IWater>();
                if (iWater != null)
                    iWater.Water();
            }
        }
    }

    void OnGameOver()
    {
        GameOver = true;
    }

    private void OnEnable()
    {
        if(GameHandler.Instance != null)
        GameHandler.Instance.GameOver.AddListener(OnGameOver);
    }

    private void OnDisable()
    {
        GameHandler.Instance.GameOver.RemoveListener(OnGameOver);
    }

}
