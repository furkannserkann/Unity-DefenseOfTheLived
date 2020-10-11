using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float BulletMoveSpeed = 50f;

    [SerializeField] private int Health = 50;
    [SerializeField] private int HitPower = 10;
    public float ShotSpeed = 1f;

    [System.NonSerialized] public GameObject SelectedCreateCube;
    public int WayNumber = -1;

    [Space]
    [SerializeField] private GameObject firePoint;
    [SerializeField] private GameObject effectToSpawn;
    [SerializeField] private GameObject myHealthBar;
    private Slider mySlider;
    private Healthbar cHealthbar;

    private Animator animator;
    private float lastAnimSpeed = 1f;

    public bool isAttackStatus = true;

    public bool isLive => Health <= 0 ? false : true;


    public static int _HitPower { get { return _HitPower; } }

    void Start()
    {
        if (isAttackStatus)
        {
            animator = GetComponent<Animator>();
            if (animator != null)
            {
                lastAnimSpeed = animator.GetFloat("speed");
                animator.SetFloat("speed", ShotSpeed);

                Attack(false);
            }
        }

        if (myHealthBar != null)
        {
            myHealthBar.active = false;
            mySlider = myHealthBar.GetComponent<Slider>();
            mySlider.maxValue = Health;
            mySlider.value = Health;

            cHealthbar = myHealthBar.GetComponent<Healthbar>();
            cHealthbar.maximumHealth = Health;
            cHealthbar.health = Health;
        }
    }

    void Update()
    {
        if (lastAnimSpeed != ShotSpeed && animator != null && ShotSpeed >= 1 && isAttackStatus)
        {
            animator.SetFloat("speed", ShotSpeed);
            lastAnimSpeed = ShotSpeed;
        }

        if (WayNumber > -1)
        {
            if (StaticObj.levelEnemyList.WayEnemyCount[WayNumber] > 0)
            {
                Attack(true);
            }
            else
            {
                Attack(false);
            }
        }
    }

    void SpawnVFX()
    {
        if (firePoint != null)
        {
            var vfx = Instantiate(effectToSpawn, firePoint.transform.position, Quaternion.identity);
            vfx.transform.parent = GameObject.FindGameObjectWithTag("Parent_Bullets").transform;
            BulletMove bm = vfx.GetComponent<BulletMove>();
            bm.hitPower = HitPower;
            bm.thisPlayerController = this;
        }
        else
        {
            Debug.Log("No Fire Point!");
        }
    }

    public void TakeDamage(int damage, EnemyMovement enemyMovement)
    {
        if (Health >= damage)
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
            if (animator != null)
            {
                if (myHealthBar != null)
                {
                    myHealthBar.active = false;
                }
                enemyMovement.DeathTarget();
                GetComponent<CharacterController>().enabled = false;
                GetComponent<CapsuleCollider>().enabled = false;
                animator.SetBool("isIdle", false);
                animator.SetBool("isAttack", false);
                animator.SetBool("isDeath", true);
            }
        }
        else if (Health == 0 && animator == null)
        {
            destroyPlayer();
        }
    }

    public void destroyPlayer()
    {
        PlaceInfo localPlaceInfo = SelectedCreateCube.GetComponent<PlaceInfo>();
        if (localPlaceInfo != null)
        {
            localPlaceInfo.isEmpty = true;
        }

        Destroy(gameObject);
    }

    private void Attack(bool isState)
    {
        if (isAttackStatus)
        {
            if (animator != null)
            {
                if (isState)
                {
                    if (ShotSpeed <= 1)
                    {
                        ShotSpeed = 1;
                    }

                    animator.SetFloat("speed", ShotSpeed);
                    animator.SetBool("isIdle", false);
                    animator.SetBool("isAttack", true);
                }
                else
                {
                    animator.SetBool("isAttack", false);
                    animator.SetBool("isIdle", true);
                }
            }
        }
    }
}
