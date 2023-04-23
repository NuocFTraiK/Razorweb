namespace CS54.Services
{
	public class EmailConfiguration
	{
		public string From { get; set; } = null;
		public string SmtpServer { get; set; } = null!;
		public int Port { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
	}
}
