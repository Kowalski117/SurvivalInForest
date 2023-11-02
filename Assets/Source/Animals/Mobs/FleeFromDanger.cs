using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class FleeFromDanger : Action
{
    public SetMovement SetMovement;
    public float RandomPointRadius;

    private NavMeshPath _navMeshPath;
    private Vector3 _randomPoint;
    private Vector3 _CurrentPoint;

    public override void OnStart()
    {
        _navMeshPath = new NavMeshPath();

        if (SetMovement.FleeRadius <= 0)
        {
            RandomPointRadius = 1;
        }
        else
        {
            RandomPointRadius = SetMovement.FleeRadius;
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (SetMovement.IsRuning == false ||
            (SetMovement.Agent.pathEndPosition - SetMovement.transform.position).magnitude < 1)
        {
            bool isCorrectPoint = false;
            while (!isCorrectPoint)
            {
                NavMeshHit navmeshHit;
                NavMesh.SamplePosition(
                    SetMovement.transform.position + new Vector3(Random.value - 0.5f, 0, Random.value - 0.5f).normalized *
                    RandomPointRadius, out navmeshHit, RandomPointRadius, NavMesh.AllAreas);
                _randomPoint = navmeshHit.position;
                SetMovement.Agent.CalculatePath(_randomPoint, _navMeshPath);
                if (_navMeshPath.status == NavMeshPathStatus.PathComplete) isCorrectPoint = true;
            }

            SetMovement.Run(_randomPoint);
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}