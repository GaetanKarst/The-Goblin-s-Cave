using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeUp : MonoBehaviour
{
    [SerializeField] float scrollDown;

    Rigidbody2D myRigidbody2D;

    private void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            DropSpike();
        }     
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.name.Equals ("Player"))
        {
            FindObjectOfType<Player>().DeathBySpike();
        } 
    }

    private void DropSpike()
    {
        myRigidbody2D.isKinematic = false;
    }

}
