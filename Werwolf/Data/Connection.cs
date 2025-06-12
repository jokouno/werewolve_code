
namespace Werwolf.Data
{
    public class Connection
    {
        public ConnectionType ConnectionType { get; set; }
        public Role From;
        public Role To;
        public List<string>? DiesToo;
        public string Message;

        public Connection(ConnectionType connectionType, Role from, Role to, List<string>? diesToo = null, string message = null)
        {
            ConnectionType = connectionType;
            From = from;
            To = to;
            DiesToo = new List<string>();
            Message = string.Empty;

            if (diesToo != null)
            {
                DiesToo = diesToo;
            }

            if (!string.IsNullOrEmpty(message))
            {
                Message = message;
            }
        }
    }

    public enum ConnectionType
    {
        None = 0,
        Couple = 1,
        Bite = 2,
        RevengeKill = 3,
        StealRole = 4,
        ChangedRole = 5
    }
}
