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
			string size = "";
			int total = 0;
			List<TestStep> steps = new List<TestStep>();
			VerifyError err = new VerifyError();

			size = driver.FindElement("xpath", "//pre").Text;
			size = size.Substring(size.IndexOf("Total Entities=") + 15);
			log.Info(size);
			
			try {
				total = Int32.Parse(size);
			}
			catch (Exception e){
				log.Error("Expected data to be a numeral. Setting data to 0.");
			}
			
			if (total > 40000) {
				log.Info("Verification PASSED. Total Entities equals " + total);
			}
			else {
				log.Error("***Verification FAILED. " + total + " is not greater than 40,000 ***");
				err.CreateVerificationError(step, "> 40,000", total.ToString());
			}
		}
	}
}