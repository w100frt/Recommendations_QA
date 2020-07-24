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
			string games = "";
			string status = "";
			string date = "";

			if (step.Name.Equals("Verify Event")) {
				if (DataManager.CaptureMap.ContainsKey("IN_SEASON")) {
					DataManager.CaptureMap["GAME"] = step.Data;
					
					//get date for scores id
					date = driver.FindElement("xpath", "//div[contains(@class,'scores-app-root')]/div[not(@style='display: none;')]//div[contains(@class,'week-selector')]//button/span[contains(@class,'title')]").GetAttribute("innerText");
					log.Info("Current segment: " + date);
					if (date.Equals("YESTERDAY")) {
						date = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
					}
					else if (date.Equals("TODAY")) {
						date = DateTime.Today.ToString("yyyy-MM-dd");
					}
					else if (date.Equals("TOMORROW")) {
						date = DateTime.Today.AddDays(+1).ToString("yyyy-MM-dd");
					}
					else {
						date = DateTime.Parse(date).ToString("yyyy-MM-dd");
					}
					date = driver.FindElement("xpath", "//div[contains(@class,'scores-header-wrapper')]//span[contains(@class,'qs-month')]").GetAttribute("innerText");
					date = DateTime.ParseExact(date, "MMMM", CultureInfo.CurrentCulture).Month.ToString();
					date = String.Concat(date, driver.FindElement("xpath", "//div[contains(@class,'scores-header-wrapper')]//div[contains(@class,'qs-active')]").GetAttribute("innerText"));
					log.Info(date);
					
					ele = driver.FindElement("xpath", "//div[@class='scores' and contains (@id,'"+ date +"')]//a[contains(@class,'score-chip')][" + step.Data +"]");
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
						status = driver.FindElement("xpath", "(//div[contains(@class,'score-section')][div[@class='scores-date'][not(div)]]//a[contains(@class,'score-chip')])[" + step.Data +"]//div[contains(@class,'status-text')]").Text; 
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