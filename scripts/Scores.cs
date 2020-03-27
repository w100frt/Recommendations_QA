using System;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using OpenQA.Selenium;

namespace SeleniumProject.Function
{
	public class Script : ScriptingInterface.IScript
	{		
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public void Execute(DriverManager driver, TestStep step)
		{
            DateTime time = DateTime.Now.TimeOfDay;
			var now = time.Hour;
			if (now < 11) {
				log.Info(now);
			}
            string wait = step.Wait != null ? step.Wait : "";
            List<TestStep> steps = new List<TestStep>();
		}
	}
}