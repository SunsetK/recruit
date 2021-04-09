namespace UnityTemplateProjects
{
    [System.Serializable]
    public class APIResponseData
    {
        public bool success;
        public int code;
        public DongData[] data;
    }
}