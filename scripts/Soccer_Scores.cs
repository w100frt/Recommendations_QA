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
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			VerifyError err = new VerifyError();
			Random random = new Random();
			bool in_season = false;
			IWebElement ele;
			string games = "";
			int scrolls = 20;
			string status = "";
			string date = "";
			int loc;
			int rand;
			int months;
			
			if (step.Name.Equals("Verify Soccer Date")) {
				if (String.IsNullOrEmpty(step.Data)) {
					if(DataManager.CaptureMap.ContainsKey("IN_SEASON")) {
						in_season = bool.Parse(DataManager.CaptureMap["IN_SEASON"]);
						if(in_season) {
							TimeSpan time = DateTime.UtcNow.TimeOfDay;
							int now = time.Hours;
							int et = now - 4;
							if (et >= 0 && et < 11){
								log.Info("Current Eastern Time hour is " + et + ". Default to Yesterday.");
								step.Data = "YESTERDAY";
								DataManager.CaptureMap["IN_SEASON"] = "YESTERDAY";
							}
							else {
								log.Info("Current Eastern Time hour is " + et + ". Default to Today.");
								step.Data = "TODAY";
								DataManager.CaptureMap["CURRENT"] = "TODAY";
							}				
						}
						else {
							step.Data = "WED, NOV 18";
						}
					}
					else {
						log.Warn("No IN_SEASON variable available.");
					}
				}

				steps.Add(new TestStep(order, "Verify Displayed Day on Soccer", step.Data, "verify_value", "xpath", "//div[contains(@class,'scores-app-root')]/div[not(@style='display: none;')]//div[contains(@class,'week-selector')]//button/span[contains(@class,'title')]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Equals("Verify Soccer Event")) {
				if (DataManager.CaptureMap.ContainsKey("IN_SEASON")) {
					DataManager.CaptureMap["GAME"] = step.Data;

					ele = driver.FindElement("xpath", "(//a[contains(@class,'score-chip')])[" + step.Data +"]");
					games = ele.GetAttribute("className");
					games = games.Substring(games.IndexOf(" ") + 1); 
					log.Info("Game State: " + games);
					if (games.Equals("pregame")) {
						step.Data = "TeamSport_FutureEvent";
						DataManager.CaptureMap["EVENT_STATUS"] = "FUTURE";
					}
					else if (games.Equals("live")){
						step.Data = "TeamSport_LiveEvent";
						DataManager.CaptureMap["EVENT_STATUS"] = "LIVE";
					}
					else {
						status = driver.FindElement("xpath", "(//a[contains(@class,'score-chip')])[" + step.Data +"]//div[contains(@class,'status-text')]").Text; 
						log.Info("Event status: " + status);
						if (status.Equals("POSTPONED") || status.Equals("CANCELED")) {
							step.Data = "TeamSport_PostponedEvent";
							DataManager.CaptureMap["EVENT_STATUS"] = "POSTPONED";
						}
						else {
							step.Data = "TeamSport_PastEvent";
							DataManager.CaptureMap["EVENT_STATUS"] = "FINAL";
						}
					}
				}
				else {
					log.Warn("No IN_SEASON variable available or data is populated. Using data.");
				}
				
				steps.Add(new TestStep(order, "Run Event Template", step.Data, "run_template", "xpath", "", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}

		}
	}
}