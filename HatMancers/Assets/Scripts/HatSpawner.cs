using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatSpawner : MonoBehaviour
{
    public GameObject[] hats;
    public GameObject bounds;

    public float timeToSpawn;
    public int spawnCount;
    public float spawnHeight;
    private float currTimer = 0;

    public int unclaimedLimit = 20;
    public int unclaimedHatCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If the game is paused, stop scene activities
        if (PlayerController.isGamePaused)
            return;

        currTimer += Time.deltaTime;
        if(currTimer >= timeToSpawn)
        {
            SpawnHats();
            currTimer = 0;
        }
    }

    void SpawnHats()
    {
        for(int i = 0; i < spawnCount; i++)
        {
            if (unclaimedHatCount >= unclaimedLimit) break;

            unclaimedHatCount++;
            int hatIndex = Random.Range(0, hats.Length);

            float rX = Random.Range(bounds.GetComponent<ArenaBounds>().PointA.x, bounds.GetComponent<ArenaBounds>().PointB.x);
            float rZ = Random.Range(bounds.GetComponent<ArenaBounds>().PointA.z, bounds.GetComponent<ArenaBounds>().PointB.z);
            Vector3 spawnPos = new Vector3(rX, spawnHeight, rZ);
            GameObject hat = Instantiate(hats[hatIndex], spawnPos, Quaternion.identity);
        }
    }
}
