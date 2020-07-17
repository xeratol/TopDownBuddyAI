using UnityEngine;

public class BuddyManager : MonoBehaviour
{
    [SerializeField]
    private BuddySimpleFollow _followBehavior = null;

    [SerializeField]
    private BuddyBattleBehavior _battleBehavior = null;

    void Start()
    {
        Debug.Assert(_followBehavior, "Follow Behavior not set", this);
        Debug.Assert(_battleBehavior, "Battle Behavior not set", this);

        EnterFollowMode();
    }

    public void EnterBattleMode()
    {
        _followBehavior.enabled = false;
        _battleBehavior.enabled = true;
    }

    public void EnterFollowMode()
    {
        _followBehavior.enabled = true;
        _battleBehavior.enabled = false;
    }

    private void Update()
    {
        if (EnemyAI.Instances != null && EnemyAI.Instances.Count > 0)
        {
            EnterBattleMode();
        }
        else
        {
            EnterFollowMode();
        }
    }
}
