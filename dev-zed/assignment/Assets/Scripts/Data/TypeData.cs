using System.Collections.Generic;

namespace UnityTemplateProjects
{
    [System.Serializable]
    public class TypeData
    {
        public string[] coordinatesBase64s;
        public Meta meta;

        [System.Serializable]
        public class Meta
        {
            public int 룸타입id;
        }
    }
}