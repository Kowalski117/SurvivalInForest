using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class GoToSpawnPoint : Action
{
    public SetMovement SetMovement;
    public float RandomPointRadius;

    private NavMeshPath _navMeshPath;
    private Vector3 _randomPoint;

    public override void OnStart()
    {
        if (SetMovement.SpawnPointRadius <= 0)
        {
            RandomPointRadius = 1;
        }
        else
        {
            RandomPointRadius = SetMovement.SpawnPointRadius;
        }
    }

    public override TaskStatus OnUpdate()
    {
        _navMeshPath = new NavMeshPath();

        if ((SetMovement.Agent.pathEndPosition - SetMovement.transform.position).magnitude == 0)
        {
            bool isCorrectPoint = false;
            while (!isCorrectPoint)
            {
                NavMeshHit navmeshHit; 
                NavMesh.SamplePosition(SetMovement.SpawnPoint + Random.insideUnitSphere * RandomPointRadius,out navmeshHit, RandomPointRadius, NavMesh.AllAreas);
                _randomPoint = navmeshHit.position;
                SetMovement.Agent.CalculatePath(_randomPoint, _navMeshPath);
                if (_navMeshPath.status == NavMeshPathStatus.PathComplete) isCorrectPoint = true;
            }

            SetMovement.Walk(_randomPoint);
            return TaskStatus.Inactive;
        }

        return TaskStatus.Running;
    }
}