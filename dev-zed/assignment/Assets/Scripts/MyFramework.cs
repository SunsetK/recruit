using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityTemplateProjects
{
    public class MyFramework : MonoBehaviour
    {
        private APIResponseData m_data;
        [SerializeField] private Texture m_texture;

        public void Start()
        {
            Init();
            Draw();
        }

        private void Init()
        {
            m_data = LoadData();
        }

        private void Draw()
        {
            foreach(var dong in m_data.data)
            {
                foreach(var type in dong.roomtypes)
                {
                    foreach(var coordinatesBase64 in type.coordinatesBase64s)
                    {
                        byte[] byteArray = Convert.FromBase64String(coordinatesBase64);
                        float[] floatArray = new float[byteArray.Length / 4];
                        Buffer.BlockCopy(byteArray, 0, floatArray, 0, byteArray.Length);

                        List<Vector3> vectorList = new List<Vector3>();
                        for(int i = 0; i < floatArray.Length; i += 3)
                            vectorList.Add(new Vector3(floatArray[i], floatArray[i + 2], floatArray[i + 1]));

                        CreateMesh(vectorList.ToArray());
                    }
                }
            }
        }

        private void CreateMesh(Vector3[] vectors)
        {
            var vertices = vectors;
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            List<int> triangles = new List<int>();
            for(int i = 0; i < vectors.Length; i += 3)
            {
                triangles.Add(i);
                triangles.Add(i + 1);
                triangles.Add(i + 2);
            }
            mesh.triangles = triangles.ToArray();

            GameObject obj = new GameObject();
            obj.AddComponent<MeshFilter>();
            obj.AddComponent<MeshRenderer>();

            obj.GetComponent<MeshFilter>().mesh = mesh;

            var material = new Material(Shader.Find("Unlit/Texture"));
            material.SetTexture("_MainTex", m_texture);
            material.SetTextureScale("_BaseMap", new Vector2(1f, 3));
            obj.GetComponent<MeshRenderer>().material = material;

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            TextureMapping(ref mesh);
        }

        private void TextureMapping(ref Mesh mesh)
        {
            var meshVertices = mesh.vertices;
            var normals = mesh.normals;
            Vector2[] uvs = new Vector2[meshVertices.Length];
            for(int i = 0; i < uvs.Length; i++)
            {
                uvs[i] = new Vector2(0, 0);
            }

            mesh.uv = uvs;
        }

        private APIResponseData LoadData()
        {
            return Json.LoadJsonFile<APIResponseData>(Application.dataPath + "/Samples/json", "dong");
        }
    }
}