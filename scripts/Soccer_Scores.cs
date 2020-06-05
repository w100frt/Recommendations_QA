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
			int games = 0;
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
							}
							else {
								log.Info("Current Eastern Time hour is " + et + ". Default to Today.");
								step.Data = "TODAY";
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

					games = driver.FindElements("xpath", "(//a[@class='score-chip'])[" + step.Data +"]//div[contains(@class,'pregame-info')]").Count; 
					if (games > 0) {
						step.Data = "TeamSport_FutureEvent";
						DataManager.CaptureMap["EVENT_STATUS"] = "FUTURE";
					}
					else {
						status = driver.FindElement("xpath", "(//a[@class='score-chip'])[" + step.Data +"]//div[contains(@class,'status-text')]").Text; 
						log.Info("Event status: " + status);
						if (status.Equals("POSTPONED") || status.Equals("CANCELED")) {
							step.Data = "TeamSport_PostponedEvent";
							DataManager.CaptureMap["EVENT_STATUS"] = "POSTPONED";
						}
						else if (status.Contains("FINAL")) {
							step.Data = "TeamSport_PastEvent";
							DataManager.CaptureMap["EVENT_STATUS"] = "FINAL";
						}
						else {
							step.Data = "TeamSport_LiveEvent";
							DataManager.CaptureMap["EVENT_STATUS"] = "LIVE";
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
			
			else if(step.Name.Equals("Scroll Back One Day")) {
				status = "//div[contains(@class,'scores-app-root')]/div[not(@style='display: none;')]//div[contains(@class,'week-selector')]";
				date = driver.FindElement("xpath", status).Text;
				DataManager.CaptureMap["CURRENT"] = date;
				log.Info("Current Day: " + date);
				if (date.Equals("TODAY")) {
					DataManager.CaptureMap["PREVIOUS"] = "YESTERDAY";
				}
				else if (date.Equals("YESTERDAY")) {
					var today = DateTime.Now;
					var yesterday = today.AddDays(-2);
					DataManager.CaptureMap["PREVIOUS"] = yesterday.ToString("ddd, MMM dd").ToUpper();
				}
				else {
					var num = int.Parse(date.Substring(10));
					num = num--;
					var old = new DateTime(DateTime.Now.Year, DateTime.Now.Month, num);
					DataManager.CaptureMap["PREVIOUS"] = old.ToString("ddd, MMM dd").ToUpper();
				}
			
				do {
					js.ExecuteScript("window.scrollBy({top: -100,left: 0,behavior: 'smooth'});");
					log.Info("Scrolling up on page...");
					date = driver.FindElement("xpath", status).Text;
					log.Info("Current Day: " + date);
					log.Info(scrolls + " scrolls until limit is reached");
				} while (date.Equals(DataManager.CaptureMap["CURRENT"]) && scrolls-- > 0);
				
				DataManager.CaptureMap["SCROLLED"] = "YES";
				
			}
			
			else if(step.Name.Equals("Scroll Forward One Day")) {
				status = "//div[contains(@class,'scores-app-root')]/div[not(@style='display: none;')]//div[contains(@class,'week-selector')]";
				date = driver.FindElement("xpath", status).Text;
				DataManager.CaptureMap["CURRENT"] = date;

				log.Info("Current Day: " + date);
				if (date.Equals("TODAY")) {
					DataManager.CaptureMap["NEXT"] = "TOMORROW";
				}
				else if (date.Equals("YESTERDAY")) {
					DataManager.CaptureMap["NEXT"] = "TODAY";
				}
				else if (date.Equals("TOMORROW")) {
					var today = DateTime.Now;
					var yesterday = today.AddDays(2);
					DataManager.CaptureMap["NEXT"] = yesterday.ToString("ddd, MMM dd").ToUpper();
				}
				else {
					var num = int.Parse(date.Substring(10));
					num = num++;
					var old = new DateTime(DateTime.Now.Year, DateTime.Now.Month, num);
					DataManager.CaptureMap["NEXT"] = old.ToString("ddd, MMM dd").ToUpper();
				}
			
				do {
					js.ExecuteScript("window.scrollBy({top: 100,left: 0,behavior: 'smooth'});");
					log.Info("Scrolling down on page...");
					date = driver.FindElement("xpath", status).Text;
					log.Info("Current Day: " + date);
					log.Info(scrolls + " scrolls until limit is reached");
				} while (date.Equals(DataManager.CaptureMap["CURRENT"]) && scrolls-- > 0);
					
				DataManager.CaptureMap["SCROLLED"] = "YES";
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}

		}
	}
}