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
			List<string> activeModelConfigIDArray = new List<string>();
			List<string> inactiveModelConfigIDArray = new List<string>();
			List<string> activeAlgoSpecIDArray = new List<string>();
			List<string> inactiveAlgoSpecIDArray = new List<string>();
			List<string> activeTrainKeyIDArray = new List<string>();
			List<string> inactiveTrainKeyIDArray = new List<string>();
			List<string> activeHyperParamIDArray = new List<string>();
			List<string> inactiveHyperParamIDArray = new List<string>();
			List<string> activeHyperParamsArray = new List<string>();
			List<string> inactiveHyperParamsArray = new List<string>();
			List<string> activeModelConfigNameArray = new List<string>();
			List<string> inactiveModelConfigNameArray = new List<string>();
			//Fetching json file
			JObject json = new JObject();
			using (var webClient = new System.Net.WebClient()) {
				var jsonString = webClient.DownloadString("http://recspublicdev-1454793804.us-east-2.elb.amazonaws.com/v2/model/configuration");
				json = JObject.Parse(jsonString);
			}
			//Getting data
			foreach (JToken x in json["result"]) {
				if (x["Status"].ToString().Equals("active")){
					//Active ModelConfigurationID
					if (x["ModelConfigurationID"] != null) {
						activeModelConfigIDArray.Add(x["ModelConfigurationID"].ToString());
					}
					else {
						activeModelConfigIDArray.Add("");
					}
					//Active AlgoSpecID
					if (x["AlgoSpecificationID"] != null) {
						activeAlgoSpecIDArray.Add(x["AlgoSpecificationID"].ToString());
					}
					else {
						activeAlgoSpecIDArray.Add("");
					}
					//Active TrainingKeyID
					if (x["TrainingKeyID"] != null) {
						activeTrainKeyIDArray.Add(x["TrainingKeyID"].ToString());
					}
					else {
						activeTrainKeyIDArray.Add("");
					}
					//Ative HyperparameterID
					if (x["HyperparameterID"] != null) {
						activeHyperParamIDArray.Add(x["HyperparameterID"].ToString());
					}
					else {
						activeHyperParamIDArray.Add("");
					}
					//Active Hyperparameters
					if (x["Hyperparameters"] != null) {
						activeHyperParamsArray.Add(x["Hyperparameters"].ToString());
					}
					else {
						activeHyperParamsArray.Add("");
					}
					//Active ModelConfigurationName
					if (x["ModelConfigurationName"] != null) {
						activeModelConfigNameArray.Add(x["ModelConfigurationName"].ToString());
					}
					else {
						activeModelConfigNameArray.Add("");
					}
				}
			}
			Dictionary<string, string[]> dataDictionary = new Dictionary<string, string[]>();
			dataDictionary.Add("activeModelConfigID", activeModelConfigIDArray.ToArray());
			dataDictionary.Add("activeAlgoSpecID", activeAlgoSpecIDArray.ToArray());
			dataDictionary.Add("activeTrainingKeyID", activeTrainKeyIDArray.ToArray());
			dataDictionary.Add("activeHyperparameterID", activeHyperParamIDArray.ToArray());
			dataDictionary.Add("activeHyperparameters", activeHyperParamsArray.ToArray());
			dataDictionary.Add("activeModelConfigName", activeModelConfigNameArray.ToArray());
			VerifyError err = new VerifyError();
			ReadOnlyCollection<IWebElement> elements;
			if (step.Name.Equals("Check Model ID")) {
				string[] id = dataDictionary["activeModelConfigID"];
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
			else if (step.Name.Equals("Check Model Name")) {
				string[] id = dataDictionary["activeModelConfigName"];
				elements = driver.FindElements("xpath", "/html/body/div/main/div/div[1]/table/tbody/tr/td[2]");
				if (elements.Count > 0) {
					for (int i=0; i< elements.Count; i++) {
						if (elements.ElementAt(i).GetAttribute("innerText").Equals(id[i])) {
							log.Info("Match! " + "Expected: "+id[i] + "  Actual: "+ elements.ElementAt(i).GetAttribute("innerText"));
						}
						else {
							err.CreateVerificationError(step, "Expected Name: " + id[i], "Actual Name: "+elements.ElementAt(i).GetAttribute("innerText"));
						}
					}
				}
				else {
					log.Error("Can't find Name values");
				}
			}
			else if (step.Name.Equals("Check Model Alg Spec")) {
				string[] id = dataDictionary["activeAlgoSpecID"];
				elements = driver.FindElements("xpath", "/html/body/div[1]/main/div/div[1]/table/tbody/tr/td[3]/div/div/div/div/div[2]/form/div[1]/span");
				if (elements.Count > 0) {
					for (int i=0; i< elements.Count; i++) {
						if (elements.ElementAt(i).GetAttribute("innerText").Equals(id[i])) {
							log.Info("Match! " + "Expected: "+id[i] + "  Actual: "+ elements.ElementAt(i).GetAttribute("innerText"));
						}
						else {
							err.CreateVerificationError(step, "Expected Alg Spec: " + id[i], "Actual Alg Spec: "+elements.ElementAt(i).GetAttribute("innerText"));
						}
					}
				}
				else {
					log.Error("Can't find Alg Spec values");
				}
			}
			else if (step.Name.Equals("Check Model Training Key")) {
				string[] id = dataDictionary["activeTrainingKeyID"];
				elements = driver.FindElements("xpath", "/html/body/div[1]/main/div/div[1]/table/tbody/tr/td[4]/div/div/div/div/div[2]/form/div[1]/span");
				if (elements.Count > 0) {
					for (int i=0; i< elements.Count; i++) {
						if (elements.ElementAt(i).GetAttribute("innerText").Equals(id[i])) {
							log.Info("Match! " + "Expected: "+id[i] + "  Actual: "+ elements.ElementAt(i).GetAttribute("innerText"));
						}
						else {
							err.CreateVerificationError(step, "Expected Training Key: " + id[i], "Actual Training Key: "+elements.ElementAt(i).GetAttribute("innerText"));
						}
					}
				}
				else {
					log.Error("Can't find Training Key values");
				}
			}
			else if (step.Name.Equals("Check Model Hyperparameter Key")) {
				string[] id = dataDictionary["activeHyperparameterID"];
				elements = driver.FindElements("xpath", "/html/body/div/main/div/div[1]/table/tbody/tr/td[5]");
				if (elements.Count > 0) {
					for (int i=0; i< elements.Count; i++) {
						if (elements.ElementAt(i).GetAttribute("innerText").Equals(id[i])) {
							log.Info("Match! " + "Expected: "+id[i] + "  Actual: "+ elements.ElementAt(i).GetAttribute("innerText"));
						}
						else {
							err.CreateVerificationError(step, "Expected Hyperparam Key: " + id[i], "Actual Hyperparam Key: "+elements.ElementAt(i).GetAttribute("innerText"));
						}
					}
				}
				else {
					log.Error("Can't find Hyperparameter Key values");
				}
			}
        }
    }
}