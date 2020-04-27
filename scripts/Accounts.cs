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
			bool stop = true;
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
			
			else if (step.Name.Equals("Click Sign In")) {
				test = (string) js.ExecuteScript("return document.readyState;");
				while (!test.Equals("complete") && size++ < 5) {
					log.Info("document.readyState = " + test);
					Thread.Sleep(1000);
					test = (string) js.ExecuteScript("return document.readyState;");
				}
				steps.Add(new TestStep(order, "Click Sign In", "", "click", "xpath", "//a[contains(@class,'sign-in')]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}