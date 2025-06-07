
namespace Werwolf.Data
{
    public class Connection
    {
        public ConnectionType ConnectionType { get; set; }
        public Role From;
        public Role To;
        public List<Role>? DiesToo;

        public Connection(ConnectionType connectionType, Role from, Role to, List<Role>? diesToo = null)
        {
            ConnectionType = connectionType;
            From = from;
            To = to;
            DiesToo = new List<Role>();

            if (diesToo != null)
            {
                DiesToo = diesToo;
            }
        }
    }

    public enum ConnectionType
    {
        None = 0,
        Couple = 1,
        Bite = 2,
        RevengeKill = 3,
        StealRole = 4
    }
}
