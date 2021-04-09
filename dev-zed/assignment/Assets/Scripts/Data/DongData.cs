using System.Collections.Generic;

namespace UnityTemplateProjects
{
    [System.Serializable]
    public class DongData
    {
        public TypeData[] roomtypes;
        public Meta meta;

        [System.Serializable]
        public class Meta
        {
            public int bd_id;
            public string 동;
            public int 지면높이;
        }
    }
}