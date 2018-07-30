using UnityEngine.UI;
using UnityEngine;
using AfricasTalkingUnityClass;

public class LoginLogic : MonoBehaviour {
	
	public Text username;
	public InputField loginCreds,loginPassword;
	public InputField regName,regEmail,regPhone,regPass;
	public Dropdown regLoc;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Login()
	{
		var at_u = new AfricasTalkingUnityGateway();
		string x = loginCreds.text;
		string y = loginPassword.text;

		string request = at_u.login(x,y);
		if(request.Substring(0,2)=="OK")
		{
			string token = request.Substring(2,request.Length-2);
			PlayerPrefs.SetString("token",token);
			username.text = at_u.retrieveUsername(token);
		}
		else
		{
			username.text = request;
		}
	}

	public void Register()
	{
		var at_u = new AfricasTalkingUnityGateway();
		string a = regEmail.text;
		string b = determineValue(regLoc.value);
		string c = regName.text;
		string d = regPass.text;
		string e = regPhone.text;
		string request = at_u.signUp(a,b,c,d,e);
		if(request=="OK")
		{
			username.text = "Successful Registration";
			Login();
		}
		else
		{
			username.text = request;
		}
	}

	string determineValue(int i)
	{
		if(i==0)
		return "Kenya";
		else if(i==1)
		return "Nigeria";
		else if(i==2)
		return "Uganda";
		else
		return "Null";
	}


}
