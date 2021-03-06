﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Set up")]
    public LayerMask groundLayer;
    public LayerMask treeLayer;
    public LayerMask superMineralLayer;
    public LayerMask fireBallLayer;
    public uint levelMaxNbrHeart = 5;

    [HideInInspector] public string groundTag = "ground";
    [HideInInspector] public string treeTag = "tree";
    [HideInInspector] public string rootHandleTag = "rootHandle";
    [HideInInspector] public string saltGroundTag = "saltGround";
    [HideInInspector] public string superMineralTag = "superMineral";
    [HideInInspector] public string sandGroundTag = "sandGround";
    [HideInInspector] public string ressourceTag = "ressource";
    [HideInInspector] public string rampTag = "ramp";
    [HideInInspector] public string vegetalTag = "vegetal";


    // Current Ressources
    [Space]
    [Header("Current Ressources")]
    public uint water = 0;
    public uint minerals = 0;
    public bool cheatActivate = false;
    public uint treeLevel = 0;
    public float growingPower = 0;

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
    public float superMineralCheckRadius = 5f;

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

    [Space]
    [Header("GrowingPowerLevel")]
    public uint powerLvl1 = 20;
    public uint powerLvl2 = 40;
    public uint powerLvl3 = 80;
    public uint powerLvl4 = 40;
    public uint powerLvl5 = 80;
    public uint powerLvl6 = 80;

    public uint maxNbrPlantLvl1 = 20;
    public uint maxNbrPlantLvl2 = 50;
    public uint maxNbrPlantLvl3 = 150;
    public uint maxNbrPlantLvl4 = 200;
    public uint maxNbrPlantLvl5 = 300;
    public uint maxNbrPlantLvl6 = 300;

    public GameObject prefabMotherTreeLvl1;
    public GameObject prefabMotherTreeLvl2;
    public GameObject prefabMotherTreeLvl3;
    public GameObject prefabMotherTreeLvl4;
    public GameObject prefabMotherTreeLvl5;
    public GameObject prefabMotherTreeLvl6;

    [Space]
    [Header("Vegetal Generation")]
    public uint maxPlant;
    public List<GameObject> vegetalAssets;
    [Range(0.1f, 1f)]
    public float randomScaleMinFactor;
    [Range(1f, 10f)]
    public float randomScaleMaxFactor;
    [Range(0f, 1f)]
    public float minSpawnRadius;
    [HideInInspector]
    public GameObject CurrentMotherTreeLevelModel;

    [Space]
    [Header("Volcano")]
    public float minForce = 5;
    public float maxForce = 100;
    public int minLavaCount = 5;
    public int maxLavaCount = 20;
    public float launchRadius = 5;

    public int Score { get; set; }

    // Private Variable
    private Volcano[] volcanos;
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
            Destroy(Instance);
            Instance = gameObject.GetComponent<GameManager>();
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        motherTree = this.GetComponent<Tree>();
        listTree = new List<Tree>();
        listTree.Add(motherTree);
        volcanos = FindObjectsOfType<Volcano>();
        PauseMenu.IsGameWin = false;
        LevelUp();
    }

    private void Update()
    {
        PauseMenu.IsGameWin = CheckWin();

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    LevelUp();
        //}
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
                AudioManager.instance.Play("DrawWater");
                break;
            case ERessourceType.waterPond:
                water += waterPond;
                AudioManager.instance.Play("DrawWater");
                break;
            case ERessourceType.waterLake:
                water += waterLake;
                AudioManager.instance.Play("DrawWater");
                break;
            case ERessourceType.oxides:
                minerals += oxides;
                AudioManager.instance.Play("DrawMinerals");
                break;
            case ERessourceType.halogens:
                minerals += halogens;
                AudioManager.instance.Play("DrawMinerals");
                break;
            case ERessourceType.nitrates:
                minerals += nitrates;
                AudioManager.instance.Play("DrawMinerals");
                break;
            case ERessourceType.coeur:
                LevelUp();
                foreach (Volcano volcano in volcanos)
                {
                    volcano.StartEruption();
                }
                break;
            default:
                break;
        }
        ressource.Active = false;
    }

    public void LevelUp()
    {
        treeLevel++;
        SetLevelStats(treeLevel);
    }

    public void SetLevelStats(uint level)
    {
        if (CurrentMotherTreeLevelModel != null)
        {
            CurrentMotherTreeLevelModel.SetActive(false);
        }

        switch (treeLevel)
        {
            case 1:
                maxPlant = maxNbrPlantLvl1;
                growingPower = maxNbrPlantLvl1;
                CurrentMotherTreeLevelModel = prefabMotherTreeLvl1;
                break;
            case 2:
                maxPlant = maxNbrPlantLvl2;
                growingPower = maxNbrPlantLvl2;
                CurrentMotherTreeLevelModel = prefabMotherTreeLvl2;
                break;
            case 3:
                maxPlant = maxNbrPlantLvl3;
                growingPower = maxNbrPlantLvl3;
                CurrentMotherTreeLevelModel = prefabMotherTreeLvl3;
                break;
            case 4:
                maxPlant = maxNbrPlantLvl4;
                growingPower = maxNbrPlantLvl4;
                CurrentMotherTreeLevelModel = prefabMotherTreeLvl4;
                break;
            case 5:
                maxPlant = maxNbrPlantLvl5;
                growingPower = maxNbrPlantLvl5;
                CurrentMotherTreeLevelModel = prefabMotherTreeLvl5;
                break;
            case 6:
                maxPlant = maxNbrPlantLvl6;
                growingPower = maxNbrPlantLvl6;
                CurrentMotherTreeLevelModel = prefabMotherTreeLvl6;
                break;
        }

        CurrentMotherTreeLevelModel.SetActive(true);
        AudioManager.instance.Play("LevelUp");

    }


    public bool CheckWin()
    {
        if (treeLevel >= levelMaxNbrHeart)
        {
            return true;
        }
        return false;
    }
}
