using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WelcomeEmail
{
    internal class EmailProvider
    {
        

        private readonly IAmazonSimpleEmailService _amazonSimpleEmailService;

        public EmailProvider()
        {

            _amazonSimpleEmailService = new AmazonSimpleEmailServiceClient();
        }
        public async Task<bool> SendEmail(LambdaInput lambdaInput)
        {
            using (var client = new AmazonSimpleEmailServiceClient())
            {
                var request = new SendEmailRequest
                {
                    Source = lambdaInput.senderEmail,
                    Destination = new Destination
                    {
                        ToAddresses = new List<string> { lambdaInput.recipientEmail }
                    },
                    Message = new Message
                    {
                        Subject = new Content("Lambda SES API Test"),
                        Body = new Body
                        {
                            Html = new Content("Thanks for Registering in Application")
                        }

                    }

                };

                try
                {

                    var res = await client.SendEmailAsync(request);
                    Console.WriteLine($"Email sent! Message ID: {res.MessageId}");
                }
                catch(Exception ex) 
                {
                    Console.WriteLine($"Failed to send email. Error: {ex.Message}");
                    return false;
                }
            }
                return true;
        }

        public async Task<bool> VerifyEmailIdentity(string mail)
        {
            bool success = false;
            try
            {
                var res = await _amazonSimpleEmailService.VerifyEmailIdentityAsync(
                new VerifyEmailIdentityRequest
                {
                    EmailAddress = mail
                });
                success = res.HttpStatusCode == HttpStatusCode.OK;
            }
            catch(Exception ex){
                Console.WriteLine($"Failed to verify email. Error: {ex.Message}");
                return success;
            }
            
            return success;
        }
    }
}
