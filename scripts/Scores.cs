using System;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using OpenQA.Selenium;
using log4net;

namespace SeleniumProject.Function
{
	public class Script : ScriptingInterface.IScript
	{		
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public void Execute(DriverManager driver, TestStep step)
		{
            TimeSpan time = DateTime.Now.TimeOfDay;
			var now = time.Hours;
			if (now < 11)
				step.Data = "YESTERDAY";
			else 
				step.Data = "TODAY";
			
			long order = step.Order;
            string wait = step.Wait != null ? step.Wait : "";
            List<TestStep> steps = new List<TestStep>();
			steps.Add(new TestStep(order, "Verify Displayed Day on Top Scores", step.Data, "verify_value", "xpath", "//div[contains(@class,'scores-date')]//div[contains(@class,'sm')]", wait));
			TestRunner.RunTestSteps(driver, null, steps);
			steps.Clear();
		}
	}
}