using System;
using System.Collections.Generic;
using System.Linq;
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
        public string Login()
        {
            return "";
        }

        class User
        {
            public string date;
            public string email;
            public string location;
            public string name;
            public string password;
            public string phone;

            public User(string date, string email, string location, string name, string password, string phone)
            {
                this.email = email;
                this.location = location;
                this.name = name;
                this.password = password;
                this.phone = phone;
            }
        }
        class Leaderboard
        {
            public string date;
            public string game_id;
            public string gamer_id;
            public int main;
            public int minor;
            public float others;

            public Leaderboard(string date, string game_id, string gamer_id, int main, int minor, float others)
            {
                this.date = date;
                this.game_id = game_id;
                this.gamer_id = gamer_id;
                this.main = main;
                this.minor = minor;
                this.others = others;
            }
        }

        public string signUp(string email, string location, string name,string password, string phone)
        {
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

            password = sha256(password);
            User addUser = new User(DateTime.Now.ToString(),email,location,name,password,phone);
            string json = JsonConvert.SerializeObject(addUser);
            try
            {
                var request = WebRequest.CreateHttp("https://atgames-infra-test.firebaseio.com/profiles/.json");
                request.Method = "POST";
                request.ContentType = "application/json";
                var buffer = Encoding.UTF8.GetBytes(json);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                var response = request.GetResponse();
                json = (new StreamReader(response.GetResponseStream())).ReadToEnd();

                return "OK";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string setLeaderboard()
        {
            //If existing update, else create
            return "";
        }

        public string addLeaderboard(string game_id,string gamer_id,int main,int minor,float others)
        {
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

            Leaderboard addleaderboard = new Leaderboard(DateTime.Now.ToString(),game_id,gamer_id,main,minor,others);

            string json = JsonConvert.SerializeObject(addleaderboard);
            try
            {
                var request = WebRequest.CreateHttp("https://atgames-infra-test.firebaseio.com/leaderboard/.json");
                request.Method = "POST";
                request.ContentType = "application/json";
                var buffer = Encoding.UTF8.GetBytes(json);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                var response = request.GetResponse();
                json = (new StreamReader(response.GetResponseStream())).ReadToEnd();

                return "OK";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string getLeaderboard()
        {
            return "";
        }

        //Africa's Talking API Functions
        public string SMS(string username, string apikey, string msg, string gamer_id)
        {
            try
            {
                var gateway = new AfricasTalkingGateway(username, apikey);
                var sms = gateway.SendMessage("+254701951089", msg);
                foreach (var res in sms["SMSMessageData"]["Recipients"])
                {
                    if (res["status"] == "Success")
                    {
                        return "OK";
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


        //Assisting Functions
        static string sha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        string retrieveUserInfo(string gamer_id, string key)
        {


            return "";
        }

        string[] splitFunction(string word)
        {
            return new string[2];
        }

        string joinFunction(string[] array)
        {
            return "";
        }

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
