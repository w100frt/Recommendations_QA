using System;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using OpenQA.Selenium;

namespace SeleniumProject.Function
{
	public class Script : ScriptingInterface.IScript
	{
		public void Execute(DriverManager driver, TestStep step)
		{
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds;
            long order = step.Order;
            var emailPrefix = secondsSinceEpoch;
            string email = emailPrefix + "@WRIGHT.EDU";
            string wait = step.Wait != null ? step.Wait : "";
            DataManager.CaptureMap.Add("EmailAddress", email);

            List<TestStep> steps = new List<TestStep>();
            steps.Add(new TestStep(order, "Input Generated Email", DataManager.CaptureMap["EmailAddress"], "input_text", step.By, "//input[@type='email']", wait));
            TestRunner.RunTestSteps(driver, null, steps);
		}
	}
}