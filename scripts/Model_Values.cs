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
			//Need to change base on local or remote machine
			var path = Path.Combine(Directory.GetCurrentDirectory());
            path = Path.Combine(path, "SeleniumProject/Postman_Collection/report_key.json");
            var result = JObject.Parse(File.ReadAllText(path));
			VerifyError err = new VerifyError();
			ReadOnlyCollection<IWebElement> elements;

			Dictionary<string, string[]> dataDictionary = new Dictionary<string, string[]>();
			foreach (JToken x in result["list"]) {
				dataDictionary.Add(x["key"].ToString(), x["value"].ToString().Trim('[', ']').Split(',').Select(s => s.Trim('"')).ToArray());
				log.Info(dataDictionary.Count());
            }
			if (step.Name.Equals("Check Model ID")) {
				string[] id = dataDictionary["activeModelConfigToCollect"];
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
				string[] id = dataDictionary["activeModelConfigNameToCollect"];
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
				string[] id = dataDictionary["activeAlgoSpecToCollect"];
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
				string[] id = dataDictionary["activeTrainingKeyToCollect"];
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
				string[] id = dataDictionary["activeHyperParamIDToCollect"];
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
			else if (step.Name.Equals("Check Prediction ID")) {
				string[] id = dataDictionary["activePredictionJobConfigIDToCollect"];
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
					log.Error("Can't find Hyperparameter Key values");
				}
			}
			//Going to need to hit Model ID page to verify this. Should be last in Pred Config test.
			else if (step.Name.Equals("Check Prediction Model Name")) {
				string[] id = dataDictionary["activeModelConfigIDToCollect"];
				elements = driver.FindElements("xpath", "/html/body/div/main/div[2]/div[2]/span");
				if (elements.Count > 0) {
					for (int i=0; i< elements.Count; i++) {
						if (elements.ElementAt(i).GetAttribute("innerText").Equals(id[i])) {
							log.Info("Match! " + "Expected Model ID: "+id[i] + "  Actual Model ID: "+ elements.ElementAt(i).GetAttribute("innerText"));
						}
						else {
							err.CreateVerificationError(step, "Expected Model ID: " + id[i], "Actual Model ID: "+elements.ElementAt(i).GetAttribute("innerText"));
						}
					}
				}
				else {
					log.Error("Can't find Hyperparameter Key values");
				}
			}
			else if (step.Name.Equals("Check Prediction Key")) {
				string[] id = dataDictionary["activePredKeyIDToCollect"];
				elements = driver.FindElements("xpath", "/html/body/div[1]/main/div/div[2]/table/tbody/tr/td[3]/div/div/div/div/div[2]/form/div[1]/span");
				if (elements.Count > 0) {
					for (int i=0; i< elements.Count; i++) {
						if (elements.ElementAt(i).GetAttribute("innerText").Equals(id[i])) {
							log.Info("Match! " + "Expected: "+id[i] + "  Actual: "+ elements.ElementAt(i).GetAttribute("innerText"));
						}
						else {
							err.CreateVerificationError(step, "Expected Pred Key: " + id[i], "Actual Pred Key: "+elements.ElementAt(i).GetAttribute("innerText"));
						}
					}
				}
				else {
					log.Error("Can't find Hyperparameter Key values");
				}
			}
			else if (step.Name.Equals("Check Prediction Entity Mapping")) {
				string[] id = dataDictionary["activePredictionEntityMapsToCollect"];
				elements = driver.FindElements("xpath", "/html/body/div[1]/main/div/div[2]/table/tbody/tr/td[4]/div/div/div/div/div[2]/form/div[1]/span");
				if (elements.Count > 0) {
					for (int i=0; i< elements.Count; i++) {
						if (elements.ElementAt(i).GetAttribute("innerText").Equals(id[i])) {
							log.Info("Match! " + "Expected: "+id[i] + "  Actual: "+ elements.ElementAt(i).GetAttribute("innerText"));
						}
						else {
							err.CreateVerificationError(step, "Expected Pred Ent Map: " + id[i], "Actual Pred Ent Map: "+elements.ElementAt(i).GetAttribute("innerText"));
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