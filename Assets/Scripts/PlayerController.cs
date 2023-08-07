using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MainCamera playerCamera;
    [SerializeField] private Character leaderCharacter;
    [SerializeField] private Character followerCharacter;

    [SerializeField] private float followerSideOffset;
    [SerializeField] private float followerBackOffset;

    [SerializeField] private Animator leaderAnimator;
    [SerializeField] private Animator followerAnimator;

    private int _velocityXHash;
    private int _velocityZHash;
    private float t = 0;

    private Vector3 _forceDirection;

    private NavMeshAgent _followerNavMeshAgent;

    ICanAttack _leaderAttack;
    ICanAttack _followeAttack;

    InteractScript _interactLeadScript;
    InteractScript _interactFollowScript;


    private void Start()
    {
        _interactLeadScript = leaderCharacter.GetComponent<InteractScript>();
        _interactFollowScript = followerCharacter.GetComponent<InteractScript>();
        _leaderAttack = leaderCharacter.GetComponent<ICanAttack>();
        _followeAttack = followerCharacter.GetComponent<ICanAttack>();

        _velocityXHash = Animator.StringToHash("VelocityX");
        _velocityZHash = Animator.StringToHash("VelocityZ");

        playerCamera.SetLookTarget(leaderCharacter.gameObject);
        leaderCharacter.SetPlayerControlled(true);
        followerCharacter.SetPlayerControlled(false);
        _followerNavMeshAgent = followerCharacter.GetNavMeshAgent();

    }

    private void Update()
    {
        leaderCharacter.SetPlayerMoveDirection(Quaternion.Euler(0, playerCamera.transform.rotation.eulerAngles.y, 0) *
                                               _forceDirection);

        var leaderTransform = leaderCharacter.transform;
        var leaderRotation = leaderCharacter.GetTargetRotation();
        var leaderPosition = leaderTransform.position;
        var followerTargetPosition1 =
            leaderPosition + leaderRotation * new Vector3(followerSideOffset, 0, -followerBackOffset);
        var followerTargetPosition2 =
            leaderPosition + leaderRotation * new Vector3(-followerSideOffset, 0, -followerBackOffset);

        var followerPosition = followerCharacter.transform.position;
        followerCharacter.SetAIDestination(
            Vector3.Distance(followerPosition, followerTargetPosition1) <
            Vector3.Distance(followerPosition, followerTargetPosition2)
                ? followerTargetPosition1
                : followerTargetPosition2);


        if (_followerNavMeshAgent.velocity != Vector3.zero)
        {
            followerAnimator.SetFloat(_velocityXHash, Mathf.Lerp(0,1,t));
            t += 100 * Time.deltaTime *Time.deltaTime;

        }
        else if (_followerNavMeshAgent.velocity.magnitude == 0)
        {
            followerAnimator.SetFloat(_velocityXHash, 0);
        }
        else
        {
            t -= 10 * Time.deltaTime * Time.deltaTime;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();

        _forceDirection = new Vector3(value.x, 0, value.y);
        
        leaderAnimator.SetFloat(_velocityXHash,value.x);
        leaderAnimator.SetFloat(_velocityZHash, Mathf.Abs(value.y));
    }

    public void OnChangeCharacter()
    {
        (leaderCharacter, followerCharacter) = (followerCharacter, leaderCharacter);
        (leaderAnimator, followerAnimator) = (followerAnimator, leaderAnimator);
        (_leaderAttack, _followeAttack) = (_followeAttack, _leaderAttack);
        (_interactLeadScript, _interactFollowScript) = (_interactFollowScript, _interactLeadScript);
        _followerNavMeshAgent = followerCharacter.GetNavMeshAgent();
        playerCamera.SetLookTarget(leaderCharacter.gameObject);
        leaderCharacter.SetPlayerControlled(true);
        followerCharacter.SetPlayerControlled(false);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        _leaderAttack.Attack(AttackType.Melee);
    }

    public void OnRangeFire(InputAction.CallbackContext context)
    {
        _leaderAttack.Attack(AttackType.Range);
    }

    public void OnUse(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_interactLeadScript.GetInteractableObject() != null && leaderCharacter.GetComponent<BoxCollider>().enabled)
            {
                _interactLeadScript.GetInteractableObject().InteractableAction(leaderCharacter.gameObject);
            }
        }
    }
}