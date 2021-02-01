using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsList : MonoBehaviour
{
    public GameObject grassobj;
    public GameObject slab;
    public Blocks block = new Blocks();

    [System.Serializable]
    public class Blocks
    {
        public GameObject grassblock;
        public GameObject blockTransparent;
        public GameObject blockOpaque;
        public GameObject blockcutout;
        public GameObject blockfade;
    }
}
