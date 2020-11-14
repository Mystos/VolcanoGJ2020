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
        if (GameManager.Instance.cheatActivate)
            return true;

        int waterCost = 0;
        int mineralCost = 0;
        switch (type)
        {
            case TreeType.Cutting:
                waterCost = GameManager.Instance.treeWaterCost;
                mineralCost = GameManager.Instance.treeMineralCost;
                break;
            case TreeType.Shield:
                waterCost = GameManager.Instance.shieldWaterCost;
                mineralCost = GameManager.Instance.shieldMineralCost;
                break;
            case TreeType.Sanitizer:
                waterCost = GameManager.Instance.sanitizerWaterCost;
                mineralCost = GameManager.Instance.sanitizerMineralCost;
                break;
        }

        if (IsSaltGrounded())
        {
            waterCost = Mathf.CeilToInt(waterCost * GameManager.Instance.saltFactor);
            mineralCost = Mathf.CeilToInt(mineralCost * GameManager.Instance.saltFactor);
        }

        int waterFinalAmount = (int)GameManager.Instance.water - waterCost;
        int mineralsFinalAmount = (int)GameManager.Instance.minerals - mineralCost;

        if (waterFinalAmount >= 0 && mineralsFinalAmount >= 0)
        {
            // If true we return the new current ressources
            GameManager.Instance.water = (uint)waterFinalAmount;
            GameManager.Instance.minerals = (uint)mineralsFinalAmount;
            return true;
        }

        // The player didn't had enough money
        return false;

    }

    public bool IsSaltGrounded()
    {
        Collider[] cols = Physics.OverlapSphere(buildPosition, 0.5f, GameManager.Instance.groundLayer);
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].gameObject.tag == GameManager.Instance.saltGroundTag)
                return true;
        }
        return false;
    }
}

