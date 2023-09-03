using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int curHp;
    public int maxHp;
    public int score;
    public float moveSpeed;
    public float jumpForce;
    public int maxJumps;

    private int jumpsAvailable;
    private float curMoveInput;

    private PlayerController curAttacker;

    private List<GameObject> curStandingGameObjects = new List<GameObject>();

    [Header("Combat")]
    public float attackRate;
    private float lastAttackTime;
    public float projectileSpeed;
    public GameObject projectilePrefab;

    [Header("Components")]
    public Rigidbody2D rig;
    public Animator anim;
    public Transform muzzle;
    public PlayerContainerUI containerUI;

    public void Spawn (Vector3 spawnPos)
    {
        transform.position = spawnPos;
        curHp = maxHp;
        containerUI.UpdateHealthBar(curHp, maxHp);
        curAttacker = null;
    }

    void FixedUpdate ()
    {
        Move();

        if(curMoveInput == 0.0f && IsGrounded())
        {
            anim.SetBool("Moving", false);
            anim.SetBool("Jumping", false);
        }
        else if(curMoveInput != 0.0f && IsGrounded())
        {
            anim.SetBool("Moving", true);
            anim.SetBool("Jumping", false);
        }
        else if(!IsGrounded())
        {
            anim.SetBool("Moving", false);
            anim.SetBool("Jumping", true);
        }

        // hareket yonune göre yön değiştirme
        if(curMoveInput != 0.0f)
        {
            transform.localScale = new Vector3(curMoveInput > 0 ? 1 : -1, 1, 1);
        }
    }

    void Update ()
    {
        if(transform.position.y < -10)
            PlayerManager.instance.OnPlayerDeath(this, curAttacker);
    }

    void Move ()
    {
        rig.velocity = new Vector2(curMoveInput * moveSpeed, rig.velocity.y);
    }

    void Jump ()
    {
        rig.velocity = new Vector2(rig.velocity.x, 0);
        rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    //move
    public void OnMoveInput (InputAction.CallbackContext context)
    {
        curMoveInput = context.ReadValue<float>();
    }

    //jump
    public void OnJumpInput (InputAction.CallbackContext context)
    {
        
        if(context.phase == InputActionPhase.Performed)
        {
            
            if(jumpsAvailable > 0)
            {
                jumpsAvailable--;
                Jump();
            }
        }
    }

    //attack
    public void OnAttackInput (InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed && Time.time - lastAttackTime > attackRate)
        {
            lastAttackTime = Time.time;
            SpawnProjectile();
        }
    }

    void SpawnProjectile ()
    {
        GameObject projectile = Instantiate(projectilePrefab, muzzle.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * projectileSpeed, 0);
        projectile.GetComponent<Projectile>().owner = this;
    }

    public void TakeDamage (int damage, PlayerController attacker)
    {
        curAttacker = attacker;
        curHp -= damage;

        if(curHp <= 0)
            PlayerManager.instance.OnPlayerDeath(this, curAttacker);

        //can
        containerUI.UpdateHealthBar(curHp, maxHp);
    }

    bool IsGrounded ()
    {
        return curStandingGameObjects.Count > 0;
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        if(collision.contacts[0].point.y < transform.position.y)
        {
            jumpsAvailable = maxJumps;

            if(!curStandingGameObjects.Contains(collision.gameObject))
                curStandingGameObjects.Add(collision.gameObject);
        }
    }

    void OnCollisionExit2D (Collision2D collision)
    {
        if(curStandingGameObjects.Contains(collision.gameObject))
            curStandingGameObjects.Remove(collision.gameObject);
    }
}