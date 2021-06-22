using System.Collections;
using Photon.Pun;
using UnityEngine;
using PlayerElements;

public class PlayerSingle : Player
{

    [SerializeField] public GameObject rollPosition;
    public override void Awake()
    {
        _canMove = true;
        
        _playerAnimator = GetComponent<PlayerAnimator>();
        _playerInput = GameObject.Find("InputConnect").GetComponent<PlayerInput>();
        _playerMoving = new PlayerMoving(_playerInput.joystick,
            GetComponent<CharacterController>());
        
        firstSkill = gameObject.AddComponent<PlayerDirect>();
        secondSkill = gameObject.AddComponent<PlayerSlash>();
        defendSkill = gameObject.AddComponent<DefendSkill>();
        currentSkill = null;

        _playerInput.AddAttackingSkill_1_Listener(FirstAttack);
        _playerInput.AddAttackingSkill_2_Listener(SecondAttack);
        _playerInput.AddDefendingSkillListener(Roll);
    }

    private void FixedUpdate()
    {
        if(PlayerHasSomeBlock())
            return;
        _playerMoving.MoveCharacterIfCanMove(_canMove);
        _playerAnimator.PlayMoveAnimationByInputVector(_playerMoving.GetInputVector());
    }

    #region Skills

    private void FirstAttack()
    {
        ActivateAttackSkill("1");
    }

    private void SecondAttack()
    {
       ActivateAttackSkill("2");
    }
    #endregion

    private void ActivateAttackSkill(string skillCode)
    {
        PlayerSkill skill;
            
        switch (skillCode)
        {
            case "1":
                skill = secondSkill;
                break;
            case "2":
                skill = firstSkill;
                break;
                 
            default:
                Debug.Log("Wrong skill code");
                return;
        }
            
        skill.Activate();
    }

    public override void OnInvincible()
    {
        base.OnInvincible();
        if (rollPosition.transform.position.z <= 5 && rollPosition.transform.position.z >= -5 &&
            rollPosition.transform.position.x <= 10 && rollPosition.transform.position.x >= -10)
            GetComponent<CharacterController>().enabled = false;
    }

    public override void OffInvincible()
    {
        base.OffInvincible();
        GetComponent<CharacterController>().enabled = true;
    }
}