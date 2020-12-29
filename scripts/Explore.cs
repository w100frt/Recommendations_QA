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
			Random random = new Random();
			string wait = step.Wait != null ? step.Wait : "";
			string sport = step.Data;
			string activeTeam;
			int total = 0;
			int size = 0;
			string explore = "";
			bool shown = false;
			string teamSelector = "";
            List<TestStep> steps = new List<TestStep>();
			
			if (step.Name.Equals("Verify Teams by Sport")) {
			    steps.Add(new TestStep(order, "Click Sport Menu Open", "", "click", "xpath", "//div[contains(@id,'App') and not(contains(@style,'none'))]//a[contains(@class,'entity-list')][div[div[contains(.,'"+ sport +"')]]]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
				
				var teams = driver.FindElements("xpath", "//div[contains(@class,'explore-basic-rows')]//a"); 
				for (int i = 0; i < teams.Count; i++) {
					int counter = i + 1;
					activeTeam = driver.FindElement("xpath", "(//div[contains(@class,'explore-basic-rows')]//a)["+ counter +"]").GetAttribute("innerText");
					
					if (activeTeam.Equals("NFL")) {
						activeTeam = "NATIONAL FOOTBALL LEAGUE";
					}

					steps.Add(new TestStep(order, "Click Team Name", "", "click", "xpath", "(//div[contains(@class,'explore-basic-rows')]//a)["+ counter +"]", wait));
					steps.Add(new TestStep(order, "Verify Team Name in Header", activeTeam.ToUpper(), "verify_value", "xpath", "//div[contains(@class,'entity-header')]//div[contains(@class,'entity-title')]//span", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
					
					if (i < teams.Count - 1) {
						steps.Add(new TestStep(order, "Click Explore", "", "click", "xpath", "//a[contains(@class,'explore-link')]", wait));
						steps.Add(new TestStep(order, "Click Sport Menu Open", "", "click", "xpath", "//div[contains(@id,'App') and not(contains(@style,'none'))]//a[contains(@class,'entity-list')][div[div[contains(.,'"+ sport +"')]]]", wait));
						TestRunner.RunTestSteps(driver, null, steps);
						steps.Clear();
					}
				}
			}
			
			else if (step.Name.Equals("Verify MLB Teams")) {
			    total = driver.FindElements(
				"xpath","//div[@id='exploreApp']//div[contains(@class,'explore-basic-rows')]//a[not(contains(@class,'header'))]").Count;
				for(int t = 1; t <= total; t++) {
					DataManager.CaptureMap["MLB_TEAM"] = driver.FindElement("xpath","//div[@id='exploreApp']//div[contains(@class,'explore-basic-rows')]//a[not(contains(@class,'header'))]["+ t +"]").Text.ToUpper();
					steps.Add(new TestStep(order, "Click Team " + t, "", "click", "xpath", "//div[@id='exploreApp']//div[contains(@class,'explore-basic-rows')]//a[not(contains(@class,'header'))]["+ t +"]", wait));
					steps.Add(new TestStep(order, "Template for Team " + t, "", "run_template", "xpath", "MLB_Team", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();					
				}
			}
			
			else if (step.Name.Equals("Verify NBA Teams")) {
			    total = driver.FindElements(
				"xpath","//div[@id='exploreApp']//div[contains(@class,'explore-basic-rows')]//a[not(contains(@class,'header'))]").Count;
				for(int t = 1; t <= total; t++) {
					DataManager.CaptureMap["NBA_TEAM"] = driver.FindElement("xpath","//div[@id='exploreApp']//div[contains(@class,'explore-basic-rows')]//a[not(contains(@class,'header'))]["+ t +"]").Text.ToUpper();
					steps.Add(new TestStep(order, "Click Team " + t, "", "click", "xpath", "//div[@id='exploreApp']//div[contains(@class,'explore-basic-rows')]//a[not(contains(@class,'header'))]["+ t +"]", wait));
					steps.Add(new TestStep(order, "Template for Team " + t, "", "run_template", "xpath", "NBA_Team", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();					
				}
			}
			
			else if (step.Name.Equals("Click Entity by Data")) {
				steps.Add(new TestStep(order, "Click " + step.Data, "", "click", "xpath", "//div[contains(@id,'App') and not(contains(@style,'none'))]//a[contains(@class,'entity-list-row-container')][div[contains(.,'"+ step.Data +"')]]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Contains("Capture") && step.Name.Contains("Random Conference")) {
				teamSelector = "//div[contains(@id,'App') and not(contains(@style,'none'))]//a[contains(@class,'entity-list-row-container') and not(contains(@class,'header'))]";
				total = driver.FindElements("xpath", teamSelector).Count; 
				total = random.Next(1, total+1);				
				steps.Add(new TestStep(order, "Capture Randomized Conference", "RANDOM_CONF", "capture", "xpath", "(" + teamSelector + ")["+ total +"]", wait));
				// click as well
				if (step.Name.Contains("Click")) {
					steps.Add(new TestStep(order, "Click Randomized Conference", "", "click", "xpath", "(" + teamSelector + ")["+ total +"]", wait));
				}
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
				DataManager.CaptureMap["RANDOM_CONF_UP"] = DataManager.CaptureMap["RANDOM_CONF"].ToUpper();
			}
			
			else if (step.Name.Contains("Capture") && step.Name.Contains("Random Team")) {
				teamSelector = "//div[contains(@id,'App') and not(contains(@style,'none'))]//a[@class='entity-list-row-container']";
				total = driver.FindElements("xpath", teamSelector).Count; 
				total = random.Next(1, total+1);				
				steps.Add(new TestStep(order, "Capture Randomized Team", "RANDOM_TEAM", "capture", "xpath", "(" + teamSelector + ")["+ total +"]", wait));
				// click as well
				if (step.Name.Contains("Click")) {
					steps.Add(new TestStep(order, "Click Randomized Team", "", "click", "xpath", "(" + teamSelector + ")["+ total +"]", wait));	
				}
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
				DataManager.CaptureMap["RANDOM_TEAM_UP"] = DataManager.CaptureMap["RANDOM_TEAM"].ToUpper();
			}
			
			else if (step.Name.Contains("Capture") && step.Name.Contains("Random Player")) {
				teamSelector = "//div[contains(@id,'App') and not(contains(@style,'none'))]//a[@class='entity-list-row-container']";
				total = driver.FindElements("xpath", teamSelector).Count; 
				total = random.Next(1, total+1);				
				steps.Add(new TestStep(order, "Capture Randomized Player", "RANDOM_PLAYER", "capture", "xpath", "(" + teamSelector + ")["+ total +"]", wait));
				// click as well
				if (step.Name.Contains("Click")) {
					steps.Add(new TestStep(order, "Click Randomized Player", "", "click", "xpath", "(" + teamSelector + ")["+ total +"]", wait));					
				}
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
				DataManager.CaptureMap["RANDOM_PLAYER_UP"] = DataManager.CaptureMap["RANDOM_PLAYER"].ToUpper();
			}		

			else if (step.Name.Equals("Click Explore")) {
				while (!shown && size++ < 3) {
					explore = "//a[contains(@class,'explore-link')]";
					steps.Add(new TestStep(order, "Click Explore", "", "click", "xpath", explore, wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
					explore = driver.FindElement("xpath","//div[@id='ssrExploreApp']").GetAttribute("style");
					log.Info("Style: " + explore);
					if (explore.Equals("display: none;"))
						shown = false;
					else 
						shown = true;
					Thread.Sleep(0500);
				}				
			}			
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}