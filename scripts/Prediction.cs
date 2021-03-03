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
			List<string> activePredConfigIDArray= new List<string>();
			List<string> inactivePredConfigIDArray = new List<string>();
            List<string> activePredictionKeyIDArray = new List<string>();
            List<string> inactivePredictionKeyIDArray = new List<string>();
            List<string> activePredictionEntityMappingIDArray = new List<string>();
            List<string> inactivePredictionEntityMappingiDArray = new List<string>();
            List<string> activeModelConfigIDArray = new List<string>();
            List<string> inactiveModelConfigIDArray = new List<string>();
            List<string> activeNameArray = new List<string>();
            List<string> inactiveNameArray = new List<string>();
			//Fetching json file
			JObject json = new JObject();
			using (var webClient = new System.Net.WebClient()) {
				var jsonString = webClient.DownloadString("http://recspublicdev-1454793804.us-east-2.elb.amazonaws.com/v2/prediction/configuration");
				json = JObject.Parse(jsonString);
			}
			//Getting data
			foreach (JToken x in json["result"]) {
				if (x["Status"].ToString().Equals("active")){
					//Active Pred Congif ID
					if (x["PredictionConfigurationID"] != null) {
						activePredConfigIDArray.Add(x["PredictionConfigurationID"].ToString());
					}
					else {
						activePredConfigIDArray.Add("");
					}
					//Active PredictionKeyID
					if (x["PredictionKeyID"]!= null) {
						activePredictionKeyIDArray.Add(x["PredictionKeyID"].ToString());
					}
					else {
						activePredictionKeyIDArray.Add("");
					}
					//Active PredictionEntityMappingID
					if (x["PredictionEntityMappingID"] != null) {
						activePredictionEntityMappingIDArray.Add(x["PredictionEntityMappingID"].ToString());
					}
					else {
						activePredictionEntityMappingIDArray.Add("");
					}
					//Ative ModelConfigurationID
					if (x["ModelConfigurationID"] != null) {
						activeModelConfigIDArray.Add(x["ModelConfigurationID"].ToString());
					}
					else {
						activeModelConfigIDArray.Add("");
					}
					//Active Name
					if (x["Name"] != null) {
						activeNameArray.Add(x["Name"].ToString());
					}
					else {
						activeNameArray.Add("");
					}
				}
			}
			Dictionary<string, string[]> dataDictionary = new Dictionary<string, string[]>();
			dataDictionary.Add("activePredConfigIDArray", activePredConfigIDArray.ToArray());
			dataDictionary.Add("activePredictionKeyIDArray", activePredictionKeyIDArray.ToArray());
			dataDictionary.Add("activePredictionEntityMappingIDArray", activePredictionEntityMappingIDArray.ToArray());
			dataDictionary.Add("activeModelConfigIDArray", activeModelConfigIDArray.ToArray());
			dataDictionary.Add("activeNameArray", activeNameArray.ToArray());
			VerifyError err = new VerifyError();
			ReadOnlyCollection<IWebElement> elements;
			if (step.Name.Equals("Check Prediction ID")) {
				string[] id = dataDictionary["activePredConfigIDArray"];
				elements = driver.FindElements("xpath", "/html/body/div/main/div/div[2]/table/tbody/tr/td[1]");
				if (elements.Count > 0) {
					for (int i=0; i< elements.Count; i++) {
						if (elements.ElementAt(i).GetAttribute("innerText").Equals(id[i])) {
							log.Info("Match! " + "Expected: "+id[i] + "  Actual: "+ elements.ElementAt(i).GetAttribute("innerText"));
						}
						else {
							err.CreateVerificationError(step, "Expected Prediction ID: " + id[i], "Actual Prediction ID: "+elements.ElementAt(i).GetAttribute("innerText"));
						}
					}
				}
				else {
					log.Error("Can't find Prediction ID values");
				}
			}
			if (step.Name.Equals("Check Prediction Configuration ID")) {
				string[] id = dataDictionary["activePredConfigIDArray"];
				elements = driver.FindElements("xpath", "/html/body/div/main/div[4]/div[2]/span");
				if (elements.Count > 0) {
					for (int i=0; i< elements.Count; i++) {
						if (elements.ElementAt(i).GetAttribute("innerText").Equals(id[i])) {
							log.Info("Match! " + "Expected: "+id[i] + "  Actual: "+ elements.ElementAt(i).GetAttribute("innerText"));
						}
						else {
							err.CreateVerificationError(step, "Expected Prediction ID: " + id[i], "Actual Prediction ID: "+elements.ElementAt(i).GetAttribute("innerText"));
						}
					}
				}
				else {
					log.Error("Can't find Prediction Configuration ID values");
				}
			}
			else if (step.Name.Equals("Check Prediction Key")) {
				string[] id = dataDictionary["activePredictionKeyIDArray"];
				elements = driver.FindElements("xpath", "/html/body/div[1]/main/div/div[2]/table/tbody/tr/td[3]/div/div/div/div/div[2]/form/div[1]/span");
				if (elements.Count > 0) {
					for (int i=0; i< elements.Count; i++) {
						if (elements.ElementAt(i).GetAttribute("innerText").Equals(id[i])) {
							log.Info("Match! " + "Expected: "+id[i] + "  Actual: "+ elements.ElementAt(i).GetAttribute("innerText"));
						}
						else {
							err.CreateVerificationError(step, "Expected Prediction Key ID: " + id[i], "Actual Prediction Key ID: "+elements.ElementAt(i).GetAttribute("innerText"));
						}
					}
				}
				else {
					log.Error("Can't find Prediction Key ID values");
				}
			}
			else if (step.Name.Equals("Check Prediction Entity Mapping ID")) {
				string[] id = dataDictionary["activePredictionEntityMappingIDArray"];
				elements = driver.FindElements("xpath", "/html/body/div[1]/main/div/div[2]/table/tbody/tr/td[4]/div/div/div/div/div[2]/form/div[1]/span");
				if (elements.Count > 0) {
					for (int i=0; i< elements.Count; i++) {
						if (elements.ElementAt(i).GetAttribute("innerText").Equals(id[i])) {
							log.Info("Match! " + "Expected: "+id[i] + "  Actual: "+ elements.ElementAt(i).GetAttribute("innerText"));
						}
						else {
							err.CreateVerificationError(step, "Expected Prediction Entity Mapping ID: " + id[i], "Actual  Prediction Entity Mapping ID: "+elements.ElementAt(i).GetAttribute("innerText"));
						}
					}
				}
				else {
					log.Error("Can't find Prediction Entity Mapping ID values");
				}
			}

        }
    }
}