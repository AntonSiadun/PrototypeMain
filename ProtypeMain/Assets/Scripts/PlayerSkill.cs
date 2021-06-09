using System.Collections;
using NewScripts;
using UnityEngine;

public abstract class PlayerSkill : MonoBehaviour
{
    public Player player;
    public float duration;
    public PlayerAnimator animator;
    public int id;

    public abstract void Activate();

    public abstract void Animate();

    public abstract void Move();
}
public interface IDealDamage
{
    int DealDamage();
}

public class BlockedSkill : PlayerSkill
{
    public void Setup(Player playerInstance)
    {
        player = playerInstance;
        animator = player.playerAnimator;
    }
        
    public override void Activate()
    {
        if(player.PlayerHasSomeBlock())
            return;
            
        Animate();
        Move();

        StartCoroutine(BlockMovingForTime(duration));
    }

    public override void Animate(){}

    public override void Move(){}
        
    protected IEnumerator BlockMovingForTime(float time)
    {
        player.canMove = false;
        yield return new WaitForSeconds(time);
        player.canMove = true;
    }
}
    
public class AttackSkill : BlockedSkill, IDealDamage
{
    protected int Damage;
        
    public override void Activate()
    {
        if(player.PlayerHasSomeBlock())
            return;
        
        Animate();
        Move();

        StartCoroutine(BlockMovingForTime(duration));
        StartCoroutine(SetSkillForTime());
    }
    public override void Move(){}
        
    public int DealDamage()
    {
        return Damage;
    }
        
    IEnumerator SetSkillForTime()
    {
        player.SetCurrentSkill(this);
        yield return new WaitForSeconds(duration);
        player.SetCurrentSkill(null);
    }
}
    
public class DefendSkill : BlockedSkill
{
    private void Awake()
    {
        duration = 2f;
    }

    public override void Animate()
    {
        animator.Roll(player.playerMoving.GetInput());
    }
}
    
//SubSkills
public class PlayerSlash : AttackSkill
{
    private void Awake()
    {
        Damage = 2;
        id = 1;
        duration = 1.85f;
    }
    public override void Animate()
    {
        animator.SlashAttack();
    }
}
public class PlayerDirect : AttackSkill
{

    private void Awake()
    {
        Damage = 1;
        id = 2;
        duration = 2.33f;
    }
    public override void Animate()
    {
        animator.DirectAttack();
    }
}