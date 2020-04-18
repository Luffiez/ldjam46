using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class WaterCan : MonoBehaviour
{ 
    Vector2 ShootDir;
    [SerializeField]
    float WaterRadius = 1.2f;
    [SerializeField]
    int MaxAmmo;
    int Ammo = 10;
    [SerializeField]
    float WateringTime = 0.5f;
    float WaterTimer =0.0f;
    [SerializeField]
    LayerMask WateringLayer;
    [SerializeField]
    LayerMask RefilLayer;
    [SerializeField]
    GameObject WaterPrefab;
    public void GetShotDir(InputAction.CallbackContext context)
    {
        Vector2 tmpVec = context.ReadValue<Vector2>();
        if (tmpVec == Vector2.zero)
            return;
        ShootDir = tmpVec;
    }

    public void OnWater(InputAction.CallbackContext context )
    {
        
        if (Ammo <= 0 || context.phase == InputActionPhase.Canceled || context.phase == InputActionPhase.Started || WaterTimer > Time.time)
            return;
        Ammo--;
        WaterTimer = Time.time + WateringTime;
        Debug.Log("watering" + context.phase);
        
        Vector2 WaterPosition = (Vector2)transform.position + ShootDir;
        WaterPrefab.transform.position = WaterPosition;
        Collider2D hit2D = Physics2D.OverlapCircle(WaterPosition, WaterRadius,RefilLayer);
        if (hit2D != null)
        {
            Debug.Log("Refil");
            Ammo = MaxAmmo;
            return;
        }
        
        Collider2D [] hits2D = Physics2D.OverlapCircleAll(WaterPosition, WaterRadius,WateringLayer);
        if (hits2D.Length > 0)
        {
            for (int i = 0; 0 < hits2D.Length; i++)
            {
                WaterInterface iWater = hits2D[i].GetComponent<WaterInterface>();
                if (iWater != null)
                    iWater.Water();
            }
        }
    }
}
