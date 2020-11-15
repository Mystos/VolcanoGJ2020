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
    public GameObject bushPrefab;

    public RectTransform panelTransform;
    public Text waterPriceCuttingTxt;
    public Text waterPriceShieldTxt;
    public Text waterPriceSanitizerTxt;
    public Text mineralsPriceCuttingTxt;
    public Text mineralsPriceShieldTxt;
    public Text mineralsPriceSanitizerTxt;

    public delegate void OnPlaceTree();
    public event OnPlaceTree onTreePlaced;

    private bool onSand = false;
    private Vector3 buildPosition;
    private int waterPriceCutting;
    private int waterPriceShield;
    private int waterPriceSanitizer;
    private int mineralsPriceCutting;
    private int mineralsPriceShield;
    private int mineralsPriceSanitizer;

    public void Show(Vector3 buildPos, Vector2 panelPos)
    {
        gameObject.SetActive(true);
        buildPosition = buildPos;
        panelTransform.position = panelPos;
        CalculateCosts();
        UpdatePriceText();
    }

    private void UpdatePriceText()
    {
        mineralsPriceCuttingTxt.text = mineralsPriceCutting.ToString();
        mineralsPriceShieldTxt.text = mineralsPriceShield.ToString();
        mineralsPriceSanitizerTxt.text = mineralsPriceSanitizer.ToString();

        waterPriceCuttingTxt.text = waterPriceCutting.ToString();
        waterPriceShieldTxt.text = waterPriceShield.ToString();
        waterPriceSanitizerTxt.text = waterPriceSanitizer.ToString();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void BuildTree(int type)
    {
        GameObject treePrefab = null;
        TreeType eType = (TreeType)type;

        int waterCost = 0;
        int mineralCost = 0;

        switch (eType)
        {
            case TreeType.Cutting:
                treePrefab = onSand ? bushPrefab : cuttingPrefab;
                waterCost = waterPriceCutting;
                mineralCost = mineralsPriceCutting;
                break;
            case TreeType.Shield:
                treePrefab = shieldPrefab;
                waterCost = waterPriceShield;
                mineralCost = mineralsPriceShield;
                break;
            case TreeType.Sanitizer:
                treePrefab = sanitizerPrefab;
                waterCost = waterPriceSanitizer;
                mineralCost = mineralsPriceSanitizer;
                break;
        }
        if (treePrefab != null)
        {
            int waterFinalAmount = (int)GameManager.Instance.water - waterCost;
            int mineralsFinalAmount = (int)GameManager.Instance.minerals - mineralCost;

            if (waterFinalAmount >= 0 && mineralsFinalAmount >= 0 || GameManager.Instance.cheatActivate)
            {
                GameManager.Instance.water = (uint)waterFinalAmount;
                GameManager.Instance.minerals = (uint)mineralsFinalAmount;

                Tree tree = Instantiate(treePrefab, buildPosition, Quaternion.identity).GetComponent<Tree>();
                if (onTreePlaced != null)
                    onTreePlaced.Invoke();
            }
        }
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

    private void CalculateCosts()
    {
        float k = 1;
        if (IsSaltGrounded() && !IsSanitized()) k = GameManager.Instance.saltFactor;
        waterPriceCutting = Mathf.CeilToInt(GameManager.Instance.cuttingWaterCost * k);
        waterPriceShield = Mathf.CeilToInt(GameManager.Instance.shieldWaterCost * k);
        waterPriceSanitizer = Mathf.CeilToInt(GameManager.Instance.sanitizerWaterCost * k);

        mineralsPriceCutting = Mathf.CeilToInt(GameManager.Instance.cuttingMineralCost * k);
        mineralsPriceShield = Mathf.CeilToInt(GameManager.Instance.shieldMineralCost * k);
        mineralsPriceSanitizer = Mathf.CeilToInt(GameManager.Instance.sanitizerMineralCost * k);
    }


    public bool IsSaltGrounded()
    {
        onSand = false;
        Collider[] cols = Physics.OverlapSphere(buildPosition, 0.5f, GameManager.Instance.groundLayer);
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].gameObject.tag == GameManager.Instance.saltGroundTag)
                return true;
            if (cols[i].gameObject.tag == GameManager.Instance.sandGroundTag)
                onSand = true;
        }
        return false;
    }

    public bool IsSanitized()
    {
        Collider[] cols = Physics.OverlapSphere(buildPosition, GameManager.Instance.sanitizeRadius, GameManager.Instance.treeLayer);
        for (int i = 0; i < cols.Length; i++)
        {
            Tree tree = cols[i].gameObject.GetComponentInParent<Tree>();
            if (tree != null && tree.type == TreeType.Sanitizer)
                return true;
        }
        return false;
    }
}

