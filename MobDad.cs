using UnityEngine;

public class MobDad : Character {

    public int MAX_JUMPS;

    override public void Start()
    {
        base.Start();
        CharacterName = "Mob_Dad";

        Height = 2;
        MaxHealth = 200;
        MoveSpeed = 15;
        fighter.JumpVelocity = 20;
        fighter.MaxJumps = MAX_JUMPS;
        JumpSize = 15;
    }
}