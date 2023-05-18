using UnityEngine;
using UnityEngine.AI;

public class AnimationAnimals : MonoBehaviour
{
    NavMeshAgent _agent;
    private Animator _animator;
    private string _speed = "Speed";
    private string _death = "Death";
    private string _eat = "Eat";
    private string _sleep = "Sleep";
    private string _sit = "Sit";
    private string _attacks ="Attack";
    private static int _maxAttacksAnimation = 3;


    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_agent.velocity.magnitude > 0)
            TurnOffAnimations();

        _animator.SetFloat(_speed, (_agent.velocity.magnitude / 10)); 
    }

    public void Sit()
    {
        TurnOffAnimations();
        _animator.SetBool(_sit,true);
    }

    public void Sleep()
    {
        
    }

    public void Eat()
    {
        TurnOffAnimations();
        _animator.SetBool(_eat,true);
    }

    public void Attack()
    {
        TurnOffAnimations();
        string attack = _attacks + Random.Range(1,_maxAttacksAnimation);
        Debug.Log(attack);
        _animator.SetTrigger(attack);
    }

    public void Death()
    {
        TurnOffAnimations();
        _animator.SetBool(_death, true);
    }

    private void TurnOffAnimations()
    {
        _animator.SetBool(_eat,false);
        _animator.SetBool(_sit,false);
        _animator.SetBool(_sleep, false);
    }

}
