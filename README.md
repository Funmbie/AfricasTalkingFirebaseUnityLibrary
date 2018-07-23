# AfricasTalkingFirebaseUnityLibrary
Hey guys!! So this project is a library that is meant to help Unity3D developers make use of Africa's Talking APIs for payment, sms and ussd. There is also an aspect for Firebase we are experimenting with so you can save leaderboards and even users' information using this library. Please bear in mind it is still in the beta phase and as such is not recommended yet for production.

### How it works
- Switch your Unity runtime version to 4x by going to Edit/Player/Other Settings then change Scripting Runtime to 4x
- Import the library and its dependencies.
- Add "using AfricasTalkingUnityClass;" to the script you intend to use it in
- Declare an instance of the class "var at_u = new AfricasTalkingUnityGateway();"
- Feel free to call the functions available
- Note: the functions return string value. The responses will be looked at below

### Functions and Responses
i. Game Functions for registering and deleting your game. These functions should be called only once. Also keep your game id safe to ensure that it can't be used by anyone else.
- at_u.registerGame(): 

ii. User Functions will help you in profile management. Ensure the users enter valid emails. Before submitting to the library to prevent errors.
- at_u.signUp(string email, string location, string name,string password, string phone): This function adds a user with the following parameters and returns an OK or error message.
- at_u.login(string credentials, string password): Here you are able to log the user in using phone number or email. It returns OKGamer_id or error message. The gamer id should be retreieved and stored temporarily as it will be used to query the database on the gamer's behalf.
- at_u.retrieveUsername(string gamer_id): use the gamer's id gotten from the login to retrieve his/her username. It returns the username or error message.
- at_u.editDetail(string gamer_id,string key,string value) change a user's 'name', 'phone', 'email' or 'location'. The parameters are the gamer's id, the key is what you want to change and the value is the new value for the key chosen. Note: Do not include comma in your locations. You usually only need to get the country for location. It returns OK or Invalid Request if you're asking for the wrong key or an error message if an error was encountered.
- at_u.Delete(gamer_id): delete a gamer's account. It returns OK or error message.

iii. Africa's Talking API Functions for airtime, mobile checkout and sms.
- at_u.SMS(string username, string apikey, string msg, string gamer_id): sends an sms to the phone number of the gamer id passed. You want to use your Africa's talking username and apikey for the respective parameters then add the message and gamer id. It returns an OK or an error message.
- at_u.inAppPurchase(string username, string apikey,string gamer_id, string productName,string currency,decimal amount, string channel, Dictionary<string,string> metadata = null): This function initiates mobile checkout on the device. It returns OK or error message
- at_u.sendAirtime(string username,string apikey,string gamer_id, string amount, string currency="KES"): This function helps to send airtime as a reward system to a user

iv. Leaderboard Functions help you to make use of the firebase database without having to worry about the strucutre or any backend. All you need to do is call a function and your data is saved.
- at_u.setLeaderboard(string game_id, string gamer_id, int main, int minor, float others): this is a useful way to add and update the same data to avoid duplicate records for your game. It returns OK or error message
- at_u.addLeaderboard(string game_id, string gamer_id, int main, int minor, float others): this function adds your records irregardless. Useful for cases where your game may need duplicate highscores for players. It returns OK or error message.
- at_u.getLeaderboard(string game_id,int limit): this function returns a json array leaderboard response for the game requested and the size requested. the format of the objects returned is as follows:
class LeaderboardResponse
        {
            public string date;
            public string gamer_name;
            public int main;
            public int minor;
            public float others;
        }

But the json is returned in an array so you will have to create an array also

###Contributions