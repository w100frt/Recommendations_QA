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
			int variable;
			int scrolls = 20;
			int months = 0;
			int year = 0;
			string date = "";
			string data = "";
			string odd = "";
			string status = "";
			string title;
			string xpath = "";
			bool stop = false;
			bool playoff = false;
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			VerifyError err = new VerifyError();
			
			if (step.Name.Equals("Verify Displayed Day on Top Scores")) {
				TimeSpan time = DateTime.UtcNow.TimeOfDay;
				int now = time.Hours;
				int et = now - 4;
				if (et >= 0 && et < 11){
					log.Info("Current Eastern Time hour is " + et + ". Default to Yesterday.");
					step.Data = "YESTERDAY";
					DataManager.CaptureMap["TOP_DATE"] = "Yesterday";
				}
				else {
					log.Info("Current Eastern Time hour is " + et + ". Default to Today.");
					step.Data = "TODAY";
					DataManager.CaptureMap["TOP_DATE"] = "Today";
				} 	

				steps.Add(new TestStep(order, "Verify Displayed Day on Top Scores", step.Data, "verify_value", "xpath", "//div[contains(@class,'scores-date')]//div[contains(@class,'sm')]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Equals("Scroll Top Scores Page to Yesterday")) {
				title = "//div[contains(@class,'scores-header')]//span";
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
					DataManager.CaptureMap["SCROLLED"] = "YES";
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
					DataManager.CaptureMap["SCROLLED"] = "YES";
				}
				else {
					log.Info("Page defaulted to TODAY");
				}
			}
			
			else if (step.Name.Equals("Scroll Top Scores Page to Tomorrow")) {
				title = "//div[contains(@class,'scores-date') or contains(@class,'week-selector')]//*[contains(@class,'sm-14')]";
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
					DataManager.CaptureMap["SCROLLED"] = "YES";
				}
				else {
					log.Info("Page defaulted to TOMORROW");
				}
			}
			
			else if (step.Name.Equals("Verify Odds Info on Chip")) {
				data = step.Data;
				for (int odds = 1; odds <= 2; odds++) {
					switch(odds) {
						case 1 : 
							date = "//a[contains(@class,'score-chip')]["+ data +"]";
							xpath = "//a[contains(@class,'score-chip')]["+ data +"]//div[contains(@class,'odds')]//span[contains(@class,'secondary-text status')]";
							status = "Spread";
							break;
						case 2 : 
							date = "//a[contains(@class,'score-chip')]["+ data +"]";
							xpath = "//a[contains(@class,'score-chip')]["+ data +"]//div[contains(@class,'odds')]//span[contains(@class,'secondary-text ffn')]";
							status = "Total";
							break;
						default: 
							break;
					}	
					ele = driver.FindElement("xpath", xpath);
					odd = ele.GetAttribute("innerText");
					title = driver.FindElement("xpath", date).GetAttribute("href");
					title = title.Substring(title.IndexOf("?") + 1);
					
					if(!String.IsNullOrEmpty(odd)) {
						log.Info("Score Chip " + data + " (" + title + ") " + status + " equals " + odd);
					}
					else {
						err.CreateVerificationError(step, "Chip: " + title + " Missing Spread", odd);
						driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
					}				
				}
			}
			
			else if (step.Name.Equals("Verify League Title on Top Scores")) {
				switch(step.Data) {
					case "Scorestrip" : 
						title = "//div[contains(@class,'homepage-module')]//a[contains(@class,'score-chip')]";
						break;
					case "Yesterday" : 
						title = DateTime.Today.AddDays(-1).ToString("yyyyMMdd");
						title = "//div[@id='"+ title +"']//a[contains(@class,'score-chip')]";
						break;
					case "Today" : 
						title = DateTime.Today.ToString("yyyyMMdd");
						title = "//div[@id='"+ title +"']//a[contains(@class,'score-chip')]";
						break;
					case "Tomorrow" : 
						title = DateTime.Today.AddDays(+1).ToString("yyyyMMdd");
						title = "//div[@id='"+ title +"']//a[contains(@class,'score-chip')]";
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
				if (DataManager.CaptureMap.ContainsKey("SCROLLED")) {
					xpath = "//div[contains(@class,'score-section')][div[@class='scores-date'][not(div)]]";
				}
				steps.Add(new TestStep(order, "Click Event " + data, "", "click", "xpath", xpath + "//a[contains(@class,'score-chip')]["+ data +"]", wait));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if(step.Name.Equals("Capture Team Info from Chip")) {
				data = step.Data;
				if (DataManager.CaptureMap.ContainsKey("SCROLLED")) {
					xpath = "//div[contains(@class,'score-section')][div[@class='scores-date'][not(div)]]";
				}
				
				if (driver.FindElements("xpath","//a[contains(@class,'score-chip-playoff')]").Count > 0) {
					playoff = true;
				}
				
				if (playoff) {
					steps.Add(new TestStep(order, "Capture Away Team", "AWAY_TEAM", "capture", "xpath", "(" + xpath + "//a[contains(@class,'score-chip-playoff')]["+ data +"]//div[@class='team-texts']//span[contains(@class,'team-name-1')])[1]", wait));
					steps.Add(new TestStep(order, "Capture Home Team", "HOME_TEAM", "capture", "xpath", "(" + xpath + "//a[contains(@class,'score-chip-playoff')]["+ data +"]//div[@class='team-texts']//span[contains(@class,'team-name-2')])[1]", wait));
				}
				
				else {
					steps.Add(new TestStep(order, "Capture Away Team Abbreviation", "AWAY_TEAM_ABB", "capture", "xpath", "(" + xpath + "//a[contains(@class,'score-chip')]["+ data +"]//div[@class='teams']//div[contains(@class,'abbreviation')]//span[contains(@class,'text')])[1]", wait));
					steps.Add(new TestStep(order, "Capture Away Team", "AWAY_TEAM", "capture", "xpath", "(" + xpath + "//a[contains(@class,'score-chip')]["+ data +"]//div[@class='teams']//div[contains(@class,' team')]//span[contains(@class,'text')])[1]", wait));
					steps.Add(new TestStep(order, "Capture Home Team Abbreviation", "HOME_TEAM_ABB", "capture", "xpath", "(" + xpath + "//a[contains(@class,'score-chip')]["+ data +"]//div[@class='teams']//div[contains(@class,'abbreviation')]//span[contains(@class,'text')])[2]", wait));
					steps.Add(new TestStep(order, "Capture Home Team", "HOME_TEAM", "capture", "xpath", "(" + xpath + "//a[contains(@class,'score-chip')]["+ data +"]//div[@class='teams']//div[contains(@class,' team')]//span[contains(@class,'text')])[2]", wait));					
				}
				
				// capture scores for event
				if (DataManager.CaptureMap["EVENT_STATUS"].Equals("LIVE") || DataManager.CaptureMap["EVENT_STATUS"].Equals("FINAL")) {
					if (playoff) {
						steps.Add(new TestStep(order, "Capture Away Team Score", "AWAY_TEAM_SCORE", "capture", "xpath", "(" + xpath + "//a[contains(@class,'score-chip-playoff')]["+ data +"]//div[contains(@class,'score')]//span[contains(@class,'score-1')])[1]", wait));
						steps.Add(new TestStep(order, "Capture Home Team Score", "HOME_TEAM_SCORE", "capture", "xpath", "(" + xpath + "//a[contains(@class,'score-chip-playoff')]["+ data +"]//div[contains(@class,'score')]//span[contains(@class,'score-2')])[1]", wait));
					}
					else {
						steps.Add(new TestStep(order, "Capture Away Team Score", "AWAY_TEAM_SCORE", "capture", "xpath", "(" + xpath + "//a[contains(@class,'score-chip')]["+ data +"]//div[@class='teams']//div[contains(@class,'team-score')])[1]", wait));
						steps.Add(new TestStep(order, "Capture Home Team Score", "HOME_TEAM_SCORE", "capture", "xpath", "(" + xpath + "//a[contains(@class,'score-chip')]["+ data +"]//div[@class='teams']//div[contains(@class,'team-score')])[2]", wait));
					}
				}
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();
			}
			
			else if (step.Name.Equals("Click NBA") || step.Name.Equals("Click NCAA BK") || step.Name.Equals("Click MLB") || step.Name.Equals("Click NASCAR") || step.Name.Equals("Click Soccer") || step.Name.Equals("Click NHL") || step.Name.Equals("Click Boxing") || step.Name.Equals("Click NCAA FB") || step.Name.Equals("Click NFL") || step.Name.Equals("Click Golf") || step.Name.Equals("Click Sport by Name") || step.Name.Equals("Verify Selected Category")) {
				if (step.Name.Equals("Verify Selected Category") || step.Name.Equals("Click Sport by Name")) {
					data = step.Data;
				}
				else {
					data = step.Name.Substring(6);
				}
				switch (data) {
					case "NBA" : 
						xpath = "//div[contains(@class,'desktop')]//a[not(contains(@class,'more-button')) and contains(.,'NBA')]";
						break;
					case "NCAA BK" : 
						xpath = "//div[contains(@class,'desktop')]//a[not(contains(@class,'more-button')) and contains(.,'NCAA BK')]";
						break;
					case "MLB" :
						xpath = "//div[contains(@class,'desktop')]//a[not(contains(@class,'more-button')) and contains(.,'MLB')]";
						break;
					case "NASCAR" : 
						xpath = "//div[contains(@class,'desktop')]//a[not(contains(@class,'more-button')) and contains(.,'NASCAR')]";
						break;
					case "Soccer" :
					case "SOCCER" :
						xpath = "//div[contains(@class,'desktop')]//a[not(contains(@class,'more-button')) and contains(.,'SOCCER')]";
						break;
					case "NHL" : 
						xpath = "//div[contains(@class,'desktop')]//a[not(contains(@class,'more-button')) and contains(.,'NHL')]";
						break;
					case "Boxing" : 
					case "BOXING" :
						xpath = "//div[contains(@class,'desktop')]//a[not(contains(@class,'more-button')) and contains(.,'BOXING')]";
						break;
					case "NCAA FB" :
						xpath = "//div[contains(@class,'desktop')]//a[not(contains(@class,'more-button')) and contains(.,'NCAA FB')]";
						break;
					case "NFL" :
						xpath = "//div[contains(@class,'desktop')]//a[not(contains(@class,'more-button')) and contains(.,'NFL')]";
						break;
					case "Golf" :
					case "GOLF" :
						xpath = "//div[contains(@class,'desktop')]//a[not(contains(@class,'more-button')) and contains(.,'GOLF')]";
						break;
					default: 
						xpath = "//div[contains(@class,'desktop')]//a[contains(.,'TOP')]";
						break;
				}
				
				stop = driver.FindElement("xpath", xpath).Displayed;
				// verify selected category. otherwise, click appropriate scores sport.
				if (step.Name.Equals("Verify Selected Category")) {
					if(!stop) {
						step.Data = "MORE";
					}

					steps.Add(new TestStep(order, "Verify Selected Tab", step.Data, "verify_value", "xpath", "//div[contains(@class,'desktop')]//*[contains(@class,'selected')]", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
				else {
					if (!stop) {
						steps.Add(new TestStep(order, "Open MORE", "", "click", "xpath", "//div[contains(@class,'desktop')]//button[contains(@class,'more-button')]", wait));
					}

					steps.Add(new TestStep(order, "Click " + data, "", "click", "xpath", xpath, wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
				if (DataManager.CaptureMap.ContainsKey("SCROLLED")) {
					DataManager.CaptureMap.Remove("SCROLLED");	
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
						data = DataManager.CaptureMap["WEEK_DATES"].Substring(0,6);
					}
					else {
						data = DataManager.CaptureMap["WEEK_DATES"];
					}
					if (DataManager.CaptureMap.ContainsKey("IN_SEASON")) {
						if (Convert.ToBoolean(DataManager.CaptureMap["IN_SEASON"])) {
							year = DateTime.Now.Year;
						}
						else {
							year = DateTime.Now.Year - 1;
						}
					}
					data = DataManager.CaptureMap["WEEK"].Trim() + " - THU, " + data.Trim();
				}
				steps.Add(new TestStep(order, "Selected Date Check", data, "verify_value", "xpath", "//button[contains(@class,'date-picker-title') or contains(@class,'dropdown-title')]", "5"));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();	
			}
			
			else if (step.Name.Equals("Store Current Number of Score Sections")) {
				title = "//div[contains(@class,'score-section')]";
				size = driver.FindElements("xpath", title).Count;
				log.Info("Storing number of Scores sections displayed: " + size);
				if (DataManager.CaptureMap.ContainsKey("SCORE_SECTIONS")) {
					DataManager.CaptureMap["SCORE_SECTIONS"] = size.ToString();
				}
				else {
					DataManager.CaptureMap.Add("SCORE_SECTIONS", size.ToString());
				}
			}
			
			else if (step.Name.Equals("Verify Number of Score Sections")) {
				if (DataManager.CaptureMap.ContainsKey("SCORE_SECTIONS") && step.Data.Contains("+")) {
					stop = int.TryParse(DataManager.CaptureMap["SCORE_SECTIONS"], out size);
					stop = int.TryParse(step.Data.Substring(step.Data.IndexOf("+") + 1), out variable);
					size = size + variable;
					step.Data = size.ToString();
				}
				steps.Add(new TestStep(order, "Verify Sections", step.Data, "verify_count", "xpath", "//div[contains(@class,'score-section')]", ""));
				TestRunner.RunTestSteps(driver, null, steps);
				steps.Clear();	
			}
			
			else if(step.Name.Equals("Scroll Back One Day")) {
				status = "//div[contains(@class,'scores-app-root')]/div[not(@style='display: none;')]//div[contains(@class,'week-selector')]";
				date = driver.FindElement("xpath", status).Text;
				DataManager.CaptureMap["CURRENT"] = date;
				log.Info("Current Day: " + date);
				if (date.Equals("TODAY")) {
					DataManager.CaptureMap["PREVIOUS"] = "YESTERDAY";
				}
				else if (date.Equals("YESTERDAY")) {
					var today = DateTime.Now;
					var yesterday = today.AddDays(-2);
					DataManager.CaptureMap["PREVIOUS"] = yesterday.ToString("ddd, MMM d").ToUpper();
				}
				else {
					var num = int.Parse(date.Substring(10));
					num = num--;
					var old = new DateTime(DateTime.Now.Year, DateTime.Now.Month, num);
					DataManager.CaptureMap["PREVIOUS"] = old.ToString("ddd, MMM d").ToUpper();
				}
			
				do {
					js.ExecuteScript("window.scrollBy({top: -100,left: 0,behavior: 'smooth'});");
					log.Info("Scrolling up on page...");
					date = driver.FindElement("xpath", status).Text;
					log.Info("Current Day: " + date);
					log.Info(scrolls + " scrolls until limit is reached");
				} while (date.Equals(DataManager.CaptureMap["CURRENT"]) && scrolls-- > 0);
				
				DataManager.CaptureMap["SCROLLED"] = "YES";
				
			}
			
			else if(step.Name.Equals("Scroll Forward One Day")) {
				status = "//div[contains(@class,'scores-app-root')]/div[not(@style='display: none;')]//div[contains(@class,'week-selector')]";
				date = driver.FindElement("xpath", status).Text;
				DataManager.CaptureMap["CURRENT"] = date;

				log.Info("Current Day: " + date);
				if (date.Equals("TODAY")) {
					DataManager.CaptureMap["NEXT"] = "TOMORROW";
				}
				else if (date.Equals("YESTERDAY")) {
					DataManager.CaptureMap["NEXT"] = "TODAY";
				}
				else if (date.Equals("TOMORROW")) {
					var today = DateTime.Now;
					var yesterday = today.AddDays(2);
					DataManager.CaptureMap["NEXT"] = yesterday.ToString("ddd, MMM d").ToUpper();
				}
				else {
					var num = int.Parse(date.Substring(10));
					num = num++;
					var old = new DateTime(DateTime.Now.Year, DateTime.Now.Month, num);
					DataManager.CaptureMap["NEXT"] = old.ToString("ddd, MMM d").ToUpper();
				}
			
				do {
					js.ExecuteScript("window.scrollBy({top: 100,left: 0,behavior: 'smooth'});");
					log.Info("Scrolling down on page...");
					date = driver.FindElement("xpath", status).Text;
					log.Info("Current Day: " + date);
					log.Info(scrolls + " scrolls until limit is reached");
				} while (date.Equals(DataManager.CaptureMap["CURRENT"]) && scrolls-- > 0);
					
				DataManager.CaptureMap["SCROLLED"] = "YES";
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}