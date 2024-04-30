using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManager : MonoBehaviour
{
    FileSelectManager fsm;

    GameObject LASObj;

    public GameObject cam;

    bool isRelocated = false;

    // Start is called before the first frame update
    void Start()
    {
        fsm = FindObjectOfType<FileSelectManager>();
    }

    // Update is called once per frame
    void Update()
    {
        LASObj = GameObject.Find("LASObject");
        if (LASObj != null && isRelocated == false)
        {
            RelocateLAS();
        }
    }

    public void DestroyLAS()
    {
        if (LASObj != null)
        {
            if (fsm.isFirst == false)
            {
                Destroy(LASObj);
                isRelocated = false;
            }
        }
    }

    private void RelocateLAS()
    {
        isRelocated = true;

        LASObj.transform.position = cam.transform.position;

        // // located the imported model at the centre of the camera
        // MeshFilter[] meshes = LASObj.GetComponentsInChildren<MeshFilter>();
        // MeshFilter middleMesh = meshes[meshes.Length / 2];
        // LASObj.transform.position -= middleMesh.transform.rotation * middleMesh.sharedMesh.bounds.center;
    }
}
