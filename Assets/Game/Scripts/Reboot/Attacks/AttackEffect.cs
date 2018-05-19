using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public event Action OnEffectComplete;
    [SerializeField] private ParticleSystem m_AttackParticles;

    public void Play()
    {
        m_AttackParticles.Play();
        Destroy(gameObject, m_AttackParticles.main.duration);
    }
}