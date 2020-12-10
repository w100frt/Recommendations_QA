using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
			string data = "";
			List<string> confTeams = new List<string>();
			string team1 = "";
			string team2 = "";
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver.GetDriver();
			VerifyError err = new VerifyError();
			ReadOnlyCollection<IWebElement> elements;
			int count = 0;
			
			if (step.Name.Contains("Verify Top Action by Conference")) {
				// set teams for each conference
				switch(step.Data) {
					case "ACC":
						string[] acc = {"BOSTON COLLEGE", "CLEMSON", "DUKE", "FLORIDA STATE", "GEORGIA TECH", "LOUISVILLE", "MIAMI (FL)", "NORTH CAROLINA STATE", "NORTH CAROLINA", "NOTRE DAME", "PITTSBURGH", "SYRACUSE", "VIRGINIA", "VIRGINIA TECH", "WAKE FOREST"};
						confTeams.AddRange(acc);
						break;
					case "Big 12":
						string[] bigTwelve = {"BAYLOR", "IOWA STATE", "KANSAS", "KANSAS STATE", "OKLAHOMA", "OKLAHOMA STATE", "TCU", "TEXAS", "TEXAS TECH", "WEST VIRGINIA"};
						confTeams.AddRange(bigTwelve);
						break;
					case "Big East":
						string[] bigEast = {"BUTLER", "CONNECTICUT", "CREIGHTON", "DEPAUL", "GEORGETOWN", "MARQUETTE", "PROVIDENCE", "SETON HALL", "ST. JOHN'S", "VILLANOVA", "XAVIER"};
						confTeams.AddRange(bigEast);
						break;
					case "Big Ten":
					case "Big 10":
					case "B1G":
						string[] bigTen = {"ILLINOIS", "INDIANA", "IOWA", "MARYLAND", "MICHIGAN STATE", "MICHIGAN", "MINNESOTA", "NEBRASKA", "NORTHWESTERN", "OHIO STATE", "PENN STATE", "PURDUE", "RUTGERS", "WISCONSIN"};
						confTeams.AddRange(bigTen);
						break;
					case "Pac-12":
						string[] pac = {"ARIZONA STATE", "ARIZONA", "CALIFORNIA", "COLORADO", "OREGON", "OREGON STATE", "STANFORD", "UCLA", "USC", "UTAH", "WASHINGTON", "WASHINGTON STATE"};
						confTeams.AddRange(pac);
						break;
					case "SEC":
						string[] sec = {"ALABAMA", "ARKANSAS", "AUBURN", "FLORIDA", "GEORGIA", "KENTUCKY", "LSU", "MISSISSIPPI STATE", "MISSOURI", "OLE MISS", "SOUTH CAROLINA", "TENNESSEE", "TEXAS A&M", "VANDERBILT"};
						confTeams.AddRange(sec);
						break;
					default :
						
						break;
				}
				count = driver.FindElements("xpath", "//a[contains(@class,'event-card')]").Count;
				if (count > 0) {
					for (int i = 1; i <= count; i++) {
						team1 = driver.FindElement("xpath","((//div[contains(@class,'event-card')])["+i+"]//div[contains(@class,'fs-14')])[1]").Text;
						team2 = driver.FindElement("xpath","((//div[contains(@class,'event-card')])["+i+"]//div[contains(@class,'fs-14')])[2]").Text;
						if (confTeams.Contains(team1) || confTeams.Contains(team2)) {
							log.Info("VERIFICATION PASSED. " + team1 + " or " + team2 + " is in " + step.Data + " conference.");
						}
						else {
							log.Error("***VERIFICATION FAILED. " + team1 + " or " + team2 + " is NOT in " + step.Data + " conference.");
							err.CreateVerificationError(step, step.Data + " Team", team1 + " & " + team2);
							driver.TakeScreenshot(DataManager.CaptureMap["TEST_ID"] + "_verification_failure_" + DataManager.VerifyErrors.Count);
						}
					}					
				}
				else {
					log.Warn("No Games Found under " + step.Data + " Odds");
					steps.Add(new TestStep(order, "Verify Number of Header Items", "- No Data Available -", "verify_value", "xpath", "//div[contains(@class,'no-data')]", wait));
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
			}
			
			else {
				throw new Exception("Test Step not found in script");
			}
		}
	}
}