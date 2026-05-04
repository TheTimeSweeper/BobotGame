using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorIndicator : MonoBehaviour
{
    [SerializeField]
    private Color color;
    public Color Color { 
        get =>  color; 
        set
        {
            if (color == value) 
                return;
            color = value;
            UpdateParticleColors();
        } 
    }

    [SerializeField]
    private ParticleSystem[] particleSystems;

    [SerializeField]
    private LineRenderer stringRenderer;

    public Transform Caster { get; set; }

    private ParticleSystem.MainModule[] mainModules;

    private void Awake()
    {
        mainModules = new ParticleSystem.MainModule[particleSystems.Length];
        for (int i = 0; i < particleSystems.Length; i++)
        {
            mainModules[i] = particleSystems[i].main;
        }
        UpdateParticleColors();
    }

    private void UpdateParticleColors()
    {
        for (int i = 0; i< mainModules.Length; i++)
        {
            mainModules[i].startColor = color;
        }
        if (stringRenderer != null)
        {
            stringRenderer.startColor = color;
            stringRenderer.endColor = color;
        }
    }

    private void Update()
    {
        if (stringRenderer != null && Caster != null)
        {
            stringRenderer.SetPosition(0, Caster.transform.position);
            stringRenderer.SetPosition(1, transform.position);
        }
    }
}
