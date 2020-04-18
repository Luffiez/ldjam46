using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour, IWater
{
    Seeker seeker;
    AIDestinationSetter setter;
    Flower currentTarget = null;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        setter = GetComponent<AIDestinationSetter>();

        FindClosestTarget();
        StartCoroutine(CheckIfTargetIsBurning());
    }

    IEnumerator CheckIfTargetIsBurning()
    {
        while (true)
        {
            if (currentTarget != null && currentTarget.IsBurning)
                FindClosestTarget();

            yield return new WaitForSeconds(0.1f);
        }
    }

    void FindClosestTarget()
    {
        // TODO: Implement flower manager instead and get flower list from there!
        Flower[] targets = FindObjectsOfType<Flower>();

        int closestLenght = 0;

        foreach (Flower flower in targets)
        {
            if (targets.Length > 1 && flower.IsBurning && currentTarget)
                continue;
            Debug.LogWarning("Start to set");
            Path path;

            if (currentTarget == null)
            {
                Debug.LogWarning("set first target");
                currentTarget = flower;
                path = seeker.StartPath(transform.position, flower.transform.position);
                closestLenght = path.path.Count;
                continue;
            }

            path = seeker.StartPath(transform.position, flower.transform.position);
            if(closestLenght < path.path.Count || currentTarget.IsBurning)
            {
                Debug.LogWarning("reseting target");
                currentTarget = flower;
                closestLenght = path.path.Count;
            }
        }

        setter.target = currentTarget.transform;
    }

    private void FixedUpdate()
    {
        if (!currentTarget)
            return;
        Debug.Log(Vector2.Distance(transform.position, currentTarget.transform.position));
        if(Vector2.Distance(transform.position, currentTarget.transform.position) <= 0.25f)
        {
            currentTarget.SetOnFire();

            // TODO: Add Effect for fire death
            Destroy(gameObject);
        }
    }

    public void Water()
    {
        // Kills the fire
        // TODO: Add Effect for fire death

        Destroy(gameObject);
    }
}
