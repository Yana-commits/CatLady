using System.Collections.Generic;
using UnityEngine;

public class CreatureCollector<T> : MonoBehaviour where T : NavMeshController
{
    [SerializeField] protected Transform zoneCenter;
    [SerializeField] protected float zoneRadius;

    public List<T> creatures = new List<T>();

    public virtual void Initialise()
    {
        foreach (var creature in creatures)
        {
            creature.Init(zoneCenter, zoneRadius);
        }
    }
}
