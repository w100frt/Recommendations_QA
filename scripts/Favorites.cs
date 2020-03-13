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
			VerifyError err = new VerifyError();
			Random random = new Random();
			int sports;
			long order = step.Order;
			string wait = step.Wait != null ? step.Wait : "";
            List<TestStep> steps = new List<TestStep>();
			
			if(step.Name.Contains("Randomize Favorite")) {
				string fullName = "";
				string sport = "";
				// Flip to Players pane if necessary. Otherwise, stay on Sports pane.
				if(step.Name.Contains("Player")) {
					steps.Add(new TestStep(order, "Click Players Pane", "", "click", "xpath", "//nav[contains(@class,'explore-subnav')]//div//a[contains(.,'players')]", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
				
				// Allows for favoriting by NCAA entity or Professional entity
				if(step.Name.Contains("NCAA")) {
					sports = driver.FindElements("xpath", "//a[contains(@class,'entity-list-row-container')][div[div[div[(contains(.,'NCAA'))]]]]").Count; 
					sports = random.Next(1, sports+1);
					steps.Add(new TestStep(order, "Click Randomized NCAA Sport", "", "click", "xpath", "(//a[contains(@class,'entity-list-row-container')][div[div[div[(contains(.,'NCAA'))]]]])["+ sports +"]", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
				else {
					sports = driver.FindElements("xpath", "//a[contains(@class,'entity-list-row-container')][div[div[div[(contains(.,'NFL') or contains(.,'MLB') or contains(.,'NBA') or contains(.,'NHL'))]]]]").Count; 
					sports = random.Next(1, sports+1);
					steps.Add(new TestStep(order, "Clicking Randomized Pro Sport", "", "click", "xpath", "(//a[contains(@class,'entity-list-row-container')][div[div[div[(contains(.,'NFL') or contains(.,'MLB') or contains(.,'NBA') or contains(.,'NHL'))]]]])["+ sports +"]", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
				
				// Capture League for Teams
				if (!step.Name.Contains("Player")) {
					steps.Add(new TestStep(order, "Capture League Entity", "LEAGUE", "capture", "xpath", "//a[contains(@class,'explore-league-header')]", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
				
				// Allows for favoriting Leagues
				if(step.Name.Contains("League") || step.Name.Contains("Conference") || step.Name.Contains("NCAA Player")) {
					// set proper league names
					if (DataManager.CaptureMap["LEAGUE"]) {
						switch(DataManager.CaptureMap["LEAGUE"]) {
							case "NFL":
								fullName = "National Football League";
								break;
							case "MLB":
								fullName = "Major League Baseball";
								break;
							case "NBA":
								fullName = "National Basketball Association";
								break;
							case "NHL":
								fullName = "National Hockey League";
								break;
							case "NCAA BK":
								fullName = "NCAA Basketball";
								sport = " Basketball";
								break;
							case "NCAA FB": 
								fullName = "NCAA Football";
								sport = " Football";
								break;
						}						
					}

					if (step.Name.Contains("Conference") || step.Name.Contains("NCAA Player")) {
						sports = driver.FindElements("xpath", "//a[not(contains(@class,'explore-league-header')) and contains(@class,'entity-list-row-container')]").Count; 
						sports = random.Next(1, sports+1);
						steps.Add(new TestStep(order, "Click into Conference", "", "click", "xpath", "(//a[not(contains(@class,'explore-league-header')) and contains(@class,'entity-list-row-container')])["+ sports +"]", wait));
						steps.Add(new TestStep(order, "Capture Conference", "CONF", "capture", "xpath", "(//a[not(contains(@class,'explore-league-header')) and contains(@class,'entity-list-row-container')])["+ sports +"]", wait));
						fullName = DataManager.CaptureMap["CONF"] + sport;
					}

					steps.Add(new TestStep(order, "Favorite League/Conference", "", "click", "xpath", "//a[contains(@class,'explore-league-header')]", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
				
				// Select a Team for Team/Player Favorites
				if(step.Name.Contains("Team") || step.Name.Contains("Player")) {
					sports = driver.FindElements("xpath", "//a[contains(@class,'entity-list-row-container')]").Count; 
					sports = random.Next(1, sports+1);
					steps.Add(new TestStep(order, "Capture Team Name", "TEAM", "capture", "xpath", "(//a[contains(@class,'entity-list-row-container')])["+ sports +"]", wait));
					steps.Add(new TestStep(order, "Click into Team", "", "click", "xpath", "(//a[contains(@class,'entity-list-row-container')])["+ sports +"]", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
				
				// Allow for Favoriting Players
				if(step.Name.Contains("Player")) {
					sports = driver.FindElements("xpath", "//a[contains(@class,'entity-list-row-container')]").Count; 
					steps.Add(new TestStep(order, "Capture Player Name", "PLAYER", "capture", "xpath", "(//a[contains(@class,'entity-list-row-container')])["+ sports +"]", wait));
					steps.Add(new TestStep(order, "Select Player", "", "click", "xpath", "(//a[contains(@class,'entity-list-row-container')])["+ sports +"]", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
					fullName = DataManager.CaptureMap["PLAYER"];
				}				
				
				// Verify the Toast Message, Close it, and clean up variables
				steps.Add(new TestStep(order, "Verify Toast", fullName + " is added to your favorites.", "verify_value", "xpath", "//span[contains(@class,'toast-msg')]", wait));
				steps.Add(new TestStep(order, "Close Toast", "", "click", "xpath", "//div[contains(@class,'toast')]//div[contains(@class,'close-icon')]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				DataManager.CaptureMap.Remove("LEAGUE");
				DataManager.CaptureMap.Remove("CONF");
				DataManager.CaptureMap.Remove("TEAM");
				DataManager.CaptureMap.Remove("PLAYER");
				steps.Clear();
			}
		}
	}
}