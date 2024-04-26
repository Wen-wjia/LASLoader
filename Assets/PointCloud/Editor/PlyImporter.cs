// Pcx - Point cloud importer & renderer for Unity
// https://github.com/keijiro/Pcx

using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LASReader;
using System.Runtime.Remoting.Messaging;

namespace Pcx
{
    [ScriptedImporter(1, "las")]
    class PlyImporter : ScriptedImporter
    {
        #region ScriptedImporter implementation

        public bool importedAsMesh = true;

        public string DefaultMaterialPath = "Assets/PointCloud/Editor/Default Point.mat";

        public override void OnImportAsset(AssetImportContext context)
        {
            
            // ComputeBuffer container
            // Create a prefab with PointCloudRenderer.
            var gameObject = new GameObject();
            
            LASFile file = new LASFile(context.assetPath);
            file.Read(true);

            Debug.Log(context.assetPath);
            Debug.Log(file.NumberOfPoints);

            if (!importedAsMesh)
            {
                Debug.Log("importing into buffer");

                PointCloudData data = ScriptableObject.CreateInstance<PointCloudData>();
                data.Initialize(file.Points, file.Colors);
                data.name = Path.GetFileNameWithoutExtension(context.assetPath);

                var renderer = gameObject.AddComponent<PointCloudRenderer>();
                renderer.sourceData = data;

                context.AddObjectToAsset("prefab", gameObject);
                if (data != null) context.AddObjectToAsset("data", data);
            }
            else
            {
                Debug.Log("importing as mesh");

                var mesh = new Mesh();
                mesh.name = Path.GetFileNameWithoutExtension(context.assetPath);

                mesh.indexFormat = file.NumberOfPoints > 65535 ?
                    IndexFormat.UInt32 : IndexFormat.UInt16;

                mesh.SetVertices(file.Points);
                mesh.SetColors(file.Colors);
                mesh.SetIndices(
                    Enumerable.Range(0, (int)file.NumberOfPoints).ToArray(),
                    MeshTopology.Points, 0
                );

                mesh.UploadMeshData(true);

                var meshFilter = gameObject.AddComponent<MeshFilter>();
                meshFilter.sharedMesh = mesh;

                var meshRenderer = gameObject.AddComponent<MeshRenderer>();
                meshRenderer.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>(DefaultMaterialPath);

                context.AddObjectToAsset("prefab", gameObject);
                if (mesh != null) context.AddObjectToAsset("mesh", mesh);
            }

            context.SetMainObject(gameObject);

            Debug.Log("Completed import");
        }

        #endregion
    }
}
