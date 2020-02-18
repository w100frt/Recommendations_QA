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
			var element = driver.FindElement("xpath", "//input[@placeholder='Search a provider']");
			element.SendKeys(Keys.DELETE);
		}
	}
}