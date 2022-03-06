using System;
using UnityEngine;

namespace _Prototype.Code.AI.Villagers.StimulusSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class StimulusData
    {
        private readonly Type _type;
        private readonly object _stimulusObject;

        private readonly Type _stimulusStateType;
        private readonly int _stimulusState;
        
        private readonly int _stimulusInt;
        private readonly float _stimulusFloat;
        private readonly string _stimulusString;
        private readonly GameObject _stimulusGameObject;

        public StimulusData(Type type, object stimulusObject)
        {
            _type = type;
            _stimulusObject = stimulusObject;
        }

        public StimulusData(Type stimulusStateType, int stimulusState)
        {
            _stimulusStateType = stimulusStateType;
            _stimulusState = stimulusState;
        }

        public StimulusData(int stimulusInt)
        {
            _stimulusInt = stimulusInt;
        }

        public StimulusData(float stimulusFloat)
        {
            _stimulusFloat = stimulusFloat;
        }

        public StimulusData(string stimulusString)
        {
            _stimulusString = stimulusString;
        }

        public StimulusData(GameObject stimulusGameObject)
        {
            _stimulusGameObject = stimulusGameObject;
        }

        public Type Type => _type;
        public object StimulusObject => _stimulusObject;
        public Type StimulusStateType => _stimulusStateType;
        public int StimulusState => _stimulusState;
        public int StimulusInt => _stimulusInt;
        public float StimulusFloat => _stimulusFloat;
        public string StimulusString => _stimulusString;
        public GameObject StimulusGameObject => _stimulusGameObject;
    }
}
