using System;
using UnityEngine;

[Serializable]
public class ChoiceData
{
    [SerializeField] private string _text;
    [SerializeField] private int _beatId;
    [SerializeField] private bool beginGame;
    [SerializeField] private bool endGame;
    [SerializeField] private bool requiresGoal;
    [SerializeField] private bool changesLevel;
    [SerializeField] private string levelName;
    [SerializeField] private bool adjustTextAlignment;
    [SerializeField] private bool onLaptop;

    public string DisplayText { get { return _text; } }
    public int NextID { get { return _beatId; } }
    public bool BeginGame { get { return beginGame; } }
    public bool EndGame { get { return endGame; } }
    public bool RequiresGoal { get { return requiresGoal; } }
    public bool ChangesLevel { get { return changesLevel; } }
    public string LevelName { get { return levelName; } }
    public bool AdjustTextAlignment { get { return adjustTextAlignment; } }
    public bool OnLaptop { get { return onLaptop; } }
}
