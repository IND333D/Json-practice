using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonReader : MonoBehaviour
{
    public TextAsset jsontxt;
    public GameObject block;

    [System.Serializable]
    public class Player
    {
        public string type;
        public float[] axes;
    }

    [System.Serializable]
    public class PlayerList
    {
        public Player[] player;
    }

    [System.Serializable]
    public class Property
    {

    }

    public PlayerList playerlist = new PlayerList();

    private void Awake()
    {
        playerlist = JsonUtility.FromJson<PlayerList>(jsontxt.text);

        for(int i =0; i< playerlist.player.Length; i++)
        {
            Vector3 points = new Vector3(playerlist.player[i].axes[0], playerlist.player[i].axes[1], playerlist.player[i].axes[2]);
            Debug.Log(playerlist.player[i].type);
            string typeofblock = playerlist.player[i].type;
            //Debug.Log(points);
            
            AppTextures(typeofblock, points);
        }
    }

    void AppTextures(string type, Vector3 points)
    {
        switch (type)
        {
            case "air":
                break;
            case "grass_block":
                GameObject grassblock = Instantiate(block, points, Quaternion.identity);
                grassblock.GetComponent<Renderer>().material.color = Color.green;
                break;
            case "stained_glass":
                GameObject glass = Instantiate(block, points, Quaternion.identity);
                glass.GetComponent<Renderer>().material.color = new Color(0,0,1,0.05f);
                break;
            case "plant":
                GameObject plant = Instantiate(block, points, Quaternion.identity);
                plant.GetComponent<Renderer>().material.color = Color.green;
                break;
            case "leaves":
                GameObject leaves = Instantiate(block, points, Quaternion.identity);
                leaves.GetComponent<Renderer>().material.color = Color.green;
                break;
            default:
                GameObject blocks = Instantiate(block, points, Quaternion.identity);
                break;
        }
    }

    void ApplyTextures(string type, Vector3 points)
    {
        switch (type)
        {
            case "dirt":
                GameObject dirt = Instantiate(block, points, Quaternion.identity);
                dirt.GetComponent<Renderer>().material.color = Color.magenta;
                break;
            case "grass_block":
                GameObject grassblock = Instantiate(block, points, Quaternion.identity);
                grassblock.GetComponent<Renderer>().material.color = Color.green;
                break;
            case "plant":
                GameObject plant = Instantiate(block, points, Quaternion.identity);
                plant.GetComponent<Renderer>().material.color = Color.yellow;
                break;
            case "air":               
                break;
            case "slab":
                GameObject slab = Instantiate(block, points, Quaternion.identity);
                slab.GetComponent<Renderer>().material.color = Color.grey;
                break;
            case "leaves":
                GameObject leaves = Instantiate(block, points, Quaternion.identity);
                leaves.GetComponent<Renderer>().material.color = Color.grey;
                break;
            case "stained_terracotta":
                GameObject stained_terracotta = Instantiate(block, points, Quaternion.identity);
                stained_terracotta.GetComponent<Renderer>().material.color = Color.grey;
                break;
            case "planks":
                GameObject planks = Instantiate(block, points, Quaternion.identity);
                planks.GetComponent<Renderer>().material.color = Color.grey;
                break;
            case "stained_glass":
                GameObject stained_glasss = Instantiate(block, points, Quaternion.identity);
                stained_glasss.GetComponent<Renderer>().material.color = Color.grey;
                break;
            case "concrete":
                GameObject concrete = Instantiate(block, points, Quaternion.identity);
                concrete.GetComponent<Renderer>().material.color = Color.grey;
                break;
            case "water":
                GameObject water = Instantiate(block, points, Quaternion.identity);
                water.GetComponent<Renderer>().material.color = Color.blue;
                break;
            case "furnace":
                GameObject furnace = Instantiate(block, points, Quaternion.identity);
                furnace.GetComponent<Renderer>().material.color = Color.grey;
                break;
            default:
                GameObject defaultmat = Instantiate(block, points, Quaternion.identity);
                defaultmat.GetComponent<Renderer>().material.color = Color.grey;
                break;
        }
    }
}
