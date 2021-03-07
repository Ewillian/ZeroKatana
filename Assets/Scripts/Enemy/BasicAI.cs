using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour
{
    [SerializeField] Transform player;

    [SerializeField] Transform attackPoint;
    private float attackRange = 0.5f;
    private float attackDamage = 25;
    [SerializeField] LayerMask playerLayer;

    private float agroRange;

    private float moveSpeed;

    private Rigidbody2D rigibody;

    private int moveDirection;

    private Animator animator;

    private bool is_attacking;

    /// <summary>
    /// TODO
    /// </summary>
    private float attackRate = 1f;

    /// <summary>
    /// TODO
    /// </summary>
    private bool is_facingRight = true;

    /// <summary>
    /// TODO
    /// </summary>
    private float nextAttackTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rigibody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        moveSpeed = 2;
        agroRange = 5;
    }

    // Update is called once per frame
    void Update()
    {
        BaseAI();

        Animate();

        StartCoroutine(AttackPlayer());
    }

    private void Animate()
    {
        if (moveDirection > 0 && !is_facingRight)
        {
            ChangeEnemyDirection();
        }
        else if (moveDirection < 0 && is_facingRight)
        {
            ChangeEnemyDirection();
        }
    }

    private void BaseAI()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < agroRange)
        {
            Chase();
        }
        else 
        {
            // Switch walk animation to idle
            animator.SetFloat("speed", 0);
        }
    }

    private void Chase()
    {

        if (transform.position.x > player.position.x && transform.position.x < player.position.x + 1.5 || transform.position.x < player.position.x && transform.position.x > player.position.x - 1.5)
        {
            // Prevent enemy to continue walking to player 
            animator.SetFloat("speed", 0); // Switch walk animation to idle
        }
        else if(transform.position.x < player.position.x)
        {
            // Enemy is on the left and need to move right
            animator.SetFloat("speed", moveDirection);
            moveDirection = 1;
            rigibody.velocity = new Vector2(moveSpeed, rigibody.velocity.y);
        }
        else if (transform.position.x > player.position.x)
        {
            // Enemy is on the right and need to move left
            animator.SetFloat("speed", Math.Abs(moveDirection));
            moveDirection = -1;
            rigibody.velocity = new Vector2(-moveSpeed, rigibody.velocity.y);
        }
    }

    /// <summary>
    /// Using facingRight variable, change the character front rotation according to enemy movement.
    /// </summary>
    /// <returns></returns>
    private void ChangeEnemyDirection()
    {
        is_facingRight = !is_facingRight; // Reverse bool
        transform.Rotate(0f, 180f, 0f);
    }

    private IEnumerator AttackPlayer()
    {
        Collider2D playerInfo = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        if (playerInfo != null && playerInfo.GetComponent<Player>().is_dead == false)
        {
            if (Time.time >= nextAttackTime && is_attacking == false)
            {
                is_attacking = true;
                // Trigger attack animation
                animator.SetTrigger("attack");

                Debug.Log("att");

                // Delay Hit in relation to animation logic
                yield return new WaitForSeconds(1f);

                Collider2D player = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
                player.GetComponent<Player>().TakeDamage(attackDamage);
                is_attacking = false;

                nextAttackTime = Time.time + 1f / attackRate;
            }
            
        }
    }

    /// <summary>
    /// TODO commentary
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    //Stop chasing player when object is in between
    //private void CanSeePlayer(float distance)
    //{
    //    Vector2 endPos = castPoint.position + Vector3.right * distance;
    //    RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPos, 1 << LayerMask.NameToLayer("Platforms"));

    //    if (hit.collider != null)
    //    {
    //        if (hit.collider.gameObject.CompareTag("Player"))
    //        {
    //            Chase();
    //        }
    //    }
    //}
}
