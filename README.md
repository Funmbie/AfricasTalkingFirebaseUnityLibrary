# AfricasTalkingFirebaseUnityLibrary
Hey guys!! So this project is a library that is meant to help Unity3D developers make use of Africa's Talking APIs for payment, sms and ussd. There is also an aspect for Firebase we are experimenting with so you can save leaderboards and even users' information using this library. Please bear in mind it is still in the beta phase and as such is not recommended yet for production.

### How it works
- Switch your Unity runtime version to 4x by going to Edit/Player/Other Settings then change Scripting Runtime to 4x
- Import the library and its dependencies.
- Add "using AfricasTalkingUnityClass;" to the script you intend to use it in
- Declare an instance of the class 
>var at_u = new AfricasTalkingUnityGateway();
- Feel free to call the functions available
- Note: the functions return string value. The responses will be looked at below

### Functions and Responses
i. Game Functions for registering and deleting your game. These functions should be called only once. Also keep your game id safe to ensure that it can't be used by anyone else.

#####Registering a game
The token returned is your game id. Save it in a txt file and in somewhere private. We don't want Mr Robot messing with this.
>string query = at_u.registerGame(string name, string publisher, int year):
>Debug.Log(query);

#####Deleting a game
>string query = at_u.deleteGame(game_id);
>if(query=='OK')
>//So your game had a good run and now it has finally been deleted successfully
>else{Debug.Log(query);}

ii. User Functions will help you in profile management. Ensure the users enter valid emails. Before submitting to the library to prevent errors.

#####Register a new Gamer
This helps you create users. The great thing is an account created in a game can be used by another game making use of the same library.
>string query = at_u.signUp(string email, string location, string name,string password, string phone);
>if(query=='OK')
>//User is registered. You can log them in
>else Debug.Log(query);

#####Login
Here you are able to log the user in using phone number or email. It returns OKGamer_id or error message. The gamer id should be retreieved and stored temporarily as it will be used to query the database on the gamer's behalf.
>string query = at_u.login(string credentials, string password);
>if(query.Substring(0,2)=='OK')
>string token = query.Substring(2,query.Length-2);
>else Debug.Log(query);

#####Username
use the gamer's id gotten from the login to retrieve his/her username. It returns the username or error message.
>Debug.Log(at_u.retrieveUsername(string gamer_id));

#####Edit Details
change a user's 'name', 'phone', 'email' or 'location'. The parameters are the gamer's id, the key is what you want to change and the value is the new value for the key chosen. Note: Do not include comma in your locations. You usually only need to get the country for location. It returns OK or Invalid Request if you're asking for the wrong key or an error message if an error was encountered.
>string query = at_u.editDetail(string gamer_id,string key,string value);
>if (query=='OK')
>Debug.Log('Edited Successfully');
>else Debug.Log(query);

#####DeleteGame
delete a gamer's account. It returns OK or error message.
>string query = at_u.Delete(gamer_id);
>if (query=='OK')
>Debug.Log('Deleted Successfully');
>else Debug.Log(query);

iii. Africa's Talking API Functions for airtime, mobile checkout and sms.

#####Send SMS to a gamer's account
sends an sms to the phone number of the gamer id passed. You want to use your Africa's talking username and apikey for the respective parameters then add the message and gamer id. It returns an OK or an error message.
>string query = at_u.SMS(string username, string apikey, string msg, string gamer_id);
>if (query=='OK')
>Debug.Log('Message sent Successfully');
>else Debug.Log(query);
 
#####In-App Purchase using M-Pesa to a gamer's account
This function initiates mobile checkout on the device of the gamer using the token. It returns OK or error message. **Note** that the function returns 'OK' when the mobile checkout is initiated successfully and not when it is completed. You will have to take care of that from your callback url. You can do this by sending the data gotten on the callback url to a database and then calling another function to check the status of the transaction once you get an OK from the function
>string query = at_u.inAppPurchase(string username, string apikey,string gamer_id, string productName,string currency,decimal amount, string channel, Dictionary<string,string> metadata = null);
>if (query=='OK')
>Debug.Log('Payment Initiated Successfully');
>else Debug.Log(query);

#####Sending Airtime to a Gamer
Okay so now we want to reward our gamer for getting to an impossible score. We can do that by sending them airtime upon achieving such intractable task.
>int impossiblescore = 10000;
>int playerscore=11000;
>if(playerscore>impossiblescore)
>string query = at_u.sendAirtime(string username,string apikey,string gamer_id, string amount, string currency="KES");
>if (query=='OK')
>Debug.Log('Airtime Sent Successfully');
>else Debug.Log(query);

###Functions for Exisiting Games
So even if you have already shipped your game with an exisiting account management management system and you just need to use our API for payment, sms or airtime - dont worry  we got you covered.

#####Using SMS with a Phone Number in Unity3D
Here you can send an sms to a phonenumber simply by calling the function
>string query = at_u.plainSMS(string username, string apikey, string msg, string phone);
>if (query=='OK')
>Debug.Log('Message Sent Successfully');
>else Debug.Log(query);

#####Mobile Checkout with Phone Number in Unity3D
Inititate mobile checkout to a phone number. This function returns OK if the mobile checkout was successfully initiated
>string query = at_u.plainIAP(string username, string apikey,string phoneNumber, string productName,string currency,decimal amount, string channel, Dictionary<string,string> metadata = null);
>if (query=='OK')
>Debug.Log('Payment initiated Successfully');
>else Debug.Log(query);

#####Airtime with Phone Number in Unity3D
Send airitme to a particular phone number
>string query = at_u.plainAirtime(string username, string apikey, string phoneNumber, string amount, string currency = "KES");
>if (query=='OK')
>Debug.Log('Airtime sent Successfully');
>else Debug.Log(query);

#####B2C with Phone Number in Unity3D
Reward your players for their dedication to the game by sending them some mpesa.
>string query = at_u.plainB2C(string username, string apikey, string phone, decimal amount, string reason, string productName, string currencyCode = "KES");
>if (query=='OK')
>Debug.Log('Money sent Successfully');
>else Debug.Log(query);

iv. Leaderboard Functions help you to make use of the firebase database without having to worry about the strucutre or any backend. All you need to do is call a function and your data is saved.

#####Set Leaderboard
If you want to put in one highscore and constantly update the same record over time, this function is best suited for that.  
>string query = at_u.setLeaderboard(string game_id, string gamer_id, int main, int minor, float others);
>if (query=='OK')
>Debug.Log('Data updated successfully');
>else if (query =='+OK')
>Debug.Log('New Data added');
>else Debug.Log(query);

#####Add Leaderboard
This function adds your records irregardless. Useful for cases where your game may need duplicate highscores for players. It returns OK or error message.
>string query = at_u.addLeaderboard(string game_id, string gamer_id, int main, int minor, float others);
>if (query=='OK')
>Debug.Log('Record added Successfully');
>else Debug.Log(query);

#####Get my leaderboard record
This function returns a json array leaderboard response for the game requested and the size requested. the format of the objects returned is as follows:
class LeaderboardResponse
{
    public string date;
    public string gamer_name;
    public int main;
    public int minor;
    public float others;
}

But the json is returned in an array so you will have to create an array object to retrieve the data. **Note** that it is always more advisable to generate your leaderboard using the getGameCount(); function so that you dont get errors when querying for a larger file. We would also have to use Newtonsoft.Json.
>using Newtonsoft.Json;
Then we create our leaderboard response class
>class LeaderboardResponse
>{
>    public string date;
>    public string gamer_name;
>    public int main;
>    public int minor;
>    public float others;
>}

>LeaderboardResponse[] response = new LeaderboardResponse[limit];
>for(int i=0;i<limit;i++)
>{
>  response[i] = new LeaderboardResponse();
>}

>int limit = 10; //The size we would normally query
>
>//Getting the number of entries we have in the database.
>int gameCount = at_u.getGameCount(string gamer_id,string game_id);
>
>if(gameCount<limit)
>limit = gameCount;
>string query = at_u.getLeaderboard(string game_id,int limit);

>response = JsonConvert.DeserializeObject<LeaderboardResponse[]>(query);

>//Generate leaderboard using the object

###Contributions
Please feel free to make any contributions to the project.