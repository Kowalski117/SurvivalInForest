using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;


public class GoToSpawnPoint : Action
{
    public AnimalsMovement SelfMob;
    public float RandomPointRadius;
    
    private NavMeshPath _navMeshPath;
    private Vector3 _randomPoint;
    
    public override TaskStatus OnUpdate()
    {
        _navMeshPath = new NavMeshPath();

        if ((SelfMob.Agent.pathEndPosition - SelfMob.transform.position).magnitude == 0)
        {
            bool isCorrectPoint = false;
            while (!isCorrectPoint)
            {
                NavMeshHit navmeshHit;
                NavMesh.SamplePosition(Random.insideUnitSphere * RandomPointRadius + SelfMob.SpawnPoint,
                    out navmeshHit, RandomPointRadius, NavMesh.AllAreas);
                _randomPoint = navmeshHit.position;
                SelfMob.Agent.CalculatePath(_randomPoint, _navMeshPath);
                if (_navMeshPath.status == NavMeshPathStatus.PathComplete) isCorrectPoint = true;
            }
            
            SelfMob.Walk(_randomPoint);
            return TaskStatus.Inactive;
        }

        return TaskStatus.Running;
    }
}