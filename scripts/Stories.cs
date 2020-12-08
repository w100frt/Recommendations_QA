using System;
using System.Globalization;
using System.Collections.Generic;
using SeleniumProject.Utilities;
using SeleniumProject;
using OpenQA.Selenium;
using log4net;
using System.Collections.ObjectModel;

namespace SeleniumProject.Function
{
	public class Script : ScriptingInterface.IScript
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		
		public void Execute(DriverManager driver, TestStep step)
		{
			long order = step.Order;
			string wait = step.Wait != null ? step.Wait : "";
			IWebElement ele;
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			int eleCount = 0;
			int total;
			int size = 0;
			int scrolls = 20;
			string date = "";
			string cat = "";
            bool displayed = false;
			List<string> categories = new List<string>();
			List<TestStep> steps = new List<TestStep>();
			VerifyError err = new VerifyError();
			TextInfo ti = new CultureInfo("en-US",false).TextInfo;

			if (step.Name.Equals("Click Arrow Forward to End of Carousel")) {
				eleCount = driver.FindElements("xpath", "//div[not(contains(@class,'scorestrip')) and contains(@class,'carousel-wrapper') and contains(@class,'can-scroll-right')]").Count; 
				while (eleCount > 0) {
					steps.Add(new TestStep(order, "Click Arrow Forward", "", "click", "xpath", "//button[@class='carousel-button-next image-button']", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();	
					eleCount = driver.FindElements("xpath", "//div[contains(@class,'carousel') and contains(@class,'can-scroll-right')]").Count;			
				}
			}
			
			else if (step.Name.Equals("Verify Number of Story Cards")) {
				try {
					total = Int32.Parse(step.Data);
				}
				catch (Exception e) {
					total = 50;
					log.Error("Expected data to be a numeral. Setting data to 50.");
				}
			
				size = driver.FindElements("xpath", "//div[contains(@class,'cards-slide')]//a[contains(@class,'card-story')]").Count;
				
				if (size >= total && size <= 100) {
					log.Info("Verification PASSED. Total Stories [" + size + "] is between " + total + " and 100.");
				}
				else {
					log.Error("***Verification FAILED. " + size + " is not between " + total +" and 100***");
					err.CreateVerificationError(step, ">= " + total + " & <= 100", size.ToString());
				}
			}
			
			else if (step.Name.Equals("Verify Story Date")) {			
				date = driver.FindElement("xpath", "//div[contains(@class,'info-text')]").Text;
				
				if (date.Contains("•")) {
					date = date.Substring(date.IndexOf("•") + 2);
				}
				
				if (date.Equals(step.Data)) {
					log.Info("Verification PASSED. Expected value [" + step.Data + "] equals  actual value [" + date + "]");
				}
				else if (date.Contains("MINS AGO") && step.Data.Contains("MINS AGO")) {
					log.Info("Verification PASSED. [" + step.Data + "] refers to same value as [" + date + "]");
				}
				else {
					log.Error("***Verification FAILED. Expected [" + step.Data + "] does not equal actual value [" + date +"]***");
					err.CreateVerificationError(step, step.Data, date);
				}
			}
			
			else if (step.Name.Contains("Stories Category by Sport")) {
				
				switch(step.Data) {
					case "NFL":
						string[] nfl_teams = {"NATIONAL FOOTBALL LEAGUE", "ARIZONA CARDINALS", "ATLANTA FALCONS", "BALTIMORE RAVENS", "BUFFALO BILLS", "CAROLINA PANTHERS", "CHICAGO BEARS", "CINCINNATI BENGALS", "CLEVELAND BROWNS", "DALLAS COWBOYS", "DENVER BRONCOS", "DETROIT LIONS", "GREEN BAY PACKERS", "HOUSTON TEXANS", "INDIANAPOLIS COLTS", "JACKSONVILLE JAGUARS", "KANSAS CITY CHIEFS", "LAS VEGAS RAIDERS", "LOS ANGELES CHARGERS", "LOS ANGELES RAMS", "MIAMI DOLPHINS", "MINNESOTA VIKINGS", "NEW ENGLAND PATRIOTS", "NEW ORLEANS SAINTS", "NEW YORK GIANTS", "NEW YORK JETS", "PHILADELPHIA EAGLES", "PITTSBURGH STEELERS", "SAN FRANCISCO 49ERS", "SEATTLE SEAHAWKS", "TAMPA BAY BUCCANEERS", "TENNESSEE TITANS", "WASHINGTON FOOTBALL TEAM"};
						string[] nfl_players = {"CAM NEWTON", "PATRICK MAHOMES II", "TOM BRADY", "TUA TAGOVAILOA", "DAK PRESCOTT", "ALEX SMITH", "GEORGE KITTLE", "TEDDY BRIDGEWATER", "JOE BURROW", "BAKER MAYFIELD", "LAMAR JACKSON"};
						categories.AddRange(nfl_teams);
						categories.AddRange(nfl_players);
						break;
					case "NBA":
						string[] nba_teams = {"NATIONAL BASKETBALL ASSOCIATION", "ATLANTA HAWKS", "BOSTON CELTICS", "BROOKLYN NETS", "CHARLOTTE HORNETS", "CHICAGO BULLS", "CLEVELAND CAVALIERS", "DALLAS MAVERICKS", "DENVER NUGGETS", "DETROIT PISTONS", "GOLDEN STATE WARRIORS", "HOUSTON ROCKETS", "INDIANA PACERS", "LOS ANGELES CLIPPERS", "LOS ANGELES LAKERS", "MEMPHIS GRIZZLIES", "MIAMI HEAT", "MILWAUKEE BUCKS", "MINNESOTA TIMBERWOLVES", "NEW ORLEANS PELICANS", "NEW YORK KNICKS", "OKLAHOMA CITY THUNDER", "ORLANDO MAGIC", "PHILADELPHIA 76ERS", "PHOENIX SUNS", "PORTLAND TRAIL BLAZERS", "SACRAMENTO KINGS", "SAN ANTONIO SPURS", "TORONTO RAPTORS", "UTAH JAZZ", "WASHINGTON WIZARDS"};
						string[] nba_players = {"LEBRON JAMES", "KEVIN DURANT", "JAMES HARDEN"};
						categories.AddRange(nba_teams);
						categories.AddRange(nba_players);
						break;
					case "NHL":
						
						break;
					case "MLB":
						string[] mlb = {"MAJOR LEAGUE BASEBALL", "ARIZONA DIAMONDBACKS", "ATLANTA BRAVES", "BALTIMORE ORIOLES", "BOSTON RED SOX", "CHICAGO CUBS", "CHICAGO WHITE SOX", "CINCINNATI REDS", "CLEVELAND INDIANS", "COLORADO ROCKIES", "DETROIT TIGERS", "HOUSTON ASTROS", "KANSAS CITY ROYALS", "LOS ANGELES ANGELS", "LOS ANGELES DODGERS", "MIAMI MARLINS", "MILWAUKEE BREWERS", "MINNESOTA TWINS", "NEW YORK METS", "NEW YORK YANKEES", "OAKLAND ATHLETICS", "PHILADELPHIA PHILLIES", "PITTSBURGH PIRATES", "SAN DIEGO PADRES", "SAN FRANCISCO GIANTS", "SEATTLE MARINERS", "ST. LOUIS CARDINALS", "TAMPA BAY RAYS", "TEXAS RANGERS", "TORONTO BLUE JAYS", "WASHINGTON NATIONALS"};
						categories.AddRange(mlb);
						break;
					case "NCAA FB":
						string[] cfb = {"COLLEGE FOOTBALL"};
						categories.AddRange(cfb);
						break;
					default :
						
						break;
				}
				size = driver.FindElements("xpath", "//div[contains(@class,'cards-slide-')]//a[contains(@class,'card-story')]").Count;
				for (int i = 1; i <= size; i++) {
					if (i==4 && step.Name.Contains("Carousel")) {
						ele = driver.FindElement("xpath", "//div[contains(@class,'carousel-wrapper')]");
						js.ExecuteScript("arguments[0].scrollIntoView(true);",ele);
						steps.Add(new TestStep(order, "Scroll Carousel Right", "", "click", "xpath", "//button[contains(@class,'carousel-button-next')]", wait));
						TestRunner.RunTestSteps(driver, null, steps);
						steps.Clear();
					}
					cat = driver.FindElement("xpath","(//div[contains(@class,'card-grid-header')])["+i+"]").Text;
					if (categories.Contains(cat)) {
						log.Info("Story " + i + " Passed. Category [" + cat + "] falls under " + step.Data);
					}
					else {
						log.Error("***VERIFICATION FAILED. Story " + i + ". Category [" + cat + "] DOES NOT fall under " + step.Data);
						err.CreateVerificationError(step, cat, step.Data);
						driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
					}
				}
			}			
			
			else if (step.Name.Equals("Verify Tag Exists by Name")) {			
					steps.Add(new TestStep(order, "Verify Tag", "", "verify_displayed", "xpath", "//div[contains(@class,'story-topic-group')]//span[.='"+ ti.ToTitleCase(step.Data.ToLower()) +"']", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
			}
			
			else if (step.Name.Equals("Scroll Through Story")) {
				ele = driver.FindElement("xpath", "//div[@class='story-favorites-section-add']");
				js.ExecuteScript("arguments[0].scrollIntoView(true);",ele);
				log.Info("Scrolling down on page...");
			}
			
			else {
				log.Warn("Test Step not found in script...");
			}
		}
	}
}