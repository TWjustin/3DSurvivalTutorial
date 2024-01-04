using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{
    public Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && 
            InventorySystem.Instance.isOpen == false && 
            CraftingSystem.Instance.isOpen == false && 
            SelectionManager.Instance.handIsVisible == false &&
            !ConstructionManager.Instance.inConstructionMode)
        {
            StartCoroutine(SwingSoundDelay());
            animator.SetTrigger("hit");
        }
    }

    public void GetHit()
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;
        if (selectedTree != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);
            selectedTree.GetComponent<ChoppableTree>().GetHit();
        }
    }

    IEnumerator SwingSoundDelay()
    {
        yield return new WaitForSeconds(0.2f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);
    }
}
