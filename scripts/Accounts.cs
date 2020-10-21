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
			List<string> channels = new List<string>();
			string test = "";
			bool stop = false;
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			VerifyError err = new VerifyError();
			
			if (step.Name.Equals("Get or Compare Device ID")) {
				try {
					test = (string) js.ExecuteScript("return document.readyState;");
					while (!test.Equals("complete") && size++ < 5) {
						log.Info("Waiting for readyState=complete");
						Thread.Sleep(0500);
						test = (string) js.ExecuteScript("return document.readyState;");
					}
					
					data = (string) js.ExecuteScript("return window.wisRegistration.getDeviceID();");
					
					while (String.IsNullOrEmpty(data) && count++ < 5) {
						log.Warn("GetDeviceID method failed. Retrying...");
						Thread.Sleep(0500);
						data = (string) js.ExecuteScript("return window.wisRegistration.getDeviceID();");
					}
					
					log.Info("Device ID equals " + data);
					
					// if device id is not stored yet, store it
					if (!DataManager.CaptureMap.ContainsKey("DEVICE_ID")) {
						DataManager.CaptureMap.Add("DEVICE_ID", data);
						log.Info("Storing " + data + " to CaptureMap as DEVICE_ID");
					}
					
					// verify device id has not changed
					if(DataManager.CaptureMap["DEVICE_ID"].Equals(data)) {
						log.Info("Comparison PASSED. Original Device ID [" + DataManager.CaptureMap["DEVICE_ID"] + "] matches current Device ID ["+ data + "]");			
					}
					else {
						log.Error("Comparison FAILED. Original Device ID [" + DataManager.CaptureMap["DEVICE_ID"] + "] does not match current Device ID ["+ data + "]");
						err.CreateVerificationError(step, DataManager.CaptureMap["DEVICE_ID"], data);
					}
				}
				catch (Exception e) {
					log.Info("ERROR: " + e);
					
					while (String.IsNullOrEmpty(data) && count++ < 5) {
						log.Warn("GetDeviceID method failed. Retrying...");
						Thread.Sleep(0500);
						data = (string) js.ExecuteScript("return window.wisRegistration.getDeviceID();");
					}

					// if device id is not stored yet, store it
					if (!DataManager.CaptureMap.ContainsKey("DEVICE_ID")) {
						DataManager.CaptureMap.Add("DEVICE_ID", data);
						log.Info("Storing " + data + " to CaptureMap as DEVICE_ID");
					}
					
					//err.CreateVerificationError(step, "Error Capturing DeviceID", data);
				}
			}
			
			else if (step.Name.Equals("Capture User Entitlements")) {
				length = Convert.ToInt32(js.ExecuteScript("return wisRegistration.getUserEntitlements().then(x => x.channels.length)"));
				for (int i = 0; i < length; i++) {
					test = (string) js.ExecuteScript("return wisRegistration.getUserEntitlements().then(x => x.channels["+i+"].name)");
					channels.Add(test);
					log.info("Adding channel: " + test);
				}
				log.Info("Total channel list size: " + channels.Count);
			}					
			
			else if (step.Name.Equals("Click Sign In With TV Provider")) {
				if (!DataManager.CaptureMap.ContainsKey("CURRENT_URL")) {
					DataManager.CaptureMap.Add("CURRENT_URL", driver.GetDriver().Url);
					log.Info("Storing " + driver.GetDriver().Url + " to CaptureMap as CURRENT_URL");
				}
				else {
					DataManager.CaptureMap["CURRENT_URL"] = driver.GetDriver().Url;
				}
				
				ele = driver.FindElement("xpath","//a[.='TV Provider Sign In']");
				if (!ele.Displayed) {
					steps.Add(new TestStep(order, "Click Sign In Again", "", "click", "xpath", "//a[contains(@class,'sign-in')]", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();					
				}
				steps.Add(new TestStep(order, "Sign In With TV Provider", "", "click", "xpath", "//a[.='TV Provider Sign In']", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Equals("Verify URL Redirect")) {
				data = driver.GetDriver().Url;
				log.Info("Captured URL: " + data);
				if (DataManager.CaptureMap.ContainsKey("CURRENT_URL")) {
					log.Info("CURRENT_URL value: " + DataManager.CaptureMap["CURRENT_URL"]);
					if (DataManager.CaptureMap["CURRENT_URL"].Equals(data)) {
						stop = true;
					}					
				}
				else {
					log.Info("No previous URL stored. Skipping verification.");
					stop = true;
				}

				// verify that the url is properly redirecting
				while (!stop && size++ < 10) {
					data = driver.GetDriver().Url;
					log.Info("Waiting for redirect...");
					Thread.Sleep(1000);				
					if (DataManager.CaptureMap["CURRENT_URL"].Equals(data)) {
						log.Info("URL redirected to " + data);
						stop = true;
					}
				}
				
				// verify that the page is currently in a readyState
				test = (string) js.ExecuteScript("return document.readyState;");
				while (!test.Equals("complete") && size++ < 5) {
					log.Info("document.readyState = " + test + ". Waiting...");
					Thread.Sleep(0500);
					test = (string) js.ExecuteScript("return document.readyState;");
				}
			}
			
			else if (step.Name.Equals("Navigate to Account")) {
				// verify that the page is currently in a readyState
				test = (string) js.ExecuteScript("return document.readyState;");
				log.Info("document.readyState = " + test);
				while (!test.Equals("complete") && size++ < 8) {
					log.Info("Waiting...");
					Thread.Sleep(0500);
					test = (string) js.ExecuteScript("return document.readyState;");
					log.Info("document.readyState = " + test);
				}
				steps.Add(new TestStep(order, "Click Account", "", "click", "xpath", "//a[contains(@class,'account-link')]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
				
				count = driver.FindElements("xpath","//div[@id='account' and contains(@class,'open')]").Count;
				if (count == 0) {
					steps.Add(new TestStep(order, "Retry Click Account", "", "click", "xpath", "//a[contains(@class,'account-link')]", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
			}
			
			else if (step.Name.Equals("Verify State of Reset Password Button")) {
				// verify that the button is currently enabled/disabled
				test = driver.FindElement("xpath","//div[span[contains(@class,'link-text') and contains(.,'Reset Password')]]").GetAttribute("class");
				test = test.Substring(test.IndexOf(" ") + 1);
				log.Info("button state = " + test);
				if (!String.IsNullOrEmpty(step.Data)) {
					if (step.Data.ToLower().Equals(test)) {
						log.Info("Verification PASSED. Expected [" + step.Data.ToLower() + "] matches Actual [" + test +"]");
					}
					else {
						log.Error("***Verification FAILED. Expected [" + step.Data.ToLower() + "] does not match Actual [" + test +"]");
						err.CreateVerificationError(step, step.Data.ToLower(), test);
						driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
					}
				}
			}
			
			else if (step.Name.Equals("Capture Current URL")) {
				data = step.Data; 
				if (String.IsNullOrEmpty(data)) {
					data = "URL";
				}
				DataManager.CaptureMap[data] = driver.GetDriver().Url;
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}