namespace TransportationService.WEB.Configuration
{
    public class SMTPSettings //simple mail transfer protocol
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
