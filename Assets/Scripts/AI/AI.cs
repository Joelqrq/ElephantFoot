#pragma warning disable CS0649
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using JoelQ.Helper;

public class AI : MonoBehaviour, IDamageable {
    [SerializeField] protected AIData data = null;
    public AIData GetData => data;
    protected NavMeshAgent agent;
    private Transform target;
    protected float health;

    public Action OnDeathEvent;
    public event Action<AI> ReturnToPoolEvent;
    [SerializeField] private Animator animator;
    [SerializeField] private Vector3 attackOffset;
    [SerializeField] private Vector3 attackSize;
    [SerializeField] private float attackRate;
    [SerializeField] private LayerMask sightMask;
    [SerializeField] private LayerMask attackMask;
    [SerializeField] private AudioClip[] attackAudios;
    [SerializeField] private ParticleSystem[] attackParticles;
    private ParticleSystem.MainModule attackModule;
    private AudioSource source;
    private Coroutine attackCoroutine;
    private AIState state;

    private void Start() {
        data = Instantiate(data);
        agent = GetComponent<NavMeshAgent>();
        agent.acceleration = data.Acceleration;
        agent.speed = data.MoveSpeed;
        agent.angularSpeed = data.RotateSpeed;
        agent.stoppingDistance = data.Range;
        health = data.Health;

        //particle
        attackModule = attackParticles[0].main;

        //sound
        source = GetComponent<AudioSource>();
    }

    public void Spawn(Vector3 start, Transform target) {
        if (start == null || target == null) {
            Debug.LogWarning("Invalid start/target given!");
            return;
        }
        transform.position = start;
        this.target = target;
    }

    private void Update() {

        animator.SetFloat("velocity", agent.desiredVelocity.sqrMagnitude);

        if (target == null)
            return;

        agent.transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        agent.SetDestination(target.position);
    }

    private void FixedUpdate() {
        if (state == AIState.Search) {
            Search();
        } else if (state == AIState.Attack) {
            Attack();
        }
    }

    public void OnDamaged(float damage) {
        health = 0f;
        if (health <= 0f)
            OnDeath();
    }

    public void OnDeath() {
        if (attackCoroutine != null) {
            StopCoroutine(attackCoroutine);
        }
        //Particles
        foreach (ParticleSystem particle in attackParticles) {
            particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        //animation
        animator.SetBool("death", true);
        target = null;
        health = data.Health;
        OnDeathEvent?.Invoke();
        if (ReturnToPoolEvent == null)
            Debug.LogWarning("No methods attached to returntopoolEvent!");
        else
            ReturnToPoolEvent.Invoke(this);
    }

    private void Search() {

        //In range
        if ((agent.transform.position - target.position).sqrMagnitude < data.Range * data.Range) {
            RaycastHit hit;
            if (Physics.BoxCast(transform.position + attackOffset, attackSize, transform.forward, out hit, Quaternion.identity, data.Range, sightMask)) {
                Debug.Log($"Saw {hit.collider.name} in search!");
                if (hit.collider != null && hit.collider.gameObject.layer.CompareLayer(attackMask)) {
                    Debug.Log($"Found player!");
                    state = AIState.Attack;
                }
            }
        }
    }

    private void Attack() {

        RaycastHit hit;
        if (Physics.BoxCast(transform.position + attackOffset, attackSize, transform.forward, out hit, Quaternion.identity, data.Range, attackMask)) {
            //animation
            animator.SetBool("attack", true);

            Debug.Log($"Attack {hit.collider.name}!");
            //Damage
            if (attackCoroutine == null) {
                attackCoroutine = StartCoroutine(AttackCoroutine(hit.collider));
            }
            //Sound
            if (!source.isPlaying) {
                source.clip = attackAudios[UnityEngine.Random.Range(0, attackAudios.Length)];
                source.Play();
            }
            //Particles
            //set particle range
            float distance = (transform.position + attackOffset - hit.point).magnitude;
            attackModule.startSpeed = distance / attackModule.startLifetime.constant;
            foreach (ParticleSystem particle in attackParticles) {
                particle.Play(true);
            }
        } else {
            Debug.Log("Switch to search state");
            if (attackCoroutine != null) {
                StopCoroutine(attackCoroutine);
            }
            //Particles
            foreach (ParticleSystem particle in attackParticles) {
                particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
            //animation
            animator.SetBool("attack", false);
            //sound
            if (!source.isPlaying) {
                source.Stop();
            }
            state = AIState.Search;
        }
    }

    private IEnumerator AttackCoroutine(Collider hit) {
        hit.GetComponent<IDamageable>()?.OnDamaged(data.Damage);
        yield return new WaitForSeconds(attackRate);
        attackCoroutine = null;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position + attackOffset, attackSize);
    }

    private enum AIState {
        Search,
        Attack
    }
}
