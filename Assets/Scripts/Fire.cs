﻿using Pathfinding;
using System.Collections;
using UnityEngine;

public class Fire : MonoBehaviour, IWater
{
    Seeker seeker;
    AIDestinationSetter setter;
    Flower currentTarget = null;
    
    float curDist= 0;
    float lastDist = 0;

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

            StartCoroutine(SetTarget());
        }
    }

    private IEnumerator SetTarget()
    {
        // TODO: Implement flower manager instead and get flower list from there!
        Flower[] targets = FindObjectsOfType<Flower>();
        Flower closestTarget = currentTarget;
        lastDist = 1000000f;
        foreach (Flower flower in targets)
        {
            if (closestTarget && flower.IsBurning && targets.Length > 1 )
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
            else if((lastDist > curDist) || 
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
        Debug.Log(Vector2.Distance(transform.position, currentTarget.transform.position));
        if(Vector2.Distance(transform.position, currentTarget.transform.position) <= 0.25f)
        {
            currentTarget.SetOnFire();

            // TODO: Add Effect for fire spread
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
