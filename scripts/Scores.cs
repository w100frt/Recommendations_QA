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
			int size;
			int scrolls = 20;
			int months;
			int year;
			string title;
			string date;
			string data = "";
			string xpath = "";
			bool stop = false;
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			VerifyError err = new VerifyError();
			
			if (step.Name.Equals("Verify Displayed Day on Top Scores")) {
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
						js.ExecuteScript("window.scrollBy({top: -100,left: 0,behavior: 'smooth'});");
						log.Info("Scrolling up on page...");
						ele = driver.FindElement("xpath", title);
						date = ele.GetAttribute("innerText");
						log.Info(scrolls + " scrolls until limit is reached");
					}
					while (!date.Equals("YESTERDAY") && scrolls-- > 0);
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
						js.ExecuteScript("window.scrollBy({top: 100,left: 0,behavior: 'smooth'});");
						log.Info("Scrolling down on page...");
						ele = driver.FindElement("xpath", title);
						date = ele.GetAttribute("innerText");
						log.Info(scrolls + " scrolls until limit is reached");
					}
					while (!date.Equals("TODAY") && scrolls-- > 0);
					steps.Add(new TestStep(order, "Verify Displayed Day on Top Scores", "TODAY", "verify_value", "xpath", title, wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
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
						js.ExecuteScript("window.scrollBy({top: 100,left: 0,behavior: 'smooth'});");
						log.Info("Scrolling down on page...");
						ele = driver.FindElement("xpath", title);
						date = ele.GetAttribute("innerText");
						log.Info(scrolls + " scrolls until limit is reached");
					}
					while (!date.Equals("TOMORROW") && scrolls-- > 0);
					steps.Add(new TestStep(order, "Verify Displayed Day on Top Scores", "TOMORROW", "verify_value", "xpath", title, wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
				else {
					log.Info("Page defaulted to TOMORROW");
				}
			}
			
			else if (step.Name.Equals("Verify League Title on Top Scores")) {
				switch(step.Data) {
					case "Scorestrip" : 
						title = "//div[contains(@class,'homepage-module')]//a[contains(@class,'score-chip')]";
						break;
					case "Yesterday" : 
						title = "(//div[@class='scores'])[1]//a[contains(@class,'score-chip')]";
						break;
					case "Today" : 
						title = "(//div[@class='scores'])[2]//a[contains(@class,'score-chip')]";
						break;
					case "Tomorrow" : 
						title = "(//div[@class='scores'])[3]//a[contains(@class,'score-chip')]";
						break;
					default: 
						title = "//div[@class='scores']//a[contains(@class,'score-chip')]";
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
			
			else if(step.Name.Equals("Click Scorechip By Number")) {
				data = step.Data;
				steps.Add(new TestStep(order, "Click Event " + data, "", "click", "xpath", "(//a[@class='score-chip'])["+ data +"]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if(step.Name.Equals("Capture Team Info from Chip")) {
				data = step.Data;
				steps.Add(new TestStep(order, "Capture Away Team Abbreviation", "AWAY_TEAM_ABB"+ data, "capture", "xpath", "((//a[@class='score-chip'])["+ data +"]//div[@class='teams']//div[contains(@class,'abbreviation')]//span[contains(@class,'text')])[1]", wait));
				steps.Add(new TestStep(order, "Capture Away Team", "AWAY_TEAM"+ data, "capture", "xpath", "((//a[@class='score-chip'])["+ data +"]//div[@class='teams']//div[contains(@class,' team')]//span[contains(@class,'text')])[1]", wait));
				steps.Add(new TestStep(order, "Capture Away Team Score", "AWAY_TEAM_SCORE"+ data, "capture", "xpath", "((//a[@class='score-chip'])["+ data +"]//div[@class='teams']//div[contains(@class,'team-score')])[1]", wait));
				steps.Add(new TestStep(order, "Capture Home Team Abbreviation", "HOME_TEAM_ABB"+ data, "capture", "xpath", "((//a[@class='score-chip'])["+ data +"]//div[@class='teams']//div[contains(@class,'abbreviation')]//span[contains(@class,'text')])[2]", wait));
				steps.Add(new TestStep(order, "Capture Home Team", "HOME_TEAM"+ data, "capture", "xpath", "((//a[@class='score-chip'])["+ data +"]//div[@class='teams']//div[contains(@class,' team')]//span[contains(@class,'text')])[2]", wait));
				steps.Add(new TestStep(order, "Capture Home Team Score", "HOME_TEAM_SCORE"+ data, "capture", "xpath", "((//a[@class='score-chip'])["+ data +"]//div[@class='teams']//div[contains(@class,'team-score')])[2]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Equals("Click NBA") || step.Name.Equals("Click NCAA BK") || step.Name.Equals("Click MLB") || step.Name.Equals("Click NASCAR") || step.Name.Equals("Click Soccer") || step.Name.Equals("Click NHL") || step.Name.Equals("Click Boxing") || step.Name.Equals("Click NCAA FB") || step.Name.Equals("Click NFL") || step.Name.Equals("Click Golf") || step.Name.Equals("Verify Selected Category")) {
				if (step.Name.Equals("Verify Selected Category")) {
					data = step.Data;
				}
				else {
					data = step.Name.Substring(6);
				}
				switch (data) {
					case "NBA" : 
						xpath = "//a[span[contains(.,'NBA')]]";
						break;
					case "NCAA BK" : 
						xpath = "//a[span[contains(.,'NCAA BK')]]";
						break;
					case "MLB" :
						xpath = "//a[span[contains(.,'MLB')]]";
						break;
					case "NASCAR" : 
						xpath = "//a[span[contains(.,'NASCAR')]]";
						break;
					case "Soccer" :
					case "SOCCER" :
						xpath = "//a[span[contains(.,'SOCCER')]]";
						break;
					case "NHL" : 
						xpath = "//a[span[contains(.,'NHL')]]";
						break;
					case "Boxing" : 
					case "BOXING" :
						xpath = "//a[span[contains(.,'BOXING')]]";
						break;
					case "NCAA FB" :
						xpath = "//a[span[contains(.,'NCAA FB')]]";
						break;
					case "NFL" :
						xpath = "//a[span[contains(.,'NFL')]]";
						break;
					case "Golf" :
					case "GOLF" :
						xpath = "//a[span[contains(.,'GOLF')]]";
						break;
					default: 
						xpath = "//a[span[contains(.,'TOP')]]";
						break;
				}
				
				stop = driver.FindElement("xpath", xpath).Displayed;
				// verify selected category. otherwise, click appropriate scores sport.
				if (step.Name.Equals("Verify Selected Category")) {
					if(!stop) {
						step.Data = "MORE";
					}

					steps.Add(new TestStep(order, "Verify Selected Tab", step.Data, "verify_value", "xpath", "//div[@id='nav-secondary']//a[contains(@class,'selected')]", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
				else {
					if (!stop) {
						steps.Add(new TestStep(order, "Open MORE", "", "click", "xpath", "//a[span[contains(.,'MORE')]]", wait));
					}

					steps.Add(new TestStep(order, "Click " + data, "", "click", "xpath", xpath, wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();					
				}
			}
			
			else if (step.Name.Equals("Verify Selected Date")) {
				switch(step.Data) {
					case "CBK" : 
						size = 3;
						break;
					case "CFB" : 
						size = 12;
						break;
					case "Golf" :
					case "GOLF" :
						size = 11;
						break;
					case "MLB" : 
						size = 9;
						break;
					case "NASCAR" :
						size = 11;
						break;
					case "NBA" : 
						size = 4;
						break;
					case "NHL" : 
						size = 4;
						break;
					case "NFL" :
						size = 12;
						break;
					case "Soccer" :
					case "SOCCER" : 
						size = 12;
						break;
					default: 
						size = 12;
						break;
				}
				
				if (DataManager.CaptureMap.ContainsKey("MONTH") && DataManager.CaptureMap.ContainsKey("DATE")) {
					months = DateTime.ParseExact(DataManager.CaptureMap["MONTH"], "MMMM", CultureInfo.CurrentCulture).Month;
					if (months > size) {
						year = DateTime.Now.Year - 1;
					}
					else {
						year = DateTime.Now.Year;
					}
					log.Info("Event Year: " + year);
					DateTime chosen = new DateTime(year, months, Int32.Parse(DataManager.CaptureMap["DATE"]));
					data = chosen.DayOfWeek.ToString();
					data = data.Substring(0,3).ToUpper() + ", " + DataManager.CaptureMap["MONTH"].Substring(0,3) + " " + DataManager.CaptureMap["DATE"];
				}
				else if (DataManager.CaptureMap.ContainsKey("WEEK") && DataManager.CaptureMap.ContainsKey("WEEK_DATES")) {
					if(DataManager.CaptureMap["WEEK_DATES"].Length >= 5) {
						data = DataManager.CaptureMap["WEEK_DATES"].Substring(1,5);
					}
					else {
						data = DataManager.CaptureMap["WEEK_DATES"];
					}
					data = DataManager.CaptureMap["WEEK"] " - " + data.Trim();
				}
				steps.Add(new TestStep(order, "Selected Date Check", data, "verify_value", "xpath", "//button[contains(@class,'date-picker-title') or contains(@class,'dropdown-title')]", "5"));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();	
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}