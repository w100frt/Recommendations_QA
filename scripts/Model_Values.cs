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
            var path = Path.Combine(@"‎⁨https://api.getpostman.com/collections/5575080-e70b2cd0-dfdd-4271-958f-b3e35f52f31b?apikey=PMAK-6033bed1cf4d87004e6f991d-130a848a2266fa5f04478f0fc80b91c6ce");
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
        }
    }
}