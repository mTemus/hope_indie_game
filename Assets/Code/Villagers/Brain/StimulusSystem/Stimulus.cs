namespace Code.Villagers.Brain.StimulusSystem
{
    public enum StimulusType
    {
        
    }
    
    public class Stimulus
    {
        private readonly string sender;
        private readonly StimulusType stimulusType;
        private readonly StimulusData data;
        
        public float Delay { get; set; }
        
        public bool Processed { get; set; }

        public Stimulus(string sender, StimulusType stimulusType, StimulusData data, float delay = 0f)
        {
            this.sender = sender;
            this.stimulusType = stimulusType;
            this.data = data;
            Delay = delay;
            
            Processed = false;
        }

        public string Sender => sender;
        public StimulusType StimulusType => stimulusType;
        public StimulusData Data => data;
    }
}
