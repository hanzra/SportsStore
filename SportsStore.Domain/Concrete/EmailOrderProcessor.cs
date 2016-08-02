﻿using SportsStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsStore.Domain.Entities;
using System.Net.Mail;
using System.Net;

namespace SportsStore.Domain.Concrete
{

    public class EmailSettings
    {
            public string MailToAddress = "orders@example.com";
            public string MailFromAddress = "sportsstore@example.com";
            public bool UseSsl = true;
            public string Username = "MySmtpUsername";
            public string Password = "MySmtpPassword";
            public string ServerName = "smtp.example.com";
            public int ServerPort = 587;
            public bool WriteAsFile = true;
            public string FileLocation = @"C:\Users\shanzra\Desktop\Temp\Emails";
        }
    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings emailsettings)
        {
            this.emailSettings = emailsettings;
        }

        public void ProcessOrder(Cart cart, ShippingDetail shippingInfo)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);

                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder()
                .AppendLine("A new order has been submitted")
                .AppendLine("---")
                .AppendLine("Items:");

                foreach (var line in cart.AllCartLines)
                {
                    var subtotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (subtotal: {2:c}",
                    line.Quantity,
                    line.Product.Name,
                    subtotal);
                }

                body.AppendFormat("Total order value: {0:c}",
                cart.ComputeTotalValue())
                .AppendLine("---")
                .AppendLine("Ship to:")
                .AppendLine(shippingInfo.Name)
                .AppendLine(shippingInfo.AddressLine1)
                .AppendLine(shippingInfo.AddressLine2 ?? "")
                .AppendLine(shippingInfo.AddressLine3 ?? "")
                .AppendLine(shippingInfo.City)
                .AppendLine(shippingInfo.State ?? "")
                .AppendLine(shippingInfo.Country)
                .AppendLine(shippingInfo.Zip)
                .AppendLine("---")
                .AppendFormat("Gift wrap: {0}",
                shippingInfo.GiftWrap ? "Yes" : "No");
                MailMessage mailMessage = new MailMessage(
                emailSettings.MailFromAddress, // From
                emailSettings.MailToAddress, // To
                "New order submitted!", // Subject
                body.ToString()); // Body
                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }
                smtpClient.Send(mailMessage);
            }
        }
    }
}