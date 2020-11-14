using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ressource : MonoBehaviour
{
    public ERessourceType type;

    // Start is called before the first frame update
    void Start()
    {

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