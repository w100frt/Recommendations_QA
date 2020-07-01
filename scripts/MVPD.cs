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
			int size = 0;
			int total;
			List<TestStep> steps = new List<TestStep>();
			VerifyError err = new VerifyError();
			
			if (step.Name.Equals("Verify Provider Count")) {
				try {
					total = Int32.Parse(step.Data);
				}
				catch (Exception e) {
					total = 500;
					log.Error("Expected data to be a numeral. Setting data to 500.");
				}
			
				size = driver.FindElements("xpath", "//div[contains(@class,'mvpd-item-groups')]//h2").Count;
				
				if (size >= total) {
					log.Info("Verification PASSED. Total Providers [" + size + "] is greater than " + total);
				}
				else {
					log.Error("***Verification FAILED. " + size + " is not greater than " + total +"***");
					err.CreateVerificationError(step, "> " + total, size.ToString());
				}
			}
		}
	}
}