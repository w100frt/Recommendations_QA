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
			IWebElement ele;
			int size = 0;
			int attempts = 20;
			string classList = "";
			List<TestStep> steps = new List<TestStep>();
			VerifyError err = new VerifyError();
			
			if (step.Name.Equals("Verify Video is Playing")) {
				ele = driver.FindElement("xpath", "//div[@aria-label='Video Player']");
				classList = ele.GetAttribute("className");
				classList = classList.Substring(classList.IndexOf("jw-state-") + 27);
				do {
					log.Info(classList);					
				}
				while (!classList.Equals("playing") && attempts-- > 0);
			}
		}
	}
}