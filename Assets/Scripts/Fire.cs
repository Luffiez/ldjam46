using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    AIPath aiPath;

    void Start()
    {
        aiPath = GetComponent<AIPath>();               
    }


    // Update is called once per frame
    void Update()
    {
       
    }
}
