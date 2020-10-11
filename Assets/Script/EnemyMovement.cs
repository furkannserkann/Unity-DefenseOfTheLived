using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private int Health = 50;
    [SerializeField] private float MoveSpeed = 1;
    [SerializeField] private int HitPower = 10;

    private Animator animator;
    private Rigidbody rigidbody;

    private PlayerController TargetPlayer;

    public int WayNumber = -1;

    [SerializeField] private GameObject myHealthBar;
    private Slider mySlider;
    private Healthbar cHealthbar;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            if (MoveSpeed <= 0)
            {
                MoveSpeed = 1;
            }
            animator.SetFloat("speed", MoveSpeed);

            startWalk();
        }

        if (myHealthBar != null)
        {
            myHealthBar.active = false;
            mySlider = myHealthBar.GetComponent<Slider>();
            mySlider.maxValue = Health;
            mySlider.value = Health;


            cHealthbar = myHealthBar.GetComponent<Healthbar>();
            cHealthbar.lowHealth = Health / 3;
            cHealthbar.highHealth = (Health / 3) * 2;
            cHealthbar.maximumHealth = Health;
            cHealthbar.health = Health;
        }
    }

    public void takeDamage(int damage)
    {
        if (Health > damage)
        {
            Health -= damage;
        }
        else
        {
            Health = 0;
        }

        if (myHealthBar != null)
        {
            myHealthBar.active = true;
        }
        mySlider.value = Health;
        cHealthbar.health = Health;

        if (Health == 0 && animator != null)
        {
            if (myHealthBar != null)
            {
                myHealthBar.active = false;
            }
            startDeath();
            GetComponent<CapsuleCollider>().enabled = false;

            StaticObj.levelEnemyList.DeletedArrayEnemy(WayNumber);
        }
        else if (Health == 0 && animator == null)
        {
            destroyEnemy();
        }
    }

    public void destroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Players")
        {
            TargetPlayer = collision.gameObject.GetComponent<PlayerController>();
            startAttack();
        } 
        else if (collision.gameObject.tag == "SınırCollider")
        {
            startAttack();
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Players")
        {
            TargetPlayer = other.gameObject.GetComponent<PlayerController>();
            startAttack();
        }
        else if (other.gameObject.tag == "SınırCollider")
        {
            startAttack();
        }
    }

    public void EnemyMovementColliderControl(bool isGround)
    {
        if (isGround)
        {
            startIdle();
        }
        else 
        {
            startWalk();
        }
    }

    private void startIdle()
    {
        if (animator != null)
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isAttack", false);
            animator.SetBool("isWalk", false);
            animator.SetBool("isDeath", false);

            if (rigidbody != null)
            {
                rigidbody.isKinematic = true;
            }
        }
    }

    private void startAttack()
    {
        if (animator != null)
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isAttack", true);
            animator.SetBool("isWalk", false);
            animator.SetBool("isDeath", false);

            if (rigidbody != null)
            {
                rigidbody.isKinematic = true;
            }
        }
    }

    private void startWalk()
    {
        if (animator != null)
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isAttack", false);
            animator.SetBool("isWalk", true);
            animator.SetBool("isDeath", false);
            if (rigidbody != null)
            {
                rigidbody.isKinematic = false;
            }
        }
    }

    private void startDeath()
    {
        if (animator != null)
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isAttack", false);
            animator.SetBool("isWalk", false);
            animator.SetBool("isDeath", true);
            if (rigidbody != null)
            {
                rigidbody.isKinematic = true;
            }
        }
    }

    public void DeathTarget()
    {
        TargetPlayer = null;
        startWalk();
    }

    private void HitTargetPlayer()
    {
        if (TargetPlayer != null)
        {
            TargetPlayer.TakeDamage(HitPower, this);
            if (!TargetPlayer.isLive)
            {
                TargetPlayer = null;
                startWalk();
            }
        }
    }
}
