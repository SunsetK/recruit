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

        public void OnDestroy()
        {
            m_texture = null;
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
            material.SetTextureScale("_MainTex", new Vector2(1f, (int)((float)mesh.vertices.Max(t => t.y) / 3f)));

            obj.GetComponent<MeshRenderer>().material = material;
            mesh.RecalculateNormals();

            TextureMapping(mesh, obj);
        }

        private void TextureMapping(Mesh mesh, GameObject gameObject)
        {
            var meshVertices = mesh.vertices;
            var normals = mesh.normals;
            Vector2[] uvs = new Vector2[meshVertices.Length];

            for(int i = 0; i < uvs.Length; i++)
            {
                if(Math.Abs(normals[i].y) >= 0.5f)
                {
                    uvs[i] = GetTopBottomTextureUV(mesh.vertices[i], mesh.vertices);
                }
                else
                {
                    var angle1 = Vector3.SignedAngle(gameObject.transform.forward, normals[i], Vector3.up) + 180.0f;
                    var angle2 = Vector3.SignedAngle(-gameObject.transform.forward, normals[i], Vector3.up) + 180.0f;

                    if((180.0f <= angle1 && angle1 <= 220.0f) || (180.0f <= angle2 && angle2 <= 220.0f))
                        uvs[i] = GetFrontTextureUV(mesh.vertices[i], mesh.vertices);
                    else
                        uvs[i] = GetSideTextureUV(mesh.vertices[i], mesh.vertices);
                }
            }

            mesh.uv = uvs;
        }

        private Vector2 GetFrontTextureUV(Vector3 meshVertice, Vector3[] meshVertices)
        {
            float minX = meshVertices.Min(t => t.x);
            float maxX = meshVertices.Max(t => t.x);
            var normalizedX = Mathf.InverseLerp(minX, maxX, meshVertice.x);
            var x = Mathf.Lerp(0.0f, 512.0f, normalizedX) / 1024.0f;

            float minY = meshVertices.Min(t => t.y);
            float maxY = meshVertices.Max(t => t.y);
            var normalizedY = Mathf.InverseLerp(minY, maxY, meshVertice.y);
            var y = Mathf.Lerp(0.0f, 256.0f, normalizedY) / 512.0f;

            return new Vector2(x, y);
        }

        private Vector2 GetSideTextureUV(Vector3 meshVertice, Vector3[] meshVertices)
        {
            float minZ = meshVertices.Min(t => t.z);
            float maxZ = meshVertices.Max(t => t.z);
            var normalizedZ = Mathf.InverseLerp(minZ, maxZ, meshVertice.z);
            var z = Mathf.Lerp(512.0f, 768.0f, normalizedZ) / 1024.0f;

            float minY = meshVertices.Min(t => t.y);
            float maxY = meshVertices.Max(t => t.y);
            var normalizedY = Mathf.InverseLerp(minY, maxY, meshVertice.y);
            var y = Mathf.Lerp(0.0f, 256.0f, normalizedY) / 512.0f;

            return new Vector2(z, y);
        }

        private Vector2 GetTopBottomTextureUV(Vector3 meshVertice, Vector3[] meshVertices)
        {
            float minX = meshVertices.Min(t => t.x);
            float maxX = meshVertices.Max(t => t.x);
            var normalizedX = Mathf.InverseLerp(minX, maxX, meshVertice.x);
            var x = (Mathf.Lerp(768.0f, 1024.0f, normalizedX)) / 1024.0f;

            float minZ = meshVertices.Min(t => t.z);
            float maxZ = meshVertices.Max(t => t.z);
            var normalizedZ = Mathf.InverseLerp(minZ, maxZ, meshVertice.z);
            var z = Mathf.Lerp(0.0f, 512.0f, normalizedZ) / 512.0f;

            return new Vector2(x, z);
        }

        private APIResponseData LoadData()
        {
            return Json.LoadJsonFile<APIResponseData>(Application.dataPath + "/Samples/json", "dong");
        }
    }
}