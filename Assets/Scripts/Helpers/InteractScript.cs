using UnityEngine;
using UnityEngine.InputSystem;

public class InteractScript : MonoBehaviour
{
    [Header("Вставь номер слоя интерактива")]
    [SerializeField] private int layerNumber;

    private IInteractable _interactable = null;
    int _layerMask;

    private void Start()
    {
        _layerMask = 1 << layerNumber;
    }

    public IInteractable GetInteractableObject()
    {
        return _interactable;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other?.GetComponent<IInteractable>() != null)
        {
            GetVisibleObject();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_interactable != null)
        {
            _interactable.OnHoverExit();
            _interactable = null;
        }
    }

    private void GetVisibleObject()
    {
        RaycastHit hit;
        Transform cameraTransform = Camera.main.transform;
        Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 100.0f, Color.green);

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 100.0f, _layerMask))
        {
            Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 100.0f, Color.red);
            if (hit.collider?.GetComponent<IInteractable>() != null)
            {
                _interactable = hit.collider.GetComponent<IInteractable>();
                _interactable.OnHover();
                return;
            }
        }
        if(_interactable != null)
        {
            _interactable.OnHoverExit();
            _interactable = null;
        }
        
    }
}
