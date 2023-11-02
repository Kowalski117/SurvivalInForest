using UnityEngine;
using UnityEngine.AI;

public abstract class SetAnimation : MonoBehaviour
{
    NavMeshAgent _agent;
    protected Animator Animator;
    private string _speed = "Speed";
    private string _death = "Death";
    private string _eat = "Eat";
    private string _sleep = "Sleep";
    private string _sit = "Sit";
    private string _attacks ="Attack";
    private string _howl ="Howl";
    private static int _maxAttacksAnimation = 3;
    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_agent.velocity.magnitude > 0)
            TurnOffAnimations();

        Animator.SetFloat(_speed, (_agent.velocity.magnitude / 10)); 
    }
    
    public void Attack()
    {
        TurnOffAnimations();
        string attack = _attacks + Random.Range(1,_maxAttacksAnimation+1);
        Animator.SetTrigger(attack);
    }

    public void Death()
    {
        TurnOffAnimations();
        Animator.SetBool(_death, true);
    }

    public virtual void TurnOffAnimations()
    {
        Animator.SetBool(_sit,false);
        Animator.SetBool(_sleep, false);
        Animator.SetBool(_eat, false); 
        Animator.SetBool(_howl, false);
    }

}
