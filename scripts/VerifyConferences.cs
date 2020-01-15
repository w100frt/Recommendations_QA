using System;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using SeleniumProject;
using OpenQA.Selenium;

namespace SeleniumProject.Function
{
	public class Script : ScriptingInterface.IScript
	{
		public void Execute(DriverManager driver, TestStep step)
		{
			long order = step.Order;
			string wait = step.Wait != null ? step.Wait : "";
            List<TestStep> steps = new List<TestStep>();
            steps.Add(new TestStep(order, "Open Conference Dropdown", "", "click", "xpath", "//a[@class='dropdown-menu-title']", wait));
            TestRunner.RunTestSteps(driver, null, steps);
		}
	}
}