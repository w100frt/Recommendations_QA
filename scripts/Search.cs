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
			int eleCount = 0;
			int total;
			int size = 0;
			string date = "";
			string cat = "";
			List<string> teams = new List<string>();
			List<TestStep> steps = new List<TestStep>();
			VerifyError err = new VerifyError();
			TextInfo ti = new CultureInfo("en-US",false).TextInfo;

			if (step.Name.Equals("Verify Team Search by Sport")) {
				
				switch(step.Data) {
					case "NFL":
						string[] nfl = {"NFL", "ARIZONA CARDINALS", "ATLANTA FALCONS", "BALTIMORE RAVENS", "BUFFALO BILLS", "CAROLINA PANTHERS", "CHICAGO BEARS", "CINCINNATI BENGALS", "CLEVELAND BROWNS", "DALLAS COWBOYS", "DENVER BRONCOS", "DETROIT LIONS", "GREEN BAY PACKERS", "HOUSTON TEXANS", "INDIANAPOLIS COLTS", "JACKSONVILLE JAGUARS", "KANSAS CITY CHIEFS", "LAS VEGAS RAIDERS", "LOS ANGELES CHARGERS", "LOS ANGELES RAMS", "MIAMI DOLPHINS", "MINNESOTA VIKINGS", "NEW ENGLAND PATRIOTS", "NEW ORLEANS SAINTS", "NEW YORK GIANTS", "NEW YORK JETS", "PHILADELPHIA EAGLES", "PITTSBURGH STEELERS", "SAN FRANCISCO 49ERS", "SEATTLE SEAHAWKS", "TAMPA BAY BUCCANEERS", "TENNESSEE TITANS", "WASHINGTON FOOTBALL TEAM"};
						categories.AddRange(nfl);
						break;
					case "NBA":
						
						break;
					case "NHL":
						
						break;
					case "MLB":
						string[] mlb = {"MLB", "ARIZONA DIAMONDBACKS", "ATLANTA BRAVES", "BALTIMORE ORIOLES", "BOSTON RED SOX", "CHICAGO CUBS", "CHICAGO WHITE SOX", "CINCINNATI REDS", "CLEVELAND INDIANS", "COLORADO ROCKIES", "DETROIT TIGERS", "HOUSTON ASTROS", "KANSAS CITY ROYALS", "LOS ANGELES ANGELS", "LOS ANGELES DODGERS", "MIAMI MARLINS", "MILWAUKEE BREWERS", "MINNESOTA TWINS", "NEW YORK METS", "NEW YORK YANKEES", "OAKLAND ATHLETICS", "PHILADELPHIA PHILLIES", "PITTSBURGH PIRATES", "SAN DIEGO PADRES", "SAN FRANCISCO GIANTS", "SEATTLE MARINERS", "ST. LOUIS CARDINALS", "TAMPA BAY RAYS", "TEXAS RANGERS", "TORONTO BLUE JAYS", "WASHINGTON NATIONALS"};
						teams.AddRange(mlb);
						break;
					default :
						
						break;
				}
				
				foreach (string team in teams) {
					// temporary fix
					steps.Add(new TestStep(order, "Click Search", "", "click", "xpath", "//input[@placeholder='Leagues, teams, players']", wait));
					steps.Add(new TestStep(order, "Enter Search", ti.ToTitleCase(team.ToLower()), "input_text", "xpath", "//input[@placeholder='Leagues, teams, players']", wait));
					if (team.Equals("MLB")) {
						steps.Add(new TestStep(order, "Verify Search Term", "Major League Baseball", "verify_value", "xpath", "(//div[contains(@class,'explore-search')]//div[contains(@class,'row-title')])[1]", wait));
					}
					else {
						steps.Add(new TestStep(order, "Verify Search Term", ti.ToTitleCase(team.ToLower()), "verify_value", "xpath", "(//div[contains(@class,'explore-search')]//div[contains(@class,'row-title')])[1]", wait));
					}
					TestRunner.RunTestSteps(driver, null, steps);
					steps.Clear();
				}
			}
			
			else {
				log.Warn("Test Step not found in script...");
			}
		}
	}
}