using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D RBody;
    [SerializeField]
    float Speed;
    [SerializeField]
    float WateringTime;
    float WaterTimer;
    Vector2 MoveDirection;
    // Start is called before the first frame update
    private void Start()
    {
        RBody = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame

    private void FixedUpdate()
    {
        if (WaterTimer > Time.time)
        { RBody.velocity = Vector2.zero; }
        else
        { RBody.velocity = MoveDirection * Speed; }
    }

    public void GetMovementInput(InputAction.CallbackContext context)
    {
        MoveDirection = context.ReadValue<Vector2>();
    }

    public void OnWater(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
         WaterTimer = Time.time + WateringTime;
    }

}
