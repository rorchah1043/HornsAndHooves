using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MainCamera playerCamera;
    [SerializeField] private Character leaderCharacter;
    [SerializeField] private Character followerCharacter;

    [SerializeField] private float followerSideOffset;
    [SerializeField] private float followerBackOffset;

    private Vector3 _forceDirection;

    private void Start()
    {
        playerCamera.SetLookTarget(leaderCharacter.gameObject);
        leaderCharacter.SetPlayerControlled(true);
        followerCharacter.SetPlayerControlled(false);
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
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        _forceDirection = new Vector3(value.x, 0, value.y);
    }

    public void OnChangeCharacter()
    {
        (leaderCharacter, followerCharacter) = (followerCharacter, leaderCharacter);
        playerCamera.SetLookTarget(leaderCharacter.gameObject);
        leaderCharacter.SetPlayerControlled(true);
        followerCharacter.SetPlayerControlled(false);
    }
}