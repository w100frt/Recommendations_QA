using System;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using SeleniumProject;
using OpenQA.Selenium;
using log4net;

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
            steps.Add(new TestStep(order, "Open Conference Dropdown", "", "click", "xpath", "//a[@class='dropdown-menu-title']", wait));
			steps.Add(new TestStep(order, "Verify Dropdown is Displayed", "", "verify_displayed", "xpath", "//div[@class='scores-home-container']//div[contains(@class,'dropdown')]//ul", wait));
            TestRunner.RunTestSteps(driver, null, steps);
			
			var conferences = driver.FindElements("xpath", "//div[@class='scores-home-container']//div[contains(@class,'dropdown')]//ul//li"); 
			foreach (IWebElement active in conferences) {
				log.Info(active.GetAttribute("innerText"));
			}
			
		}
	}
}