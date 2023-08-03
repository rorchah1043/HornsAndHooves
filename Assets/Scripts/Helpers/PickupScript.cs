using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour,IInteractable
{
    private Outline _outline;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.OutlineWidth = 0;
    }

    public void OnHover()
    {
        _outline.OutlineWidth = 2;
    }

    public void OnHoverExit()
    {
        _outline.OutlineWidth = 0;
    }

    public void InteractableAction(Vector3 vector)
    {
        Debug.Log("Item Pickup");
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
        
    }
}
