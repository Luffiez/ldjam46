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


    Vector2 MoveDirection;
    // Start is called before the first frame update
    private void Start()
    {
        RBody = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame

    private void FixedUpdate()
    {
        RBody.velocity = MoveDirection* Speed;
    }

    public void GetMovementInput(InputAction.CallbackContext context)
    {
        MoveDirection = context.ReadValue<Vector2>();
    }



}
