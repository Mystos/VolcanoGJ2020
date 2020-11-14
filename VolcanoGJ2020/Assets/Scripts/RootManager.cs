using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RootManager : MonoBehaviour
{
    public BuildManager buildManager;

    [Header("Radius renderer")]
    public float radiusFactor = 6;
    public GameObject radiusRenderer;

    [Header("Prefabs")]
    public GameObject rootPrefab;
    public GameObject rootHandlePrefab;

    //Root building
    private Camera camera;
    private bool isPlacing = false;
    private bool placingFromTree = false;
    private Transform selectedSource;
    private RootHandle lastHandle;
    private Vector3 rootOffset = new Vector3(0, 0.2f, 0);

    // Start is called before the first frame update
    void Start()
    {
        ClearSelection();
        buildManager.onTreePlaced += TreePlaced;
        camera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //Clear selection if right click
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ClearSelection();
        }

        //If left click
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Cancel click if mouse is hovering build panel
            if (buildManager.gameObject.activeSelf && buildManager.CheckHovering(Input.mousePosition))
                return;

            //Hit init
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            //Camera ray casting
            if (Physics.Raycast(ray, out hit))
            {
                //Disable the building panel
                buildManager.Hide();

                //If tree was hit
                if (hit.transform.gameObject.tag == GameManager.Instance.treeTag)
                {
                    //Get tree
                    Tree tree = hit.transform.gameObject.GetComponentInParent<Tree>();
                    if (tree != null)
                    {
                        //Select tree
                        selectedSource = tree.transform;
                        UpdateSelectionEffect(selectedSource.position, radiusFactor * tree.Radius * new Vector3(1, 0, 1));

                        //If we are not trying to place root OR we are placing but from rootHandle
                        if (!isPlacing || (isPlacing == true && placingFromTree == false))
                        {
                            //Then we are placing from a tree
                            isPlacing = true;
                            placingFromTree = true;
                        }
                    }
                    else
                        Debug.LogError("Hit object has no Tree component");
                }

                //If root handle was hit
                else if (hit.transform.gameObject.tag == GameManager.Instance.rootHandleTag)
                {
                    //Get rootHandle
                    RootHandle rootHandle = hit.transform.gameObject.GetComponentInParent<RootHandle>();
                    if (rootHandle != null)
                    {
                        //Update selected source and lastHandle
                        selectedSource = rootHandle.transform;
                        lastHandle = rootHandle;

                        //Get connected tree
                        Tree tree = lastHandle.sourceRoot.connectedTree;

                        //Update selection sprite
                        UpdateSelectionEffect(tree.transform.position, radiusFactor * tree.Radius * new Vector3(1, 0, 1));
                        //Enable build manager
                        buildManager.Show(selectedSource.position, Input.mousePosition);

                        //If we are not trying to place root then we are placing from a rootHandle
                        if (!isPlacing)
                        {
                            isPlacing = true;
                            placingFromTree = false;
                        }
                    }
                    else
                        Debug.LogError("Hit object has no RootHandle component");
                }

                //If ground was hit
                else if (hit.transform.gameObject.tag == GameManager.Instance.groundTag ||
                    hit.transform.gameObject.tag == GameManager.Instance.sandGroundTag ||
                    hit.transform.gameObject.tag == GameManager.Instance.saltGroundTag ||
                    hit.transform.gameObject.tag == GameManager.Instance.ressourceTag)

                {
                    //If we are trying to place root
                    if (isPlacing)
                    {
                        bool pointInRange = false;
                        //And if placing from tree
                        if (placingFromTree)
                        {
                            //Get tree
                            Tree tree = selectedSource.GetComponent<Tree>();
                            //Check if hit point is in tree range
                            if (tree.InRange(hit.point))
                            {
                                //Place root and update connected tree
                                Root root = Instantiate(rootPrefab, Vector3.zero, Quaternion.identity).GetComponent<Root>();
                                root.TraceRoot(selectedSource.position + rootOffset, hit.point + rootOffset);
                                root.connectedTree = tree;

                                if (hit.transform.gameObject.tag == GameManager.Instance.ressourceTag)
                                {
                                    Ressource ressource = hit.transform.GetComponent<Ressource>();
                                    if (ressource != null)
                                        GameManager.Instance.CollectRessource(ressource);
                                }
                                else
                                {
                                    CreateRootHandle(hit.point, root, false);
                                }
                                pointInRange = true;


                            }
                        }
                        //If placing from root
                        else
                        {
                            //Get root connected tree
                            Tree tree = lastHandle.sourceRoot.connectedTree;
                            //If hit point in tree range
                            if (tree.InRange(hit.point))
                            {
                                //Then plongate root and create handle
                                lastHandle.sourceRoot.ProlongateRoot(hit.point + rootOffset);
                                CreateRootHandle(hit.point, lastHandle.sourceRoot, true);
                                pointInRange = true;
                            }
                        }

                        //If point in range
                        if (pointInRange)
                        {
                            //Then we are placing from rootHandle
                            placingFromTree = false;
                            isPlacing = true;
                            //Enable build panel
                            buildManager.Show(selectedSource.position, Input.mousePosition);
                        }
                    }
                }
            }
        }
    }

    private void CreateRootHandle(Vector3 point, Root sourceRoot, bool destroyPreviousHandle)
    {
        RootHandle handle = Instantiate(rootHandlePrefab, point, Quaternion.identity).GetComponent<RootHandle>();
        handle.sourceRoot = sourceRoot;

        //Destroy previous handle
        if (destroyPreviousHandle)
            Destroy(lastHandle.gameObject);

        //Update selected source and last handle
        selectedSource = handle.transform;
        lastHandle = handle;
    }

    private void ClearSelection()
    {
        radiusRenderer.SetActive(false);
        buildManager.Hide();
        isPlacing = false;
        placingFromTree = false;
        lastHandle = null;
    }

    private void TreePlaced()
    {
        Destroy(lastHandle.gameObject);
        ClearSelection();
    }

    private void UpdateSelectionEffect(Vector3 position, Vector3 size)
    {
        radiusRenderer.SetActive(true);
        radiusRenderer.transform.position = position + Vector3.up * 0.2f;
        radiusRenderer.transform.localScale = size;
    }
}
