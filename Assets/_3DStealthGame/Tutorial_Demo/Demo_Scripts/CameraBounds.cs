using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }

        MeshFilter mf = gameObject.AddComponent<MeshFilter>();
        mf.mesh = new Mesh();
        mf.mesh.CombineMeshes(combine);

        MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();
        MeshCollider mc = gameObject.AddComponent<MeshCollider>();
        mc.sharedMesh = mf.mesh;
        mc.convex = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
