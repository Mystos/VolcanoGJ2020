using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RootManager : MonoBehaviour
{
    [Header("Radius renderer")]
    public float radiusFactor = 6;
    public GameObject radiusRenderer;

    [Header("Prefabs")]
    public GameObject rootPrefab;
    public GameObject rootHandlePrefab;

    //Root building
    private BuildManager buildManager;
    private Camera camera;
    private bool isPlacing = false;
    private bool placingFromTree = false;
    private Transform selectedSource;
    private RootHandle lastHandle;
    private Vector3 rootOffset = new Vector3(0, 0.2f, 0);
    private float maxHeigt = 1;


    // Start is called before the first frame update
    void Start()
    {
        buildManager = FindObjectOfType<BuildManager>();
        ClearSelection();
        buildManager.onTreePlaced += TreePlaced;
        camera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //Clear selection if right click
        if (Input.GetKeyDown(KeyCode.Mouse1) && !PauseMenu.IsGamePaused)
        {
            ClearSelection();
        }

        //If left click
        if (Input.GetKeyDown(KeyCode.Mouse0) && !PauseMenu.IsGamePaused)
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
                    hit.transform.gameObject.tag == GameManager.Instance.ressourceTag ||
                    hit.transform.gameObject.tag == GameManager.Instance.rampTag)

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
                            bool placementAuthorized = IsOnSameLevel(tree.transform.position, hit.point) ||
                                hit.transform.gameObject.tag == GameManager.Instance.ressourceTag ||
                                hit.transform.gameObject.tag == GameManager.Instance.rampTag;

                            //Check if hit point is in tree range
                            if (tree != null && tree.InRange(hit.point) && placementAuthorized)
                            {
                                //Place root and update connected tree
                                Root root = Instantiate(rootPrefab, Vector3.zero, Quaternion.identity).GetComponent<Root>();
                                root.connectedTree = tree;
                                pointInRange = true;

                                if (hit.transform.gameObject.tag == GameManager.Instance.ressourceTag)
                                {
                                    root.TraceRoot(selectedSource.position + rootOffset, hit.transform.position);
                                    Ressource ressource = hit.transform.GetComponent<Ressource>();
                                    if (ressource != null)
                                        GameManager.Instance.CollectRessource(ressource);
                                    ClearSelection();
                                    return;
                                }
                                else if (hit.transform.gameObject.tag == GameManager.Instance.rampTag)
                                {
                                    Ramp ramp = hit.transform.GetComponent<Ramp>();
                                    if (ramp != null)
                                    {
                                        Vector3 firstPos = ramp.GetPrimaryPoint(selectedSource.position);
                                        Vector3 secondPos = ramp.GetSecondaryPoint(selectedSource.position);
                                        root.TraceRoot(selectedSource.position + rootOffset, firstPos + rootOffset);
                                        root.ProlongateRoot(secondPos + rootOffset);
                                        CreateRootHandle(secondPos, root, false);
                                    }
                                }
                                else
                                {
                                    root.TraceRoot(selectedSource.position + rootOffset, hit.point + rootOffset);
                                    CreateRootHandle(hit.point, root, false);
                                }
                            }
                        }
                        //If placing from root
                        else if (lastHandle != null)
                        {
                            //Get root connected tree
                            Tree tree = lastHandle.sourceRoot.connectedTree;
                            bool placementAuthorized = IsOnSameLevel(lastHandle.transform.position, hit.point) ||
                                hit.transform.gameObject.tag == GameManager.Instance.ressourceTag ||
                                hit.transform.gameObject.tag == GameManager.Instance.rampTag;

                            //If hit point in tree range
                            if (tree.InRange(hit.point) && placementAuthorized)
                            {
                                pointInRange = true;
                                if (hit.transform.gameObject.tag == GameManager.Instance.ressourceTag)
                                {
                                    lastHandle.sourceRoot.ProlongateRoot(hit.transform.position);
                                    Ressource ressource = hit.transform.GetComponent<Ressource>();
                                    if (ressource != null)
                                        GameManager.Instance.CollectRessource(ressource);
                                    Destroy(lastHandle.gameObject);
                                    ClearSelection();
                                    return;
                                }
                                else if (hit.transform.gameObject.tag == GameManager.Instance.rampTag)
                                {
                                    Ramp ramp = hit.transform.GetComponent<Ramp>();
                                    if (ramp != null)
                                    {
                                        Vector3 firstPos = ramp.GetPrimaryPoint(lastHandle.transform.position);
                                        Vector3 secondPos = ramp.GetSecondaryPoint(lastHandle.transform.position);
                                        lastHandle.sourceRoot.ProlongateRoot(firstPos + rootOffset);
                                        lastHandle.sourceRoot.ProlongateRoot(secondPos + rootOffset);
                                        CreateRootHandle(secondPos, lastHandle.sourceRoot, true);
                                    }
                                }
                                else
                                {
                                    //Then prolongate root and create handle
                                    lastHandle.sourceRoot.ProlongateRoot(hit.point + rootOffset);
                                    CreateRootHandle(hit.point, lastHandle.sourceRoot, true);
                                }
                            }

                            AudioManager.instance.Play("GrowRoots");

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

    private bool IsOnSameLevel(Vector3 previousPoint, Vector3 newPoint)
    {
        if (Mathf.Abs(previousPoint.y - newPoint.y) > maxHeigt)
            return false;
        return true;
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
        AudioManager.instance.Play("GrowTrees");

    }

    private void UpdateSelectionEffect(Vector3 position, Vector3 size)
    {
        radiusRenderer.SetActive(true);
        radiusRenderer.transform.position = position + Vector3.up * 0.2f;
        radiusRenderer.transform.localScale = size;
    }
}
