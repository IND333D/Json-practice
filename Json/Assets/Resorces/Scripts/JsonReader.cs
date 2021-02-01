using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JsonReader : MonoBehaviour
{
    //for loading directly from url
    [Header("Url things", order = 1)]
    public string _url;
    string txtfromurl;

    //josn data, objects list and textures list reference
    [Header("Assets list", order = 2)]
    public TextAsset jsonText;
    public ObjectsList objlists;
    public Textures texturelist;

    //each blocks data like position and type, etc.
    [System.Serializable]
    public class Blockdata
    {
        public string type;
        public float[] axes;
        public Property properties = new Property();
    }

    //list of all the blocks in the json data.
    //around 15600 blocks.
    [System.Serializable]
    public class BlockList
    {
        public Blockdata[] List;
    }

    //detailed information of the blocks.
    //like material, color, facing towards, etc.
    [System.Serializable]
    public class Property
    {
        public bool snowy;
        public string plant_type;
        public string type;
        public string material;
        public bool east;
        public bool west;
        public bool north;
        public bool south;
        public string half;
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
        ConvertData(jsonText.text);
        //StartCoroutine(GetJsonData());
    }

    //for loading json data directly from the url.
    //not called
    IEnumerator GetJsonData()
    {
        Debug.Log("started");
        using (UnityWebRequest webRequest = UnityWebRequest.Post(_url, txtfromurl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log(webRequest.error);
            }

            else
            {
                txtfromurl = webRequest.downloadHandler.text;
                ConvertData(txtfromurl);
            }
        }
    }

    //converting data
    private void ConvertData(string data)
    {
        //parsing json data to unity.
        blocklist = JsonUtility.FromJson<BlockList>(data);

        for(int i =0; i< blocklist.List.Length; i++)
        {
            //saving float array position to vector 3.
            Vector3 points = new Vector3(blocklist.List[i].axes[0], blocklist.List[i].axes[1], blocklist.List[i].axes[2]);
            string typeofblock = blocklist.List[i].type;

            Debug.Log(typeofblock);
            //for applying textures and spawning blocks.
            SpawnapplyTexture(typeofblock, points, blocklist.List[i].properties);
        }
    }

    //Spawning and Applying textures
    void SpawnapplyTexture(string type, Vector3 points, Property property)
    {
        switch (type)
        {
            case "air":
                break;

            case "grass_block":
                GameObject grassblock = Instantiate(objlists.block.grassblock, points, Quaternion.identity);
                grassblock.GetComponent<Renderer>().material.mainTexture = texturelist.blocktxtre.grassblock;
                grassblock.name = "grassblock";
                break;

            case "stained_glass":
                GameObject glass = Instantiate(objlists.block.blockTransparent, points, Quaternion.identity);
                glass.GetComponent<Renderer>().material.mainTexture = texturelist.blocktxtre.glass;
                glass.name = "glass";
                break;

            case "plant":
                
                if (property.plant_type == "grass")
                {
                    GameObject plant = Instantiate(objlists.grassobj, points + new Vector3(0,-0.5f,0), Quaternion.identity);
                    //random size, rotation.
                    plant.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                    float random = Random.Range(0.4f, 1f);
                    plant.transform.localScale = new Vector3(random, random, random);
                    Renderer[] plantmat = plant.GetComponentsInChildren<Renderer>();
                    foreach (Renderer mater in plantmat)
                    {
                        mater.material.color = Color.green;
                    }
                    plant.name = "grass";
                }
                break;

            case "leaves":
                GameObject leaves = Instantiate(objlists.block.blockcutout, points, Quaternion.identity);
                leaves.GetComponent<Renderer>().material.mainTexture = texturelist.plant.plants[5];
                leaves.GetComponent<Renderer>().material.color = Color.green;
                leaves.name = "leaves";
                break;

            case "dirt":
                GameObject dirt = Instantiate(objlists.block.blockOpaque, points, Quaternion.identity);
                dirt.GetComponent<Renderer>().material.mainTexture = texturelist.blocktxtre.dirt;
                break;

            case "slab":
                GameObject slab;
                if (property.type == "bottom")
                {
                    slab = Instantiate(objlists.slab, points + new Vector3(0,-0.25f,0), Quaternion.identity);
                }
                else
                {
                    slab = Instantiate(objlists.slab, points + new Vector3(0, 0.25f, 0), Quaternion.identity);     
                }

                slab.GetComponent<Renderer>().material.color = Color.white;
                slab.name = "slab";
                break;

            case "planks":
                GameObject planks = Instantiate(objlists.block.blockOpaque, points, Quaternion.identity);
                planks.GetComponent<Renderer>().material.mainTexture = texturelist.blocktxtre.oakplank;
                break;

            case "concrete":
                GameObject concrete = Instantiate(objlists.block.blockOpaque, points, Quaternion.identity);
                concrete.GetComponent<Renderer>().material.mainTexture = texturelist.blocktxtre.concrete;
                concrete.name = "concrete";
                break;

            case "water":
                GameObject water = Instantiate(objlists.block.blockfade, points, Quaternion.identity);
                water.GetComponent<Renderer>().material.mainTexture = texturelist.blocktxtre.water;
                water.GetComponent<Renderer>().material.color = new Color(0, 0, 1,0.3f);
                break;

            case "furnace":
                GameObject furnace = Instantiate(objlists.block.blockOpaque, points, Quaternion.identity);
                furnace.GetComponent<Renderer>().material.color = Color.grey;
                furnace.name = "furnace";
                break;

            case "flower_pot":
                GameObject flowerpot = Instantiate(objlists.flowerpot, points, Quaternion.identity);
                flowerpot.GetComponentInChildren<Renderer>().material.color = Color.red;
                flowerpot.name = "flower_pot";
                break;

            case "fence":
                float rot = (property.north) ? 0f : (property.east) ? 90 : (property.south) ? 180 : (property.east) ? 270 : 0;
                GameObject fence = Instantiate(objlists.fence, points, Quaternion.Euler(0,rot,0));

                //this is expensive bcuz of loop.
                //todo: instead use fence model and apply texture;
                //iterating just for now.
                Renderer[] mat = fence.GetComponentsInChildren<Renderer>();
                foreach(Renderer mater in mat)
                {
                    mater.material.mainTexture = texturelist.blocktxtre.oakplank;
                }
                fence.name = "fence";
                break;

            case "double_plant":
                if(property.half == "lower")
                {
                    if (property.plant_type == "rose_bush")
                    {
                        GameObject plant = Instantiate(objlists.grassobj, points + new Vector3(0, -0.5f, 0), Quaternion.identity);

                        //this is expensive bcuz of loop.
                        Renderer[] plantmat = plant.GetComponentsInChildren<Renderer>();
                        foreach (Renderer mater in plantmat)
                        {
                            mater.material.mainTexture = texturelist.plant.plants[0];
                        }
                        plant.name = "rose_bush";

                    }
                    else if (property.plant_type == "peony")
                    {
                        GameObject plant = Instantiate(objlists.grassobj, points + new Vector3(0, -0.5f, 0), Quaternion.identity);

                        //this is expensive bcuz of loop.
                        Renderer[] plantmat = plant.GetComponentsInChildren<Renderer>();
                        foreach (Renderer mater in plantmat)
                        {
                            mater.material.mainTexture = texturelist.plant.plants[2];
                        }
                        plant.name = "peony";
                    }
                    else if (property.plant_type == "tall_grass")
                    {
                        GameObject plant = Instantiate(objlists.grassobj, points + new Vector3(0, -0.5f, 0), Quaternion.identity);
                        //this is expensive bcuz of loop.
                        Renderer[] plantmat = plant.GetComponentsInChildren<Renderer>();
                        foreach (Renderer mater in plantmat)
                        {
                            mater.material.mainTexture = texturelist.plant.plants[3];
                            mater.material.color = Color.green;
                        }
                        plant.name = "tall_grass";
                    }
                }
                else
                {
                    if (property.plant_type == "rose_bush")
                    {
                        GameObject plant = Instantiate(objlists.grassobj, points + new Vector3(0, -0.5f, 0), Quaternion.identity);

                        //this is expensive bcuz of loop.
                        Renderer[] plantmat = plant.GetComponentsInChildren<Renderer>();
                        foreach (Renderer mater in plantmat)
                        {
                            mater.material.mainTexture = texturelist.plant.plants[6];
                        }
                        plant.name = "rose_bush";

                    }
                    else if (property.plant_type == "peony")
                    {
                        GameObject plant = Instantiate(objlists.grassobj, points + new Vector3(0, -0.5f, 0), Quaternion.identity);

                        //this is expensive bcuz of loop.
                        Renderer[] plantmat = plant.GetComponentsInChildren<Renderer>();
                        foreach (Renderer mater in plantmat)
                        {
                            mater.material.mainTexture = texturelist.plant.plants[7];
                        }
                        plant.name = "peony";
                    }
                    else if (property.plant_type == "tall_grass")
                    {
                        GameObject plant = Instantiate(objlists.grassobj, points + new Vector3(0, -0.5f, 0), Quaternion.identity);
                        //this is expensive bcuz of loop.
                        Renderer[] plantmat = plant.GetComponentsInChildren<Renderer>();
                        foreach (Renderer mater in plantmat)
                        {
                            mater.material.mainTexture = texturelist.plant.plants[8];
                            mater.material.color = Color.green;
                        }
                        plant.name = "tall_grass";
                    }
                }
                
                break;

            default:
                GameObject blocks = Instantiate(objlists.block.blockOpaque, points, Quaternion.identity);
                blocks.name = "block";
                break;
        }
    }

    //applying different colors for different blocks
    //not called
    void ApplyTextures(string type, Vector3 points)
    {
        switch (type)
        {
            case "dirt":
                GameObject dirt = Instantiate(objlists.block.blockOpaque, points, Quaternion.identity);
                dirt.GetComponent<Renderer>().material.color = Color.magenta;
                break;
            case "grass_block":
                GameObject grassblock = Instantiate(objlists.block.blockOpaque, points, Quaternion.identity);
                grassblock.GetComponent<Renderer>().material.color = Color.green;
                break;
            case "plant":
                GameObject plant = Instantiate(objlists.block.blockOpaque, points, Quaternion.identity);
                plant.GetComponent<Renderer>().material.color = Color.yellow;
                break;
            case "air":               
                break;
            case "slab":
                GameObject slab = Instantiate(objlists.block.blockOpaque, points, Quaternion.identity);
                slab.GetComponent<Renderer>().material.color = Color.grey;
                break;
            case "leaves":
                GameObject leaves = Instantiate(objlists.block.blockOpaque, points, Quaternion.identity);
                leaves.GetComponent<Renderer>().material.color = Color.grey;
                break;
            case "stained_terracotta":
                GameObject stained_terracotta = Instantiate(objlists.block.blockOpaque, points, Quaternion.identity);
                stained_terracotta.GetComponent<Renderer>().material.color = Color.grey;
                break;
            case "planks":
                GameObject planks = Instantiate(objlists.block.blockOpaque, points, Quaternion.identity);
                planks.GetComponent<Renderer>().material.color = Color.grey;
                break;
            case "stained_glass":
                GameObject stained_glasss = Instantiate(objlists.block.blockOpaque, points, Quaternion.identity);
                stained_glasss.GetComponent<Renderer>().material.color = Color.grey;
                break;
            case "concrete":
                GameObject concrete = Instantiate(objlists.block.blockOpaque, points, Quaternion.identity);
                concrete.GetComponent<Renderer>().material.color = Color.grey;
                break;
            case "water":
                GameObject water = Instantiate(objlists.block.blockOpaque, points, Quaternion.identity);
                water.GetComponent<Renderer>().material.color = Color.blue;
                break;
            case "furnace":
                GameObject furnace = Instantiate(objlists.block.blockOpaque, points, Quaternion.identity);
                furnace.GetComponent<Renderer>().material.color = Color.grey;
                break;
            default:
                GameObject defaultmat = Instantiate(objlists.block.blockOpaque, points, Quaternion.identity);
                defaultmat.GetComponent<Renderer>().material.color = Color.grey;
                break;
        }
    }
}
