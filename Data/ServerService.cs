using Microsoft.Identity.Client;

namespace CoOpCalculator.Data
{
    public interface IServerService
    {
    }
    public class ServerService : IServerService
    {
        private Dictionary<Byte[],GameServer> gameServers = new Dictionary<Byte[], GameServer>  ();
        private Dictionary<Matcher,int> matchMaker, allmatchers;
        public ServerService() {
            matchMaker = new Dictionary<Matcher, int>();
            allmatchers = new Dictionary<Matcher, int>();
        }
        public int queued
        {
            get
            {
                return matchMaker.Count;
            }
        }
        public bool stopMatchMake(Matcher id)
        {
            var x = id.id;
            id = getbyID(x);
            matchMaker.Remove(id);
            allmatchers.Remove(id);
            return true;
        }
        public Matcher StartMatchMake(Byte[] id, int gm)
        {
            var x = new Matcher(id);
            matchMaker.Add(x, gm);
            return x;
        }
        public bool checkMatchMake(Matcher id, TimeSpan t)
        {
            matchMake();
            var x = id.id;
            id = getbyID(x);
            if (id.matched || t.Seconds < 10) { 
            return id.matched;
            }
            else
            {
                AImatchMake();
                id = getbyID(x);
                return id.matched;
            }
        }
        public bool checkMatchMake(Matcher id)
        {
            matchMake();
            var x = id.id;
            id = getbyID(x);
            return id.matched;
        }
        private Matcher getbyID(Byte[] x)
        {
            foreach(var r in allmatchers.Keys)
            {
                if(r.id == x)
                {
                    return r;
                }
            }
            foreach (var r in matchMaker.Keys)
            {
                if (r.id == x)
                {
                    return r;
                }
            }
            return null;
        }
        public async void matchMake1(List<Matcher> Queue1)
        {

            Random rnd = new Random( );
            for (int i = 0; i < Queue1.Count - 1; i += 2)
            {
                var x = Queue1[i];
                var y = Queue1[i + 1];
                var gs = new GameServer();
                gs.player1ID = x.id;
                gs.player2ID = y.id;
                gameServers.Add(x.id, gs);
                gameServers.Add(y.id, gs);
                Queue1[i].gameServer = gs;
                Queue1[i + 1].gameServer = gs;
                Queue1[i].matched = true;
                Queue1[i + 1].matched = true;
                gs.start(1);
                allmatchers.Add(Queue1[i], 1);
                allmatchers.Add(Queue1[i + 1], 1);
                matchMaker.Remove(Queue1[i]);
                matchMaker.Remove(Queue1[i + 1]);
            }
        }
        public async Task matchMake3(List<Matcher> Queue3)
        {

            Random rnd = new Random();
            for (int i = 0; i < Queue3.Count - 1; i += 2)
            {
                var x = Queue3[i];
                var y = Queue3[i + 1];
                var gs = new GameServer();
                gs.player1ID = x.id;
                gs.player2ID = y.id;
                gameServers.Add(x.id, gs);
                gameServers.Add(y.id, gs);
                Queue3[i].gameServer = gs;
                Queue3[i + 1].gameServer = gs;
                Queue3[i].matched = true;
                Queue3[i + 1].matched = true;
                gs.start(3);
                allmatchers.Add(Queue3[i], 3);
                allmatchers.Add(Queue3[i + 1], 3);
                matchMaker.Remove(Queue3[i]);
                matchMaker.Remove(Queue3[i + 1]);
            }
        }
        public async Task matchMake2(List<Matcher> Queue2)
        {

            Random rnd = new Random();
            for (int i = 0; i < Queue2.Count - 1; i += 2)
            {
                var x = Queue2[i];
                var y = Queue2[i + 1];
                var gs = new GameServer();
                gs.player1ID = x.id;
                gs.player2ID = y.id;
                gameServers.Add(x.id, gs);
                gameServers.Add(y.id, gs);
                Queue2[i].gameServer = gs;
                Queue2[i + 1].gameServer = gs;
                Queue2[i].matched = true;
                Queue2[i + 1].matched = true;
                gs.start(2);
                allmatchers.Add(Queue2[i], 1);
                allmatchers.Add(Queue2[i + 1], 1);
                matchMaker.Remove(Queue2[i]);
                matchMaker.Remove(Queue2[i + 1]);
            }
        }
        public void AImatchMake()
        {
            List<Matcher> Queue1 = new List<Matcher>(), Queue2 = new List<Matcher>(), Queue3 = new List<Matcher>();
            foreach (Matcher m in matchMaker.Keys)
            {
                var x = matchMaker.GetValueOrDefault(m);
                Random rnd = new Random();
                if (x == 4)
                {
                    x = rnd.Next(1, 3);
                }
                            var gs = new AIGameServer();
                            gs.player1ID = m.id;
                            gameServers.Add(m.id, gs);
                            m.gameServer = gs;
                            m.matched = true;
                            gs.start(x);
                            allmatchers.Add(m, x);
                            matchMaker.Remove(m);
                }
            }
        

        public void matchMake() {
            if (matchMaker.Count >= 2) {
                List<Matcher> Queue1 = new List<Matcher>(), Queue2 = new List<Matcher>(), Queue3 = new List<Matcher>(), Queue4 = new List<Matcher>();
                foreach(Matcher m in matchMaker.Keys)
                {
                    switch (matchMaker.GetValueOrDefault(m)) { 
                        case 1:
                            Queue1.Add(m);
                        break;

                    case 2:
                            Queue2.Add(m);  
                        break;
                    case 3:
                            Queue3.Add(m);
                        break;
                    case 4:
                            if (Queue1.Count % 2 == 1)
                            {
                                Queue1.Add(m);
                            }
                            else
                            {
                                if (Queue2.Count % 2 == 1)
                                {
                                    Queue2.Add(m);
                                }
                                else
                                {
                                    if (Queue3.Count % 2 == 1)
                                    {
                                        Queue3.Add(m);
                                    }
                                    else
                                    {
                                        Queue4.Add(m);
                                    }
                                }
                            }
                            break;
                    }
                    }
                foreach(Matcher m in Queue4)
                {
                    if (Queue1.Count % 2 == 1)
                    {
                        Queue1.Add(m);
                    }
                    else
                    {
                        if (Queue2.Count % 2 == 1)
                        {
                            Queue2.Add(m);
                        }
                        else
                        {
                            if (Queue3.Count % 2 == 1)
                            {
                                Queue3.Add(m);
                            }
                            else
                            {
                                Queue1.Add(m);

                            }
                        }
                    }
                }
                gameServers.Clear();
                matchMake1(Queue1);
                matchMake2(Queue2);
                matchMake3(Queue3);
            }
        }
    }


    public class Matcher(Byte[] id)
    {
        public Byte[] id = id;
        public bool matched = false;
        public GameServer gameServer;
    }
}
