namespace _Prototype.Code.AI.Villagers.StimulusSystem
{
    /// <summary>
    /// 
    /// </summary>
    public enum StimulusType
    {
        
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class Stimulus
    {
        private readonly string _sender;
        private readonly StimulusType _stimulusType;
        private readonly StimulusData _data;
        
        public float Delay { get; set; }
        
        public bool Processed { get; set; }

        public Stimulus(string sender, StimulusType stimulusType, StimulusData data, float delay = 0f)
        {
            _sender = sender;
            _stimulusType = stimulusType;
            _data = data;
            Delay = delay;
            
            Processed = false;
        }

        public string Sender => _sender;
        public StimulusType StimulusType => _stimulusType;
        public StimulusData Data => _data;
    }
}
