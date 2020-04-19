using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D RBody;
    Animator anim;
    [SerializeField]
    float Speed;
    [SerializeField]
    float WateringTime;
    float WaterTimer;
    Vector2 MoveDirection;
    bool GameOver = false;
    bool isFacingRight = true;
    bool isFacingUp = false;
    [SerializeField]
    private SpriteRenderer wateringCanSprite;
    // Start is called before the first frame update
    private void Start()
    {
        RBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GameHandler.Instance.GameOver.AddListener(OnGameOver);
    }
    // Update is called once per frame

    private void FixedUpdate()
    {
        if (GameOver)
            return;
        if (WaterTimer > Time.time)
        { RBody.velocity = Vector2.zero; }
        else
        { 
            RBody.velocity = MoveDirection * Speed; 
            if(RBody.velocity.x > 0 && !isFacingRight ||
                RBody.velocity.x < 0 && isFacingRight)
            {
                FlipX();
            }
            if(RBody.velocity.y > 0 && !isFacingUp ||
                RBody.velocity.y < 0 && isFacingUp ||
                RBody.velocity.x != 0 && RBody.velocity.y < 0 && isFacingUp)
            {
                FlipY();
            }
        }

        anim.SetFloat("x", RBody.velocity.x);
        anim.SetFloat("y", RBody.velocity.y);

    }

    public void GetMovementInput(InputAction.CallbackContext context)
    {
        MoveDirection = context.ReadValue<Vector2>();
    }

    public void OnWater(InputAction.CallbackContext context)
    {
        if (GameOver)
            return;

        if (context.phase == InputActionPhase.Performed)
        {
            WaterTimer = Time.time + WateringTime;
            anim.SetTrigger("Water");
        }
    }

    void OnGameOver()
    {
        GameOver = true;
        RBody.velocity = Vector2.zero;
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

    void FlipX()
    {
        isFacingRight = !isFacingRight;

        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    void FlipY()
    {
        isFacingUp = !isFacingUp;
        if (isFacingUp)
            wateringCanSprite.sortingOrder = 3;
        else
            wateringCanSprite.sortingOrder = 5;
    }
}
