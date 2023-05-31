using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigSiteSpawner : MonoBehaviour
{
    public bool canSpawn = true;
    public int maximumDigSites = 3;
    public GameObject digSitePrefab;
    public List<Transform> digSpawnPositions = new List<Transform>();
    private List<GameObject> digSiteList = new List<GameObject>();
    private List<int> currentIndexes = new List<int>();
    public float checkRate = 1;


    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    void Update()
    {

    }

    private void SpawnDigSite()
    {
        int index;
        while (true)
        {
            index = Random.Range(0, digSpawnPositions.Count);
            if (!currentIndexes.Contains(index)) break;
        }
        Vector3 randomPosition = digSpawnPositions[index].position;
        GameObject digSite = Instantiate(digSitePrefab, randomPosition, digSitePrefab.transform.rotation);
        digSiteList.Add(digSite);
        currentIndexes.Add(index);
        DigSite digSiteComponent = digSite.GetComponent<DigSite>();
        digSiteComponent.SetSpawner(this, index);
    }

    private IEnumerator SpawnRoutine()
    {
        while (canSpawn)
        {

            if (digSiteList.Count < maximumDigSites)
            {
                SpawnDigSite();
            }
            yield return new WaitForSeconds(checkRate);

        }
    }

    public void RemoveDigSiteFromList(GameObject digSite)
    {
        digSiteList.Remove(digSite);
    }

    public void DestroyAllDigSites()
    {
        foreach (GameObject digSite in digSiteList)
        {
            Destroy(digSite);
        }

        digSiteList.Clear();
    }
}