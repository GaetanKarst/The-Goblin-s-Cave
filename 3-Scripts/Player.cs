using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float climbSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float gravityScaleAtStart;
    [SerializeField] float health;
    [SerializeField] Vector2 deathKick;
    [SerializeField] float playerDamages;
    [SerializeField] GameObject currentTarget;
    [SerializeField] float _hitRange;
    [SerializeField] Transform attackPos;
    [SerializeField] LayerMask enemies;

    Rigidbody2D playerRb;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeet;


    bool isAlive = true;

    AudioSource SFX;
    [SerializeField] TextMeshProUGUI HpText;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = playerRb.gravityScale;
        
    
        HpText.text = health.ToString();
        GameDifficulty();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; }//if player dead don't do the things below
        Move();
        Jump();
        ClimbLadder();
        FlipSprite();
        DeathByHazard();
        Attack(playerDamages);

        SFX = GetComponent<AudioSource>();
    }

    //Player Attack

    private void Attack(float playerDamages)
    {
        if (Input.GetMouseButtonDown(0))
        {
            SFX.Play();
            myAnimator.SetTrigger("isAttacking");
            Target();
            Debug.Log(playerDamages);
        }
    }

    public void Target()
    {
        //stores the enemies found within a circle
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, _hitRange, enemies);
        for (int nbOfEnemies = 0; nbOfEnemies < enemiesToDamage.Length; nbOfEnemies++)//run the look a certain amount of time depending on the number of enemies found in the circle
        {
            enemiesToDamage[nbOfEnemies].GetComponent<Enemies>().TakeDamage(playerDamages);
            enemiesToDamage[nbOfEnemies].GetComponentInChildren<SpriteRenderer>().color = Color.red;
            Invoke("ResetMaterial", .5f);
        }
    }

    private void ResetMaterial()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, _hitRange);
    }

    //Player Movements

    private void Move()
    {
        float _moveInput = Input.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(_moveInput * moveSpeed, playerRb.velocity.y);
        playerRb.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(playerRb.velocity.x) > Mathf.Epsilon;
         myAnimator.SetBool("isRunning", playerHasHorizontalSpeed); // avoid if statement and faster if you don't have any other actions to do at the same moment    
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRb.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2 (Mathf.Sign(playerRb.velocity.x), transform.localScale.y);
        }
    }

    private void Jump()
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        else
        {
            if (Input.GetButtonDown("Jump"))

            {
                Vector2 playerVelocityToAdd = new Vector2(0f, jumpSpeed);
                playerRb.velocity += playerVelocityToAdd;
            }
        }
    }

    private void ClimbLadder()
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myAnimator.SetBool("isClimbing", false);
            playerRb.gravityScale = gravityScaleAtStart;
            return;
        }
        else
        {
            float _moveInputY = Input.GetAxis("Vertical");
            Vector2 playerVelocity = new Vector2(playerRb.velocity.x, _moveInputY * climbSpeed);
            playerRb.velocity = playerVelocity;
            playerRb.gravityScale = 0;

            bool playerHasVerticalSpeed = Mathf.Abs(playerRb.velocity.y) > Mathf.Epsilon;
            myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
        }
    }

    //Player's Death

    public void TakeDamage(float damages)
    {
        health -= damages;
        HpText.text = health.ToString();
        if (health <= 0)
        {
            StartCoroutine(PlayerDeath());
        }
    }

    public void DeathByHazard()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            Debug.Log("Death from Hazard");
            StartCoroutine(PlayerDeath());
        }
    }

    public void DeathBySpike()
    {
        Debug.Log("Death from Spike");
        health = 0;
        isAlive = false;
        myAnimator.SetBool("isDead", true);
        GetComponent<Rigidbody2D>().velocity = deathKick;
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }

    IEnumerator PlayerDeath()
    {
        Debug.Log("Player is dead");
        health = 0;
        isAlive = false;
        myAnimator.SetBool("isDead", true);
        GetComponent<Rigidbody2D>().velocity = deathKick;
        yield return new WaitForSeconds(3);
        FindObjectOfType<GameSession>().ProcessPlayerDeath();      
    }

    //difficulty

    public void GameDifficulty()
    {
        var gameDifficulty = PlayerPrefsController.GetMasterDifficulty();
        Debug.Log("Difficulty set to " + gameDifficulty);
        //easy
        if(gameDifficulty == 0f)
        {
            return;
        }
        //normal
        else if(gameDifficulty == 1f)
        {
            health = 100f;
            HpText.text = health.ToString();
        }
        //hard
        else if(gameDifficulty == 2f)
        {
            health = 50f;
            HpText.text = health.ToString();
        }
    }
}
