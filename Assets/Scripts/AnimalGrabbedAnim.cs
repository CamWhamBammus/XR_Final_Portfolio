using Oculus.Interaction;
using UnityEngine;

public class AnimalGrabbedAnim : MonoBehaviour
{
    public Animator animator;
    public bool pickedup = false;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("hand")) {
            animator.SetBool("pickedup", true);

            if (CompareTag("fox"))
            {
                Audiomanager.Instance.PlayFoxSound();
            } else if (CompareTag("chicken"))
            {
                Audiomanager.Instance.PlayChickenSound();
            }

            pickedup = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("hand")) {
            animator.SetBool("pickedup", false);

            if (CompareTag("fox"))
            {
                Audiomanager.Instance.PlayFoxSound();
            } else if (CompareTag("chicken"))
            {
                Audiomanager.Instance.PlayChickenSound();
            }

            pickedup = false;
        }
    }
}
