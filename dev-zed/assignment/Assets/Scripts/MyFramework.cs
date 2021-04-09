using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTemplateProjects
{
    public class MyFramework : MonoBehaviour
    {
        private APIResponseData m_data;

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
                        {
                            vectorList.Add(new Vector3(floatArray[i], floatArray[i + 2], floatArray[i + 1]));
                            //Debug.Log($"{(new Vector3(floatArray[i], floatArray[i + 2], floatArray[i + 1])).ToString()}");

                            if(vectorList.Count >= 6)
                            {
                                CreateMesh(vectorList.ToArray());
                                vectorList.Clear();
                            }
                        }
                        Debug.Log($"{vectorList.Count}");
                    }
                }
            }
        }

        private void CreateMesh(Vector3[] vectors)
        {
            var vertices = vectors;
            int[] triangles = new int[] { 0, 2, 1, 1, 2, 3, 3, 2, 4, 4, 2, 5, 5, 2, 0 };
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;

            GameObject obj = new GameObject();
            obj.AddComponent<MeshFilter>();
            obj.AddComponent<MeshRenderer>();
            obj.GetComponent<MeshFilter>().mesh = mesh;
        }

        private APIResponseData LoadData()
        {
            return Json.LoadJsonFile<APIResponseData>(Application.dataPath + "/Samples/json", "dong");
        }
    }
}