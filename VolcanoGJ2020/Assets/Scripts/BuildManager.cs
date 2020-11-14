using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        switch (type)
        {
            case 0:
                treePrefab = cuttingPrefab;
                break;
            case 1:
                treePrefab = shieldPrefab;
                break;
            case 2:
                treePrefab = sanitizerPrefab;
                break;
            default:
                break;
        }
        if (treePrefab != null)
        {
            Tree tree = Instantiate(treePrefab, buildPosition, Quaternion.identity).GetComponent<Tree>();
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

}

