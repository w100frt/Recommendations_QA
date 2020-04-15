using System;
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
			int size;
			string title;
			string date;
			string data;
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			VerifyError err = new VerifyError();
			
			if (step.Name.Equals("Verify Displayed Day on Top Scores")) {
				TimeSpan time = DateTime.Now.TimeOfDay;
				var now = time.Hours;
				if (now < 11)
					step.Data = "YESTERDAY";
				else 
					step.Data = "TODAY";
				
				steps.Add(new TestStep(order, "Verify Displayed Day on Top Scores", step.Data, "verify_value", "xpath", "//div[contains(@class,'scores-date')]//div[contains(@class,'sm')]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Equals("Scroll Top Scores Page to Yesterday")) {
				title = "//div[contains(@class,'scores-date')]//div[contains(@class,'sm')]";
				ele = driver.FindElement("xpath", title);
				date = ele.GetAttribute("innerText");
				
				if (!date.Equals("YESTERDAY")) {
					do {
						js.ExecuteScript("window.scrollBy(0,-250)");
						log.Info("Scrolling up on page...");
						Thread.Sleep(1000);
						ele = driver.FindElement("xpath", title);
						date = ele.GetAttribute("innerText");
						var bottom = js.ExecuteScript("return document.body.scrollHeight");
						log.Info(bottom);
					}
					while (!date.Equals("YESTERDAY"));
					steps.Add(new TestStep(order, "Verify Displayed Day on Top Scores", "YESTERDAY", "verify_value", "xpath", title, wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
				else {
					log.Info("Page defaulted to YESTERDAY");
				}
			}
			
			else if (step.Name.Equals("Scroll Top Scores Page Down to Today")) {
				title = "//div[contains(@class,'scores-date')]//div[contains(@class,'sm')]";
				ele = driver.FindElement("xpath", title);
				date = ele.GetAttribute("innerText");
				
				if (!date.Equals("TODAY")) {
					do {
						js.ExecuteScript("window.scrollBy(0,250)");
						log.Info("Scrolling down on page...");
						Thread.Sleep(1000);
						ele = driver.FindElement("xpath", title);
						date = ele.GetAttribute("innerText");						
						var bottom = js.ExecuteScript("return document.body.scrollHeight");
						log.Info(bottom);
					}
					while (!date.Equals("TODAY"));
				}
				else {
					log.Info("Page defaulted to TODAY");
				}
			}
			
			else if (step.Name.Equals("Scroll Top Scores Page to Tomorrow")) {
				title = "//div[contains(@class,'scores-date')]//div[contains(@class,'sm')]";
				ele = driver.FindElement("xpath", title);
				date = ele.GetAttribute("innerText");
				
				if (!date.Equals("TOMORROW")) {
					do {
						js.ExecuteScript("window.scrollBy(0,250)");
						log.Info("Scrolling down on page...");
						Thread.Sleep(1000);
						ele = driver.FindElement("xpath", title);
						date = ele.GetAttribute("innerText");
					}
					while (!date.Equals("TOMORROW"));
					steps.Add(new TestStep(order, "Verify Displayed Day on Top Scores", "TOMORROW", "verify_value", "xpath", title, wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
					var bottom = js.ExecuteScript("return document.body.scrollHeight");
						log.Info(bottom);
				}
				else {
					log.Info("Page defaulted to TOMORROW");
				}
			}
			
			else if (step.Name.Equals("Verify League Title on Top Scores")) {
				switch(step.Data) {
					case "Scorestrip" : 
						title = "//div[contains(@class,'homepage-module')]//a[@class='score-chip']";
						break;
					case "Yesterday" : 
						title = "(//div[@class='scores'])[1]//a[@class='score-chip']";
						break;
					case "Today" : 
						title = "(//div[@class='scores'])[2]//a[@class='score-chip']";
						break;
					case "Tomorrow" : 
						title = "(//div[@class='scores'])[3]//a[@class='score-chip']";
						break;
					default: 
						title = "//div[@class='scores']//a[@class='score-chip']";
						break;
				}
				
				size = driver.FindElements("xpath", title).Count;
				for (int i = 1; i <= size; i++) {
					ele = driver.FindElement("xpath", "(" + title + "//div[@class='highlight-text']//div[contains(@class,'league-title')])[" + i + "]");
					data = ele.GetAttribute("innerText");
					
					if(!String.IsNullOrEmpty(data)) {
						log.Info("Score Chip " + i + " League Title equals " + data);
					}
					else {
						err.CreateVerificationError(step, "Expected League Title", data);
					}
				}
			}
			
			else if(step.Name.Equals("Capture Team Info from Scorestrip")) {
				data = step.Data;
				steps.Add(new TestStep(order, "Capture Away Team Abbreviation", "AWAY_TEAM_ABB"+ data, "capture", "xpath", "((//a[@class='score-chip'])["+ data +"]//div[@class='teams']//div[contains(@class,'abbreviation')])[1]", wait));
				steps.Add(new TestStep(order, "Capture Away Team", "AWAY_TEAM"+ data, "capture", "xpath", "((//a[@class='score-chip'])["+ data +"]//div[@class='teams']//div[contains(@class,' team')])[1]", wait));
				steps.Add(new TestStep(order, "Capture Home Team Abbreviation", "HOME_TEAM_ABB"+ data, "capture", "xpath", "((//a[@class='score-chip'])["+ data +"]//div[@class='teams']//div[contains(@class,'abbreviation')])[2]", wait));
				steps.Add(new TestStep(order, "Capture Home Team", "HOME_TEAM"+ data, "capture", "xpath", "((//a[@class='score-chip'])["+ data +"]//div[@class='teams']//div[contains(@class,' team')])[2]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if(step.Name.Equals("Click Scorechip By Number")) {
				data = step.Data;
				steps.Add(new TestStep(order, "Click Event " + data, "", "click", "xpath", "(//a[@class='score-chip'])["+ data +"]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}

		}
	}
}