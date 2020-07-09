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
				classList = classList.Substring(classList.IndexOf("jw-state-") + 9);
				classList = classList.Substring(0, classList.IndexOf(" "));
				do {
					log.Info("Video State: " + classList);					
				}
				while (!classList.Equals("playing") && attempts-- > 0);
				if (classList.Equals("playing")) {
					log.Info("Verification PASSED. Video returned " + classList);
				}
				else {
					log.Error("***Verification FAILED. Video returned " + classList + " ***");
					err.CreateVerificationError(step, "playing", classList);
					driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
				}
			}
			
			else if (step.Name.Equals("Verify Video is Paused")) {
				ele = driver.FindElement("xpath", "//div[@aria-label='Video Player']");
				classList = ele.GetAttribute("className");
				classList = classList.Substring(classList.IndexOf("jw-state-") + 9);
				classList = classList.Substring(0, classList.IndexOf(" "));
				do {
					log.Info("Video State: " + classList);					
				}
				while (!classList.Equals("paused") && attempts-- > 0);
				if (classList.Equals("paused")) {
					log.Info("Verification PASSED. Video returned " + classList);
				}
				else {
					log.Error("***Verification FAILED. Video returned " + classList + " ***");
					err.CreateVerificationError(step, "paused", classList);
					driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
				}
			}
		}
	}
}