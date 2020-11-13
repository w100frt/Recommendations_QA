using System;
using System.Globalization;
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
			IWebElement ele;
			int size = 0;
			var length = 0;
			int count = 0;
			string data = "";
			string test = "";
			bool stop = false;
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			VerifyError err = new VerifyError();
			
			if (step.Name.Equals("Verify PPV Entitlement")) {
				test = (string) js.ExecuteScript("return document.readyState;");
				
				while (!test.Equals("complete") && size++ < 5) {
					log.Info("Waiting for readyState=complete");
					Thread.Sleep(0500);
					test = (string) js.ExecuteScript("return document.readyState;");
				}
				
				length = Convert.ToInt32(js.ExecuteScript("return wisRegistration.getUserEntitlements().then(x => x.ppvEvents.length)"));
				if (length > 0) {
					log.Info("PPV Entitlement Count: " + length);
					for (int i = 0; i < length; i++) {
						test = (string) js.ExecuteScript("return wisRegistration.getUserEntitlements().then(x => x.ppvEvents["+i+"].name)");
						if (step.Data.Equals(test)) {
							log.Info("Verification PASSED Expected Entitlement [" + step.Data + "] matches Actual Entitlement [" + test + "]");
						}
						else {
							log.Error("***Verification FAILED. Expected ["+ step.Data +"] does not match Actual [" + test + "]***");
							err.CreateVerificationError(step, step.Data, test);
						}
					}
				}
				else {
					log.Error("***Verification FAILED. Expected more than 1 PPV entitlement. Current entitlement count: [" + length + "]***");
					err.CreateVerificationError(step, "1 or More PPV", length.ToString());					
				}
			}
			
			else if (step.Name.Equals("Verify PPV Entitlement Count")) {
				test = (string) js.ExecuteScript("return document.readyState;");
				
				while (!test.Equals("complete") && size++ < 5) {
					log.Info("Waiting for readyState=complete");
					Thread.Sleep(0500);
					test = (string) js.ExecuteScript("return document.readyState;");
				}
				
				length = Convert.ToInt32(js.ExecuteScript("return wisRegistration.getUserEntitlements().then(x => x.ppvEvents.length)"));
				count = Convert.ToInt32(step.Data);
				
				if (count == length) {
					log.Info("Verification PASSED. Expected entitlement count [" + count + "] matches Actual entitlement count: [" + length + "]");
				}
				else {
					log.Error("***Verification FAILED. Expected entitlement count [" + count + "] does not match Actual entitlement count: [" + length + "]***");
					err.CreateVerificationError(step, count.ToString(), length.ToString());	
				}
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}