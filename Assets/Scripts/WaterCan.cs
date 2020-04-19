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
    GameObject WaterPrefab;
    public void GetShotDir(InputAction.CallbackContext context)
    {
        Vector2 tmpVec = context.ReadValue<Vector2>();
        if (tmpVec == Vector2.zero)
            return;
        ShootDirection = tmpVec;
    }

    public void OnWater(InputAction.CallbackContext context )
    {
        
        if ( context.phase == InputActionPhase.Canceled || context.phase == InputActionPhase.Started || WaterTimer > Time.time)
            return;
        Vector2 WaterPosition = (Vector2)transform.position + ShootDirection;
        Collider2D hit2D = Physics2D.OverlapCircle(WaterPosition, WaterRadius, RefilLayer);
        if (hit2D != null)
        {
            Debug.Log("Refil");
            Ammo = MaxAmmo;
            return;
        }
        if (Ammo <= 0)
            return;
        Ammo--;
        WaterPrefab.transform.position = WaterPosition;
        Debug.Log("ammo " + Ammo);
        WaterTimer = Time.time + WateringTime;
        Debug.Log("watering" + context.phase);
        
   
        
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
}
