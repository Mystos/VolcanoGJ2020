using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RootManager : MonoBehaviour
{
    public Camera camera;
    public BuildManager buildManager;

    [Header("Radius renderer")]
    public float radiusFactor = 6;
    public GameObject radiusRenderer;

    [Header("Prefabs")]
    public GameObject rootPrefab;
    public GameObject rootHandlePrefab;

    //Root building
    bool isPlacing = false;
    bool placingFromTree = false;
    Transform selectedSource;
    RootHandle lastHandle;

    // Start is called before the first frame update
    void Start()
    {
        ClearSelection();
        buildManager.onTreePlaced += TreePlaced;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            radiusRenderer.SetActive(false);
            buildManager.Hide();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (buildManager.gameObject.activeSelf && buildManager.CheckHovering(Input.mousePosition))
                return;

            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                buildManager.Hide();

                if (hit.transform.gameObject.tag == GameManager.Instance.treeTag)
                {

                    Tree tree = hit.transform.gameObject.GetComponentInParent<Tree>();
                    if (tree != null)
                    {
                        selectedSource = tree.transform;
                        UpdateSelectionEffect(selectedSource.position, radiusFactor * tree.Radius * new Vector3(1, 0, 1));
                        if (!isPlacing || (isPlacing == true && placingFromTree == false))
                        {
                            isPlacing = true;
                            placingFromTree = true;
                        }
                    }
                    else
                        Debug.LogError("Hit object has no Tree component");
                }
                else if (hit.transform.gameObject.tag == GameManager.Instance.rootHandleTag)
                {
                    RootHandle rootHandle = hit.transform.gameObject.GetComponentInParent<RootHandle>();
                    if (rootHandle != null)
                    {
                        selectedSource = rootHandle.transform;
                        lastHandle = rootHandle;
                        UpdateSelectionEffect(selectedSource.position);
                        buildManager.Show(selectedSource.position, Input.mousePosition);

                        if (!isPlacing)
                        {
                            isPlacing = true;
                            placingFromTree = false;
                        }
                    }
                    else
                        Debug.LogError("Hit object has no RootHandle component");
                }
                else if (hit.transform.gameObject.tag == GameManager.Instance.groundTag ||
                    hit.transform.gameObject.tag == GameManager.Instance.sandGroundTag ||
                    hit.transform.gameObject.tag == GameManager.Instance.saltGroundTag ||
                    hit.transform.gameObject.tag == GameManager.Instance.superMineralTag)

                {
                    if (isPlacing)
                    {
                        bool succed = false;
                        if (placingFromTree)
                        {
                            Tree tree = selectedSource.GetComponent<Tree>();
                            if (tree.InRange(hit.point))
                            {
                                Root root = PlaceRoot(selectedSource.position, hit.point);
                                root.connectedTree = tree;
                                succed = true;
                            }
                            //Root root = Instantiate(rootPrefab, Vector3.zero, Quaternion.identity).GetComponent<Root>();
                            //root.TraceRoot(start + Vector3.up * 0.8f, end + Vector3.up * 0.8f);
                            //RootHandle handle = Instantiate(rootHandlePrefab, end, Quaternion.identity).GetComponent<RootHandle>();
                            //handle.sourceRoot = root;
                            //selectedSource = handle.transform;
                            //lastHandle = handle;
                        }
                        else
                        {
                            Tree tree = lastHandle.sourceRoot.connectedTree;
                            if (tree.InRange(hit.point))
                            {
                                lastHandle.sourceRoot.ProlongateRoot(hit.point + Vector3.up * 0.8f);
                                RootHandle handle = Instantiate(rootHandlePrefab, hit.point, Quaternion.identity).GetComponent<RootHandle>();
                                handle.sourceRoot = lastHandle.sourceRoot;
                                Destroy(lastHandle.gameObject);
                                selectedSource = handle.transform;
                                lastHandle = handle;
                                succed = true;
                            }
                        }

                        if (succed)
                        {
                            UpdateSelectionEffect(selectedSource.position);
                            placingFromTree = false;
                            isPlacing = true;
                            buildManager.Show(selectedSource.position, Input.mousePosition);
                        }
                    }
                }
            }
        }
    }

    private Root PlaceRoot(Vector3 start, Vector3 end)
    {
        Root root = Instantiate(rootPrefab, Vector3.zero, Quaternion.identity).GetComponent<Root>();
        root.TraceRoot(start + Vector3.up * 0.8f, end + Vector3.up * 0.8f);
        RootHandle handle = Instantiate(rootHandlePrefab, end, Quaternion.identity).GetComponent<RootHandle>();
        handle.sourceRoot = root;
        selectedSource = handle.transform;
        lastHandle = handle;
        return root;
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

    private void UpdateSelectionEffect(Vector3 position)
    {
        UpdateSelectionEffect(position, new Vector3(2, 0, 2));
    }

}
