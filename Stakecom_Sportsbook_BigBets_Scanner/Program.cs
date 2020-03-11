using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Text.RegularExpressions;
using System.Drawing;

namespace Stakecom_Sportsbook_BigBets_Scanner
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Bet> bets = new List<Bet>();

            var url = "https://stake.com/sports";

            var chromeDriver = new ChromeDriver();
            chromeDriver.Manage().Window.Size = new Size(1920, 1080); //resize chrome window to fullsize page version
            chromeDriver.Navigate().GoToUrl(url);

            //chromeDriver.FindElement(By.XPath("//*[@id='scrollable-main']/main/div/div/div[3]/ul/li[3]/label/span[1]/select/option[3]")).Click(); //show last 50 bets (default is 10)
            chromeDriver.FindElement(By.XPath("//*[@id='scrollable-main']/main/div/div/div[3]/ul/li[2]")).Click(); //select High Rollers card
            System.Threading.Thread.Sleep(1000); //this page after loading have a next short reload after a while - this delay is to prevent reading data from unloaded page

            //Get table with last bets
            var table = chromeDriver.FindElement(By.XPath("//*[@id='scrollable-main']/main/div/div/div[3]/div/div/div/table/tbody"));

            for (int i = 1; i < 10; i++) //10 last bets
            {
                //Get bet direct link
                string link = chromeDriver.FindElement(By.XPath("//*[@id='scrollable-main']/main/div/div/div[3]/div/div/div/table/tbody/tr[" + i + "]/td[1]/a")).GetAttribute("href");

                //Get sport name
                string sportName = chromeDriver.FindElement(By.XPath("//*[@id='scrollable-main']/main/div/div/div[3]/div/div/div/table/tbody/tr[" + i + "]/td[1]")).Text;

                //Get event name
                string eventName = chromeDriver.FindElement(By.XPath("//*[@id='scrollable-main']/main/div/div/div[3]/div/div/div/table/tbody/tr[" + i + "]/td[2]")).Text;

                //Get username
                string username = chromeDriver.FindElement(By.XPath("//td[3]")).Text;

                //Get odds (multiplayer)
                string strOdds = chromeDriver.FindElement(By.XPath("//*[@id='scrollable-main']/main/div/div/div[3]/div/div/div/table/tbody/tr[" + i + "]/td[5]")).Text;
                double odds = double.Parse(strOdds.Replace("×", "")); //convert string to double

                //Get Stake
                string strStake = chromeDriver.FindElement(By.XPath("//*[@id='scrollable-main']/main/div/div/div[3]/div/div/div/table/tbody/tr[" + i + "]/td[6]")).Text;
                double stake = double.Parse(strStake.Replace('.', ',')); //replace '.' to ',' to get correct format before parsing

                //Get Cryptocurrency name
                var element = chromeDriver.FindElement(By.XPath("//*[@id='scrollable-main']/main/div/div/div[3]/div/div/div/table/tbody/tr[" + i + "]/td[6]"));
                element = element.FindElement(By.TagName("use"));
                string cryptocurrency = element.GetAttribute("xlink:href").Substring(6);

                bets.Add(new Bet(link, sportName, eventName, username, odds, stake, cryptocurrency));
            }


            foreach (var bet in bets) //print all bets
            {
                Console.WriteLine(bet.ToString());
            }


            Console.ReadLine();
        }
    }
}