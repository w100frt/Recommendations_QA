using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using SeleniumProject;
using OpenQA.Selenium;
using log4net;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace SeleniumProject.Function
{
	public class Script : ScriptingInterface.IScript
	{		
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public void Execute(DriverManager driver, TestStep step)
		{
			long order = step.Order;
			string wait = step.Wait != null ? step.Wait : "";
			List<TestStep> steps = new List<TestStep>();
			IWebElement ele;
			string data = "";
			string xpath = "";
			int count = 0;
			//List of necessary data
			List<string> activeTrainingJobConfigIDArray= new List<string>();
			List<string> inactiveTrainingJobConfigIDArray = new List<string>();
            List<string> activeTrainingJobConfigTypeIDArray = new List<string>();
            List<string> inactiveTrainingJobConfigTypeIDArray = new List<string>();
            List<string> activeJobSettingsArray = new List<string>();
            List<string> inactiveJobSettingsArray= new List<string>();
            List<string> activeModelConfigIDArray = new List<string>();
            List<string> inactiveModelConfigIDArray = new List<string>();
            List<string> activeNameArray = new List<string>();
            List<string> inactiveNameArray = new List<string>();
			//Fetching json file
			JObject json = new JObject();
			using (var webClient = new System.Net.WebClient()) {
				var jsonString = webClient.DownloadString("http://recspublicdev-1454793804.us-east-2.elb.amazonaws.com/v2/training/job/configuration");
				json = JObject.Parse(jsonString);
			}
			//Getting data
			foreach (JToken x in json["result"]) {
				if (x["ConfigurationStatus"].ToString().Equals("active")){
					//Active TrainingJobConfigurationID
					if (x["TrainingJobConfigurationID"] != null) {
						activeTrainingJobConfigIDArray.Add(x["TrainingJobConfigurationID"].ToString());
					}
					else {
						activeTrainingJobConfigIDArray.Add("");
					}
					//Active Name
					if (x["Name"] != null) {
						activeNameArray.Add(x["Name"].ToString());
					}
					else {
						activeNameArray.Add("");
					}
					//Active TrainingJobConfigurationTypeID
					if (x["TrainingJobConfigurationTypeID"] != null) {
						activeTrainingJobConfigTypeIDArray.Add(x["TrainingJobConfigurationTypeID"].ToString());
					}
					else {
						activeTrainingJobConfigTypeIDArray.Add("");
					}
					//Active JobSettings
					if (x["JobSettings"] != null) {
						activeJobSettingsArray.Add(x["JobSettings"].ToString());
					}
					else {
						activeJobSettingsArray.Add("");
					}
					//Active ModelConfigurationID
					if (x["ModelConfigurationID"] != null) {
						activeModelConfigIDArray.Add(x["ModelConfigurationID"].ToString());
					}
					else {
						activeModelConfigIDArray.Add("");
					}
				}
			}
			Dictionary<string, string[]> dataDictionary = new Dictionary<string, string[]>();
			dataDictionary.Add("activeTrainingJobConfigIDArray", activeTrainingJobConfigIDArray.ToArray());
			dataDictionary.Add("activeNameArray", activeNameArray.ToArray());
			dataDictionary.Add("TrainingJobConfigurationTypeID", activeTrainingJobConfigTypeIDArray.ToArray());
			dataDictionary.Add("activeJobSettingsArray", activeJobSettingsArray.ToArray());
			dataDictionary.Add("activeModelConfigIDArray", activeModelConfigIDArray.ToArray());
			VerifyError err = new VerifyError();
			ReadOnlyCollection<IWebElement> elements;
			if (step.Name.Equals("Check Training Job ID")) {
				string[] id = dataDictionary["activeTrainingJobConfigIDArray"];
				elements = driver.FindElements("xpath", "/html/body/div/main/div/div[1]/table/tbody/tr/td[1]");
				if (elements.Count > 0) {
					for (int i=0; i< elements.Count; i++) {
						if (elements.ElementAt(i).GetAttribute("innerText").Equals(id[i])) {
							log.Info("Match! " + "Expected: "+id[i] + "  Actual: "+ elements.ElementAt(i).GetAttribute("innerText"));
						}
						else {
							err.CreateVerificationError(step, "Expected ID: " + id[i], "Actual ID: "+elements.ElementAt(i).GetAttribute("innerText"));
						}
					}
				}
				else {
					log.Error("Can't find ID values");
				}
			}
			else if (step.Name.Equals("Check Training Name")) {
				string[] id = dataDictionary["activeNameArray"];
				elements = driver.FindElements("xpath", "/html/body/div/main/div/div[1]/table/tbody/tr/td[3]");
				if (elements.Count > 0) {
					for (int i=0; i< elements.Count; i++) {
						if (elements.ElementAt(i).GetAttribute("innerText").Equals(id[i])) {
							log.Info("Match! " + "Expected: "+id[i] + "  Actual: "+ elements.ElementAt(i).GetAttribute("innerText"));
						}
						else {
							err.CreateVerificationError(step, "Expected Training Name: " + id[i], "Actual Training Name: "+elements.ElementAt(i).GetAttribute("innerText"));
						}
					}
				}
				else {
					log.Error("Can't Find Traning Name Value(s)");
				}
			}

        }
    }
}