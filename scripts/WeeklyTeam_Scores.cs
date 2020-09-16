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
            OpenQA.Selenium.Interactions.Actions actions = new OpenQA.Selenium.Interactions.Actions(driver.GetDriver());
			IWebElement ele;
			int total = 0;
			string day = "";
			string data = "";
			string events = "";
			string games = "";
			string status = "";
			string date = "";
			bool over = false;

			if (step.Name.Equals("Verify Event")) {
				if (DataManager.CaptureMap.ContainsKey("IN_SEASON")) {
					DataManager.CaptureMap["GAME"] = step.Data;
					
					//get date for scores id
					date = driver.FindElement("xpath", "//div[contains(@class,'scores-app-root')]/div[not(@style='display: none;')]//div[contains(@class,'week-selector')]//button/span[contains(@class,'title')]").GetAttribute("innerText");
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
				date = driver.FindElement("xpath", "//h2[contains(@class,'section-title fs-30 desktop-show') and not(@style='display: none;')]").GetAttribute("innerText").Substring(5);
				log.Info("Current segment: " + date);
				DataManager.CaptureMap["NFL_WEEK"] = date;
				day = DateTime.Now.DayOfWeek.ToString();

				if (day.Equals("Tuesday") || day.Equals("Wednesday") || day.Equals("Thursday")) {
					day = "NFL_Thursday";
				}
				else if (day.Equals("Friday") || day.Equals("Saturday") || day.Equals("Sunday")) {
					day = "NFL_Sunday";
				}
				else {
					day = "NFL_Monday";
				}	

				steps.Add(new TestStep(order, "Run Segment by Day", day, "run_template", "xpath", "", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Equals("Verify Events by Day")) {
				DataManager.CaptureMap["NFL_DAY"] = step.Data;
				total = driver.FindElements("xpath", "//div[@class='scores' and contains (@id,'w"+ DataManager.CaptureMap["NFL_WEEK"] + step.Data +"')]//a[contains(@class,'score-chip')]").Count;
				
				for (int game = 1; game <= total; game++) {
					DataManager.CaptureMap["GAME"] = game.ToString();
					ele = driver.FindElement("xpath", "//div[@class='scores' and contains (@id,'w"+ DataManager.CaptureMap["NFL_WEEK"] + step.Data + "')]//a[contains(@class,'score-chip')][" + game +"]");
					games = ele.GetAttribute("className");
					games = games.Substring(games.IndexOf(" ") + 1); 
					log.Info("Game State: " + games);
					if (games.Equals("pregame")) {
						events = "Football_FutureEvent";
						DataManager.CaptureMap["EVENT_STATUS"] = "FUTURE";
					}
					else if (games.Equals("live")){
						events = "TeamSport_LiveEvent";
						DataManager.CaptureMap["EVENT_STATUS"] = "LIVE";
					}
					else {
						status = driver.FindElement("xpath", "//div[@class='scores' and contains (@id,'"+ DataManager.CaptureMap["NFL_WEEK"] + step.Data +"')]//a[contains(@class,'score-chip')][" + game +"]//div[contains(@class,'status-text')]").Text; 
						log.Info("Event status: " + status);
						if (status.Equals("POSTPONED") || status.Equals("CANCELED")) {
							events = "TeamSport_PostponedEvent";
							DataManager.CaptureMap["EVENT_STATUS"] = "POSTPONED";
						}
						else {
							events = "TeamSport_PastEvent";
							DataManager.CaptureMap["EVENT_STATUS"] = "FINAL";
						}
					}
					
					steps.Add(new TestStep(order, "Run Event Template", events, "run_template", "xpath", "", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
					
					steps.Add(new TestStep(order, "Return to Scores Segment", "TeamSport_ScoresToday", "run_template", "xpath", "", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
			}
			
			else if (step.Name.Equals("Scroll to Sunday")) {
				ele = driver.FindElement("xpath", "//div[@class='scores' and contains (@id,'w"+ DataManager.CaptureMap["NFL_WEEK"] + "sun" +"')]");
                js.ExecuteScript("arguments[0].scrollIntoView(true);", ele);
                actions.MoveToElement(ele).Perform();				
			}
			
			else if (step.Name.Equals("Click Scorechip By Number")) {
				data = step.Data;
				steps.Add(new TestStep(order, "Click Event " + data, "", "click", "xpath", "//div[@class='scores' and contains (@id,'w"+ DataManager.CaptureMap["NFL_WEEK"] + DataManager.CaptureMap["NFL_DAY"] +"')]//a[contains(@class,'score-chip')]["+ data +"]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();		
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}

		}
	}
}