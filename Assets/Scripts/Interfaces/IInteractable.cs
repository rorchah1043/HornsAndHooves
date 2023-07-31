using System;
using UnityEngine;

public interface IInteractable
{
    public void InteractableAction(Vector3 vector);
    public void OnHover();
    public void OnHoverExit();
}


