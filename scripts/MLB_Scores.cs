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
			IWebElement chip;
			int total;
			int week;
			int months;
			int year;
			string title;
			string date;
			string data = "";
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			VerifyError err = new VerifyError();
			Random random = new Random();
			
			if (step.Name.Equals("Verify MLB Date")) {
				if(DataManager.CaptureMap.ContainsKey("IN_SEASON")) {
					if(bool.Parse(DataManager.CaptureMap["IN_SEASON"])) {
						steps.Add(new TestStep(order, "Verify Displayed Day on Top Scores", "", "script", "xpath", "Scores", wait));
						TestRunner.RunTestSteps(driver, null, steps);
						steps.Clear();
					}
					else {
						steps.Add(new TestStep(order, "Verify Date on Date Picker", "WORLD SERIES", "verify_value", "xpath", "//button[contains(@class,'date-picker-title') or contains(@class,'dropdown-title')]", wait));
						TestRunner.RunTestSteps(driver, null, steps);
						steps.Clear();
					}
				}
				
				else {
					log.Warn("No IN_SEASON variable available. Using data.");
					steps.Add(new TestStep(order, "Run Template", "", "run_template", "xpath", "", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}

		}
	}
}