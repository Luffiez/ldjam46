using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSpawner :  Spawner
{

    List<Flower> flowerList = new List<Flower>();

    public List<Flower> GetFlowerList()
    {
        return flowerList;
    }
    public override GameObject Spawn()
    {
       GameObject spawnObject =  base.Spawn();
        if(spawnObject != null)
        flowerList.Add(spawnObject.GetComponent<Flower>());
        return spawnObject;
    }
}
