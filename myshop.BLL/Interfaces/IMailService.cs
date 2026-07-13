namespace myshop.BLL.Interfaces
{
	public interface IMailService
	{
		Task SendMailAsync(string mailTo, string subject, string body);
	}
}
