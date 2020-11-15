﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeVegetalManager : MonoBehaviour
{
    private Tree tree;

    [Header("Radius renderer")]
    public float radiusFactor = 1.15f;
    public GameObject vegetalRendererPrefab;
    public GameObject vegetalRenderer;
    public int nbrPlant = 0;
    public float reloadTime = 2f;
    public float reloadProgress = 0f;

    // Start is called before the first frame update
    void Start()
    {
        tree = GetComponent<Tree>();
        //vegetalRenderer = Instantiate(vegetalRendererPrefab, tree.transform);
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateVegetalZoneEffect(transform.position, radiusFactor * GetVegetalRadius(tree.Radius) * new Vector3(1, 1, 1));
        reloadProgress += Time.deltaTime;
        if (reloadProgress >= reloadTime)
        {
            if(nbrPlant < GameManager.Instance.maxPlant)
            {
                GameObject randomAsset = GameManager.Instance.vegetalAssets[Mathf.FloorToInt(Random.Range(0, (float)GameManager.Instance.vegetalAssets.Count))];
                GameObject go = Instantiate(randomAsset, tree.transform);

                go.transform.position = RandomPosInCircle(GetVegetalRadius(tree.Radius)) + new Vector3(0,0.2f,0);
                Quaternion rot = go.transform.rotation;
                rot.eulerAngles = new Vector3(rot.eulerAngles.x, Random.Range(-180, 180), rot.eulerAngles.z);
                go.transform.rotation = rot;

                float randomScale = Random.Range(GameManager.Instance.randomScaleMinFactor, GameManager.Instance.randomScaleMaxFactor);
                go.transform.localScale = new Vector3(randomScale/100, randomScale / 100, randomScale / 100);

                if(Physics.Raycast(new Ray(go.transform.position, Vector3.down), out RaycastHit hit, 10, GameManager.Instance.groundLayer))
                {
                    if (hit.transform.gameObject.tag == GameManager.Instance.groundTag ||
                    hit.transform.gameObject.tag == GameManager.Instance.sandGroundTag ||
                    hit.transform.gameObject.tag == GameManager.Instance.saltGroundTag)
                    {
                        nbrPlant++;
                    }
                }
                else
                {
                    Destroy(go);
                }

            }
            reloadProgress = 0;
            reloadTime = Random.Range(0.5f,1.5f);

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