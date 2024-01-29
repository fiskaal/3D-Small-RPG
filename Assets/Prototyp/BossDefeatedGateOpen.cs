using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDefeatedGateOpen : MonoBehaviour
{
    public AudioSource audioSourceGate;
    public Animator gateAnim;
    
    public void OpenGateOnDeath()
    {
        gateAnim.SetBool("OpenGate", true);
        audioSourceGate.Play();
    }
}
