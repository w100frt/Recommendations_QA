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
			List<string> activeTrainJobConfigIDArray= new List<string>();
			List<string> inactiveTrainJobConfigIDArray = new List<string>();
            List<string> activeTrainJobConfigTypeIDArray = new List<string>();
            List<string> inactiveTrainJobConfigTypeIDArray = new List<string>();
            List<string> activeJobSettingsArray = new List<string>();
            List<string> inactiveJobSettingsArray= new List<string>();
            List<string> activeTriggerTrainArray = new List<string>();
            List<string> inactiveTriggerTrainArray = new List<string>();
            List<string> activeTrainConfigIDArray = new List<string>();
            List<string> inactiveTrainConfigIDArray = new List<string>();
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
						activeTrainJobConfigIDArray.Add(x["TrainingJobConfigurationID"].ToString());
					}
					else {
						activeTrainJobConfigIDArray.Add("");
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
						activeTrainJobConfigTypeIDArray.Add(x["TrainingJobConfigurationTypeID"].ToString());
					}
					else {
						activeTrainJobConfigTypeIDArray.Add("");
					}
					//Ative JobSettings
					if (x["JobSettings"] != null) {
						activeJobSettingsArray.Add(x["JobSettings"].ToString());
					}
					else {
						activeJobSettingsArray.Add("");
					}
					//Active TriggerTrainingJobConfigurationID
					if (x["TriggerTrainingJobConfigurationID"] != null) {
						activeTriggerTrainArray.Add(x["TriggerTrainingJobConfigurationID"].ToString());
					}
					else {
						activeTriggerTrainArray.Add("");
					}
                    //Active TrainingConfigurationID
                    if (x["TrainingConfigurationID"] != null) {
						activeTrainConfigIDArray.Add(x["TrainingConfigurationID"].ToString());
					}
					else {
						activeTrainConfigIDArray.Add("");
					}
				}
			}
			Dictionary<string, string[]> dataDictionary = new Dictionary<string, string[]>();
			dataDictionary.Add("activeTrainJobConfigIDArray", activeTrainJobConfigIDArray.ToArray());
			dataDictionary.Add("activeNameArray", activeNameArray.ToArray());
			dataDictionary.Add("activeTrainJobConfigTypeIDArray", activeTrainJobConfigTypeIDArray.ToArray());
			dataDictionary.Add("activeJobSettingsArray", activeJobSettingsArray.ToArray());
			dataDictionary.Add("activeTriggerTrainArray", activeTriggerTrainArray.ToArray());
            dataDictionary.Add("activeTrainConfigIDArray", activeTrainConfigIDArray.ToArray());
			VerifyError err = new VerifyError();
			ReadOnlyCollection<IWebElement> elements;
			if (step.Name.Equals("Check Training Job ID")) {
				string[] id = dataDictionary["activeTrainJobConfigIDArray"];
				elements = driver.FindElements("xpath", "/html/body/div/main/div/div[2]/table/tbody/tr/td[1]");
				if (elements.Count > 0) {
					for (int i=0; i< elements.Count; i++) {
						if (elements.ElementAt(i).GetAttribute("innerText").Equals(id[i])) {
							log.Info("Match! " + "Expected: "+id[i] + "  Actual: "+ elements.ElementAt(i).GetAttribute("innerText"));
						}
						else {
							err.CreateVerificationError(step, "Expected Training Job ID: " + id[i], "Actual Training Job ID: "+elements.ElementAt(i).GetAttribute("innerText"));
						}
					}
				}
				else {
					log.Error("Can't find Training Job ID values");
				}
			}
			else if (step.Name.Equals("Check Training Name")) {
				string[] id = dataDictionary["activeNameArray"];
				elements = driver.FindElements("xpath", "/html/body/div/main/div/div[2]/table/tbody/tr/td[2]");
				if (elements.Count > 0) {
					for (int i=0; i< elements.Count; i++) {
						if (elements.ElementAt(i).GetAttribute("innerText").Equals(id[i])) {
							log.Info("Match! " + "Expected: "+id[i] + "  Actual: "+ elements.ElementAt(i).GetAttribute("innerText"));
						}
						else {
							err.CreateVerificationError(step, "Expected Training Job Name: " + id[i], "Actual Training Job Name: "+elements.ElementAt(i).GetAttribute("innerText"));
						}
					}
				}
				else {
					log.Error("Can't find Training Job Name values");
				}
			}
			else if (step.Name.Equals("Check Training Configuration ID")) {
				string[] id = dataDictionary["activeTrainConfigIDArray"];
				elements = driver.FindElements("xpath", "/html/body/div/main/div[4]/div[2]/span");
				if (elements.Count > 0) {
					for (int i=0; i< elements.Count; i++) {
						if (elements.ElementAt(i).GetAttribute("innerText").Equals(id[i])) {
							log.Info("Match! " + "Expected: "+id[i] + "  Actual: "+ elements.ElementAt(i).GetAttribute("innerText"));
						}
						else {
							err.CreateVerificationError(step, "Expected Training Config ID: " + id[i], "Actual Training Config ID: "+elements.ElementAt(i).GetAttribute("innerText"));
						}
					}
				}
				else {
					log.Error("Can't find Training Job Name values");
				}
			}
			else if (step.Name.Equals("Check Trigger Training Job Config ID")) {
				string[] id = dataDictionary["activeTriggerTrainArray"];
				for (int i=0; i< id.Length; i++) {
					steps.Add(new TestStep(order, "Click Training Job with ID: "+ dataDictionary["activeTrainJobConfigIDArray"][i], "", "click", "xpath", "/html/body/div/main/div/div[2]/table/tbody/tr/td[1][1][.='"+ dataDictionary["activeTrainJobConfigIDArray"][i]+"']/a", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
					elements = driver.FindElements("xpath", "/html/body/div/main/div/div[3]/div[5]/div/div[3]");
					if (elements.Count > 0) {
						if (elements.ElementAt(i).GetAttribute("innerText").Equals(id[i])) {
							log.Info("Match! " + "Expected: "+id[i] + "  Actual: "+ elements.ElementAt(i).GetAttribute("innerText"));
						}
						else {
							err.CreateVerificationError(step, "Expected Trigger Training Job ID: " + id[i], "Actual Trigger Training Job ID: "+elements.ElementAt(i).GetAttribute("innerText"));
						}
					}
					else {
						log.Error("Can't find Trigger Training Job ID values");
					}
				}
			}
        }
    }
}