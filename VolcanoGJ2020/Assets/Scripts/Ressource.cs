using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ressource : MonoBehaviour
{
    public ERessourceType type;
    public bool Active { get; set; }

    private void Start()
    {
        Active = true;
    }
}


public enum ERessourceType
{
    waterPuddle,
    waterPond,
    waterLake,
    oxides,
    halogens,
    nitrates,
    coeur
}