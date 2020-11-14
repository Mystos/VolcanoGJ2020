using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeVegetalManager : MonoBehaviour
{
    private Tree tree;

    [Header("Radius renderer")]
    public float radiusFactor = 1.15f;
    public GameObject vegetalRendererPrefab;
    public GameObject vegetalRenderer;

    public int maxPlant = 10;
    public int nbrPlant = 0;
    public float reloadTime = 2f;
    public float reloadProgress = 0f;

    // Start is called before the first frame update
    void Start()
    {
        tree = GetComponent<Tree>();
        vegetalRenderer = Instantiate(vegetalRendererPrefab, tree.transform);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVegetalZoneEffect(transform.position, radiusFactor * GetVegetalRadius(tree.Radius) * new Vector3(1, 1, 1));
        reloadProgress += Time.deltaTime;
        if (reloadProgress >= reloadTime)
        {
            if(nbrPlant <= maxPlant)
            {
                GameObject randomAsset = GameManager.Instance.vegetalAssets[Mathf.FloorToInt(Random.Range(0, (float)GameManager.Instance.vegetalAssets.Count))];
                GameObject go = Instantiate(randomAsset, tree.transform);
                go.transform.position = RandomPosInCircle(GetVegetalRadius(tree.Radius));

                float randomScale = Random.Range(GameManager.Instance.randomScaleMinFactor, GameManager.Instance.randomScaleMaxFactor);
                go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                nbrPlant++;
            }
            reloadProgress = 0;
        }
    }

    private float GetVegetalRadius(float radius)
    {
        return radius * (GameManager.Instance.growingPower / 100);
    }
    private void UpdateVegetalZoneEffect(Vector3 position, Vector3 size)
    {
        vegetalRenderer.transform.position = position + Vector3.up * 0.2f;
        vegetalRenderer.transform.localScale = size;
    }

    public Vector3 RandomPosInCircle(float radius)
    {
        Debug.Log(radius);
        float randomR = radius * Mathf.Sqrt(Random.Range(0f, 1f));
        float randomTheta = Random.Range(0f, 1f) * 2 * Mathf.PI;

        float x = transform.position.x + randomR * Mathf.Cos(randomTheta);
        float z = transform.position.z + randomR * Mathf.Sin(randomTheta);
        return new Vector3(x, transform.position.y, z);
    }

}
