namespace CoOpCalculator.Data
{
     
    public class Stats
    {
        public int SuccessCount { get; set; } = 0;
        public int fastest { get; set; } = 0;
        public List<string> collection = new List<string>();
        public Stats()
        {
            SuccessCount = 0;
            fastest = 0;
            for(int i = 10; i < 100; i++)
            {
                collection.Add("");
            }
        }
        public Boolean markdone(int i, int gm)
        {
            if (collection[i].Contains(gm.ToString()))
            {
                return false;
            }
            else
            {
                collection[i] += (gm.ToString());
                return true;
            }
        }
    }
}
