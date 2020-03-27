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
			if (now < 11) {
				log.Info(now);
			}
            string wait = step.Wait != null ? step.Wait : "";
            List<TestStep> steps = new List<TestStep>();
		}
	}
}