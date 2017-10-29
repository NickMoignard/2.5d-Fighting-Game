using UnityEngine;

public class Character : MonoBehaviour
{
    public string characterName;
    public string CharacterName
    {
        get { return characterName; }
        set { characterName = value; }    
    }

    public float height;
    public float Height
    {
        get { return height; }
        set { height = value; }
    }

    // Maybye find a way to have this variable in the fighter class
    public int MaxHealth
    {
        get { return fighter.maximumHealth; }
        set { fighter.maximumHealth = value; }
    }


    public float moveSpeed;
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    public float jumpSize;
    public float JumpSize
    {
        get { return jumpSize; }
        set { jumpSize = value; }
    }

    public Fighter fighter;

    public virtual void Start()
    {
        fighter = GetComponent<Fighter>();
    }


}


    