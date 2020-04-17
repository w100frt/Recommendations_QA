using System;
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
			int size;
			VerifyError err = new VerifyError();
			
			if (step.Name.Equals("Verify Scorechip Count")) {
				size = driver.FindElements("xpath", "//div[contains(@class,'score-chip')]").Count;
				if (size > 0 && size <= Int32.Parse(step.Data)) {
					log.Info("Verification Passed. " + size + " is between 0 and " + step.Data); 
				}
				else {
					err.CreateVerificationError(step, "Number Between 0 and " + step.Data, size.ToString());
				}
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}