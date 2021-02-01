using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Textures : MonoBehaviour
{
    public Plants plant = new Plants();
    public Blocktxtre blocktxtre = new Blocktxtre();

    [System.Serializable]
    public class Plants
    {
        public Texture[] plants;
    }

    [System.Serializable]
    public class Blocktxtre
    {
        public Texture grassblock;
        public Texture glass;
        public Texture dirt;
        public Texture oakplank;
        public Texture concrete;
        public Texture water;
    }
}
