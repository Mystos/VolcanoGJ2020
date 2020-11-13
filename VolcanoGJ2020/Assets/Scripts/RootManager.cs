﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootManager : MonoBehaviour
{
    public LayerMask treeLayer;
    public LayerMask groundLayer;
    public GameObject rootPrefab;
    public GameObject rootHandlePrefab;
    public Camera camera;

    //Root building
    bool isPlacing = false;
    bool placingFromTree = false;
    Transform source;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit treeHit;
            RaycastHit groundHit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out treeHit, 100000, treeLayer))
            {
                if (isPlacing)
                {
                    if ((placingFromTree && treeHit.transform == source) == false)
                    {
                        PlaceRoot(treeHit.point, source.position);
                        isPlacing = false;
                    }
                }
                else
                {
                    isPlacing = true;
                    Tree tree = treeHit.transform.gameObject.GetComponentInParent<Tree>();
                    source = tree.transform;
                    placingFromTree = true;
                }

            }
            else if (Physics.Raycast(ray, out groundHit, 100000, groundLayer))
            {
                //TODO : check if first hit object is ground ?
                //GameObject objectHit = treeHit.transform.gameObject;

                if (isPlacing)
                {
                    if (placingFromTree)
                    {
                        PlaceRoot(source.position, groundHit.point);
                    }
                    else
                    {
                        //sourceRoot.ProlongateRoot(groundHit.point);
                    }
                    isPlacing = false;
                }
            }
        }
    }

    private void PlaceRoot(Vector3 start, Vector3 end)
    {
        GameObject rootGo = Instantiate(rootPrefab, start, Quaternion.identity);
        Root root = rootGo.GetComponent<Root>();
        root.TraceRoot(start, end);
        Instantiate(rootHandlePrefab, end, Quaternion.identity);
    }
}
