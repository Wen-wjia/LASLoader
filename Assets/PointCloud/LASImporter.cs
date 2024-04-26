using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using LASReader;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections.Generic;
// using System.Drawing.Printing;

public class LASImporter : MonoBehaviour
{
    public Material defaultPointCloudMaterial;
    
    public Quaternion sceneRotationQuaternion;

    public float downsampleRate = 0.5f;

    // public string LASPath;

    private LASFile pointCloudLAS;

    private GameObject LASObject;

    /*private void Start()
    {
        StartCoroutine(CallCorontineToImportLASAsMesh(LASPath));
    }*/

    public void ImportLAS(string LASPath)
    {

        Debug.Log(LASPath);

        StartCoroutine(CallCorontineToImportLASAsMesh(LASPath));
    }

    private IEnumerator CallCorontineToImportLASAsMesh(string path)
    {
        pointCloudLAS = new LASFile(path);
        pointCloudLAS.Read();
        
        while (pointCloudLAS.Progress < 1f)
        {
            yield return new WaitForSeconds(0.1f);
            Debug.Log(pointCloudLAS.Progress);
        }

        pointCloudLAS.Close();
        bool hasColor = pointCloudLAS.hasColor;
        pointCloudLAS.Points = pointCloudLAS.Points.Select(p => sceneRotationQuaternion * p).ToList();

        Debug.Log("Importing " + pointCloudLAS.NumberOfPoints + " Points as Mesh");

        Mesh mesh = createMesh(pointCloudLAS.Points, pointCloudLAS.Colors);

        UpdatePointCloud();

        yield return null;
    }

    public Mesh createMesh(List<Vector3> points, List<Color32> colors)
    {
        Mesh mesh = new();

        int pointCount = points.Count;

        mesh.indexFormat = IndexFormat.UInt32;

        mesh.SetVertices(points);
        mesh.SetColors(colors);

        mesh.SetIndices(
            Enumerable.Range(0, pointCount).ToArray(),
            MeshTopology.Points, 0
        );
        mesh.Optimize();

        mesh.UploadMeshData(true);

        return mesh;
    }
        

    private void AddIntoScene(Mesh mesh)
    {
        if (LASObject != null)
        {
            DestroyImmediate(LASObject.GetComponent<MeshFilter>().sharedMesh, true);
            LASObject.GetComponent<MeshFilter>().sharedMesh = mesh;
        }
        else
        {
            LASObject = new GameObject("LASObject");
            LASObject.AddComponent<MeshFilter>().sharedMesh = mesh;

            LASObject.transform.position = new Vector3(-207.300003f, -53.7000008f, 107f);

            var meshRenderer = LASObject.AddComponent<MeshRenderer>();
            
            if (defaultPointCloudMaterial != null)
            {
                meshRenderer.sharedMaterial = Instantiate(defaultPointCloudMaterial);
            }
        }
    }

    private void UpdatePointCloud()
    {
        bool[] isSelected = new bool[pointCloudLAS.NumberOfPoints];
        isSelected = isSelected.Select(x => Random.value >= downsampleRate).ToArray();

        List<Vector3> newPoints = new();
        List<Color32> newColors = new();

        for (int i = 0; i < isSelected.Length; i++)
        {
            if (isSelected[i])
            {
                newPoints.Add(pointCloudLAS.Points[i]);
                newColors.Add(pointCloudLAS.Colors[i]);
            }
        }

        AddIntoScene(createMesh(newPoints, newColors));
    }
}
