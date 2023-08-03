using System;
using UnityEngine;

public interface IInteractable
{
    public void InteractableAction(GameObject gameObject);
    public void OnHover();
    public void OnHoverExit();
}


