using System;
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

                        for(int i = 0; i < floatArray.Length; i += 12)
                        {
                            var vertice1 = new Vector3(floatArray[i], floatArray[i + 2], floatArray[i + 1]);
                            var vertice2 = new Vector3(floatArray[i + 3], floatArray[i + 5], floatArray[i + 4]);
                            var vertice3 = new Vector3(floatArray[i + 6], floatArray[i + 8], floatArray[i + 7]);
                            var vertice4 = new Vector3(floatArray[i + 9], floatArray[i + 11], floatArray[i + 10]);
                            //Debug.Log(vector.ToString());
                            CreateMesh(vertice1, vertice2, vertice3, vertice4);
                        }
                    }
                }
            }
        }

        private void CreateMesh(Vector3 vertice1, Vector3 vertice2, Vector3 vertice3, Vector3 vertice4)
        {
            var vertices = new Vector3[] { vertice1, vertice2, vertice3, vertice4 };
            int[] triangles = new int[] { 0, 1, 2, 0, 2, 3 };
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;

            GameObject obj = new GameObject();
            obj.AddComponent<MeshFilter>();
            obj.GetComponent<MeshFilter>().mesh = mesh;
        }

        private APIResponseData LoadData()
        {
            return Json.LoadJsonFile<APIResponseData>(Application.dataPath + "/Samples/json", "dong");
        }
    }
}