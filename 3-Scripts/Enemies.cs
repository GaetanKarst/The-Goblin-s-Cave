using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemies : MonoBehaviour
{
    public float movementSpeed;
    public float health;
    //damages set within the animation, passed in StrikeCurrentTarget method

    private void Start()
    {

    }

    public virtual void Movement(float movementSpeed)
    {
        //pattern for enenmies
    }

    public virtual void TargetPlayer()
    {
        //pattern for enenmies
    }

    public void TakeDamage(float damages)
    {
        health -= damages;
        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        //pattern for enemies
    }

}
