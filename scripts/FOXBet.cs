using System;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using SeleniumProject;
using OpenQA.Selenium;
using log4net;
using System.Collections.ObjectModel;

namespace SeleniumProject.Function
{
	public class Script : ScriptingInterface.IScript
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		
		public void Execute(DriverManager driver, TestStep step)
		{
			long order = step.Order;
			string wait = step.Wait != null ? step.Wait : "";
			string size = "";
			string data = "";
			int total = 0;
			List<TestStep> steps = new List<TestStep>();
			VerifyError err = new VerifyError();
			
			if (step.Name.Equals("Capture Number of FB Entities")) {
				size = "//a[contains(@id,'event-selection') and not(contains(@class,'disabled'))]";
				total = driver.FindElements("xpath", size).Count;
				
				data = step.Data;
				
				if (String.IsNullOrEmpty(data)) {
					data = "PLAYERS_LISTED";
				}
				DataManager.CaptureMap[data] = total.ToString();
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}