using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public Collider2D swordCollider;
    public float damage = 3;
    float WIDTH_HORIZONTAL = 0.6f, HEIGHT_HORIZONTAL = 0.85f;
    float WIDTH_VERTICAL = 0.6f, HEIGHT_VERTICAL = 0.85f;
    Vector2 rightAttackOffset = new Vector2(0.1f, 0.1f);
    Vector2 leftAttackOffset = new Vector2(-0.1f, 0.1f);
    Vector2 upAttackOffset = new Vector2(0f, 0.1f);
    Vector2 downAttackOffset = new Vector2(0f, -0.2f);

    public enum  AttackDirection
    {
        left, right, up, down
    }
    private void Start()
    {
    }

    public void AttackRight()
    {
        swordCollider.enabled = true;
        transform.localPosition = rightAttackOffset;
    }

    public void AttackLeft()
    {
        swordCollider.enabled = true;
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
    }

    public void StopAttack()
    {
        swordCollider.enabled = false;
    }
}
