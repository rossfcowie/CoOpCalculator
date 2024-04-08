using Microsoft.Identity.Client;

namespace CoOpCalculator.Data
{
    public interface IServerService
    {
    }
    public class ServerService : IServerService
    {
        private Dictionary<Byte[],GameServer> gameServers = new Dictionary<Byte[], GameServer>  ();
        private List<Matcher> matchMaker, allmatchers;
        public ServerService() {
            matchMaker = new List<Matcher>();
            allmatchers = new List<Matcher>();
        }

        public Matcher StartMatchMake(Byte[] id)
        {
            var x = new Matcher(id);
            matchMaker.Add(x);
            return x;
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
            foreach(var r in allmatchers)
            {
                if(r.id == x)
                {
                    return r;
                }
            }
            foreach (var r in matchMaker)
            {
                if (r.id == x)
                {
                    return r;
                }
            }
            return null;
        }

        public void matchMake() {
            if (matchMaker.Count >= 2) {
                Random rnd = new Random();
                for (int i=0; i<matchMaker.Count-1; i += 2)
                {
                    var x = matchMaker[i];
                    var y = matchMaker[i+1];
                    var gs = new GameServer();
                    gs.player1ID = x.id;
                    gs.player2ID = y.id;    
                    gameServers.Add(x.id, gs);
                    gameServers.Add(y.id, gs);
                    matchMaker[i].gameServer = gs;
                    matchMaker[i + 1].gameServer = gs;
                    matchMaker[i].matched = true;
                    matchMaker[i + 1].matched = true;
                    gs.start();
                } 
            }
            if(matchMaker.Count % 2 == 0)
            {
                allmatchers.AddRange(matchMaker);
                matchMaker.Clear();
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
