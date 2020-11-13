using System.Collections;
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
    RootHandle lastHandle;

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
                Tree tree = treeHit.transform.gameObject.GetComponentInParent<Tree>();
                RootHandle rootHandle = treeHit.transform.gameObject.GetComponentInParent<RootHandle>();
                if (isPlacing)
                {
                    if ((placingFromTree && tree.transform == source) == false && !tree.isConnected)
                    {
                        PlaceRoot(treeHit.point, source.position);
                        isPlacing = false;
                    }
                }
                else
                {
                    isPlacing = true;
                    if (tree != null)
                    {
                        source = tree.transform;
                        placingFromTree = true;
                    }
                    else if (rootHandle != null)
                    {
                        source = rootHandle.transform;
                        lastHandle = rootHandle;
                        placingFromTree = false;
                    }
                    else
                        Debug.LogError("Hit object has no Rree or RootHandle component");
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
                        //PlaceRoot(lastHandle.transform.position, groundHit.point);
                        lastHandle.sourceRoot.ProlongateRoot(groundHit.point);
                        RootHandle handle = Instantiate(rootHandlePrefab, groundHit.point, Quaternion.identity).GetComponent<RootHandle>();
                        handle.sourceRoot = lastHandle.sourceRoot;
                        Destroy(lastHandle.gameObject);
                    }
                    isPlacing = false;
                }
            }
        }
    }

    private void PlaceRoot(Vector3 start, Vector3 end)
    {
        Root root = Instantiate(rootPrefab, Vector3.zero, Quaternion.identity).GetComponent<Root>();
        root.TraceRoot(start, end);
        RootHandle handle = Instantiate(rootHandlePrefab, end, Quaternion.identity).GetComponent<RootHandle>();
        handle.sourceRoot = root;
    }
}
