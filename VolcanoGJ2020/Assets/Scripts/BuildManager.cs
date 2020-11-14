using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public GameObject cutingPrefab;
    public GameObject shieldPrefab;
    public GameObject sanitizerPrefab;

    public RectTransform panelTransform;
    private Vector3 buildPosition;
    

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
}
