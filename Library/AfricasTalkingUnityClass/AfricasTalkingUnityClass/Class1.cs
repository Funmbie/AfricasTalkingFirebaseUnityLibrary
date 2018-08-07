using System;
using System.Collections.Generic;
using System.Text;
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
        //Classes
        class Game
        {
            public string name;
            public string publisher;
            public string year;
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
        class GameId
        {
            public string name;
        }
        class Count
        {
            public string game_id;
            public int count;
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
        class LeaderboardResponse
        {
            public string date;
            public string gamer_name;
            public int main;
            public int minor;
            public float others;
        }
        class TokenRequest
        {
            public string email;
            public string password;
            public bool returnSecureToken;
        }
        class TokenResponse
        {
            public string kind;
            public string localId;
            public string email;
            public string displayName;
            public string idToken;
            public bool registered;
            public string refreshToken;
            public string expiresIn;
        }

        //Game Functions
        public string registerGame(string name, string publisher, string year)
        {
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            //Insert game. If successful
            //Retrieve all games and get their ids
            //Compare game id from json to game id from params
            Game newGame = new Game();
            newGame.name = name;
            newGame.publisher = publisher;
            newGame.year = year;
            string json = JsonConvert.SerializeObject(newGame);
            try
            {
                var request = WebRequest.CreateHttp("https://atgames-infra-test.firebaseio.com/games/.json");
                request.Method = "POST";
                request.ContentType = "application/json";
                var buffer = Encoding.UTF8.GetBytes(json);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                var response = request.GetResponse();
                json = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                GameId gameId = new GameId();
                gameId = JsonConvert.DeserializeObject<GameId>(json);
                return gameId.name;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string deleteGame(string game_id)
        {
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

            try
            {
                var request = WebRequest.CreateHttp("https://atgames-infra-test.firebaseio.com/games/" + game_id + "/.json");
                request.Method = "DELETE";
                request.ContentType = "application/json";
                var response = request.GetResponse();
                return "OK";
            }

            catch (Exception e)
            {
                return e.Message;
            }
        }
        //User's Function

        public string signUp(string email, string location, string name,string password, string phone)
        {
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            bool exists = false;

            //Read leaderboard
            string result = getJson("https://atgames-infra-test.firebaseio.com/profiles/.json");
            if (result.Length > 20)
            {
                string[] words = splitFunction(result, '{');
                result = joinFunction(words);
                words = splitFunction(result, '}');
                result = joinFunction(words);
                words = splitFunction(result, ',');

                //Get game id and gamer id
                int counter = 0;
                string[] usernames = new string[words.Length / 5];
                string[] emails = new string[words.Length / 5];

                for (int i = 0; i < words.Length; i += 5)
                {
                    string trim = words[i + 2].Substring(8, words[i + 2].Length - 9);
                    usernames[counter] = trim;
                    trim = words[i].Substring(32, words[i].Length - 33);
                    emails[counter] = trim;
                    counter++;
                }

                for (int i = 0; i < usernames.Length; i++)
                {
                    if ((usernames[i] == name) || (emails[i] == email))
                    {
                        exists = true;
                        break;
                    }
                }
            }
            else
                exists = false;

            if (!exists)
            {
                password = sha256(password);
                User addUser = new User(DateTime.Now.ToString(), email, location, name, password, phone);
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
                }
                catch (Exception e)
                {
                    return e.Message;
                }

                //Add user to firebase client
                try
                {
                    TokenRequest signUp = new TokenRequest();
                    signUp.email = email;
                    signUp.password = password;
                    signUp.returnSecureToken = true;
                    json = JsonConvert.SerializeObject(signUp);
                    string apikey = "AIzaSyDHT4x9HNQwi_LedoqNWylBmhMZQDSEy9M";
                    var request = WebRequest.CreateHttp("https://www.googleapis.com/identitytoolkit/v3/relyingparty/signupNewUser?key=" + apikey);
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    var buffer = Encoding.UTF8.GetBytes(json);
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                    var response = request.GetResponse();
                    json = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                    TokenResponse tokenResponse = new TokenResponse();
                    tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(json);
                    return "OK";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
            else
            {
                return "That email or username is already in use. Try another username";
            }
        }

        public string editDetail(string gamer_id,string key,string value)
        {
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

            string json = "";

            if (key == "name")
            {
               json = JsonConvert.SerializeObject(new { name = value });
            }
            else if (key == "phone")
            {
               json = JsonConvert.SerializeObject(new { phone = value });
            }
            else if (key == "email")
            {
               json = JsonConvert.SerializeObject(new { email = value });
            }
            else if (key == "location")
            {
               json = JsonConvert.SerializeObject(new { location = value });
            }
            else
                return "Invalid Request";

            try
            {
                var request = WebRequest.CreateHttp("https://atgames-infra-test.firebaseio.com/profiles/"+gamer_id+ "/.json");
                request.Method = "PATCH";
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

        public string retrieveUsername(string gamer_id)
        {
            return retrieveUserInfo(gamer_id,"name");
        }

        public string login(string credentials, string password)
        {
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

            string result = getJson("https://atgames-infra-test.firebaseio.com/profiles/.json");
            if (result.Length > 12)
            {
                string[] words = splitFunction(result, '{');
                result = joinFunction(words);
                words = splitFunction(result, '}');
                result = joinFunction(words);
                words = splitFunction(result, ',');

                int counter = 0;
                string[] emails = new string[words.Length / 5];
                string[] phones = new string[words.Length / 5];
                string[] passwords = new string[words.Length / 5];
                string[] keys = new string[words.Length / 5];

                for (int i = 0; i < words.Length; i += 5)
                {
                    string trim = words[i].Substring(32, words[i].Length - 33);
                    emails[counter] = trim;
                    trim = words[i + 3].Substring(12, words[i + 3].Length - 13);
                    passwords[counter] = trim;
                    trim = words[i + 4].Substring(9, words[i + 4].Length - 10);
                    phones[counter] = trim;
                    trim = words[i].Substring(1, 20);
                    keys[counter] = trim;
                    counter++;
                }
                password = sha256(password);

                for (int i = 0; i < keys.Length; i++)
                {
                    if (passwords[i] == password)
                    {
                        if (phones[i] == credentials)
                        {
                            return "OK" + keys[i];
                        }
                        else
                        {
                            if (emails[i] == credentials)
                            {
                                return "OK" + keys[i];
                            }
                            else
                            {
                                return "Error Logging In. Please try again later";
                            }
                        }
                    }
                }
            }
            else
            {
                return "Not Registered";
            }

            return "Done";
        }

        public string Delete(string gamer_id)
        {
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

            try
            {
                var request = WebRequest.CreateHttp("https://atgames-infra-test.firebaseio.com/profiles/"+gamer_id+"/.json");
                request.Method = "DELETE";
                request.ContentType = "application/json";
                var response = request.GetResponse();
                //Delete the user from the authentication side
                return "OK";
            }

            catch(Exception e)
            {
                return e.Message;
            }
        }

        //Leaderboard Functions

        public string setLeaderboard(string game_id, string gamer_id, int main, int minor, float others)
        {
            string idToken = "";

            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            string reqemail = retrieveUserInfo(gamer_id, "email");
            string reqpass = retrieveUserInfo(gamer_id, "password");

            TokenRequest tokenRequest = new TokenRequest();
            tokenRequest.email = reqemail;
            tokenRequest.password = reqpass;
            tokenRequest.returnSecureToken = true;
            string jsn = JsonConvert.SerializeObject(tokenRequest);
            try
            {
                string apikey = "AIzaSyDHT4x9HNQwi_LedoqNWylBmhMZQDSEy9M";
                var request = WebRequest.CreateHttp("https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=" + apikey);
                request.Method = "POST";
                request.ContentType = "application/json";
                var buffer = Encoding.UTF8.GetBytes(jsn);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                var response = request.GetResponse();
                jsn = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                TokenResponse tokenResponse = new TokenResponse();
                tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(jsn);
                idToken = tokenResponse.idToken;
            }
            catch (Exception e)
            {
                return e.Message;
            }

            //Read leaderboard
            string result = getJson("https://atgames-infra-test.firebaseio.com/leaderboard/.json?auth="+idToken);
            string[] words = splitFunction(result,'{');
            result = joinFunction(words);
            words = splitFunction(result, '}');
            result = joinFunction(words);
            words = splitFunction(result, ',');

            //Get game id and gamer id
            int counter = 0;
            string[] gamerid = new string[words.Length/6];
            string[] gameid = new string[words.Length / 6];
            string[] keys = new string[words.Length / 6];

            for (int i=0;i<words.Length;i+=6)
            {
                string trim = words[i + 1].Substring(11,words[i+1].Length-12);
                gameid[counter] = trim;
                trim = words[i + 2].Substring(12,words[i+2].Length-13);
                gamerid[counter] = trim;
                trim = words[i].Substring(1,20);
                keys[counter] = trim;
                counter++;
            }

            bool exists = false;
            string key = "";

            for (int i=0;i<gameid.Length;i++)
            {
                if (gameid[i] == game_id)
                {
                    if (gamerid[i]==gamer_id)
                    {
                        exists = true;
                        key = keys[i];
                        break;
                    }
                }
            }

            //if game id exists in the json, patch the data
            if (exists)
            {
                Leaderboard leaderboard = new Leaderboard(DateTime.Now.ToString(),game_id, gamer_id, main, minor, others);
                string json = JsonConvert.SerializeObject(leaderboard);

                try
                {
                    var request = WebRequest.CreateHttp("https://atgames-infra-test.firebaseio.com/leaderboard/"+key+ "/.json?auth=" + idToken);
                    request.Method = "PATCH";
                    request.ContentType = "application/json";
                    var buffer = Encoding.UTF8.GetBytes(json);
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                    var response = request.GetResponse();
                    json = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                    addCountFunction(gamer_id, game_id);

                    return "OK";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
            //Else add new data
            else
            {
                Leaderboard newEntry = new Leaderboard(DateTime.Now.ToString(),game_id,gamer_id,main,minor,others);
                string json = JsonConvert.SerializeObject(newEntry);
                try
                {
                    var request = WebRequest.CreateHttp("https://atgames-infra-test.firebaseio.com/leaderboard/.json?auth=" + idToken);
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    var buffer = Encoding.UTF8.GetBytes(json);
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                    var response = request.GetResponse();
                    json = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                    addCountFunction(gamer_id, game_id);

                    return "+OK";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
        }

        public string addLeaderboard(string game_id,string gamer_id,int main,int minor,float others)
        {
            string idToken = "";
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            string reqemail = retrieveUserInfo(gamer_id, "email");
            string reqpass = retrieveUserInfo(gamer_id, "password");

            TokenRequest tokenRequest = new TokenRequest();
            tokenRequest.email = reqemail;
            tokenRequest.password = reqpass;
            tokenRequest.returnSecureToken = true;
            string json = JsonConvert.SerializeObject(tokenRequest);
            try
            {
                string apikey = "AIzaSyDHT4x9HNQwi_LedoqNWylBmhMZQDSEy9M";
                var request = WebRequest.CreateHttp("https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=" + apikey);
                request.Method = "POST";
                request.ContentType = "application/json";
                var buffer = Encoding.UTF8.GetBytes(json);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                var response = request.GetResponse();
                json = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                TokenResponse tokenResponse = new TokenResponse();
                tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(json);
                idToken = tokenResponse.idToken;
            }
            catch (Exception e)
            {
                return e.Message;
            }
            Leaderboard addleaderboard = new Leaderboard(DateTime.Now.ToString(),game_id,gamer_id,main,minor,others);

            json = JsonConvert.SerializeObject(addleaderboard);
            try
            {
                var request = WebRequest.CreateHttp("https://atgames-infra-test.firebaseio.com/leaderboard/.json?auth="+idToken);
                request.Method = "POST";
                request.ContentType = "application/json";
                var buffer = Encoding.UTF8.GetBytes(json);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                var response = request.GetResponse();
                json = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                //Increment count on that gameid
                string x = addCountFunction(gamer_id,game_id);

                return "OK";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string getLeaderboard(string gamer_id,string game_id,int limit)
        {
            string idToken="";

            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            //Retrieve ID token
            string reqemail = retrieveUserInfo(gamer_id,"email");
            string reqpass = retrieveUserInfo(gamer_id,"password");

            TokenRequest tokenRequest = new TokenRequest();
            tokenRequest.email = reqemail;
            tokenRequest.password = reqpass;
            tokenRequest.returnSecureToken = true;
            string json = JsonConvert.SerializeObject(tokenRequest);
            try
            {
                string apikey = "AIzaSyDHT4x9HNQwi_LedoqNWylBmhMZQDSEy9M";
                var request = WebRequest.CreateHttp("https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key="+apikey);
                request.Method = "POST";
                request.ContentType = "application/json";
                var buffer = Encoding.UTF8.GetBytes(json);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                var response = request.GetResponse();
                json = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                TokenResponse tokenResponse = new TokenResponse();
                tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(json);
                idToken = tokenResponse.idToken;
            }
            catch (Exception e)
            {
                return e.Message;
            }

            string result = getJson("https://atgames-infra-test.firebaseio.com/leaderboard/.json?auth="+idToken);
            string[] words = splitFunction(result, '{');
            result = joinFunction(words);
            words = splitFunction(result, '}');
            result = joinFunction(words);
            words = splitFunction(result, ',');

            string[] dates = new string[words.Length / 6];
            string[] game_ids = new string[words.Length / 6];
            string[] gamer_ids = new string[words.Length/6];
            string[] mains = new string[words.Length / 6];
            string[] minors = new string[words.Length / 6];
            string[] others = new string[words.Length / 6];
            int counter = 0;

            for (int i=words.Length-6;i>=0;i-=6)
            {
                string trim = words[i].Substring(31,words[i].Length-32);
                dates[counter] = trim;
                trim = words[i + 1].Substring(11,words[i+1].Length-12);
                game_ids[counter]=trim;
                trim = words[i + 2].Substring(12,words[i+2].Length-13);
                gamer_ids[counter] = trim;
                trim = words[i + 3].Substring(7,words[i+3].Length-7);
                mains[counter] = trim;
                trim = words[i + 4].Substring(8, words[i + 4].Length - 8);
                minors[counter] = trim;
                trim = words[i + 5].Substring(9, words[i + 5].Length - 9);
                others[counter] = trim;
                counter++;
            }

            int sizeCount = 0;
            Leaderboard[] resultLeaderboard = new Leaderboard[limit];
            for (int i= 0;i<limit;i++)
            {
                resultLeaderboard[i] = new Leaderboard("","","",0,0,0f);
            }

            for (int i=0;i< gamer_ids.Length;i++) {
                if (sizeCount < limit)
                {
                    if (game_id == game_ids[i])
                    {
                        resultLeaderboard[sizeCount].date = dates[i];
                        resultLeaderboard[sizeCount].game_id = game_ids[i];
                        resultLeaderboard[sizeCount].gamer_id = gamer_ids[i];
                        resultLeaderboard[sizeCount].main = int.Parse(mains[i]);
                        resultLeaderboard[sizeCount].minor = int.Parse(minors[i]);
                        resultLeaderboard[sizeCount].others = float.Parse(others[i]);
                        sizeCount++;
                    }
                }
            }


            //get the names and swap for the id
            LeaderboardResponse[] finalBoard = new LeaderboardResponse[limit];

            for (int i = 0; i < finalBoard.Length; i++)
            {
                finalBoard[i] = new LeaderboardResponse();
            }

            string[] gamer_names = new string[limit];

            for (int i = 0; i < gamer_names.Length; i++)
            {
                gamer_names[i] = retrieveUserInfo(gamer_ids[i], "name");
            }

            for (int i=0;i<gamer_names.Length;i++)
            {
                finalBoard[i].date = resultLeaderboard[i].date;
                finalBoard[i].gamer_name = gamer_names[i];
                finalBoard[i].main = resultLeaderboard[i].main;
                finalBoard[i].minor = resultLeaderboard[i].minor;
                finalBoard[i].others = resultLeaderboard[i].others;
            }
            result = JsonConvert.SerializeObject(finalBoard);

            return result;
        }

        public int getGameCount(string gamer_id,string game_id)
        {
            string idToken = "";
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            string reqemail = retrieveUserInfo(gamer_id, "email");
            string reqpass = retrieveUserInfo(gamer_id, "password");

            TokenRequest tokenRequest = new TokenRequest();
            tokenRequest.email = reqemail;
            tokenRequest.password = reqpass;
            tokenRequest.returnSecureToken = true;
            string json = JsonConvert.SerializeObject(tokenRequest);
            try
            {
                string apikey = "AIzaSyDHT4x9HNQwi_LedoqNWylBmhMZQDSEy9M";
                var request = WebRequest.CreateHttp("https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=" + apikey);
                request.Method = "POST";
                request.ContentType = "application/json";
                var buffer = Encoding.UTF8.GetBytes(json);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                var response = request.GetResponse();
                json = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                TokenResponse tokenResponse = new TokenResponse();
                tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(json);
                idToken = tokenResponse.idToken;
            }
            catch (Exception e)
            {
                return 0;
            }
            string result = getJson("https://atgames-infra-test.firebaseio.com/count/.json?auth=" + idToken);
            if (result.Length > 12)
            {
                string[] words = splitFunction(result, '{');
                result = joinFunction(words);
                words = splitFunction(result, '}');
                result = joinFunction(words);
                words = splitFunction(result, ',');
                string[] keys = new string[words.Length / 2];
                string[] gameids = new string[words.Length / 2];
                int[] counts = new int[words.Length / 2];

                //If the id is equal, take the corresponding count and return it


                //Get game id and gamer id
                int counter = 0;

                for (int i = 0; i < words.Length; i += 2)
                {
                    string trim = words[i + 1].Substring(11, words[i + 1].Length - 12);
                    gameids[counter] = trim;
                    trim = words[i].Substring(1, 20);
                    keys[counter] = trim;
                    trim = words[i].Substring(31, words[i].Length - 31);
                    counts[counter] = int.Parse(trim);
                    counter++;
                }

                //Check if the requested game_id exists
                bool exists = false;
                int count = 0;

                for (int i = 0; i < gameids.Length; i++)
                {
                    if (gameids[i] == game_id)
                    {
                        exists = true;
                        count = counts[i];
                        break;
                    }
                }

                if (exists)
                    return count;
                else
                    //else return zero
                    return 0;
            }
            else
            {
                return 0;
            }
        }

        //Africa's Talking API Functions

        public string SMS(string username, string apikey, string msg, string gamer_id)
        {
            try
            {
                var gateway = new AfricasTalkingGateway(username, apikey);
                string phone = retrieveUserInfo(gamer_id,"phone");
                var sms = gateway.SendMessage(phone, msg);
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

        public string plainSMS(string username, string apikey, string msg, string phone)
        {
            try
            {
                var gateway = new AfricasTalkingGateway(username, apikey);
                var sms = gateway.SendMessage(phone, msg);
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

        public string sendAirtime(string username, string apikey, string gamer_id, string amount, string currency = "KES")
        {
            string phoneNumber = retrieveUserInfo(gamer_id, "phone");
            var airtimerecipients = @"{'phoneNumber':'" + phoneNumber + "','amount':'" + currency + " " + amount + "'}"; // Send any JSON object of n-Length
            var gateway = new AfricasTalkingGateway(username, apikey);
            try
            {
                var airtimeTransaction = gateway.SendAirtime(airtimerecipients);
                return "OK";
            }
            catch (AfricasTalkingGatewayException e)
            {
                return e.Message;
            }
        }

        public string plainAirtime(string username, string apikey, string phoneNumber, string amount, string currency = "KES")
        {
            var airtimerecipients = @"{'phoneNumber':'" + phoneNumber + "','amount':'" + currency + " " + amount + "'}"; // Send any JSON object of n-Length
            var gateway = new AfricasTalkingGateway(username, apikey);
            try
            {
                var airtimeTransaction = gateway.SendAirtime(airtimerecipients);
                return "OK";
            }
            catch (AfricasTalkingGatewayException e)
            {
                return e.Message;
            }
        }

        public string inAppPurchase(string username, string apikey,string gamer_id, string productName,string currency,decimal amount, string channel, Dictionary<string,string> metadata = null)
        {
            string phoneNumber = retrieveUserInfo(gamer_id,"phone");

            var gateway = new AfricasTalkingGateway(username, apikey);

            try
            {
                var checkout = gateway.Checkout(productName, phoneNumber, currency, amount, channel, metadata);
                return "OK";
            }
            catch (AfricasTalkingGatewayException e)
            {
                return e.Message;
            }
        }

        public string plainIAP(string username, string apikey,string phoneNumber, string productName,string currency,decimal amount, string channel, Dictionary<string,string> metadata = null)
        {
            var gateway = new AfricasTalkingGateway(username, apikey);

            try
            {
                var checkout = gateway.Checkout(productName, phoneNumber, currency, amount, channel, metadata);
                return "OK";
            }
            catch (AfricasTalkingGatewayException e)
            {
                return e.Message;
            }
        }

        public string B2C(string username, string apikey, string gamer_id, decimal amount, string reason, string productName, string currencyCode = "KES")
        {
            string PhoneNum = retrieveUserInfo(gamer_id,"phone");
            string Name = retrieveUsername(gamer_id);

            // We invoke our gateway
            var gateway = new AfricasTalkingGateway(username, apikey);

            // Let's create a bunch of people who'll be receiving the refund or monthly salary etc...
            var person = new MobileB2CRecepient(Name, PhoneNum, currencyCode, amount);

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
                return "OK";
            }
            catch (AfricasTalkingGatewayException e)
            {
                return ("We ran into problems: " + e.StackTrace + e.Message);
            }
        }

        public string plainB2C(string username, string apikey, string phone, decimal amount, string reason, string productName, string currencyCode = "KES")
        {
            string Name = "John Doe";

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
                return "OK";
            }
            catch (AfricasTalkingGatewayException e)
            {
                return ("We ran into problems: " + e.StackTrace + e.Message);
            }
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

        string addCountFunction(string gamer_id, string game_id)
        {
            string idToken = "";
            bool exists = false;
            int count = 0;
            string key = "";

            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            string reqemail = retrieveUserInfo(gamer_id, "email");
            string reqpass = retrieveUserInfo(gamer_id, "password");

            TokenRequest tokenRequest = new TokenRequest();
            tokenRequest.email = reqemail;
            tokenRequest.password = reqpass;
            tokenRequest.returnSecureToken = true;
            string jsn = JsonConvert.SerializeObject(tokenRequest);
            try
            {
                string apikey = "AIzaSyDHT4x9HNQwi_LedoqNWylBmhMZQDSEy9M";
                var request = WebRequest.CreateHttp("https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=" + apikey);
                request.Method = "POST";
                request.ContentType = "application/json";
                var buffer = Encoding.UTF8.GetBytes(jsn);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                var response = request.GetResponse();
                jsn = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                TokenResponse tokenResponse = new TokenResponse();
                tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(jsn);
                idToken = tokenResponse.idToken;
            }
            catch (Exception e)
            {
                return e.Message;
            }

            //Read Count Json
            string result = getJson("https://atgames-infra-test.firebaseio.com/count/.json?auth=" + idToken);
            //Strip extra things
            if (result.Length > 30)
            {
                string[] words = splitFunction(result, '{');
                result = joinFunction(words);
                words = splitFunction(result, '}');
                result = joinFunction(words);
                words = splitFunction(result, ',');
                //Fill the arrays game id and count
                string[] keys = new string[words.Length / 2];
                string[] gameids = new string[words.Length / 2];
                int[] counts = new int[words.Length / 2];

                int counter = 0;

                for (int i = 0; i < words.Length; i += 2)
                {
                    string trim = words[i+1].Substring(11, words[i+1].Length - 12);
                    gameids[counter] = trim;
                    trim = words[i].Substring(1, 20);
                    keys[counter] = trim;
                    trim = words[i].Substring(31, words[i].Length - 31);
                    counts[counter] = int.Parse(trim);
                    counter++;
                }

                //Check if the requested game_id exists
                for (int i = 0; i < gameids.Length; i++)
                {
                    if (gameids[i] == game_id)
                    {
                        exists = true;
                        count = counts[i];
                        key = keys[i];
                        break;
                    }
                }
            }
            else
            {
                exists = false;
            }

            //if game id exists in the json, patch the data
            if (exists)
            {
                count += 1;
                string json = JsonConvert.SerializeObject(new { count = count });

                try
                {
                    var request = WebRequest.CreateHttp("https://atgames-infra-test.firebaseio.com/count/" + key + "/.json?auth=" + idToken);
                    request.Method = "PATCH";
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
            //Else add new data
            else
            {
                Count newCounter = new Count();
                newCounter.count = 1;
                newCounter.game_id = game_id;
                string json = JsonConvert.SerializeObject(newCounter);
                try
                {
                    var request = WebRequest.CreateHttp("https://atgames-infra-test.firebaseio.com/count/.json?auth=" + idToken);
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    var buffer = Encoding.UTF8.GetBytes(json);
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                    var response = request.GetResponse();
                    json = (new StreamReader(response.GetResponseStream())).ReadToEnd();

                    return "+OK";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
        }

        string retrieveUserInfo(string gamer_id, string key)
        {
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

            string result = getJson("https://atgames-infra-test.firebaseio.com/profiles/"+gamer_id+"/"+key+"/.json");
            result = result.Substring(1,result.Length-2);

            return result;
        }

        string[] splitFunction(string word, char splitParam)
        {
            string[] final = word.Split(splitParam); 

            return final;
        }

        string joinFunction(string[] array)
        {
            string word = "";
            for (int i=0;i<array.Length;i++)
            {
                word += array[i];
            }
            return word;
        }

        string getJson(string URL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.ContentType = "application/json: charset=utf-8";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader read = new StreamReader(responseStream, Encoding.UTF8);
                return read.ReadToEnd();
            }
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