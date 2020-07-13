using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin2 : Enemies //Sub-class of the main Enemies class
{
    public GameObject currentTarget;

    bool targetAlive;

    Rigidbody2D myRigidBody;
    CapsuleCollider2D bodyCollider;
    BoxCollider2D wallCollider;
    Animator myAnimator;


    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        wallCollider = GetComponent<BoxCollider2D>();
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

    private bool IsFacingLeft()
    {
        return transform.localScale.x < 0;
    }

    public override void TargetPlayer()
    {
        if (wallCollider.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            GetComponent<Animator>().SetBool("isAttacking", true);
            myRigidBody.velocity = new Vector3(0, 0, 0); 
        }

        if (!wallCollider.IsTouchingLayers(LayerMask.GetMask("Player")) && !GetComponent<Player>())
        {
            GetComponent<Animator>().SetBool("isAttacking", false);
        }
    }

    public void StrikeCurrentTarget(int damages)
    {
        Debug.Log("Enemy damages: " + damages);
        if (!currentTarget) { Debug.Log("Have no target"); return; }
        var playerTarget = currentTarget.GetComponent<Player>();

        if(playerTarget)
        {
            playerTarget.TakeDamage(damages);
        }
    }

    public override void Die()
    {
        myRigidBody.velocity = new Vector3(0, 0, 0);
        myAnimator.SetTrigger("isDying"); 
        StartCoroutine(IsDying());
    }

    IEnumerator IsDying()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
}
