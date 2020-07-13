using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 2.6f;
    [SerializeField] int damages = 1;
    Vector3 playerPos;
    Vector3 PlayerTargeted;

    Transform _player;
    Vector2 _target;
    Rigidbody2D myRigidBody2D;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _target = new Vector2(_player.position.x, _player.position.y);
    }

    private void Update()
    {
        ThrowToPlayer();
    }

    private void ThrowToPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, _target, projectileSpeed * Time.deltaTime);
        if (transform.position.x == _target.x && transform.position.y == _target.y)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var targetPlayer = collision.gameObject.GetComponent<Player>();
        if (targetPlayer)
        {
            targetPlayer.TakeDamage(damages);
        }
        Destroy(gameObject);
    }

}
