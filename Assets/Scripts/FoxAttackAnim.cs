using System.Collections;
using UnityEngine;

public class FoxAttackAnim : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(PlayAttack());
    }

    void Start() { }

    void Update() { }

    private IEnumerator PlayAttack()
    {
        animator.SetBool("fox_attack_b", true);
        yield return new WaitForSeconds(10f);
        animator.SetBool("fox_attack_b", false);
    }
}
