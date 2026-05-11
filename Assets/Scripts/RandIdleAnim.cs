using System.Collections;
using UnityEngine;

public class RandIdleAnim : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("No Animator component found");
            return;
        }

        int randomIdleIndex = Random.Range(0, 2); // 0 inclusive, 2 exclusive
        animator.SetInteger("idleIndex", randomIdleIndex);
        Debug.Log(randomIdleIndex);
        //Debug.Log($"Idle index set to: {randomIdleIndex}");
    }

    void Start()
    {
        StartCoroutine(IdleOffset());
    }

    private IEnumerator IdleOffset()
    {
        yield return null;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float randomStartTime = Random.Range(0f, 1f);
        animator.Play(stateInfo.fullPathHash, 0, randomStartTime);
    }
}