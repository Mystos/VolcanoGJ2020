using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Tree;

public class BuildManager : MonoBehaviour
{
    public GameObject cuttingPrefab;
    public GameObject shieldPrefab;
    public GameObject sanitizerPrefab;

    public RectTransform panelTransform;
    private Vector3 buildPosition;

    public delegate void OnPlaceTree();
    public event OnPlaceTree onTreePlaced;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show(Vector3 buildPos, Vector2 panelPos)
    {
        gameObject.SetActive(true);
        buildPosition = buildPos;
        panelTransform.position = panelPos;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void BuildTree(int type)
    {
        GameObject treePrefab = null;
        TreeType eType = (TreeType)type;
        switch (eType)
        {
            case TreeType.Cutting:
                treePrefab = cuttingPrefab;
                break;
            case TreeType.Shield:
                treePrefab = shieldPrefab;
                break;
            case TreeType.Sanitizer:
                treePrefab = sanitizerPrefab;
                break;
        }
        if (treePrefab != null)
        {
            if (CalculateCost(eType))
            {
                Tree tree = Instantiate(treePrefab, buildPosition, Quaternion.identity).GetComponent<Tree>();
            }
        }
        if (onTreePlaced != null)
            onTreePlaced.Invoke();
    }

    public bool CheckHovering(Vector2 mousePosition)
    {
        float radius = panelTransform.sizeDelta.x / 2;
        if (panelTransform.position.x - radius < mousePosition.x && mousePosition.x < panelTransform.position.x + radius
            && panelTransform.position.y - radius < mousePosition.y && mousePosition.y < panelTransform.position.y + radius)
            return true;
        else
            return false;
    }

    private bool CalculateCost(TreeType type)
    {
        int waterCost = 0;
        int mineralCost = 0;
        switch (type)
        {
            case TreeType.Cutting:
                waterCost = GameManager.instance.treeWaterCost;
                mineralCost = GameManager.instance.treeMineralCost;
                break;
            case TreeType.Shield:
                waterCost = GameManager.instance.shieldWaterCost;
                mineralCost = GameManager.instance.shieldMineralCost;
                break;
            case TreeType.Sanitizer:
                waterCost = GameManager.instance.sanitizerWaterCost;
                mineralCost = GameManager.instance.sanitizerMineralCost;
                break;
        }

        int waterFinalCost = (int)GameManager.instance.water - waterCost;
        int mineralsFinalCost = (int)GameManager.instance.minerals - mineralCost;

        if(waterFinalCost >= 0 && mineralsFinalCost >= 0)
        {
            // If true we return the new current ressources
            GameManager.instance.water = (uint)waterFinalCost;
            GameManager.instance.minerals = (uint)mineralsFinalCost;
            return true;
        }

        // The player didn't had enough money
        return false;

    }

}

