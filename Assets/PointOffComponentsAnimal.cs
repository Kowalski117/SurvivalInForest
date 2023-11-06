using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class PointOffComponentsAnimal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Animals>())
        {
            other.GetComponent<TrueAndFalseComponentsDistanceToPlayer>().ComponentsEnabledTrue();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Animals>())
        {          
            other.GetComponent<TrueAndFalseComponentsDistanceToPlayer>().ComponentsEnabledFalse();
        }
    }
}
