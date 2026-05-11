using UnityEngine;
using System.Collections;

public class ChickenScaredAnim : MonoBehaviour
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
        animator.SetBool("chicken_panic_b", true);
        yield return new WaitForSeconds(10f);
        animator.SetBool("chicken_panic_b", false);
    }
}
