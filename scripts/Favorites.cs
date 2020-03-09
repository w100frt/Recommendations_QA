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
			string sport = step.Data;
            List<TestStep> steps = new List<TestStep>();
			
			if(step.Name.Contains("Randomize Favorite")) {
				string fullName = "";
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
					steps.Add(new TestStep(order, "Capture League Entity", "LEAGUE", "capture", "xpath", "//a[contains(@class,'explore-league-header')]", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
				
				// Allows for favoriting Leagues
				if(step.Name.Contains("League")) {
					// set proper league names
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
					}
					
					steps.Add(new TestStep(order, "Favorite League", "", "click", "xpath", "//a[contains(@class,'explore-league-header')]", wait));
					steps.Add(new TestStep(order, "Verify Toast", fullName + " is added to your favorites.", "verify_value", "xpath", "//span[contains(@class,'toast-msg')]", wait));
					steps.Add(new TestStep(order, "Close Toast", "", "click", "xpath", "//div[contains(@class,'toast')]//div[contains(@class,'close-icon')]", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
			}
		}
	}
}