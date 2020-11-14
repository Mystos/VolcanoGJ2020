using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Set up")]
    public GameObject prefabTree;

    // Ressources
    [Header("Water")]
    [Space]
    [Header("Ressources")]
    public int waterPuddle = 20;
    public int waterPond = 60;
    public int waterLake= 100;
    [Header("Minerals")]
    [Space]
    public int oxides = 20;
    public int halogens = 60;
    public int nitrates = 100;
    [Header("Trees")]
    [Space]
    public int treeRadiusEarth = 20;
    public int treeRadiusSuperMineral = 40;
    public int treeRadiusSand = 10;
    [Header("Cost")]
    [Space]
    public int rootWaterCost = 0;
    public int rootMineralCost = 0;
    public int treeWaterCost = 5;
    public int treeMineralCost = 5;
    public int sanitizerWaterCost = 7;
    public int sanitizerMineralCost = 3;
    public int shieldWaterCost = 3;
    public int shieldMineralCost = 7;

    // Private Variable
    private Tree motherTree;
    private List<Tree> listTree;

    // Start is called before the first frame update
    void Start()
    {
        motherTree = this.GetComponent<Tree>();
        listTree = new List<Tree>();
        listTree.Add(motherTree);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !PauseMenu.IsGamePaused)
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {

                foreach(Tree tree in listTree)
                {
                    float distanceToClick = (hit.point - tree.gameObject.transform.position).magnitude;
                    if (distanceToClick < tree.effectRadius)
                    {
                        GameObject go = GameObject.Instantiate(prefabTree, hit.point, Quaternion.identity, null);
                        listTree.Add(go.GetComponent<Tree>());
                        break;
                    }
                }


            }
        }
    }
}
