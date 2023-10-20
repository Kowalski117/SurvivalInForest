using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class GoToSpawnPoint : Action
{
    public AnimalsMovement Animals;
    public float RandomPointRadius;

    private NavMeshPath _navMeshPath;
    private Vector3 _randomPoint;

    public override void OnStart()
    {
        if (Animals.SpawnPointRadius <= 0)
        {
            RandomPointRadius = 1;
        }
        else
        {
            RandomPointRadius = Animals.SpawnPointRadius;
        }
    }

    public override TaskStatus OnUpdate()
    {
        _navMeshPath = new NavMeshPath();

        if ((Animals.Agent.pathEndPosition - Animals.transform.position).magnitude < 1)
        {
            bool isCorrectPoint = false;
            while (!isCorrectPoint)
            {
                NavMeshHit navmeshHit; 
                NavMesh.SamplePosition(Animals.SpawnPoint + Random.insideUnitSphere * RandomPointRadius,out navmeshHit, RandomPointRadius, NavMesh.AllAreas);
                _randomPoint = navmeshHit.position;
                Animals.Agent.CalculatePath(_randomPoint, _navMeshPath);
                if (_navMeshPath.status == NavMeshPathStatus.PathComplete) isCorrectPoint = true;
            }

            Animals.Walk(_randomPoint);
            return TaskStatus.Inactive;
        }

        return TaskStatus.Running;
    }
}