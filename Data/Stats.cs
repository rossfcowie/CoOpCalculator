namespace CoOpCalculator.Data
{
     
    public class Stats()
    {
        public byte[] collection { get; set; } = new byte[90];
        public String getCardBacks(byte i)
        {
            switch (i)
            {
                case <= 0:
                    return "";
                case > 0 and <= 7:
                    return "B";
                case > 7 and <= 14:
                    return "S";
                case > 14 and <= 21:
                    return "BS";
                case > 21 and <= 28:
                    return "BG";
                case > 28 and <= 35:
                    return "G";
                case > 35 and <= 42:
                    return "GS";
                case > 42 and <= 49:
                    return "BGS";
                default:
                    return "";

            }
        }

        public byte getCardBacks(string i)
        {
            switch (i)
            {
                case "B":
                    return 0;
                case "S":
                    return 7;
                case "BS":
                    return 14;
                case "BG":
                    return 21;
                case "G":
                    return 28;
                case "GS":
                    return 35;
                case "BGS":
                    return 42;
                default:
                    return 0;
            }
        }
        public string getCardStars(byte i)
        {
            switch (i%7)
            {
                case 1:
                    return "R";
                case 2:
                    return "L";
                case 3:
                    return "M";
                case 4:
                    return "RL";
                case 5:
                    return "RM";
                case 6:
                    return "ML";
                case 0:
                    return "RML";
                default:
                    return "";
            }
            return "";
        }
        public byte getCardStars(string i)
        {
            switch (i)
            {
                case "R":
                    return 1;
                case "L":
                    return 2;
                case "M":
                    return 3;
                case "RL":
                    return 4;
                case "RM":
                    return 5;
                case "ML":
                    return 6;
                case "RML":
                    return 7;
                default:
                    return 0;

            }
            return 0;
        }

        public Boolean getCard(int i, string level)
        {
            byte b = (byte)collection.GetValue(i - 10);
            string c = getCardBacks(b);
            string s = getCardStars(b);
            if (c.Contains(level))
            {
                return false;
            }
            else
            {
                if (level.Equals("B"))
                {
                    c = "B" + c;
                }
                if (level.Equals("G"))
                {
                    if (c.Contains("B"))
                    {
                        c = c + "G";
                    }
                    else
                    {

                        if (c.Contains("S"))
                        {
                            c = "G" + c;
                        }
                        else
                        {
                            c = "G" + c;
                        }

                    }
                }
                if (level.Equals("S"))
                {
                    c = c + "S";
                }
                if (c.Length >= 3)
                {
                    c = "BGS";
                }
                collection.SetValue((byte)(getCardBacks(c) + getCardStars(s)), i-10);
                return true;
            }
        }

        public Boolean getStar(int i, string gm)
        {
            byte b = (byte)collection.GetValue(i-10);
            string s = getCardBacks(b);
            string c = getCardStars(b);
            if (c.Contains(gm))
            {
                return false;
            }
            else
            {
                if (gm.Equals("R"))
                {
                    c = "R" + c;
                }
                if (gm.Equals("M"))
                {
                    if ( c.Contains("R"))
                {
                    c = c + "M";
                }
                else
                {

                    if (c.Contains("L"))
                    {
                        c = "M" + c;
                    }
                    else
                    {
                            c = "M" + c;
                        }

                    }
                }
                if (gm.Equals("L"))
                {
                    c = c + "L";
                }
                if (c.Length >= 3)
                {
                    c = "RML";
                }
                collection.SetValue((byte)(getCardBacks(s) + getCardStars(c)), i - 10);
                return true;
            }
        }
    }
}
