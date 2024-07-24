using AbilitySystem;
using AbilitySystem.Authoring;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private int _playerId;
    private Color _playerColor;
    public int PlayerId => _playerId;
    public Color PlayerColor => _playerColor;
    private GameObject _currentCharacter;
    private CharacterMovement CharacterMovement;
    private CharacterAnimation CharacterAnimation;
    private AbilitySystemCharacter AbilitySystemComponent;
    private PlayerCharacterInputs _characterInputs;

    public AbstractAbilityScriptableObject ability;

    public void SetPlayerId(int playerId)
    {
        _playerId = playerId;
    }

    public void SetPlayerColor(Color playerColor)
    {
        _playerColor = playerColor;
    }

    private void Start()
    {
        _characterInputs = new PlayerCharacterInputs();
        
        GameObject character = CharacterManager.instance.GetCharacter(0);
        Possess(character);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Period))
        {
            AbstractAbilitySpec abilitySpec = ability.CreateSpec(AbilitySystemComponent);
            StartCoroutine(abilitySpec.TryActivateAbility());
        }
    }

    private void Possess(GameObject character)
    {
        if(!character)
        {
            Debug.Log("No character to possess");
            return;
        }

        
        if(!character.TryGetComponent<CharacterMovement>(out CharacterMovement outCharacterMovement))
        {
            Debug.Log("Missing movement component");
            return;
        }

        if(!character.TryGetComponent<CharacterAnimation>(out CharacterAnimation outCharacterAnimation))
        {
            Debug.Log("Animation component missing");
            return;
        }

        if(!character.TryGetComponent<AbilitySystemCharacter>(out var outAbilitySystemComponent))
        {
            Debug.Log("Missing ability system component");
            return;
        }

        CharacterMovement = outCharacterMovement;        
        CharacterAnimation = outCharacterAnimation;
        AbilitySystemComponent = outAbilitySystemComponent;

        CharacterMovement.SetInputs(ref _characterInputs);
        CharacterAnimation.SetParams(ref _characterInputs);

        _currentCharacter = character;

        if(_currentCharacter.TryGetComponent<PossesionNotifier>(out PossesionNotifier outPossessionNotifier))
        {
            outPossessionNotifier.OnPosses.Invoke(_playerId, _playerColor);
        }
    }

    private void UnPossess()
    {
        if(_currentCharacter.TryGetComponent<PossesionNotifier>(out PossesionNotifier outPossessionNotifier))
        {
            outPossessionNotifier.OnUnPossess.Invoke();
        }

        PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();
        CharacterMovement.SetInputs(ref characterInputs);
        CharacterAnimation.SetParams(ref characterInputs);
    }

    public void OnNextCharacter(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            UnPossess();
            
            GameObject character = CharacterManager.instance.GetCharacter(0, _currentCharacter);
            Possess(character);
        }
    }

    public void OnPreviousCharacter(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            UnPossess();

            GameObject character = CharacterManager.instance.GetCharacter(-1, _currentCharacter);
            Possess(character);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveDirection = context.ReadValue<Vector2>();

        // Build the CharacterInputs struct
        _characterInputs.MoveAxisForward =  moveDirection.y;
        _characterInputs.MoveAxisRight =  moveDirection.x;

        // Apply inputs to character
        CharacterMovement.SetInputs(ref _characterInputs);
        // Apply inputs to animator
        CharacterAnimation.SetParams(ref _characterInputs);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed) _characterInputs.JumpDown = true;

        // Apply inputs to character
        CharacterMovement.SetInputs(ref _characterInputs);

        // Reset input 
        // This is to force the player to trigger the action again
        _characterInputs.JumpDown = false;
        CharacterMovement.SetInputs(ref _characterInputs);
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if(context.performed) _characterInputs.DashDown = true;

        // Apply inputs to character
        CharacterMovement.SetInputs(ref _characterInputs);

        // Reset input 
        // This is to force the player to trigger the action again
        _characterInputs.DashDown = false;
        CharacterMovement.SetInputs(ref _characterInputs);
    }

    public void OnAbilityOne(InputAction.CallbackContext context)
    {
    }

    public void OnAbilityTwo(InputAction.CallbackContext context)
    {
    }

    public void OnAbiliyThree(InputAction.CallbackContext context)
    {
    }

    public void OnAbilityFour(InputAction.CallbackContext context)
    {
    }
}
