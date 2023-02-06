using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class BeatData
{
    [SerializeField] private List<ChoiceData> _choices;
    [SerializeField] private string _text;
    [SerializeField] private int _id;
    [SerializeField] private bool _editTime;
    [SerializeField] private float displayTextToScreenTime;
    [SerializeField] private bool instantWipePreviousInfo;


    public List<ChoiceData> Decision { get { return _choices; } }
    public string DisplayText { get { return _text; } }
    public int ID { get { return _id; } }

    public bool EditTime { get { return _editTime; } }

    public float ShortTime { get { return displayTextToScreenTime; } }
    public float LongTime { get; } = 0.1f;
    public bool InstantWipe { get { return instantWipePreviousInfo; } }

}
