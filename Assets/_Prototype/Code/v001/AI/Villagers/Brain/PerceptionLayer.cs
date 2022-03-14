using System.Collections.Generic;
using System.Linq;
using _Prototype.Code.v001.AI.Villagers.StimulusSystem;
using UnityEngine;

namespace _Prototype.Code.v001.AI.Villagers.Brain
{
    /// <summary>
    /// 
    /// </summary>
    public class PerceptionLayer : BrainLayer
    {
        private readonly List<Stimulus> _stimuli = new List<Stimulus>();

        private float _delayDecrement;

        public override void Initialize(Brain brain)
        {
            _delayDecrement = Time.deltaTime;
        }

        private void ProcessStimulus(Stimulus stimulus)
        {
            switch (stimulus.StimulusType) {
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ManualUpdate()
        {
            if (_stimuli.Count <= 0) return;

            foreach (Stimulus stimulus in _stimuli) {
                if (stimulus.Delay > 0) 
                    stimulus.Delay -= _delayDecrement;
                else {
                    ProcessStimulus(stimulus);
                    stimulus.Processed = true;
                }
            }

            List<Stimulus> tmpMessages = new List<Stimulus>(_stimuli);
            
            foreach (Stimulus stimulus in tmpMessages
                .Where(stimulus => stimulus.Processed)) { _stimuli.Remove(stimulus); }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void ReceiveStimulusMessage(Stimulus message)
        {
            _stimuli.Add(message);
        }
    }
}
