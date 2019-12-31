#pragma warning disable CS0649
using System.Collections;
using UnityEngine;
using JoelQ.Helper;

public class CharacterAttack : MonoBehaviour {
    [SerializeField] private Animator animator;
    [SerializeField] private Vector3 laserOffset;
    [SerializeField] private Vector3 laserSize;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCD;
    [SerializeField] private LayerMask attackMask;
    [SerializeField] private LayerMask blockingMask;
    [SerializeField] private AudioClip laserAudio;
    [SerializeField] private AudioClip[] absorbAudios;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private ParticleSystem[] laserParticles;
    [SerializeField] private float fadeTime;
    private Coroutine attackCoroutine;
    private AudioSource audioSource;

    private void Start() {
        if (!lineRenderer) {
            Debug.Log("Attack line renderer is missing!");
        }
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && attackCoroutine == null) {
            audioSource.PlayOneShot(laserAudio, 0.5f);
            attackCoroutine = StartCoroutine(TimedAttack());
        }
    }

    private IEnumerator TimedAttack() {
        Attack();
        yield return new WaitForSeconds(attackCD);
        attackCoroutine = null;
    }

    private void Attack() {

        //Check whether anything is blocking the laser.
        RaycastHit sight;
        Physics.BoxCast(transform.position + laserOffset, laserSize / 2, transform.forward, out sight, Quaternion.identity, attackRange, blockingMask, QueryTriggerInteraction.Ignore);
        if (sight.collider != null) {
            if (sight.collider.gameObject.layer.CompareLayer(attackMask)) {

                StartCoroutine(AttackRenderer(transform.position + laserOffset, (transform.position + laserOffset) + (transform.forward * attackRange)));
                RaycastHit[] hits;
                hits = Physics.BoxCastAll(transform.position + laserOffset, laserSize / 2, transform.forward, Quaternion.identity, attackRange, attackMask);
                foreach (RaycastHit hit in hits) {
                    Debug.Log(hit.collider.name);
                }
                if (hits.Length != 0) {
                    animator.SetTrigger("Attack");
                    foreach (RaycastHit hit in hits) {
                        IDamageable iDamage = hit.collider.GetComponent<IDamageable>();
                        iDamage?.OnDeath();
                    }
                }
            } else {
                StartCoroutine(AttackRenderer(transform.position + laserOffset, sight.point));
                Debug.Log($"Blocked by {sight.collider.name}");
            }
        } else {
            StartCoroutine(AttackRenderer(transform.position + laserOffset, transform.position + laserOffset + (transform.forward * attackRange)));
        }
    }

    private IEnumerator AttackRenderer(Vector3 startPos, Vector3 endPos) {
        foreach (ParticleSystem particle in laserParticles) {
            particle.gameObject.SetActive(true);
        }
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
        float tempFade = 0f;
        Color color = lineRenderer.startColor;
        while (tempFade < fadeTime) {
            tempFade += Time.deltaTime;
            yield return null;
        }
        lineRenderer.enabled = false;
        foreach (ParticleSystem particle in laserParticles) {
            particle.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.layer.CompareLayer(attackMask)) {
            IDamageable iDamage = other.collider.GetComponent<IDamageable>();
            audioSource.PlayOneShot(absorbAudios[Random.Range(0, absorbAudios.Length)], 0.5f);
            iDamage?.OnDamaged(100f);
        }
    }
}