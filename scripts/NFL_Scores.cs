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
			IWebElement chip;
			int total;
			int week;
			int months;
			int year;
			string title;
			string date;
			string data = "";
			string teamSelector = "";
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			VerifyError err = new VerifyError();
			Random random = new Random();
			
			string[] playoffTeams = {"Kansas City Chiefs", "Buffalo Bills", "Baltimore Ravens", "Cleveland Browns", "Green Bay Packers", "New Orleans Saints", "Tampa Bay Buccaneers", "Los Angeles Rams"};

			string[] preSeason = {"August"};
			string[] preSeasonWeeks = {"HALL OF FAME GAME", "PRE WEEK 1", "PRE WEEK 2", "PRE WEEK 3", "PRE WEEK 4"};
			string[] regSeason = {"September", "October", "November", "December", "January", "February"};
			string[] regSeasonWeek = {"WEEK 1", "WEEK 2", "WEEK 3", "WEEK 4", "WEEK 5", "WEEK 6", "WEEK 7", "WEEK 8", "WEEK 9", "WEEK 10", "WEEK 11", "WEEK 12", "WEEK 13", "WEEK 14", "WEEK 15", "WEEK 16", "WEEK 17"};
			string[] postSeason = {"January", "February"};
			string[] postSeasonWeeks = {"WILD CARD", "DIVISIONAL CHAMPIONSHIP", "CONFERENCE CHAMPIONSHIP", "PRO BOWL", "SUPER BOWL"};
			
			if (step.Name.Equals("Select Regular Season NFL Date")) {
				title = "//div[contains(@class,'scores') and not(@style='display: none;')][div[contains(@class,'dropdown')]]//ul[li[contains(.,'REGULAR SEASON')]]//li[not(contains(@class,'label'))]";
				total = driver.FindElements("xpath", title).Count;
				week = random.Next(1, total+1);

				steps.Add(new TestStep(order, "Capture Week", "WEEK", "capture", "xpath", "(" + title + ")["+ week +"]//div//div[1]", wait));
				steps.Add(new TestStep(order, "Capture Dates", "WEEK_DATES", "capture", "xpath", "(" + title + ")["+ week +"]//div//div[2]", wait));
				steps.Add(new TestStep(order, "Select Week", "", "click", "xpath", "(" + title + ")["+ week +"]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();	
			}

			else if (step.Name.Contains("Capture") && step.Name.Contains("Playoff Team")) {
				teamSelector = "//div[contains(@id,'App') and not(contains(@style,'none'))]//a[@class='entity-list-row-container']";
				total = playoffTeams.Length; 
				total = random.Next(1, total);				
				steps.Add(new TestStep(order, "Capture Randomized Team", "RANDOM_TEAM", "capture", "xpath", teamSelector + "[div[contains(.,'" + playoffTeams[total - 1] + "')]]", wait));
				// click as well
				if (step.Name.Contains("Click")) {
					steps.Add(new TestStep(order, "Click Randomized Team", "", "click", "xpath", teamSelector + "[div[contains(.,'" + playoffTeams[total - 1] + "')]]", wait));	
				}
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
				DataManager.CaptureMap["RANDOM_TEAM_UP"] = DataManager.CaptureMap["RANDOM_TEAM"].ToUpper();
			}
			
			else if (step.Name.Equals("Verify NFL Week")) {
				if (String.IsNullOrEmpty(step.Data)) {
					DateTime today = DateTime.Now;
					
					// determine week of season by today's date and time
					if (today >= DateTime.Parse("09/01/2020") && today < DateTime.Parse("09/15/2020 11:00:00")) {
						step.Data = "WEEK 1";
					}
					else if (today >= DateTime.Parse("09/15/2020 11:00:01") && today < DateTime.Parse("09/22/2020 11:00:00")) {
						step.Data = "WEEK 2";
					}
					else if (today >= DateTime.Parse("09/22/2020 11:00:01") && today < DateTime.Parse("09/29/2020 11:00:00")) {
						step.Data = "WEEK 3";
					}
					else if (today >= DateTime.Parse("09/29/2020 11:00:01") && today < DateTime.Parse("10/6/2020 11:00:00")) {
						step.Data = "WEEK 4";
					}
					else if (today >= DateTime.Parse("10/6/2020 11:00:01") && today < DateTime.Parse("10/13/2020 11:00:00")) {
						step.Data = "WEEK 5";
					}
					else if (today >= DateTime.Parse("10/13/2020 11:00:01") && today < DateTime.Parse("10/20/2020 11:00:00")) {
						step.Data = "WEEK 6";
					}
					else if (today >= DateTime.Parse("10/20/2020 11:00:01") && today < DateTime.Parse("10/27/2020 11:00:00")) {
						step.Data = "WEEK 7";
					}
					else if (today >= DateTime.Parse("10/27/2020 11:00:01") && today < DateTime.Parse("11/03/2020 11:00:00")) {
						step.Data = "WEEK 8";
					}
					else if (today >= DateTime.Parse("11/03/2020 11:00:01") && today < DateTime.Parse("11/10/2020 11:00:00")) {
						step.Data = "WEEK 9";
					}
					else if (today >= DateTime.Parse("11/10/2020 11:00:01") && today < DateTime.Parse("11/17/2020 11:00:00")) {
						step.Data = "WEEK 10";
					}
					else if (today >= DateTime.Parse("11/17/2020 11:00:01") && today < DateTime.Parse("11/24/2020 11:00:00")) {
						step.Data = "WEEK 11";
					}
					else if (today >= DateTime.Parse("11/24/2020 11:00:01") && today < DateTime.Parse("12/01/2020 11:00:00")) {
						step.Data = "WEEK 12";
					}
					else if (today >= DateTime.Parse("12/01/2020 11:00:01") && today < DateTime.Parse("12/08/2020 11:00:00")) {
						step.Data = "WEEK 13";
					}
					else if (today >= DateTime.Parse("12/08/2020 11:00:01") && today < DateTime.Parse("12/15/2020 11:00:00")) {
						step.Data = "WEEK 14";
					}
					else if (today >= DateTime.Parse("12/15/2020 11:00:01") && today < DateTime.Parse("12/22/2020 11:00:00")) {
						step.Data = "WEEK 15";
					}
					else if (today >= DateTime.Parse("12/22/2020 11:00:01") && today < DateTime.Parse("12/29/2020 11:00:00")) {
						step.Data = "WEEK 16";
					}
					else if (today >= DateTime.Parse("12/29/2020 11:00:01") && today < DateTime.Parse("01/05/2021 11:00:00")) {
						step.Data = "WEEK 17";
					}
					else if (today >= DateTime.Parse("01/05/2021 11:00:01") && today < DateTime.Parse("01/11/2021 11:00:00")) {
						step.Data = "WILD CARD";
					}
					else if (today >= DateTime.Parse("01/11/2021 11:00:01") && today < DateTime.Parse("01/18/2021 11:00:00")) {
						step.Data = "DIVISIONAL CHAMPIONSHIP";
					}
					else if (today >= DateTime.Parse("01/18/2021 11:00:01") && today < DateTime.Parse("01/25/2021 11:00:00")) {
						step.Data = "CONFERENCE CHAMPIONSHIP";
					}
					else if (today >= DateTime.Parse("01/25/2021 11:00:01") && today < DateTime.Parse("02/01/2021 11:00:00")) {
						step.Data = "PRO BOWL";
					}
					else if (today >= DateTime.Parse("01/25/2021 11:00:01") && today < DateTime.Parse("02/01/2021 11:00:00")) {
						step.Data = "PRO BOWL";
					}
					else {
						step.Data = "SUPER BOWL";
					}		
				}

				steps.Add(new TestStep(order, "Verify Displayed Week on NFL", step.Data, "verify_value", "xpath", "//h2[contains(@class,'section-title fs-30 desktop-show') and not(@style='display: none;')]", wait));
				DataManager.CaptureMap["CURRENT"] = driver.FindElement("xpath", "//div[contains(@class,'scores-app-root')]/div[not(@style='display: none;')]//div[contains(@class,'week-selector')]//button/span[contains(@class,'title')]").Text;
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			
			else if (step.Name.Equals("Store Team's Conference and Division")) {
				switch(step.Data) {
						case "Baltimore Ravens" : 
						case "Cincinnati Bengals" : 
						case "Cleveland Browns" : 
						case "Pittsburgh Steelers" : 
							DataManager.CaptureMap["TEAM_CONF"] = "AFC";
							DataManager.CaptureMap["TEAM_DIV"] = "AFC NORTH";
							break;
						case "Buffalo Bills" : 
						case "Miami Dolphins" : 
						case "New England Patriots" : 
						case "New York Jets" :  
							DataManager.CaptureMap["TEAM_CONF"] = "AFC";
							DataManager.CaptureMap["TEAM_DIV"] = "AFC EAST";
							break;
						case "Houston Texas" : 
						case "Indianapolis Colts" : 
						case "Jacksonville Jaguars" : 
						case "Tennessee Titans" :  
							DataManager.CaptureMap["TEAM_CONF"] = "AFC";
							DataManager.CaptureMap["TEAM_DIV"] = "AFC SOUTH";
							break;
						case "Denver Broncos" : 
						case "Kansas City Chiefs" : 
						case "Los Angeles Chargers" : 
						case "Las Vegas Raiders" :  
							DataManager.CaptureMap["TEAM_CONF"] = "AFC";
							DataManager.CaptureMap["TEAM_DIV"] = "AFC WEST";
							break;
						case "Chicago Bears" : 
						case "Detroit Lions" : 
						case "Green Bay Packers" : 
						case "Minnesota Vikings" : 
							DataManager.CaptureMap["TEAM_CONF"] = "NFC";
							DataManager.CaptureMap["TEAM_DIV"] = "NFC NORTH";
							break;
						case "Dallas Cowboys" : 
						case "New York Giants" : 
						case "Philadelphia Eagles" : 
						case "Washington Football Team" :  
							DataManager.CaptureMap["TEAM_CONF"] = "NFC";
							DataManager.CaptureMap["TEAM_DIV"] = "NFC EAST";
							break;
						case "Atlanta Falcons" : 
						case "Carolina Panthers" : 
						case "New Orleans Saints" : 
						case "Tampa Bay Buccaneers" :  
							DataManager.CaptureMap["TEAM_CONF"] = "NFC";
							DataManager.CaptureMap["TEAM_DIV"] = "NFC SOUTH";
							break;
						default:
							DataManager.CaptureMap["TEAM_CONF"] = "NFC";
							DataManager.CaptureMap["TEAM_DIV"] = "NFC WEST";
							break;
					}
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}

		}
	}
}