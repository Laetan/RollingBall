using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
	public GameObject[] spawnList;
    public float speed = 3f;
    public float minX = -5f;
    public float maxX = 5f;

    private Random rand = new Random();

    private struct SpawnBounds {
        public float left, right;
        public GameObject elem;
    }
    private List<SpawnBounds> spawnBounds = new List<SpawnBounds>();

    void Start() {
        foreach (GameObject obj in spawnList)
        {
            //Debug.Log("Getting bounds for " + obj.name);
            float delta = obj.transform.Find("Entry").transform.localPosition.x;
            //Debug.Log("    - entry pos : " + delta);
            Bounds b = CalculateBounds(obj);
            //Debug.Log("    - bounds : " + b.ToString());
            spawnBounds.Add(new SpawnBounds() { left = b.min.x - delta, right = b.max.x - delta, elem = obj });
            //Debug.Log("    - deltas : " + spawnBounds[spawnBounds.Count - 1].left + " / " + spawnBounds[spawnBounds.Count - 1].right);
        }
    }

    private Bounds CalculateBounds(GameObject go)
    {
        Bounds bounds = new Bounds();
        foreach (Renderer rend in go.GetComponentsInChildren<Renderer>())
        {
            if (rend.CompareTag("Path"))
            {
                bounds.Encapsulate(rend.bounds);
            }
        }
        return bounds;

    }


    void OnTriggerEnter2D(Collider2D other){
        //Debug.Log("spawn ! ------");
        Transform spawnPos = other.gameObject.transform;
        //Debug.Log("Spawning at " + spawnPos.position.ToString());
        float maxDeltaLeft = minX - spawnPos.position.x; 
        float maxDeltaRight = maxX - spawnPos.position.x;
        List< GameObject> validSpawn = new List<GameObject>();
        //Debug.Log("max delta : " + maxDeltaLeft + " - " + maxDeltaRight);
        spawnBounds.ForEach(obj =>
       {
           //Debug.Log("obj " + obj.elem.name + ": " + obj.left + " / " + obj.right);
           if(obj.left > maxDeltaLeft && obj.right < maxDeltaRight)
           {
               //Debug.Log("valid");
               validSpawn.Add(obj.elem);
           }
       });
        //Debug.Log("valid spawn : " + validSpawn.Count, this);
        if(validSpawn.Count == 0)
        {
            //Debug.LogError("Nothing to spawn !");
            Debug.Break();
        }
        GameObject toSpawn = validSpawn[Random.Range(0 , validSpawn.Count)];
        toSpawn.GetComponent<SliderScript>().speed = speed;
        Instantiate(toSpawn, spawnPos.position -  toSpawn.transform.Find("Entry").transform.localPosition, Quaternion.identity);
    }
}
