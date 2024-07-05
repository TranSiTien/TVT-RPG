using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float MOVE_SPEED = 1.0f;
    public float COLLISION_OFFSET = 0.2f;
    public ContactFilter2D contactFilter;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    SpriteRenderer spriteRenderer;
    Animator animator;
    Vector2 movementInput;
    Rigidbody2D rb;
    public GameObject swordPrefab;
    bool canMove = true;
    public float ENERGY_SPEED = 10f;
    public float DESTROY_AFTER_SC = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            if (movementInput != Vector2.zero)
            {
                if ((movementInput.x == 0 || movementInput.y == 0) && TryMove(movementInput) == false)
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
                current_animation == "Idle_right" && animator.GetFloat("moveX") < 0 ||
                current_animation == "Swing_sword_right" && animator.GetFloat("moveX") < 0)
                spriteRenderer.flipX = true;
            else
                spriteRenderer.flipX = false;
        }

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
            return true;
        }
        Debug.Log("count" + count);
        animator.SetBool("isMoving", false);
        return false;
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }
    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    void OnFire()
    {
        animator.SetTrigger("swordAttack");
        createSwordEnergy();
    }

    private void createSwordEnergy()
    {
        Direction direction = Direction.Up;
        Vector2 dirVect = Vector2.zero;
        float z_rotation = 0;
        string current_animation = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if (current_animation == "Idle_Down" || current_animation == "Walk_down")
            direction = Direction.Down;
        if (current_animation == "Idle_Up" || current_animation == "Walk_up")
            direction = Direction.Up;
        if (current_animation == "Idle_right" || current_animation == "Walk_right")
            if (animator.GetFloat("moveX") < 0)
                direction = Direction.Left;
            else
                direction = Direction.Right;
        switch (direction)
        {
            case Direction.Right:
                z_rotation = 90;
                dirVect = new Vector2(ENERGY_SPEED, 0);
                break;
            case Direction.Left:
                z_rotation = -90;
                dirVect = new Vector2(-ENERGY_SPEED, 0);
                break;
            case Direction.Up:
                z_rotation = 180;
                dirVect = new Vector2(0, ENERGY_SPEED);
                break;
            case Direction.Down:
                z_rotation = 0;
                dirVect = new Vector2(0, -ENERGY_SPEED);
                break;
        }


        Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, z_rotation);
        GameObject energy = Instantiate(swordPrefab, transform.position, transform.rotation);
        Rigidbody2D energyRb = energy.GetComponent<Rigidbody2D>();
        energy.transform.rotation = rotation;
        energyRb.velocity = dirVect;
        Destroy(energy, DESTROY_AFTER_SC);
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }
}
