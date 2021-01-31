using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JsonReader : MonoBehaviour
{
    public TextAsset jsontxt;
    public GameObject blockopaque;
    public GameObject blocktransparent;
    public GameObject grass;
    public GameObject grassblktop;
    public GameObject slabs;

    [System.Serializable]
    public class Blockdata
    {
        public string type;
        public float[] axes;
        public Property properties = new Property();
    }

    [System.Serializable]
    public class BlockList
    {
        public Blockdata[] List;
    }

    [System.Serializable]
    public class Property
    {
        public bool snowy;
        public string plant_type;
        public string type;
        //in glss and stained terracotta --color
        //in plak -- material
        //in water -- falling, flowing, levels
        //in stairs -- facing,half,material,shape.
        //in fence -- east, north,south,west,material
        //in leaves -- check_decay,distance,material,persistent
        //in double_plant -- half,plant_type
        //in slab -- material, type.
    }

    public BlockList blocklist = new BlockList();

    private void Awake()
    {
        blocklist = JsonUtility.FromJson<BlockList>(jsontxt.text);

        for(int i =0; i< blocklist.List.Length; i++)
        {
            Vector3 points = new Vector3(blocklist.List[i].axes[0], blocklist.List[i].axes[1], blocklist.List[i].axes[2]);
            //Debug.Log(blocklist.List[i].properties.plant_type);
            string typeofblock = blocklist.List[i].type;
            //Debug.Log(playerlist.player[i].properties.plant_type);
            //Debug.Log(points);     
            if(blocklist.List[i].type == "plant")
            {
                Debug.Log(blocklist.List[i].properties.plant_type);
            }

            AppTextures(typeofblock, points, blocklist.List[i].properties);
        }
    }

    public Texture[] textures;
    public Texture[] Planttype;
    public Texture[] dirttype;

    void AppTextures(string type, Vector3 points, Property property)
    {
        switch (type)
        {
            case "air":
                break;

            case "grass_block":
                GameObject grassblock = Instantiate(grassblktop, points, Quaternion.identity);
                grassblock.GetComponent<Renderer>().material.mainTexture = textures[0];
                grassblock.name = "grassblock";
                break;

            case "stained_glass":
                GameObject glass = Instantiate(blocktransparent, points, Quaternion.identity);
                glass.GetComponent<Renderer>().material.mainTexture = textures[1];
                glass.name = "glass";
                break;

            case "plant":
                
                if(property.plant_type == "rose_bush")
                {
                    GameObject plant = Instantiate(grass, points, Quaternion.identity);
                    plant.GetComponent<Renderer>().material.mainTexture = Planttype[0];
                    plant.name = "rose_bush";

                }
                else if (property.plant_type == "grass")
                {
                    GameObject plant = Instantiate(grass, points, Quaternion.identity);
                    plant.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                    plant.name = "grass";
                }
                else if (property.plant_type == "peony")
                {
                    GameObject plant = Instantiate(grass, points, Quaternion.identity);
                    plant.GetComponent<Renderer>().material.mainTexture = Planttype[2];
                    plant.name = "peony";
                }
                else if (property.plant_type == "tall_grass")
                {
                    GameObject plant = Instantiate(grass, points, Quaternion.identity);
                    plant.GetComponent<Renderer>().material.mainTexture = Planttype[3];
                    plant.name = "tall_grass";
                }
                break;

            case "leaves":
                GameObject leaves = Instantiate(blockopaque, points, Quaternion.identity);
                leaves.GetComponent<Renderer>().material.mainTexture = textures[3];
                leaves.GetComponent<Renderer>().material.color = Color.green;
                leaves.name = "leaves";
                break;

            case "dirt":
                GameObject dirt = Instantiate(blockopaque, points, Quaternion.identity);
                dirt.GetComponent<Renderer>().material.mainTexture = dirttype[0];
                break;

            case "slab":
                GameObject slab;
                if (property.type == "bottom")
                {
                    slab = Instantiate(slabs, points + new Vector3(0,-0.25f,0), Quaternion.identity);
                }
                else
                {
                    slab = Instantiate(slabs, points + new Vector3(0, 0.25f, 0), Quaternion.identity);     
                }

                slab.GetComponent<Renderer>().material.color = Color.white;
                slab.name = "slab";
                break;

            default:
                GameObject blocks = Instantiate(blockopaque, points, Quaternion.identity);
                blockopaque.name = "block";
                break;
        }
    }

    //applying different colors for different blocks
    void ApplyTextures(string type, Vector3 points)
    {
        switch (type)
        {
            case "dirt":
                GameObject dirt = Instantiate(blockopaque, points, Quaternion.identity);
                dirt.GetComponent<Renderer>().material.color = Color.magenta;
                break;
            case "grass_block":
                GameObject grassblock = Instantiate(blockopaque, points, Quaternion.identity);
                grassblock.GetComponent<Renderer>().material.color = Color.green;
                break;
            case "plant":
                GameObject plant = Instantiate(blockopaque, points, Quaternion.identity);
                plant.GetComponent<Renderer>().material.color = Color.yellow;
                break;
            case "air":               
                break;
            case "slab":
                GameObject slab = Instantiate(blockopaque, points, Quaternion.identity);
                slab.GetComponent<Renderer>().material.color = Color.grey;
                break;
            case "leaves":
                GameObject leaves = Instantiate(blockopaque, points, Quaternion.identity);
                leaves.GetComponent<Renderer>().material.color = Color.grey;
                break;
            case "stained_terracotta":
                GameObject stained_terracotta = Instantiate(blockopaque, points, Quaternion.identity);
                stained_terracotta.GetComponent<Renderer>().material.color = Color.grey;
                break;
            case "planks":
                GameObject planks = Instantiate(blockopaque, points, Quaternion.identity);
                planks.GetComponent<Renderer>().material.color = Color.grey;
                break;
            case "stained_glass":
                GameObject stained_glasss = Instantiate(blockopaque, points, Quaternion.identity);
                stained_glasss.GetComponent<Renderer>().material.color = Color.grey;
                break;
            case "concrete":
                GameObject concrete = Instantiate(blockopaque, points, Quaternion.identity);
                concrete.GetComponent<Renderer>().material.color = Color.grey;
                break;
            case "water":
                GameObject water = Instantiate(blockopaque, points, Quaternion.identity);
                water.GetComponent<Renderer>().material.color = Color.blue;
                break;
            case "furnace":
                GameObject furnace = Instantiate(blockopaque, points, Quaternion.identity);
                furnace.GetComponent<Renderer>().material.color = Color.grey;
                break;
            default:
                GameObject defaultmat = Instantiate(blockopaque, points, Quaternion.identity);
                defaultmat.GetComponent<Renderer>().material.color = Color.grey;
                break;
        }
    }
}
