using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class FleeFromDanger : Action
{
    public AnimalsMovement Animals;
    public float RandomPointRadius;
    
    private NavMeshPath _navMeshPath;
    private Vector3 _randomPoint;
    private Vector3 _CurrentPoint;

    public override void OnStart()
    {
        _navMeshPath = new NavMeshPath();
        
        if (Animals.FleeRadius <= 0)
        {
            RandomPointRadius = 1;
        }
        else
        {
            RandomPointRadius = Animals.FleeRadius;
        }
    }

    public override TaskStatus OnUpdate()
    {
        SendRandomPoint();
        return TaskStatus.Running;
    }

    private void SendRandomPoint()
    {
        if (Animals.IsRuning == false ||
            (Animals.Agent.pathEndPosition - Animals.transform.position).magnitude == 0)
        {
            bool isCorrectPoint = false;
            while (!isCorrectPoint)
            {
                NavMeshHit navmeshHit;
                NavMesh.SamplePosition(Animals.transform.position+new Vector3(Random.value-0.5f,0,Random.value-0.5f).normalized*RandomPointRadius,
                    out navmeshHit, RandomPointRadius, NavMesh.AllAreas);
                _randomPoint = navmeshHit.position;
                Animals.Agent.CalculatePath(_randomPoint, _navMeshPath);
                if (_navMeshPath.status == NavMeshPathStatus.PathComplete) isCorrectPoint = true;
            }
            
            Animals.Run(_randomPoint);
        }
    }
}