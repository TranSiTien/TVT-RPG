using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float MOVE_SPEED = 1.0f;
    public float COLLISION_OFFSET = 0.05f;
    public ContactFilter2D contactFilter;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    SpriteRenderer spriteRenderer;
    Animator animator;
    Vector2 movementInput;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {

        if (movementInput != Vector2.zero) {
            if((movementInput.x == 0 || movementInput.y == 0) && TryMove(movementInput) == false)
            { return; }
            bool moved = TryMove(movementInput);
            if (!moved)
            {
                moved = TryMove(new Vector2(movementInput.x, 0));
            }
            if (!moved)
            {
                TryMove(new Vector2(0, movementInput.y));
            }

        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        // get name of current animation
        string current_animation = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if (current_animation == "Walk_right" && movementInput.x < 0 ||
            current_animation == "Idle_right" && animator.GetFloat("moveX") < 0)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;

    }

    private bool TryMove(Vector2 movementInput)
    {
        int count = rb.Cast(
                        movementInput,
                        contactFilter,
                        castCollisions,
                        MOVE_SPEED * Time.fixedDeltaTime + COLLISION_OFFSET
                        );
        if (count == 0)
        {

            animator.SetFloat("moveX", movementInput.x);
            animator.SetFloat("moveY", movementInput.y);
            animator.SetBool("isMoving", true);
            rb.MovePosition(rb.position + MOVE_SPEED * movementInput * Time.fixedDeltaTime);
            Debug.Log("new position" + (rb.position + MOVE_SPEED * movementInput * Time.fixedDeltaTime));
            Debug.Log("move" + MOVE_SPEED * movementInput * Time.fixedDeltaTime);
            Debug.Log("count" + count);
            return true;
        }
        animator.SetBool("isMoving", false);
        return false;
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }
}
