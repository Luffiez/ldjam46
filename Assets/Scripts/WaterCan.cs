using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaterCan : MonoBehaviour
{
    Vector2 ShootDir;
    [SerializeField]
    LayerMask LMask;
    [SerializeField]
    float WaterRadius = 1.2f;
    [SerializeField]
    int MaxAmmo;
    int Ammo = 10;
    float waterTime = 0.0f;
    public void GetShotDir(InputAction.CallbackContext context)
    {
        Vector2 tmpVec = context.ReadValue<Vector2>();
        if (tmpVec == Vector2.zero)
            return;
        ShootDir = tmpVec;
    }

    public void OnWater(InputAction.CallbackContext context )
    {
        
        if (Ammo <= 0)
            return;
        Debug.Log("watering" );
        Ammo--;
        Vector2 WaterPosition = (Vector2)transform.position + ShootDir;
       Collider2D [] hits2D = Physics2D.OverlapCircleAll(WaterPosition, WaterRadius,LMask);
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
