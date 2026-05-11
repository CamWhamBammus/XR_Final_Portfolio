using Oculus.Interaction;
using UnityEngine;

public class AnimalGrabbedAnimTryTwo : MonoBehaviour
{
    private bool IsGrabbed;
    public Animator animator;

    public void OnGrabbed()
    {
        IsGrabbed = true;
        animator.SetBool("pickedup", IsGrabbed);
    }

    public void OnReleased()
    {
        IsGrabbed = false;
        animator.SetBool("pickedup", IsGrabbed);
    }

}
