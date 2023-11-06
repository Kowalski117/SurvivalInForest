using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class CheckingDistansTransformAndObject : Action
{
    [SerializeField] private SharedTransform _object;
    [SerializeField] private float _minDistance = 1f;
    
    public override void OnStart()
    {
        float distance = Vector3.Distance(transform.position, _object.Value.transform.position);
        NavMeshAgent agent;
        if (distance>_minDistance)
        {
            agent = gameObject.GetComponent<NavMeshAgent>();
            agent.enabled = false;
            transform.position = _object.Value.transform.position;
            agent.enabled = true;
        }
    }
}
