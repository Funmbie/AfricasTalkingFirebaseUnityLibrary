using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AfricasTalkingCS;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace AfricasTalkingUnityClass
{
    public class AfricasTalkingUnityGateway
    {
        //Africa's Talking API Functions
        public string SMS(string username, string apikey, string msg, string phone)
        {
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

            try
            {
                var gateway = new AfricasTalkingGateway(username, apikey);
                var sms = gateway.SendMessage(phone, msg);
                foreach (var res in sms["SMSMessageData"]["Recipients"])
                {
                    if (res["status"] == "Success")
                    {
                        return sms.ToString();
                    }
                    else
                        return res["status"];
                }
            }
            catch (AfricasTalkingGatewayException exception)
            {
                return exception.Message;
            }

            return "Done";
        }

        public string Airtime(string username, string apikey, string phoneNumber, string amount, string currency = "KES")
        {
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

            var airtimerecipients = @"{'phoneNumber':'" + phoneNumber + "','amount':'" + currency + " " + amount + "'}"; // Send any JSON object of n-Length
            var gateway = new AfricasTalkingGateway(username, apikey);
            try
            {
                var airtimeTransaction = gateway.SendAirtime(airtimerecipients);
                return airtimeTransaction.ToString();
            }
            catch (AfricasTalkingGatewayException e)
            {
                return e.Message;
            }
        }

        public string IAP(string username, string apikey,string phoneNumber, string productName,string currency,decimal amount, string channel, Dictionary<string,string> metadata = null)
        {
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            var gateway = new AfricasTalkingGateway(username, apikey);

            try
            {
                var checkout = gateway.Checkout(productName, phoneNumber, currency, amount, channel, metadata);
                return checkout;
            }
            catch (AfricasTalkingGatewayException e)
            {
                return e.Message;
            }
        }

        public string B2C(string username, string apikey, string phone, string Name, decimal amount, string reason, string productName, string currencyCode = "KES")
        {
            // We invoke our gateway
            var gateway = new AfricasTalkingGateway(username, apikey);

            // Let's create a bunch of people who'll be receiving the refund or monthly salary etc...
            var person = new MobileB2CRecepient(Name, phone, currencyCode, amount);

            // we can add metadata...like why we're paying them/refunding them etc...
            person.AddMetadata("reason", reason);
            IList<MobileB2CRecepient> payment = new List<MobileB2CRecepient>
            {
                person
            };

            // let's refund them so that they can keep saving the planet
            try
            {
                var response = gateway.MobileB2C(productName, payment);
                return response.ToString();
            }
            catch (AfricasTalkingGatewayException e)
            {
                return ("We ran into problems: " + e.StackTrace + e.Message);
            }
        }

        //Assisting Functions
        bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            bool isOk = true;
            // If there are errors in the certificate chain, look at each error to determine the cause.
            if (sslPolicyErrors != SslPolicyErrors.None)
            {
                for (int i = 0; i < chain.ChainStatus.Length; i++)
                {
                    if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                    {
                        chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                        chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                        chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                        chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                        bool chainIsValid = chain.Build((X509Certificate2)certificate);
                        if (!chainIsValid)
                        {
                            isOk = false;
                        }
                    }
                }
            }
            return isOk;
        }
    }
}