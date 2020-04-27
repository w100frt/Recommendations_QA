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
			string data = "";
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
						log.Error("Comparison FAILED. Original Device ID (" + DataManager.CaptureMap["DEVICE_ID"] + " does not match current Device ID ["+ data + "].");
						err.CreateVerificationError(step, DataManager.CaptureMap["DEVICE_ID"], data);

					}
				}
				catch (Exception e) {
					log.Info("ERROR: " + e);
					err.CreateVerificationError(step, "Error Capturing DeviceID", data);
				}
			}
			
			else if (step.Name.Equals("Click Sign In With TV Provider")) {
				if (!DataManager.CaptureMap.ContainsKey("CURRENT_URL")) {
					DataManager.CaptureMap.Add("CURRENT_URL", driver.GetDriver().Url);
					log.Info("Storing " + driver.GetDriver().Url + " to CaptureMap as CURRENT_URL");
				}
				else {
					DataManager.CaptureMap["CURRENT_URL"] = driver.GetDriver().Url;
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
				while (!stop && size++ < 5) {
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
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}