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
    public Health health;

    public CharacterController knockbackController;
    public ImpactReceiver knockbackReceiver;

    private void Start()
    {
        health.OnDeath += HandleDeath;
    }

    private void Update()
    {
        if (knockbackReceiver.impacting)
        {
            agent.isStopped = true;
            knockbackController.enabled = true;
            return;
        }
        else
            knockbackController.enabled = false;

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

            target.GetComponent<MovementController>().AddKnockback(transform.position, 25);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Time.time > _attackDeltaTime ? Color.yellow : Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Weapon"))
        {
            AddKnockback(other.transform.parent.position, 25);
            animator.SetTrigger("Hit");
            GetComponent<Damageable>().DealDamage(1);
        }
    }
    public void AddKnockback(Vector3 impactLocation, int strength)
    {
        Vector3 dir = transform.position - impactLocation;
        dir.Normalize();

        dir.y = 0.5f;

        GetComponent<ImpactReceiver>().AddImpact(dir, strength);
    }

    private void HandleDeath()
    {
        Destroy(gameObject);
    }
}
