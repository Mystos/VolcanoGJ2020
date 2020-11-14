using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RootManager : MonoBehaviour
{
    public Camera camera;
    public BuildManager buildManger;

    [Header("Radius renderer")]
    public float radiusFactor = 6;
    public GameObject radiusRenderer;

    [Header("Prefabs")]
    public GameObject rootPrefab;
    public GameObject rootHandlePrefab;

    private string groundTag = "ground";
    private string treeTag = "tree";
    private string rootHandleTag = "rootHandle";

    //Root building
    bool isPlacing = false;
    bool placingFromTree = false;
    Transform selectedSource;
    RootHandle lastHandle;

    // Start is called before the first frame update
    void Start()
    {
        radiusRenderer.SetActive(false);
        buildManger.Hide();
        isPlacing = false;
        placingFromTree = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            radiusRenderer.SetActive(false);
            buildManger.Hide();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                buildManger.Hide();

                if (hit.transform.gameObject.tag == treeTag)
                {

                    Tree tree = hit.transform.gameObject.GetComponentInParent<Tree>();
                    if (tree != null)
                    {
                        selectedSource = tree.transform;
                        UpdateSelectionEffect(selectedSource.position, radiusFactor * new Vector3(tree.effectRadius, 0, tree.effectRadius));
                        if (!isPlacing || (isPlacing == true && placingFromTree == false))
                        {
                            isPlacing = true;
                            placingFromTree = true;
                        }
                    }
                    else
                        Debug.LogError("Hit object has no Tree component");
                }
                else if (hit.transform.gameObject.tag == rootHandleTag)
                {
                    RootHandle rootHandle = hit.transform.gameObject.GetComponentInParent<RootHandle>();
                    if (rootHandle != null)
                    {
                        selectedSource = rootHandle.transform;
                        lastHandle = rootHandle;
                        UpdateSelectionEffect(selectedSource.position);
                        buildManger.Show(selectedSource.position, Input.mousePosition);

                        if (!isPlacing)
                        {
                            isPlacing = true;
                            placingFromTree = false;
                        }
                    }
                    else
                        Debug.LogError("Hit object has no RootHandle component");
                }
                else if (hit.transform.gameObject.tag == groundTag)
                {
                    if (isPlacing)
                    {
                        if (placingFromTree)
                        {
                            PlaceRoot(selectedSource.position, hit.point);

                            //Root root = Instantiate(rootPrefab, Vector3.zero, Quaternion.identity).GetComponent<Root>();
                            //root.TraceRoot(start + Vector3.up * 0.8f, end + Vector3.up * 0.8f);
                            //RootHandle handle = Instantiate(rootHandlePrefab, end, Quaternion.identity).GetComponent<RootHandle>();
                            //handle.sourceRoot = root;
                            //selectedSource = handle.transform;
                            //lastHandle = handle;
                        }
                        else
                        {
                            lastHandle.sourceRoot.ProlongateRoot(hit.point + Vector3.up * 0.8f);
                            RootHandle handle = Instantiate(rootHandlePrefab, hit.point, Quaternion.identity).GetComponent<RootHandle>();
                            handle.sourceRoot = lastHandle.sourceRoot;
                            Destroy(lastHandle.gameObject);
                            selectedSource = handle.transform;
                            lastHandle = handle;

                        }
                        UpdateSelectionEffect(selectedSource.position);

                        placingFromTree = false;
                        isPlacing = true;
                        buildManger.Show(selectedSource.position, Input.mousePosition);
                    }
                }
            }
        }
    }

    private void PlaceRoot(Vector3 start, Vector3 end)
    {
        Root root = Instantiate(rootPrefab, Vector3.zero, Quaternion.identity).GetComponent<Root>();
        root.TraceRoot(start + Vector3.up * 0.8f, end + Vector3.up * 0.8f);
        RootHandle handle = Instantiate(rootHandlePrefab, end, Quaternion.identity).GetComponent<RootHandle>();
        handle.sourceRoot = root;
        selectedSource = handle.transform;
        lastHandle = handle;
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
