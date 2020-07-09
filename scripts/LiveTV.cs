using System;
using System.Threading;
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
			int channel = 0;
			int attempts = 10;
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
					if(!classList.Equals("playing")) {
						Thread.Sleep(1000);
					}
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
			
			else if (step.Name.Equals("Capture Number of Additional Channels")) {
				size = driver.FindElements("xpath", "//div[@class='live-tv-channels']//div[contains(@class,'item')]").Count;
				DataManager.CaptureMap["CHANNELS"] = size.ToString();
			}
			
			else if (step.Name.Equals("Select Additional Channels")) {
				if (DataManager.CaptureMap.ContainsKey("CHANNELS")) {
					size = Int32.Parse(DataManager.CaptureMap["CHANNELS"]);
					for (int i = 1; i <= size; i++) {
						steps.Add(new TestStep(order, "Run Template", "VerifyChannel", "run_template", "xpath", "", wait));
						DataManager.CaptureMap["CURRENT_CHANNEL_NUM"] = i.ToString();
						TestRunner.RunTestSteps(driver, null, steps);
						steps.Clear();
					}
				}
			}
			
			else if (step.Name.Equals("Select Additional Channel")) {
				if (DataManager.CaptureMap.ContainsKey("CHANNELS")) {
					channel = Int32.Parse(DataManager.CaptureMap["CURRENT_CHANNEL_NUM"]);
					steps.Add(new TestStep(order, "Select Channel " + channel, "", "click", "xpath", "(//a[@class='pointer video'])[" + channel + "]", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}