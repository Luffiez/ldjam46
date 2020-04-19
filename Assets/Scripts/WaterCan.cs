using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

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
    GameObject WaterParticlePrefab;
    ParticleSystem WaterParticle;
    bool GameOver = false;
    [Header("Audio")]
    [SerializeField]
    AudioClip WateringClip;
    [SerializeField]
    AudioClip RefilingClip;
    [SerializeField]
    AudioClip EmptyClip;
    AudioSource ASource;
    private void Start()
    {
        ASource = GetComponent<AudioSource>();
        GameHandler.Instance.SetAmmoText(Ammo);
        WaterParticle = WaterParticlePrefab.GetComponent<ParticleSystem>();
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
        if (hit2D != null)
        {
            // Debug.Log("Refill");
            Ammo = MaxAmmo;
            ASource.clip = RefilingClip;
            ASource.Play();
            GameHandler.Instance.SetAmmoText(Ammo);
            WaterTimer = Time.time + WateringTime;
            return;
        }
        if (Ammo <= 0)
        {
            ASource.PlayOneShot(EmptyClip);
            WaterTimer = Time.time + 0.5f;
            return;
        }
        Ammo--;
        ASource.clip = WateringClip;
        ASource.Play();
        GameHandler.Instance.SetAmmoText(Ammo);
        WaterParticlePrefab.transform.position = WaterPosition;
        WaterParticle.Play();
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
