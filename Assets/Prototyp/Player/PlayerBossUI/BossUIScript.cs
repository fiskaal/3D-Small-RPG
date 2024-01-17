using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossUIScript : MonoBehaviour
{
    [SerializeField]public TextMeshProUGUI BossName;
    
    [SerializeField]private Animation animation;
    [SerializeField]private AnimationClip showAnim;
    [SerializeField]private AnimationClip hideAnim;
    [SerializeField] public EnemyHpBar _enemyHpBar;
    
    private bool isShow = false;
    private bool isHidden = true;

    private void Start()
    {
    }

    public void ShowBar()
    {
        if (!isShow)
        {
            animation.clip = showAnim;
            animation.Play();
            
            isShow = true;
            isHidden = false;
        }
    }

    public void HideBar()
    {
        if (!isHidden)
        {
            animation.clip = hideAnim;
            animation.Play();

            isHidden = true;
            isShow = false;
        }
    }
}
