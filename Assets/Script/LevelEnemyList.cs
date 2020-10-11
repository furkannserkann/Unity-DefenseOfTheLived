using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelEnemyList : MonoBehaviour
{
    // Test Ekranı

    public GameObject[] EnemyList;
    public GameObject[] EnemySpawnPoints = new GameObject[5];

    public int TotalEnemyCount = 0;
    [System.NonSerialized] public int[] WayEnemyCount = { 0, 0, 0, 0, 0 };

    [SerializeField] private int WaveCount = 0;

    float deltaTime = 0.0f;

    /*
     * Test-) 1 tane en ezik zombiden ortadan
     *        ölünce ortadan 1 tane daha
     *        ölünce ortadan 2 tane, bitir
     */
    void Start()
    {
        StartCoroutine(levelTest());
    }

    IEnumerator levelTest()
    {
        if (EnemyList != null)
        {
            if (TotalEnemyCount == 0)
            {
                yield return new WaitForSeconds(0.5f);
                if (WaveCount == 0)
                {
                    createEnemy(EnemyList[0], false, false, true, false, false);
                }
                else if (WaveCount == 1)
                {
                    createEnemy(EnemyList[0], true, false, false, false, false);
                }
                else if (WaveCount == 2)
                {
                    createEnemy(EnemyList[0], true, false, true, false, false);
                }
                else if (WaveCount == 3)
                {
                    createEnemy(EnemyList[0], false, false, true, false, false);
                    yield return new WaitForSeconds(0.5f);
                    createEnemy(EnemyList[0], true, false, true, false, false);
                }
                else if (WaveCount == 4)
                {
                    createEnemy(EnemyList[0], false, false, true, false, false);
                    yield return new WaitForSeconds(0.5f);
                    createEnemy(EnemyList[0], true, false, true, false, false);
                    yield return new WaitForSeconds(1f);
                    createEnemy(EnemyList[0], true, false, true, false, false);
                    yield return new WaitForSeconds(0.5f);
                    createEnemy(EnemyList[0], true, false, false, false, false);
                }

                if (WaveCount >= 4)
                    WaveCount = 2;

                WaveCount++;
            }
        }
    }

    public void DeletedArrayEnemy(int row)
    {
        WayEnemyCount[row]--;
        TotalEnemyCount--;
        StartCoroutine(levelTest());
    }

    private void createEnemy(GameObject enemy, bool Way1, bool Way2, bool Way3, bool Way4, bool Way5)
    {
        bool[] wayList = { Way1, Way2, Way3, Way4, Way5 };
        for (int i = 0; i < wayList.Length; i++)
        {
            if (wayList[i])
            {
                WayEnemyCount[i]++;

                var lEnemy = Instantiate(enemy, EnemySpawnPoints[i].transform.position, Quaternion.identity);
                lEnemy.transform.parent = GameObject.FindGameObjectWithTag("Parent_Enemy").transform;
                lEnemy.transform.rotation = EnemyList[0].transform.rotation;
                
                EnemyMovement lEnemyMovement = lEnemy.GetComponent<EnemyMovement>();
                if (lEnemyMovement != null)
                {
                    lEnemyMovement.WayNumber = i;
                }

                TotalEnemyCount++;
            }
        }
    }
}
