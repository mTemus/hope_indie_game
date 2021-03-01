namespace Code.Utilities
{
    public static class Utilities
    {
        public static int IncrementIdx(int idx, int value, int maxValue)
        {
            if (idx + value > maxValue - 1) 
                return 0;
            if(idx + value < 0) 
                return maxValue - 1;
            
            return idx + value;
        }
    }
}