using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System;
using System.Diagnostics;

namespace CoOpCalculator.Data
{
    public class GameServer
    {
        public int Player1Value { get; set; } = 10;
        public int Player2Value { get; set; } = 10;
        public int GoalValue { get; set; } = 1000;
        public Dictionary<TimeSpan,int> Player1Values = new Dictionary<TimeSpan, int>();
        public Dictionary<TimeSpan, int> Player2Values = new Dictionary<TimeSpan, int>();
        public Byte[] player1ID { get; set; }
        public Byte[] player2ID { get; set; }
        public Stopwatch t;
        public TimeSpan total = TimeSpan.Zero;
        public bool ingame = false;
        public void start()
        {
            Random rnd = new Random();
            GoalValue = rnd.Next(10, 99) * rnd.Next(10, 99);
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
                if (Player1Value * Player2Value == GoalValue)
                {
                    success();
                }


            }
        }

        private void success()
        {
            t.Stop();

            ingame = false;
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
                Gc.myValue = Player2Value;
                Gc.otherValue = Player1Value;
            }
            if (id.Equals(player1ID))
            {
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
