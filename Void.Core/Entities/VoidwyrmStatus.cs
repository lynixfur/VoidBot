namespace Void.Core.Entities
{
    public class VoidwyrmStatus
    {
        public string Type { get; set; }
        public string Reason { get; set; }
        public ServerStatus Servers { get; set; }
    }
}
