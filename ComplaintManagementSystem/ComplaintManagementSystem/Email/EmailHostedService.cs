using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
// i made this service by following this tutorial - https://www.youtube.com/watch?v=lCHKwyekbT4
// I am also not setting up a SMTP relay server to send the emails. instead i am using MailPit to view the emails (recommended by the tutorial) - https://mailpit.axllent.org/docs/install/
public class EmailHostedService
{
    public SendComplaintEmailResponse SendComplaintEmail(SendComplaintEmailRequest request)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("Complaint Management System", request.SendingEmail));
        email.To.Add(new MailboxAddress("Consumer", request.ReceivingEmail));
        email.Subject = "Complaint Registered!";
        email.Body = new TextPart(TextFormat.Plain)
        {
            Text = request.EmailText
        };
        using var smtp = new SmtpClient();
        smtp.Connect("localhost", 1025);
        smtp.Send(email);
        smtp.Disconnect(true);
        return new SendComplaintEmailResponse();
    }
}