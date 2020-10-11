using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public GameObject muzzlePrefab;
    public GameObject hitPrefab;
    public PlayerController thisPlayerController;
    private float localSpeed;

    [System.NonSerialized] public int hitPower = 10;

    void Start()
    {
        localSpeed = thisPlayerController.BulletMoveSpeed;

        if (muzzlePrefab != null)
        {
            var muzzleVFX = Instantiate(muzzlePrefab, transform.position, Quaternion.identity);
            muzzleVFX.transform.parent = GameObject.FindGameObjectWithTag("Parent_Bullets").transform;
            muzzleVFX.transform.forward = gameObject.transform.forward;

            var psMuzzle = muzzleVFX.GetComponent<ParticleSystem>();
            if (psMuzzle != null)
            {
                Destroy(muzzleVFX, psMuzzle.main.duration);
            }
            else
            {
                var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(muzzleVFX, psChild.main.duration);
            }
        }
    }

    void Update()
    {
        if (localSpeed != 0)
        {
            Vector3 tp = transform.position;
            tp.x += localSpeed * Time.deltaTime;
            transform.position = tp;
        }
        else
        {
            Debug.Log("No Speed!");
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Players")
        {
            if (collision.gameObject.tag == "Enemy")
            {
                EnemyTakeDamage(collision.gameObject.GetComponent<EnemyMovement>());
            }

            ContactBullet();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Players")
        {
            if (other.gameObject.tag == "Enemy")
            {
                EnemyTakeDamage(other.gameObject.GetComponent<EnemyMovement>());
            }

            ContactBullet();
        }
    }

    private void EnemyTakeDamage(EnemyMovement localEnemyMovement)
    {
        localEnemyMovement.takeDamage(StaticObj.HitDamage(hitPower));
    }

    public void ContactBullet()
    {
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, gameObject.transform.position);
        Vector3 pos = gameObject.transform.position;

        if (hitPrefab != null)
        {
            var hitVFX = Instantiate(hitPrefab, pos, rot);
            hitVFX.transform.parent = GameObject.FindGameObjectWithTag("Parent_Bullets").transform;

            var psMuzzle = hitVFX.GetComponent<ParticleSystem>();
            if (psMuzzle != null)
            {
                Destroy(hitVFX, psMuzzle.main.duration);
            }
            else
            {
                var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitVFX, psChild.main.duration);
            }
        }

        localSpeed = 0;
        Destroy(gameObject);
    }
}
