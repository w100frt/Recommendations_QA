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
			int overlay;
			int size = 0;
			int channel = 0;
			int attempts = 10;
			string classList = "";
			string title = "";
			string edit = "";
			string top = "";
			bool topTitle = true;
			bool live = false;
			List<TestStep> steps = new List<TestStep>();
			VerifyError err = new VerifyError();
			
			if (step.Name.Equals("Verify Video is Playing")) {
				ele = driver.FindElement("xpath", "//div[@aria-label='Video Player']");
				classList = ele.GetAttribute("className");
				classList = classList.Substring(classList.IndexOf("jw-state-") + 9);
				classList = classList.Substring(0, classList.IndexOf(" "));
				
				// state returns idle if overlay button is present
				overlay = driver.FindElements("xpath", "//div[@class='overlays']/div").Count;
				if(overlay > 1) {
					steps.Add(new TestStep(order, "Click Overlay Play Button", "", "click", "xpath", "//*[@class='overlay-play-button']", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
					ele = driver.FindElement("xpath", "//div[@aria-label='Video Player']");
					classList = ele.GetAttribute("className");
					classList = classList.Substring(classList.IndexOf("jw-state-") + 9);
					classList = classList.Substring(0, classList.IndexOf(" "));
				}
				
				
				// check video state. if not playing, wait and check again for 10 seconds
				do {
					log.Info("Video State: " + classList);
					if(!classList.Equals("playing")) {
						Thread.Sleep(1000);
						ele = driver.FindElement("xpath", "//div[@aria-label='Video Player']");
						classList = ele.GetAttribute("className");
						classList = classList.Substring(classList.IndexOf("jw-state-") + 9);
						classList = classList.Substring(0, classList.IndexOf(" "));
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
				size = driver.FindElements("xpath", "//div[@class='live-tv-channels']//div[contains(@class,'item') or @class='live-tv-channel']").Count;
				
				// if size is zero, stream is attached to event. return and count
				if (size == 0) {
					log.Info("No Channels found. Stream is on event. Returning to Live TV.");
					steps.Add(new TestStep(order, "Return to Live TV", "", "click", "xpath", "//a[@href='/live']", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
					size = driver.FindElements("xpath", "//div[@class='live-tv-channels']//div[contains(@class,'item') or @class='live-tv-channel']").Count;
					DataManager.CaptureMap["EVENT"] = "Y";
				}

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
					
					if (!driver.GetDriver().Url.Contains("live/")) {
						steps.Add(new TestStep(order, "Hover Channel " + channel, "", "click", "xpath", "//div[@class='live-tv-channel'][" + channel + "]//span", wait));
						TestRunner.RunTestSteps(driver, null, steps);
						steps.Clear();
					}
					
					steps.Add(new TestStep(order, "Select Channel " + channel, "", "click", "xpath", "(//div[contains(@class,'live-on-fox-secondary') or @class='live-tv-channel']//a[@class='pointer video'])[" + channel + "]", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
			}
			
			else if (step.Name.Equals("Check for Event")) {
				if (!driver.GetDriver().Url.Contains("live") || DataManager.CaptureMap.ContainsKey("EVENT")) {
					log.Info("At least one stream is on an event. Returning to Live TV for channels.");
					steps.Add(new TestStep(order, "Return to Live TV", "", "click", "xpath", "//a[@href='/live']", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
			}
			
			else if (step.Name.Equals("Click Live Play Button")) {
				// check if event 
				overlay = driver.FindElements("xpath", "//div[contains(@class,'event has-stream')]").Count;
				if (overlay > 0) {
					// wait for one second, check for live play button				
					Thread.Sleep(1000);
					overlay = driver.FindElements("xpath", "//div[contains(@class,'scroll-resize') or contains(@class,'live-tv-watch')]//div[@class='live-arrow']").Count;
					if (overlay > 0) {
						live = true;
					}
				}
				else {
					log.Info("Not an event. Live Play Button is present.");
					live = true;
				}

				if (live == true) {
					steps.Add(new TestStep(order, "Click Live Play Button", "", "click", "xpath", "//div[contains(@class,'scroll-resize') or contains(@class,'live-tv-watch')]//div[@class='live-arrow']", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();				
				}
			}
			
			else if (step.Name.Equals("Verify Top Show Title")) {
				title = step.Data;
				top = "//div[contains(@class,'live-tv-main')]//div[contains(@class,'video-container')]//div[contains(@class,'video-title')]";
				if (driver.FindElements("xpath", top).Count == 0) {
					topTitle = false;
				}
				
				if (!topTitle) {
					log.Info("Top Title not found. Checking for Promo...");
					size = driver.FindElements("xpath","//div[@class='promo-overlay']").Count;
					if (size > 0) {
						log.Info("Promo Found. No Top Title Expected.");
					}
					else {
						log.Error("***VERIFICATION FAILED. No Title or Promo Found ***");
						err.CreateVerificationError(step, "Title or Promo", "NEITHER FOUND");
						driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
					}
				}
				else {
					if (title.Contains("...") && title.Length == 54) {
						edit = driver.FindElement("xpath", top).Text;
						edit = edit.Substring(0, 51) + "...";
						log.Info("Title was shortened at 50 characters: " + edit);
						if(title.Equals(edit)) {
							log.Info("VERIFICATION PASSED. Shortened expected title [" +title + "] matches shortened actual title [" + edit + "]");
						}
						else {
							log.Error("***VERIFICATION FAILED. Shortened expected title [" +title + "] DOES NOT match shortened actual title [" + edit + "]***");
							err.CreateVerificationError(step, title, edit);
							driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
						}
					}
					else {
						steps.Add(new TestStep(order, "Verify Top Show Title", title, "verify_value", "xpath", top, wait));
						TestRunner.RunTestSteps(driver, null, steps);
						steps.Clear();					
					}
				}
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}