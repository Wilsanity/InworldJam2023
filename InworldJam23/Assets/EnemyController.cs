using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{ 

    public float visionRadius;
    public float attackRange;
    public float attackSpeed;
    public float _attackDeltaTime;
    public LayerMask playerLayerMask;
    public LayerMask groundLayerMask;

    public Transform target;
    public NavMeshAgent agent;
    public Animator animator;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (target == null)
        {
            var colliders = Physics.OverlapSphere(transform.position, visionRadius, playerLayerMask);

            if (colliders.Length > 0)
            {
                Debug.DrawLine(transform.position, colliders[0].transform.position, Color.white, 2f);
                target = colliders[0].transform;
            }

            return;
        }

        if(Mathf.Abs((target.position - transform.position).magnitude) > attackRange)
        {            
            if(NavMesh.SamplePosition(target.position, out NavMeshHit hit, 10f, groundLayerMask))
            {
                agent.isStopped = false;
                animator.SetFloat("Speed", 1);
                agent.SetDestination(hit.position);
            }
        }
        else
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            animator.SetFloat("Speed", 0);
            Attack();
        }
    }

    void Attack()
    {
        if(Time.time > _attackDeltaTime)
        {
            EntityStats stats = GetComponent<EntityStats>();
            stats.stats.TryGetValue(Stats.Strength, out int damage);
            target.GetComponent<Damageable>().DealDamage(damage);
            _attackDeltaTime = Time.time + 1f/attackSpeed;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Time.time > _attackDeltaTime ? Color.yellow : Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }
}
