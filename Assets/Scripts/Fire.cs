using Pathfinding;
using System.Collections;
using UnityEngine;

public class Fire : MonoBehaviour, IWater
{
    Seeker seeker;
    AIDestinationSetter setter;
    Flower currentTarget = null;
    
    float curDist= 0;
    float lastDist = 10000;

    public GameObject explosionPrefab;
    public GameObject smokePrefab;


    void Start()
    {
        seeker = GetComponent<Seeker>();
        setter = GetComponent<AIDestinationSetter>();

        StartCoroutine(CheckTargetStatus());
    }

    IEnumerator CheckTargetStatus()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if(currentTarget == null || currentTarget.IsBurning)
                StartCoroutine(SetTarget());
        }
    }

    private IEnumerator SetTarget()
    {
        // TODO: Implement flower manager instead and get flower list from there!
        Flower[] targets = FindObjectsOfType<Flower>();
        Flower closestTarget = currentTarget;
        lastDist = 10000;

        foreach (Flower flower in targets)
        {
            if (closestTarget && flower.IsBurning && targets.Length > 1 ||
                currentTarget == flower)
            {
                continue;
            }

            Path path;
            path = seeker.StartPath(transform.position, flower.transform.position, OnPathCalculated);

            yield return StartCoroutine(path.WaitForPath());

            Debug.Log(flower.name + " : " + curDist);

            if (closestTarget == null)
            {               
                closestTarget = flower;
            }
            else if((lastDist >= curDist) || 
                (currentTarget && currentTarget.IsBurning))
            {
                Debug.Log("New target!: " + flower.name);
                lastDist = curDist;
                closestTarget = flower;
            }
            //Debug.Log("Target: " + closestTarget.name + " - Length: " + closestLenght);
        }
      
        currentTarget = closestTarget;
        setter.target = currentTarget.transform;
    }

    private void OnPathCalculated(Path path)
    {
        curDist = path.GetTotalLength();
    }

    private void FixedUpdate()
    {
        if (!currentTarget)
            return; 

        if(Vector2.Distance(transform.position, currentTarget.transform.position) <= 0.25f)
        {
            currentTarget.SetOnFire();

            GameObject explosion = Instantiate(explosionPrefab, currentTarget.transform.position, Quaternion.identity);
            Destroy(explosion, 3f);

            Destroy(gameObject);
        }
    }

    public void Water()
    {
        // Kills the fire

        GameObject smoke = Instantiate(smokePrefab, transform.position, Quaternion.identity);
        Destroy(smoke, 3f);

        Destroy(gameObject);
    }
}
