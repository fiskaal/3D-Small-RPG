using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUiColliderScript : MonoBehaviour
{
    private BossUIScript _bossUIScript;
    public SphereCollider collider;
    public String bossName;
    public EnemyBoss enemyBoss;
    public Enemy biggestTree;

    private bool ghoulBoss;
    private bool treeElder;
    
    private void Start()
    {
        _bossUIScript = FindObjectOfType<BossUIScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _bossUIScript.BossName.text = bossName;
            _bossUIScript.ShowBar();
            
            if (enemyBoss != null)
            {
                _bossUIScript._enemyHpBar.SetMaxHP(enemyBoss.maxHp);
                _bossUIScript._enemyHpBar.SetHP(enemyBoss.currentHp);

                ghoulBoss = true;
                treeElder = false;
            }

            if (biggestTree != null)
            {
                _bossUIScript._enemyHpBar.SetMaxHP(biggestTree.maxHp);
                _bossUIScript._enemyHpBar.SetHP(biggestTree.currentHp);
                
                treeElder = true;
                ghoulBoss = false;
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _bossUIScript.HideBar();
        }
    }

    private void Update()
    {
        if (enemyBoss != null && ghoulBoss)
        {
            _bossUIScript._enemyHpBar.SetHP(enemyBoss.currentHp);

            if (enemyBoss.currentHp <= 0)
            {
                _bossUIScript.HideBar();
            }
        }
        
        if (biggestTree != null && treeElder)
        {
            _bossUIScript._enemyHpBar.SetHP(biggestTree.currentHp);

            if (biggestTree.currentHp <= 0)
            {
                _bossUIScript.HideBar();
            }
        }
    }
}
