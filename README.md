# AfricasTalkingFirebaseUnityLibrary
Hey guys!! So this project is a library that is meant to help Unity3D developers make use of Africa's Talking APIs for payment, sms and ussd. There is also an aspect for Firebase so you can save leaderboards and even users' information using this library. Please bear in mind it is still in the beta phase and as such is not recommended yet for production.

### How it works
- Switch your Unity runtime version to 4x by going to Edit/Player/Other Settings then change Scripting Runtime to 4x
- Import the library and its dependencies.
- Add "using AfricasTalkingUnityClass;" to the script you intend to use it in
- Declare an instance of the class "var at_u = new AfricasTalkingUnityGateway();"
- Feel free to call the functions available
- Note: the functions return string value. The responses will be looked at below


### Responses
string credentials = "email/phone";
signUp()-> returns OK or error message
Login(credentials, password)->returns OKgamer_id or error message



###Contributions