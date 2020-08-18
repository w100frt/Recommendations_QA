using System;
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
            List<TestStep> steps = new List<TestStep>();
			string sport = "";
			string player = "";
			int count = 0;
			int total = 0;
			int size;
			IWebElement element;
			
			if (step.Name.Equals("Click Pagination Link by Number")) {
				steps.Add(new TestStep(order, "Click " + step.Data, "", "click", "xpath", "//nav[@class='pagination']//a[text()='"+ step.Data +"']", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Equals("Verify Standings Dropdown List By Sport")) {
				sport = driver.FindElement("xpath","//div[contains(@class,'entity-title')]").Text;
				
				size = 1;
				switch(sport) {
					case "NFL":
						standings.Add("DIVISION");
						standings.Add("CONFERENCE");
						standings.Add("PRESEASON");
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
							sport = "";
							break;
						case "NHL":
							sport = "";
							break;
						case "MLB":
							sport = "30";
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
							sport = "";
							player = "";
							break;
						case "NHL":
							sport = "";
							player = "";
							break;
						case "MLB":
							player = "15";
							sport = "14";
							break;
						default :
							sport = "";
							player = "";
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
				
				while (count == 0 && total > 5) {
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
				count = driver.FindElements("xpath","//div[@class='scores']//a").Count;
				
				switch (sport) {
					case "NFL":
						element = driver.FindElement("xpath","//div[contains(@class,'date-picker-container') and @style]//span[@class='title-text']");
						element.Click;
						sport = driver.FindElement("xpath","//div[contains(@class,'week-selector') and contains(@class,'active')]//li[contains(@class,'selected')]//div[contains(@class,'week')]//div[1]").Text;
						player = driver.FindElement("xpath","//div[contains(@class,'week-selector') and contains(@class,'active')]//li[contains(@class,'selected')]//div[contains(@class,'week')]//div[2]").Text;
						if (sport.StartsWith("PRE")) {
							sport.Replace("PRE", "PRESEASON");
						}	
						sport = sport + ": " + player;
						break;
					case "NBA":
						sport = count + " GAMES " + sport;
						break;
					case "NHL":
						sport = count + " GAMES " + sport;
						break;
					case "MLB":
						sport = driver.FindElement("xpath","//div[contains(@class,'date-picker-container') and @style]//span[@class='title-text']").Text;
						sport = count + " GAMES " + sport;
						break;
					default :
						break;	
				}
				
				steps.Add(new TestStep(order, "Verify Text", sport, "verify_value", "xpath", "//div[contains(@class,'entity-header')]/div/span", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}