using System;
using System.Globalization;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using SeleniumProject;
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
			VerifyError err = new VerifyError();
			long order = step.Order;
			string wait = step.Wait != null ? step.Wait : "";
			List<string> standings = new List<string>();
			List<string> polls = new List<string>();
            List<TestStep> steps = new List<TestStep>();
			IWebElement ele;
			string sport = "";
			string games = " GAMES ";
			string player = "";
			string playoffs = "";
			string xpath = "";
			bool skip = false;
			int count = 0;
			int total = 0;
			int size;
			int upper = 0;
			int lower = 0;
			IWebElement element;
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			
			if (step.Name.Equals("Click Pagination Link by Number")) {
				steps.Add(new TestStep(order, "Click " + step.Data, "", "click", "xpath", "//nav[@class='pagination']//a[text()='"+ step.Data +"']", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Equals("Verify Standings Dropdown List By Sport")) {
				if (DataManager.CaptureMap.ContainsKey("SPORT")) {
					sport = DataManager.CaptureMap["SPORT"];
				}
				else {
					sport = driver.FindElement("xpath","//div[contains(@class,'entity-title')]").Text;
				}
				
				size = 1;
				switch(sport) {
					case "NFL":
						standings.Add("DIVISION");
						standings.Add("CONFERENCE");
						//standings.Add("PRESEASON");
						break;
					case "NBA":
						standings.Add("CONFERENCE");
						standings.Add("DIVISION");
						standings.Add("PRESEASON");
						break;
					case "NHL":
						standings.Add("CONFERENCE");
						standings.Add("DIVISION");
						standings.Add("WILD CARD");
						standings.Add("PRESEASON");
						break;
					case "MLB":
						standings.Add("DIVISION");
						standings.Add("WILD CARD");
						standings.Add("SPRING TRAINING");
						break;
					case "ACC FOOTBALL":
					case "BIG 12 FOOTBALL":
					case "BIG TEN FOOTBALL":
					case "PAC-12 FOOTBALL":
					case "SEC FOOTBALL":
						standings.Add("CONFERENCE");
						break;
					default :
						standings.Add("");
						standings.Add("");
						standings.Add("");
						break;
				}
				
				foreach (string s in standings) {
					steps.Add(new TestStep(order, "Verify Dropdown Value " + size, standings[size-1], "verify_value", "xpath", "//div[contains(@class,'standings')]//ul//li[contains(@class,'dropdown')]["+size+"]", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
					size++;					
				}
			}
			
			else if (step.Name.Equals("Verify Count of Teams")) {
				sport = step.Data;
				bool success = Int32.TryParse(sport, out total);
				
				if (!success) {
					switch (sport) {
						case "NFL":
							sport = "32";
							break;
						case "NBA":
							sport = "30";
							break;
						case "NHL":
							sport = "31";
							break;
						case "MLB":
							sport = "30";
							break;
						case "BIG TEN FOOTBALL":
						case "SEC FOOTBALL":
							sport = "14";
							break;
						case "ACC FOOTBALL":
							sport = "15";
							break;
						case "PAC-12 FOOTBALL":
							sport = "12";
							break;
						case "BIG 12 FOOTBALL":
							sport = "10";
							break;
						default :
							sport = "32";
							break;
					}		
				}

				steps.Add(new TestStep(order, "Verify Count", sport, "verify_count", "xpath", "//div[contains(@class,'teams-list')]//a", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Equals("Verify Number of Player Stats Categories") || step.Name.Equals("Verify Number of Team Stats Categories")) {
				sport = step.Data;
				player = step.Data;
				bool success = Int32.TryParse(sport, out total);
				
				if (!success) {
					switch (sport) {
						case "NFL":
							player = "16";
							sport = "11";
							break;
						case "NBA":
							player = "8";
							sport = "8";
							break;
						case "NHL":
							player = "10";
							sport = "7";
							break;
						case "MLB":
							player = "15";
							sport = "14";
							break;
						case "NCAA FOOTBALL":
						case "ACC FOOTBALL":
						case "BIG 12 FOOTBALL":
						case "BIG TEN FOOTBALL":
						case "PAC-12 FOOTBALL":
						case "SEC FOOTBALL":
							player = "14";
							sport = "9";
							break;
						default :
							player = "";
							sport = "";
							break;
					}	
				}
				
				if (step.Name.Contains("Player Stats")) {
					steps.Add(new TestStep(order, "Verify Count", player, "verify_count", "xpath", "//div[contains(@class,'stats-overview-component')][div[.='PLAYER STATS']]//a[contains(@class,'stats-overview')]", wait));
				}
				else if (step.Name.Contains("Team Stats")) {
					steps.Add(new TestStep(order, "Verify Count", sport, "verify_count", "xpath", "//div[contains(@class,'stats-overview-component')][div[.='TEAM STATS']]//a[contains(@class,'stats-overview')]", wait));
				}
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Equals("Verify Tweet is Displayed")) {
				count = driver.FindElements("xpath", "//div[@class='loader']").Count;
				
				while (count != 0 && total < 5) {
					log.Info("Spinners found: " + count + ". Waiting for social posts to load.");
					Thread.Sleep(1000);
					count = driver.FindElements("xpath", "//div[@class='loader']").Count;
					total++;
				}
				
				steps.Add(new TestStep(order, "Verify Tweet", "", "verify_displayed", "xpath", "//*[contains(@id,'twitter-widget')]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Equals("Verify Header Subtext")) {
				sport = step.Data;
				count = driver.FindElements("xpath","(//div[@class='scores'])[1]//a").Count;
				
				if (count == 1) {
					games = " GAME ";
				}
				
				switch (sport) {
					case "NFL":
						driver.FindElement("xpath","//div[contains(@class,'scores-app-root')]/div[not(@style='display: none;')]//span[@class='title-text']").Click();
						sport = driver.FindElement("xpath","//div[contains(@class,'week-selector') and contains(@class,'active')]//li[contains(@class,'selected')]//div[contains(@class,'week')]//div[1]").Text;
						player = driver.FindElement("xpath","//div[contains(@class,'week-selector') and contains(@class,'active')]//li[contains(@class,'selected')]//div[contains(@class,'week')]//div[2]").Text;
						if (sport.StartsWith("PRE")) {
							sport = sport.Replace("PRE", "PRESEASON");
						}	
						sport = sport + ": " + player;
						break;
					case "NCAA FOOTBALL":
						driver.FindElement("xpath","//div[contains(@class,'scores-app-root')]/div[not(@style='display: none;')]//span[@class='title-text']").Click();
						sport = driver.FindElement("xpath","//div[contains(@class,'week-selector') and contains(@class,'active')]//li[contains(@class,'selected')]//div[contains(@class,'week')]//div[1]").Text;
						player = driver.FindElement("xpath","//div[contains(@class,'week-selector') and contains(@class,'active')]//li[contains(@class,'selected')]//div[contains(@class,'week')]//div[2]").Text;
						sport = sport + ": " + player;
						break;
					case "NBA":
						DateTime NBA_season = new DateTime(2021, 1, 01);
						DateTime NBA_playoff = new DateTime(2021, 7, 30);
						if (DateTime.Now > NBA_season) {
							sport = driver.FindElement("xpath","//div[contains(@class,'date-picker-container') and @style]//span[@class='title-text']").Text;
						}
						else {
							skip = true;
						}
						if (DateTime.Now > NBA_playoff) {
							playoffs = "PLAYOFFS: ";
						}
						sport = playoffs + count + games + sport;
						break;
					case "NHL":
						DateTime NHL_season = new DateTime(2021, 1, 01);
						DateTime NHL_playoff = new DateTime(2021, 5, 30);
						if (DateTime.Now > NHL_season) {
							sport = driver.FindElement("xpath","//div[contains(@class,'date-picker-container') and @style]//span[@class='title-text']").Text;
						}
						else {
							skip = true;
						}
						if (DateTime.Now > NHL_playoff) {
							playoffs = "PLAYOFFS: ";
						}
						sport = count + games + sport;
						break;
					case "MLB":
						DateTime MLB_season = new DateTime(2021, 4, 01);
						DateTime MLB_playoff = new DateTime(2021, 10, 04);
						if (DateTime.Now > MLB_season) {
							sport = driver.FindElement("xpath","//div[contains(@class,'date-picker-container') and @style]//span[@class='title-text']").Text;
						}
						else {
							skip = true;
						}
						if (DateTime.Now > MLB_playoff) {
							playoffs = "PLAYOFFS: ";
						}
						sport = playoffs + count + games + sport;
						break;
					default :
						break;	
				}
				
				if (!skip) {
					steps.Add(new TestStep(order, "Verify Text", sport, "verify_value", "xpath", "//div[contains(@class,'entity-header')]/div/span", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();					
				}
				else {
					log.Info("No subtext current for Out of Season sports. Skipping...");
				}
			}
			
			else if (step.Name.Equals("Capture Number of Players")) {
				total = driver.FindElements("xpath","(//div[@class='table-roster'])[1]//tbody//tr").Count;
				log.Info("Storing total as " + total.ToString());
				DataManager.CaptureMap["PLAYER_COUNT"] = total.ToString();
			}	
			
			else if (step.Name.Equals("Verify Polls Dropdown List")) {
				polls.Add("ASSOCIATED PRESS");
				polls.Add("USA TODAY COACHES POLL");
				
				size = 1;
				foreach (string s in polls) {
					steps.Add(new TestStep(order, "Verify Polls List " + size, polls[size-1], "verify_value", "xpath", "//div[contains(@class,'polls')]//ul//li[contains(@class,'dropdown')]["+size+"]", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();	
					size++;					
				}			
			}
			
			else if (step.Name.Equals("Click Open Standings Dropdown")) {
				xpath = "//div[contains(@class,'standings')]//a[contains(@class,'dropdown-title')]";
				ele = driver.FindElement("xpath", xpath);
                js.ExecuteScript("window.scrollTo(0,0);");
				
				steps.Add(new TestStep(order, "Click Open Standings", "", "click", "xpath", xpath, wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();	
			}
			
			else if (step.Name.Equals("Verify Number of Stories & Videos")) {
				try {
					upper = Int32.Parse(step.Data);
					lower = upper - 2;
				}
				catch (Exception e){
					log.Error("Expected data to be a numeral. Setting data to 0.");
					upper = 0;
				}
				size = driver.FindElements("xpath", "//*[@class='news' or @class='news pointer-default' or contains(@class,'video-container')]").Count;
				if (size >= lower && size <= upper) {
					log.Info("Verification Passed. " + size + " is between " + lower + " and " + upper); 
				}
				else {
					log.Info("Verification FAILED. " + size + " is not between " + lower + " and " + upper); 
					err.CreateVerificationError(step, "Number Between " + lower + " and " + upper.ToString(), size.ToString());
					driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
				}	
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}