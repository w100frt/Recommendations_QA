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
			string activeTeam;
            List<TestStep> steps = new List<TestStep>();
			
			if(step.Name.Contains("Randomize Favorite")) {
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
					steps.Add(new TestStep(order, "Click Sports Menu Open", "", "click", "xpath", "(//a[contains(@class,'entity-list-row-container')][div[div[div[(contains(.,'NCAA'))]]]])["+ sports +"]", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
				else {
					sports = driver.FindElements("xpath", "//a[contains(@class,'entity-list-row-container')][div[div[div[not(contains(.,'NCAA'))]]]]").Count; 
					sports = random.Next(1, sports+1);
					steps.Add(new TestStep(order, "Click Sports Menu Open", "", "click", "xpath", "(//a[contains(@class,'entity-list-row-container')][div[div[div[(contains(.,'NCAA'))]]]])["+ sports +"]", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
				
				// Allows for favoriting Leagues
				if(step.Name.Contains("League")) {
					steps.Add(new TestStep(order, "Capture League Entity", "#LEAGUE", "capture", "//a[contains(@class,'explore-league-header')]", wait));
					steps.Add(new TestStep(order, "Favorite League", "", "click", "xpath", "//a[contains(@class,'explore-league-header')]", wait));
					steps.Add(new TestStep(order, "Verify Toast", DataManager.CaptureMap["LEAGUE"] + " is added to your favorites.", "verify_value", "xpath", "//a[contains(@class,'explore-league-header')]", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
			}
		}
	}
}