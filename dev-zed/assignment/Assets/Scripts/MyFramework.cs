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
            //material.SetTextureScale("_BaseMap", new Vector2(1f, 3));
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
                float angle = Vector3.SignedAngle(transform.up, transform.forward - normals[i], -transform.forward);
                if(angle >= 180 && angle <= 220)
                    uvs[i] = SetFrontTexture();
                else if(normals[i].y == Vector3.up.y || normals[i].y == Vector3.down.y)
                    uvs[i] = SetTopDownTexture();
                else
                    uvs[i] = SetSideTexture();
            }

            mesh.uv = uvs;
        }

        private Vector2 SetFrontTexture()
        {
            int x = 512;
            int y = 512;
            int width = 512;
            int height = 512;
            return new Vector2(width / 1024, height / 512);
        }

        private Vector2 SetSideTexture()
        {
            int x = 768;
            int y = 512;
            int width = 256;
            int height = 512;
            return new Vector2(width / 1024, height / 512);
        }

        private Vector2 SetTopDownTexture()
        {
            int x = 1023;
            int y = 512;
            int width = 255;
            int height = 512;
            return new Vector2(width / 1024, height / 512);
        }

        private APIResponseData LoadData()
        {
            return Json.LoadJsonFile<APIResponseData>(Application.dataPath + "/Samples/json", "dong");
        }
    }
}