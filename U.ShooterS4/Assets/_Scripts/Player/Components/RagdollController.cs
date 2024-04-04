using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    private Collider playerCollider; 
    private List<Rigidbody> bonesRigidbodies;
    private Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerCollider = GetComponentInParent<Collider>();
        
        bonesRigidbodies = GetComponentsInChildren<Rigidbody>().ToList();
        DisableRagdoll();
    }
    
    public void EnableRagdoll()
    {
        playerCollider.enabled = false;
        animator.enabled = false;
        for(int i = 0; i < bonesRigidbodies.Count; i++)
        {
            bonesRigidbodies[i].isKinematic = false;
        }
    }
    
    private void DisableRagdoll()
    {
        for(int i = 0; i < bonesRigidbodies.Count; i++)
        {
            bonesRigidbodies[i].isKinematic = true;
        }
    }
}
