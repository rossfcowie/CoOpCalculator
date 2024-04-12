using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System;
using System.Diagnostics;

namespace CoOpCalculator.Data
{
    public class AIGameServer : GameServer
    {
        private int mytarget = 10;
        Random rnd;
        public void start(int gm)
        {
            gamemode = gm; 
            rnd = new Random();
            mytarget = rnd.Next(10, 99);
            switch (gamemode)
            {
                case 1:
                    GoalValue = rnd.Next(10, 99) * mytarget;
                    break;
                case 2:
                    GoalValue = rnd.Next(10, 99) + mytarget;
                    break;
                case 3:
                    GoalValue = rnd.Next(10, 99) - mytarget;
                    break;
            }
            Player1Value = rnd.Next(10, 99);
            Player2Value = rnd.Next(10, 99);
            t = new Stopwatch();
            t.Start();
            ingame = true;
            ThreadPool.QueueUserWorkItem(tick);
        }
        public void tick(object state)
        {
            while (ingame)
            {
                if (t.Elapsed.Seconds - player1Live.Seconds > 10)
                {
                    Player1DC();
                }
                if (t.Elapsed.Seconds - player2Live.Seconds > 8)
                {
                    var min = Math.Max(10, mytarget - Math.Min(0,(30 - t.Elapsed.Seconds)));
                    var max = Math.Min(99, mytarget + Math.Min(0,(30 - t.Elapsed.Seconds)));
                    Player2Value = rnd.Next(min,max);
                    player2Live = t.Elapsed;
                }
                switch (gamemode)
                {
                    case 1:
                        if (Player1Value * Player2Value == GoalValue)
                        {
                            success();
                        }
                        break;
                    case 2:
                        if (Player1Value + Player2Value == GoalValue)
                        {
                            success();
                        }
                        break;
                    case 3:
                        if (Player1Value - Player2Value == GoalValue)
                        {
                            success();
                        }
                        if (Player2Value - Player1Value == GoalValue)
                        {
                            success();
                        }
                        break;
                    case 4:
                        if (Player1Value / Player2Value == GoalValue)
                        {
                            success();
                        }
                        if (Player2Value / Player1Value == GoalValue)
                        {
                            success();
                        }
                        break;
                }


            }
        }
    }
    public class GameServer
    {
        public int Player1Value { get; set; } = 10;
        public int Player2Value { get; set; } = 10;
        public int GoalValue { get; set; } = 1000;
        public Dictionary<TimeSpan,int> Player1Values = new Dictionary<TimeSpan, int>();
        public Dictionary<TimeSpan, int> Player2Values = new Dictionary<TimeSpan, int>();
        public Byte[] player1ID { get; set; }
        public Byte[] player2ID { get; set; }
        public TimeSpan player1Live { get; set; } = TimeSpan.Zero;
        public TimeSpan player2Live { get; set; } = TimeSpan.Zero;
        public Stopwatch t;
        public TimeSpan total = TimeSpan.Zero;
        public bool ingame = false;
        public int gamemode = 1;
        public void start(int gm)
        {
            gamemode = gm;
            Random rnd = new Random();
             switch (gamemode)
            {
                case 1:
                    GoalValue = rnd.Next(10, 99) * rnd.Next(10, 99);
                    break;
                case 2:
                    GoalValue = rnd.Next(10, 99) + rnd.Next(10, 99);
                    break;
                case 3:
                    GoalValue = rnd.Next(10, 99) - rnd.Next(10, 99);
                    break;
            }
            Player1Value = rnd.Next(10, 99);
                Player2Value = rnd.Next(10, 99);
            t = new Stopwatch();
            t.Start();
            ingame = true;
            ThreadPool.QueueUserWorkItem(tick);
        }
        public void tick(object state)
        {
            while (ingame)
            {
                if (t.Elapsed.Seconds - player1Live.Seconds > 10)
                { 
                    Player1DC();
            }
                if (t.Elapsed.Seconds - player2Live.Seconds > 10)
                { 
                    Player2DC();
            }
                switch (gamemode)
                {
                    case 1:
                        if (Player1Value * Player2Value == GoalValue)
                        {
                            success();
                        }
                        break;
                    case 2:
                        if (Player1Value + Player2Value == GoalValue)
                        {
                            success();
                        }
                        break;
                    case 3:
                        if (Player1Value - Player2Value == GoalValue)
                        {
                            success();
                        }
                        if (Player2Value - Player1Value == GoalValue)
                        {
                            success();
                        }
                        break;
                    case 4:
                        if (Player1Value / Player2Value == GoalValue)
                        {
                            success();
                        }
                        if (Player2Value / Player1Value == GoalValue)
                        {
                            success();
                        }
                        break;
                }


            }
        }
        public string returnMessage = "";
        public void Player2DC()
        {
            t.Stop();

            returnMessage = "Other player disconnect detected";
            ingame = false;
        }

        public void Player1DC()
        {
            t.Stop();

            returnMessage = "Other player disconnect detected";
            ingame = false;
        }

        public void success()
        {
            t.Stop();
            prizeFull = genPrize();
            returnMessage = $"You got a {prize} Card!  Time:";
            ingame = false;
        }
        public string prizeFull;
        public string prize { get {
                return prizeFull.Substring(0, 1);
            }
        }
        public string genPrize()
        {
            Random rnd = new Random();
            var x = rnd.Next(1, 100 * Math.Min(gamemode, 2)) * Math.Min(gamemode, 2);
            var g = (35 - t.Elapsed.Seconds);
            var s = (80 - t.Elapsed.Seconds);

            if (x<= g)
            {

                return "Gold";
            }
            if (x <= s)
            {

                return "Silver";
            }

                    return "Bronze";
        }
        public GameServer() { 
        
        }
        public void send(Byte[] id, int i)
        {

            if (id.Equals(player2ID))
            {
                Player2Values.Add(t.Elapsed, Player2Value);
                Player2Value = i;
            }
            if (id.Equals(player1ID))
            {
                Player1Values.Add(t.Elapsed, Player1Value);
                Player1Value = i;
            }
        }
        public GameClient Getmy(Byte[] id)
        {
            var Gc = new GameClient(this);
            if (id.Equals(player2ID))
            {
                player2Live = t.Elapsed;
                Gc.myValue = Player2Value;
                Gc.otherValue = Player1Value;
            }
            if (id.Equals(player1ID))
            {
                player1Live = t.Elapsed;
                Gc.myValue = Player1Value;
                Gc.otherValue = Player2Value;
            }
            return Gc;
        }
    }
    public class GameClient
    {
        public int targetValue = 0;
        public int myValue = 10;
        public int otherValue = 10;
        public GameServer myserver;
        public GameClient()
        {

        }
        public GameClient(GameServer server)
        {
            myserver = server;
            targetValue = server.GoalValue;
        }
       

    }
    public class gameHistory
    {
        public int finalmyValue = 0;
        public int finalotherValue = 0;
        public int goalValue = 0;
        public TimeSpan totalTime = TimeSpan.Zero;       
        public Dictionary<TimeSpan,int> myvalues = new Dictionary<TimeSpan, int>();

        public gameHistory()
        {

        }
        private void success()
        {
            totalTime = TimeSpan.Zero;
            foreach (var ts in myvalues.Keys)
            {
                totalTime += ts;
            }
            //myLoggedInUser.Stats.log()
        }

    }
}
