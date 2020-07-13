using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemies //Sub-class of the main Enemies class 
{
    [SerializeField] GameObject projectile, Gun;
    [SerializeField] float throwSpeed;
    [SerializeField] float playerSpottedRange;

    Rigidbody2D myRigidBody;
    CapsuleCollider2D bodyCollider;
    BoxCollider2D wallCollider;
    Player player;
    Animator myAnimator;

    bool facingRight;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        wallCollider = GetComponent<BoxCollider2D>();
        player = FindObjectOfType<Player>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement(movementSpeed);
        TargetPlayer();
    }

    public override void Movement(float movementSpeed)
    {
        if (IsFacingLeft())
        {
            myRigidBody.velocity = new Vector2(movementSpeed, 0f);
        }
        else
        {
            myRigidBody.velocity = new Vector2(-movementSpeed, 0f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), transform.localScale.y);
    }

    public bool IsFacingLeft()
    {
        return transform.localScale.x < 0;
    }
    

    public override void TargetPlayer()
    {
        Vector2 playerPos = player.transform.position;//barbarian code but better to avoid some bug with layers
        var playerSpotted = Vector2.Distance(transform.position, playerPos);
        if (playerSpotted <= playerSpottedRange)
        {
            myAnimator.SetBool("isAttacking", true);
            RotateTowards(playerPos);
            myRigidBody.velocity = new Vector3(0, 0, 0);
        }

        else
        {
            myAnimator.SetBool("isAttacking", false);
            return;
        }
    }

    private void RotateTowards(Vector2 target)
    {

        if (target.x > transform.position.x && !facingRight) //if the target is to the right of enemy and the enemy is not facing right
        {
            Flip();
        }
            
        if (target.x < transform.position.x && facingRight)
        {
            Flip();
        }
        /*var offset = 180f;
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.up * (angle + offset));*/
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingRight = !facingRight;
    }

        private void Fire()
    {
        GameObject newProjectile = Instantiate(projectile, Gun.transform.position, Quaternion.identity) as GameObject;
    }

    public override void Die()
    {
        myRigidBody.gravityScale = 1;//To Debug if time
        myAnimator.SetTrigger("isDying");
        StartCoroutine(IsDying());
    }

    IEnumerator IsDying()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
}
