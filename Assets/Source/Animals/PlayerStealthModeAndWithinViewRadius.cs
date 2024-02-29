using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using StarterAssets;
using UnityEngine;

public class PlayerStealthModeAndWithinViewRadius : Conditional
{
    public SharedPlayer Player;
    public SharedBool IsAttack;
    public float angle = 120;

    public override TaskStatus OnUpdate()
    {
        if (Player.Value.GetComponent<StarterAssetsInputs>().Stealth == true && IsAttack.Value == false)

        {
            Vector3 directionToTarget = (Player.Value.transform.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Failure;
            }
        }
        else
        {
            return TaskStatus.Success;
        }
    }
}