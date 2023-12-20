using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent agent;

    protected Transform zoneCenter;
    protected float zoneRadius;
    protected Vector3 randomPoint;

    public void Init(Transform _zoneCenter, float _zoneRadius)
    {
        zoneCenter = _zoneCenter;
        zoneRadius = _zoneRadius;
    }
    public void NavRandomPoint()
    {
        NavMeshHit navmeshHit;
        NavMesh.SamplePosition(Random.insideUnitSphere * zoneRadius + zoneCenter.position, out navmeshHit, zoneRadius, NavMesh.AllAreas);
        randomPoint = navmeshHit.position;

        agent.SetDestination(randomPoint);
    }
}
