using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootManager : MonoBehaviour
{
    public float radiusFactor = 6;
    public GameObject radiusRenderer;

    public GameObject rootPrefab;
    public GameObject rootHandlePrefab;
    public Camera camera;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == treeTag)
                {

                    Tree tree = hit.transform.gameObject.GetComponentInParent<Tree>();
                    if (tree != null)
                    {
                        selectedSource = tree.transform;
                        radiusRenderer.SetActive(true);
                        radiusRenderer.transform.position = tree.transform.transform.position + Vector3.up * 0.2f;
                        radiusRenderer.transform.localScale = new Vector3(tree.effectRadius * radiusFactor, 0, tree.effectRadius * radiusFactor);
                        if (!isPlacing)
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
                        radiusRenderer.SetActive(true);
                        radiusRenderer.transform.position = rootHandle.transform.transform.position + Vector3.up * 0.2f;
                        radiusRenderer.transform.localScale = new Vector3(2, 0, 2);
                        if (!isPlacing)
                        {

                            isPlacing = true;
                            placingFromTree = false;
                        }
                        else
                            Debug.LogError("Hit object has no RootHandle component");
                    }
                }
                else if (hit.transform.gameObject.tag == groundTag)
                {
                    radiusRenderer.SetActive(false);

                    if (isPlacing)
                    {
                        if (placingFromTree)
                        {
                            PlaceRoot(selectedSource.position, hit.point);
                        }
                        else
                        {
                            //PlaceRoot(lastHandle.transform.position, groundHit.point);
                            lastHandle.sourceRoot.ProlongateRoot(hit.point + Vector3.up * 0.8f);
                            RootHandle handle = Instantiate(rootHandlePrefab, hit.point, Quaternion.identity).GetComponent<RootHandle>();
                            handle.sourceRoot = lastHandle.sourceRoot;
                            Destroy(lastHandle.gameObject);
                        }
                        isPlacing = false;
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
    }

}
