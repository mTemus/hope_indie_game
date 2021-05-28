using System;
using UnityEngine;

namespace Code.AI.VillagerBrain.StimulusSystem
{
    public class StimulusData
    {
        private readonly Type type;
        private readonly object stimulusObject;

        private readonly Type stimulusStateType;
        private readonly int stimulusState;
        
        private readonly int stimulusInt;
        private readonly float stimulusFloat;
        private readonly string stimulusString;
        private readonly GameObject stimulusGameObject;

        public StimulusData(Type type, object stimulusObject)
        {
            this.type = type;
            this.stimulusObject = stimulusObject;
        }

        public StimulusData(Type stimulusStateType, int stimulusState)
        {
            this.stimulusStateType = stimulusStateType;
            this.stimulusState = stimulusState;
        }

        public StimulusData(int stimulusInt)
        {
            this.stimulusInt = stimulusInt;
        }

        public StimulusData(float stimulusFloat)
        {
            this.stimulusFloat = stimulusFloat;
        }

        public StimulusData(string stimulusString)
        {
            this.stimulusString = stimulusString;
        }

        public StimulusData(GameObject stimulusGameObject)
        {
            this.stimulusGameObject = stimulusGameObject;
        }

        public Type Type => type;
        public object StimulusObject => stimulusObject;
        public Type StimulusStateType => stimulusStateType;
        public int StimulusState => stimulusState;
        public int StimulusInt => stimulusInt;
        public float StimulusFloat => stimulusFloat;
        public string StimulusString => stimulusString;
        public GameObject StimulusGameObject => stimulusGameObject;
    }
}
