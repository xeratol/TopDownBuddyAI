using UnityEngine;
using UnityEngine.UI;

public class BuddyChat : MonoBehaviour
{
    [SerializeField]
    private BuddyBattleBehavior _battleBehavior = null;

    [SerializeField]
    private BuddySimpleFollow _followBehavior = null;

    [SerializeField]
    private Image _buddyIcon = null;
    [SerializeField]
    private Image _buddyChatBkgd = null;
    [SerializeField]
    private Text _buddyChatText = null;

    private Color _buddyIconColor = Color.white;
    private Color _buddyIconColorFaded = Color.white;
    private Color _buddyChatBkgdColor = Color.white;
    private Color _buddyChatBkgdColorFaded = Color.white;
    private Color _buddyChatTextColor = Color.white;
    private Color _buddyChatTextColorFaded = Color.white;

    [SerializeField, Min(1.0f)]
    private float _showDuration = 3.0f;
    [SerializeField, Min(0.0f)]
    private float _fadeDuration = 3.0f;

    private float _timeLastShown = float.MinValue;

    [SerializeField]
    private string[] _battleHidingTexts = null;

    [SerializeField]
    private string[] _battleHealingTexts = null;

    [SerializeField]
    private string[] _battleAttackingTexts = null;

    [SerializeField]
    private string[] _followFollowTexts = null;

    [SerializeField]
    private string[] _followWanderTexts = null;

    [SerializeField]
    private string[] _followLostTexts = null;

    void Start()
    {
        Debug.Assert(_buddyIcon, "Buddy Icon not set", this);
        Debug.Assert(_buddyChatBkgd, "Buddy Chat Bkgd not set", this);
        Debug.Assert(_buddyChatText, "Buddy Chat Text not set", this);

        _buddyIconColor = _buddyIcon.color;
        _buddyIconColorFaded = _buddyIconColor;
        _buddyIconColorFaded.a = 0;
        _buddyIcon.color = _buddyIconColorFaded;

        _buddyChatBkgdColor = _buddyChatBkgd.color;
        _buddyChatBkgdColorFaded = _buddyChatBkgdColor;
        _buddyChatBkgdColorFaded.a = 0;
        _buddyChatBkgd.color = _buddyChatBkgdColorFaded;

        _buddyChatTextColor = _buddyChatText.color;
        _buddyChatTextColorFaded = _buddyChatTextColor;
        _buddyChatTextColorFaded.a = 0;
        _buddyChatText.color = _buddyChatTextColorFaded;

        if (_battleBehavior)
        {
            _battleBehavior.OnStateChangeListener += OnBattleStateChanged;
        }

        if (_followBehavior)
        {
            _followBehavior.OnStateChangeListener += OnFollowStateChanged;
        }
    }

    void OnDestroy()
    {
        if (_battleBehavior)
        {
            _battleBehavior.OnStateChangeListener -= OnBattleStateChanged;
        }

        if (_followBehavior)
        {
            _followBehavior.OnStateChangeListener -= OnFollowStateChanged;
        }
    }

    private void OnFollowStateChanged(BuddySimpleFollow.BuddyState state)
    {
        if (Random.Range(0, 2) == 0)
        {
            return;
        }

        switch (state)
        {
            case BuddySimpleFollow.BuddyState.Follow:
                Show(SelectText(_followFollowTexts));
                break;
            case BuddySimpleFollow.BuddyState.Wander:
                Show(SelectText(_followWanderTexts));
                break;
            case BuddySimpleFollow.BuddyState.Lost:
                Show(SelectText(_followLostTexts));
                break;
        }
    }

    private void OnBattleStateChanged(BuddyBattleBehavior.BuddyState state)
    {
        if (Random.Range(0, 2) == 0)
        {
            return;
        }

        switch (state)
        {
            case BuddyBattleBehavior.BuddyState.Hiding:
                Show(SelectText(_battleHidingTexts));
                break;
            case BuddyBattleBehavior.BuddyState.Healing:
                Show(SelectText(_battleHealingTexts));
                break;
            case BuddyBattleBehavior.BuddyState.Attacking:
                Show(SelectText(_battleAttackingTexts));
                break;
        }
    }

    void Update()
    {
        if (Time.time > _timeLastShown + _showDuration)
        {
            var fadeTime = Time.time - (_timeLastShown + _showDuration);
            var percent = fadeTime / _fadeDuration;
            Fade(percent);
        }
    }

    void Show(string text)
    {
        if (text.Length == 0)
        {
            return;
        }

        _timeLastShown = Time.time;
        _buddyIcon.color = _buddyIconColor;
        _buddyChatBkgd.color = _buddyChatBkgdColor;
        _buddyChatText.color = _buddyChatTextColor;
        _buddyChatText.text = text;
    }

    void Fade(float percent)
    {
        _buddyIcon.color = Color.Lerp(_buddyIconColor, _buddyIconColorFaded, percent);
        _buddyChatBkgd.color = Color.Lerp(_buddyChatBkgdColor, _buddyChatBkgdColorFaded, percent);
        _buddyChatText.color = Color.Lerp(_buddyChatTextColor, _buddyChatTextColorFaded, percent);
    }

    string SelectText(string[] choices)
    {
        if (choices == null || choices.Length == 0)
        {
            return "";
        }

        var choice = Random.Range(0, choices.Length - 1);
        return choices[choice];
    }
}
