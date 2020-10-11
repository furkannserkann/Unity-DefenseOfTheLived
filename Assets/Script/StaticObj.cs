using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObj : MonoBehaviour
{
    private static LevelEnemyList localEnemyList;
    public static LevelEnemyList levelEnemyList
    {
        get
        {
            if (localEnemyList == null)
            {
                localEnemyList = GameObject.FindGameObjectWithTag("LoadSettings").GetComponent<LevelEnemyList>();
            }

            return localEnemyList;
        }
    }

    public static int HitDamage(int _damage)
    {
        if (_damage > 0)
        {
            int yuzdeSeksen = Mathf.RoundToInt(_damage * 0.8f);
            int rant = Random.Range(0, 101);

            if (rant < 40)
            {
                _damage -= _damage - Random.Range(yuzdeSeksen, _damage);
            }
            else if (rant > 45)
            {
                _damage += _damage - Random.Range(yuzdeSeksen, _damage);
            }
        }
        else
            return 0;

        return _damage;
    }
}
