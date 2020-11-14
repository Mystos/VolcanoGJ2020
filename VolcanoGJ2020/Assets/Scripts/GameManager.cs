﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Set up")]
    public LayerMask groundLayer;
    public LayerMask treeLayer;
    public LayerMask superMineralLayer;
    public LayerMask fireBallLayer;
    public string groundTag = "ground";
    public string treeTag = "tree";
    public string rootHandleTag = "rootHandle";
    public string saltGroundTag = "saltGround";
    public string superMineralTag = "superMineral";
    public string sandGroundTag = "sandGround";
    public string ressourceTag = "ressource";
    public float superMineralCheckRadius = 5f;

    // Current Ressources
    [Space]
    [Header("Current Ressources")]
    public uint water = 0;
    public uint minerals = 0;
    public bool cheatActivate = false;
    // Ressources stats

    [Header("Water")]
    [Space]
    [Header("Ressources stats")]
    public uint waterPuddle = 20;
    public uint waterPond = 60;
    public uint waterLake = 100;

    [Header("Minerals")]
    public uint oxides = 20;
    public uint halogens = 60;
    public uint nitrates = 100;

    [Header("Trees")]
    public int treeRadiusEarth = 20;
    public int treeRadiusSuperMineral = 40;
    public int treeRadiusSand = 10;
    public int sanitizeRadius = 15;
    public int shieldRadius = 15;

    [Header("Cost")]
    public int rootWaterCost = 0;
    public int rootMineralCost = 0;
    public int cuttingWaterCost = 5;
    public int cuttingMineralCost = 5;
    public int sanitizerWaterCost = 7;
    public int sanitizerMineralCost = 3;
    public int shieldWaterCost = 3;
    public int shieldMineralCost = 7;
    public float saltFactor = 2f;


    // Private Variable
    internal Tree motherTree;
    private List<Tree> listTree;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        motherTree = this.GetComponent<Tree>();
        listTree = new List<Tree>();
        listTree.Add(motherTree);
    }


    public void AddTree(Tree tree)
    {
        if (!listTree.Contains(tree))
        {
            listTree.Add(tree);
        }
    }

    public void CollectRessource(Ressource ressource)
    {
        if (!ressource.Active)
            return;

        switch (ressource.type)
        {
            case ERessourceType.waterPuddle:
                water += waterPuddle;
                break;
            case ERessourceType.waterPond:
                water += waterPond;
                break;
            case ERessourceType.waterLake:
                water += waterLake;
                break;
            case ERessourceType.oxides:
                minerals += oxides;
                break;
            case ERessourceType.halogens:
                minerals += halogens;
                break;
            case ERessourceType.nitrates:
                minerals += nitrates;
                break;
            case ERessourceType.coeur:
                break;
            default:
                break;
        }
        ressource.Active = false;
    }
}
