using System;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using SeleniumProject;
using OpenQA.Selenium;
using log4net;

namespace SeleniumProject.Function
{
	public class Script : ScriptingInterface.IScript
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		
		public void Execute(DriverManager driver, TestStep step)
		{
			long order = step.Order;
			string wait = step.Wait != null ? step.Wait : "";
			string title;
			string conf = "";
			int week;
			int total;
			Random random = new Random();
			VerifyError err = new VerifyError();
            List<TestStep> steps = new List<TestStep>();
			//"BOWLS", "TOP 25", 
			string[] expectedConf = {"BOWLS", "TOP 25", "FBS (I-A)", "AAC", "ACC", "BIG 12", "BIG SKY", "BIG SOUTH", "BIG TEN", "C-USA", "CAA", "IND-FCS", "INDEPENDENTS", "IVY", "MAC", "MEAC", "MVC", "MW", "NEC", "OVC", "PAC-12", "PATRIOT LEAGUE", "PIONEER", "SEC", "SOUTHERN", "SOUTHLAND", "SUN BELT", "SWAC"};
			
			string[] regSeason = {"August", "September", "October", "November", "December"};
			string[] regSeasonWeek = {"WEEK 1", "WEEK 2", "WEEK 3", "WEEK 4", "WEEK 5", "WEEK 6", "WEEK 7", "WEEK 8", "WEEK 9", "WEEK 10", "WEEK 11", "WEEK 12", "WEEK 13", "WEEK 14", "WEEK 15", "WEEK 16"};
			string[] postSeason = {"December", "January"};
			string[] postSeasonWeeks = {"BOWL WEEK"};
			
            if (step.Name.Equals("Verify CFB Groups")) {
				steps.Add(new TestStep(order, "Open Conference Dropdown", "", "click", "xpath", "//a[@class='dropdown-menu-title']", wait));
				steps.Add(new TestStep(order, "Verify Dropdown is Displayed", "", "verify_displayed", "xpath", "//div[contains(@class,'scores-home-container')]//div[contains(@class,'dropdown-root active')]//ul", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
				
				var conferences = driver.FindElements("xpath", "//div[contains(@class,'scores-home-container')]//div[contains(@class,'dropdown-root active')]//ul//li"); 
				for (int i = 0; i < conferences.Count; i++) {
					if (expectedConf[i].Equals(conferences[i].GetAttribute("innerText"))) {
						log.Info("Success. " + expectedConf[i] + " matches " + conferences[i].GetAttribute("innerText"));
					}
					else {
						log.Error("***Verification FAILED. Expected data [" + expectedConf[i] + "] does not match actual data [" + conferences[i].GetAttribute("innerText") + "] ***");
						err.CreateVerificationError(step, expectedConf[i], conferences[i].GetAttribute("innerText"));
					}
				}
			}
			
			else if (step.Name.Equals("Select Regular Season CFB Date")) {
				title = "//ul[li[contains(.,'REGULAR SEASON')]]//li[not(contains(@class,'label'))]";
				total = driver.FindElements("xpath", title).Count;
				week = random.Next(1, total+1);

				steps.Add(new TestStep(order, "Capture Week", "CFB_WEEK", "capture", "xpath", "(" + title + ")["+ week +"]//div//div[1]", wait));
				steps.Add(new TestStep(order, "Capture Dates", "CFB_WEEK_DATES", "capture", "xpath", "(" + title + ")["+ week +"]//div//div[2]", wait));
				steps.Add(new TestStep(order, "Select Week", "", "click", "xpath", "(" + title + ")["+ week +"]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();	
			}
			
			else if (step.Name.Equals("Check Conference Name")) {
					switch (step.Data) {
						case "AAC":
							conf = "American Athletic";
							break;
						case "C-USA":
							conf = "Conference USA";
							break;
						case "CAA":
							conf = "Colonial Athletic";
							break;
						case "IND-FCS":
							conf = "Independents (FCS)";
							break;
						case "Independents":
							conf = "Independents (FBS)";
							break;
						case "MAC":
							conf = "Mid-American";
							break;
						case "MEAC":
							conf = "Mid-Eastern Athletic";
							break;
						case "MVC":
							conf = "Missouri Valley";
							break;
						case "MW":
							conf = "Mountain West";
							break;
						case "NEC":
							conf = "Northeast";
							break;
						case "OVC":
							conf = "Ohio Valley";
							break;
						case "SWAC":
							conf = "Southwestern Athletic";
							break;
						default :
							conf = DataManager.CaptureMap["RANDOM_CONF"];
							break;
					}
				DataManager.CaptureMap["RANDOM_CONF"] = conf;
			}
			
			
			else if (step.Name.Contains("Verify College") && step.Name.Contains("Header Text")) {
				if (step.Name.Contains("Football")) {
					step.Data = step.Data + " FOOTBALL";					
				}
				else if (step.Name.Contains("Basketball")) {
					step.Data = step.Data + " BASKETBALL";
				}

				steps.Add(new TestStep(order, "Verify Header Text", step.Data, "verify_value", "xpath", "//div[contains(@class,'entity-header')]//div[contains(@class,'entity-title')]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();	
			}
			
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}