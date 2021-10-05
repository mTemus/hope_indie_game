using System.Collections.Generic;
using System.Linq;
using HopeMain.Code.AI.Villagers.StimulusSystem;
using UnityEngine;

namespace HopeMain.Code.AI.Villagers.Brain
{
    public class PerceptionLayer : BrainLayer
    {
        private readonly List<Stimulus> stimuli = new List<Stimulus>();

        private float delayDecrement;

        public override void Initialize(Brain brain)
        {
            delayDecrement = Time.deltaTime;
        }

        private void ProcessStimulus(Stimulus stimulus)
        {
            switch (stimulus.StimulusType) {
                
            }
        }

        public void ManualUpdate()
        {
            if (stimuli.Count <= 0) return;

            foreach (Stimulus stimulus in stimuli) {
                if (stimulus.Delay > 0) 
                    stimulus.Delay -= delayDecrement;
                else {
                    ProcessStimulus(stimulus);
                    stimulus.Processed = true;
                }
            }

            List<Stimulus> tmpMessages = new List<Stimulus>(stimuli);
            
            foreach (Stimulus stimulus in tmpMessages
                .Where(stimulus => stimulus.Processed)) { stimuli.Remove(stimulus); }
        }
        
        public void ReceiveStimulusMessage(Stimulus message)
        {
            stimuli.Add(message);
        }
    }
}
