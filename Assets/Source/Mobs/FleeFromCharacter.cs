using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class FleeFromCharacter : Action
{
    public SharedMob SelfMob;
    public float RandomPointRadius;
    
    private NavMeshPath _navMeshPath;
    private Vector3 _random_point;
    private Vector3 Currentpoint;

    public override void OnStart()
    {
        _navMeshPath = new NavMeshPath();
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
            bool get_correct_point = false;
            while (!get_correct_point)
            {
                NavMeshHit navmesh_hit;
                NavMesh.SamplePosition(Random.insideUnitSphere * RandomPointRadius + SelfMob.Value.transform.position,
                    out navmesh_hit, RandomPointRadius, NavMesh.AllAreas);
                _random_point = navmesh_hit.position;
                SelfMob.Value.Agent.CalculatePath(_random_point, _navMeshPath);
                if (_navMeshPath.status == NavMeshPathStatus.PathComplete) get_correct_point = true;
            }

            SelfMob.Value.Run(_random_point);
        }
    }
}