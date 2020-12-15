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
			int total = 0;
			string day = "";
			string games = "";
			string status = "";
			string date = "";

			if (step.Name.Equals("Verify Event")) {
				if (DataManager.CaptureMap.ContainsKey("IN_SEASON")) {
					DataManager.CaptureMap["GAME"] = step.Data;
					
					//get date for scores id
					if(DataManager.CaptureMap["SPORT"].Equals("TOP")) {
						date = driver.FindElement("xpath", "//div[contains(@class,'scores-date')]//div[contains(@class,'sm')]").GetAttribute("innerText");
					}
					else {
						date = driver.FindElement("xpath", "//div[contains(@class,'scores-app-root')]/div[not(@style='display: none;')]//div[contains(@class,'week-selector')]//button/span[contains(@class,'title')]").GetAttribute("innerText");
					}
					log.Info("Current segment: " + date);
					if (date.Equals("YESTERDAY")) {
						date = DateTime.Today.AddDays(-1).ToString("yyyyMMdd");
						log.Info(date);
					}
					else if (date.Equals("TODAY")) {
						date = DateTime.Today.ToString("yyyyMMdd");
						log.Info(date);
					}
					else if (date.Equals("TOMORROW")) {
						date = DateTime.Today.AddDays(+1).ToString("yyyyMMdd");
						log.Info(date);
					}
					else {
						date = DateTime.Parse(date).ToString("MMdd");
						log.Info(date);
					}

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
						status = driver.FindElement("xpath", "//div[@class='scores' and contains (@id,'"+ date +"')]//a[contains(@class,'score-chip')][" + step.Data +"]//div[contains(@class,'status-text')]").Text; 
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
			
			else if (step.Name.Equals("Verify Events in Segment")) {
				DataManager.CaptureMap["SPORT"] = step.Data;
				//get date for scores id
				if(DataManager.CaptureMap["SPORT"].Equals("TOP")) {
					date = driver.FindElement("xpath", "//div[contains(@class,'scores-date')]//div[contains(@class,'sm')]").GetAttribute("innerText");
				}
				else {
					date = driver.FindElement("xpath", "//div[contains(@class,'scores-app-root')]/div[not(@style='display: none;')]//div[contains(@class,'week-selector')]//button/span[contains(@class,'title')]").GetAttribute("innerText");					
				}
				log.Info("Current segment: " + date);
				if (date.Equals("YESTERDAY")) {
					date = DateTime.Today.AddDays(-1).ToString("yyyyMMdd");
					log.Info(date);
					day = "TeamSport_ScoresYesterday";
				}
				else if (date.Equals("TODAY")) {
					date = DateTime.Today.ToString("yyyyMMdd");
					log.Info(date);
					day = "TeamSport_ScoresToday";
				}
				else if (date.Equals("TOMORROW")) {
					date = DateTime.Today.AddDays(+1).ToString("yyyyMMdd");
					log.Info(date);
					day = "TeamSport_ScoresTomorrow";
				}
				else {
					date = DateTime.Parse(date).ToString("MMdd");
					log.Info(date);
					day = "TeamSport_ScoresFuture";
				}
				
				total = driver.FindElements("xpath", "//div[@class='scores' and contains (@id,'"+ date +"')]//a[contains(@class,'score-chip')]").Count;
				
				for (int game = 1; game <= total; game++) {
					DataManager.CaptureMap["GAME"] = game.ToString();
					ele = driver.FindElement("xpath", "//div[@class='scores' and contains (@id,'"+ date +"')]//a[contains(@class,'score-chip')][" + game +"]");
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
						status = driver.FindElement("xpath", "//div[@class='scores' and contains (@id,'"+ date +"')]//a[contains(@class,'score-chip')][" + game +"]//div[contains(@class,'status-text')]").Text; 
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
					
					steps.Add(new TestStep(order, "Run Event Template", step.Data, "run_template", "xpath", "", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
					
					steps.Add(new TestStep(order, "Return to Scores Segment", day, "run_template", "xpath", "", wait));
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