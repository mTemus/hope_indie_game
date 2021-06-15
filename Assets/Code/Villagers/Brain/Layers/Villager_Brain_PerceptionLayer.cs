using System.Collections.Generic;
using System.Linq;
using Code.Villagers.Brain.StimulusSystem;
using UnityEngine;

namespace Code.Villagers.Brain.Layers
{
    public class Villager_Brain_PerceptionLayer : BrainLayer
    {
        private readonly List<Stimulus> stimuli = new List<Stimulus>();

        private float delayDecrement;

        public override void Initialize(Villager_Brain villagerBrain)
        {
            delayDecrement = Time.deltaTime;
            base.Initialize(villagerBrain);
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
