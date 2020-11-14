using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Set up")]
    public LayerMask groundLayer;
    public LayerMask superMineralLayer;
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
    public int waterPuddle = 20;
    public int waterPond = 60;
    public int waterLake = 100;

    [Header("Minerals")]
    public int oxides = 20;
    public int halogens = 60;
    public int nitrates = 100;

    [Header("Trees")]
    public int treeRadiusEarth = 20;
    public int treeRadiusSuperMineral = 40;
    public int treeRadiusSand = 10;

    [Header("Cost")]
    public int rootWaterCost = 0;
    public int rootMineralCost = 0;
    public int treeWaterCost = 5;
    public int treeMineralCost = 5;
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
        switch (ressource.type)
        {
            case ERessourceType.waterPuddle:
                break;
            case ERessourceType.waterPond:
                break;
            case ERessourceType.waterLake:
                break;
            case ERessourceType.oxides:
                break;
            case ERessourceType.halogens:
                break;
            case ERessourceType.nitrates:
                break;
            case ERessourceType.coeur:
                break;
            default:
                break;
        }
    }
}
