using System;
using System.Globalization;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using OpenQA.Selenium;
using log4net;
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
			VerifyError err = new VerifyError();

			string[] golfGroups = {"PGA TOUR", "LPGA TOUR", "EUROPEAN TOUR", "KORN FERRY TOUR", "CHAMPIONS TOUR"};
			
			if (step.Name.Equals("Verify Golf Groups")) {
				data = "//div[contains(@class,'scores-home-container')]//div[contains(@class,'dropdown')]//ul//li";
				steps.Add(new TestStep(order, "Verify Number of Groups", "34", "verify_count", "xpath", data, wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
				
				var groups = driver.FindElements("xpath", data); 
				for (int i = 0; i < groups.Count; i++) {
					if (golfGroups[i].Equals(groups[i].GetAttribute("innerText"))) {
						log.Info("Success. " + golfGroups[i] + " matches " + groups[i].GetAttribute("innerText"));
					}
					else {
						log.Error("***Verification FAILED. Expected data [" + golfGroups[i] + "] does not match actual data [" + groups[i].GetAttribute("innerText") + "] ***");
						err.CreateVerificationError(step, golfGroups[i], groups[i].GetAttribute("innerText"));
					}
				}
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}

		}
	}
}