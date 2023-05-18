using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class FleeFromDanger : Action
{
    public SharedMob SelfMob;
    public float RandomPointRadius;
    
    private NavMeshPath _navMeshPath;
    private Vector3 _randomPoint;
    private Vector3 _CurrentPoint;

    public override void OnStart()
    {
        _navMeshPath = new NavMeshPath();
        
        if (SelfMob.Value.FleeRadius <= 0)
        {
            RandomPointRadius = 1;
        }
        else
        {
            RandomPointRadius = SelfMob.Value.FleeRadius;
        }
    }

    public override TaskStatus OnUpdate()
    {
        SendRandomPoint();
        return TaskStatus.Running;
    }

    private void SendRandomPoint()
    {
        if (SelfMob.Value.IsRuning == false ||
            (SelfMob.Value.Agent.pathEndPosition - SelfMob.Value.transform.position).magnitude == 0)
        {
            bool isCorrectPoint = false;
            while (!isCorrectPoint)
            {
                NavMeshHit navmeshHit;
                NavMesh.SamplePosition(SelfMob.Value.transform.position+new Vector3(Random.value-0.5f,0,Random.value-0.5f).normalized*RandomPointRadius,
                    out navmeshHit, RandomPointRadius, NavMesh.AllAreas);
                _randomPoint = navmeshHit.position;
                SelfMob.Value.Agent.CalculatePath(_randomPoint, _navMeshPath);
                if (_navMeshPath.status == NavMeshPathStatus.PathComplete) isCorrectPoint = true;
            }
            
            SelfMob.Value.Run(_randomPoint);
        }
    }
}