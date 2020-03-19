using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Text.RegularExpressions;
using System.Drawing;
using Newtonsoft.Json;
using System.IO;

namespace Stakecom_Sportsbook_BigBets_Scanner
{
    class Program
    {
        static void Main(string[] args)
        {       
           
            List<Bet> bets = new List<Bet>();
            JsonSerializer serializer = new JsonSerializer();
            var url = "https://stake.com/sports";
            var chromeDriver = new ChromeDriver();

            var urlUniqueList= new List<string>();

            PrepareWebPageToRead(url, chromeDriver);

            while (true)
            {
                try
                {
                    ReadAllBets(serializer, chromeDriver, ref urlUniqueList);
                }
                catch { }
                System.Threading.Thread.Sleep(3 * 60 * 1000); //3 minutes check interval
            }
        }

        private static void PrepareWebPageToRead(string url, ChromeDriver chromeDriver)
        {
            chromeDriver.Manage().Window.Size = new Size(1920, 1080); //resize chrome window to fullsize page version
            chromeDriver.Navigate().GoToUrl(url);
            System.Threading.Thread.Sleep(2000); //this page after loading have a next short reload after a while - this delay is to prevent reading data from unloaded page
            chromeDriver.FindElement(By.XPath("//*[@id='scrollable-main']/main/div/div/div[4]/ul/li[3]/label/span[1]/select/option[3]")).Click(); //show last 50 bets (default is 10)
            chromeDriver.FindElement(By.XPath("//*[@id='scrollable-main']/main/div/div/div[4]/ul/li[2]")).Click(); //select High Rollers card
            System.Threading.Thread.Sleep(2000); //safe time to load table
        }

        private static void ReadAllBets(JsonSerializer serializer, ChromeDriver chromeDriver, ref List<string> urlUniqueList)
        {
            for (int i = 1; i < 50; i++) //50 last bets
            {
                //Get bet direct link
                string link = chromeDriver.FindElement(By.XPath("//*[@id='scrollable-main']/main/div/div/div[4]/div/div/div/table/tbody/tr[" + i + "]/td[1]/a")).GetAttribute("href");

                if (!urlUniqueList.Contains(link)) //check if bet is in database already
                {
                    urlUniqueList.Add(link);

                    //Get sport name
                    string sportName = chromeDriver.FindElement(By.XPath("//*[@id='scrollable-main']/main/div/div/div[4]/div/div/div/table/tbody/tr[" + i + "]/td[1]")).Text;

                    //Get event name
                    string eventName = chromeDriver.FindElement(By.XPath("//*[@id='scrollable-main']/main/div/div/div[4]/div/div/div/table/tbody/tr[" + i + "]/td[2]")).Text;

                    //Get username
                    string username = chromeDriver.FindElement(By.XPath("//td[3]")).Text;

                    //Get odds (multiplayer)
                    string strOdds = chromeDriver.FindElement(By.XPath("//*[@id='scrollable-main']/main/div/div/div[4]/div/div/div/table/tbody/tr[" + i + "]/td[5]")).Text;
                    double odds = double.Parse(strOdds.Replace("×", "")); //convert string to double

                    //Get Stake
                    string strStake = chromeDriver.FindElement(By.XPath("//*[@id='scrollable-main']/main/div/div/div[4]/div/div/div/table/tbody/tr[" + i + "]/td[6]")).Text;
                    double stake = double.Parse(strStake.Replace('.', ',')); //replace '.' to ',' to get correct format before parsing

                    //Get Cryptocurrency name
                    var element = chromeDriver.FindElement(By.XPath("//*[@id='scrollable-main']/main/div/div/div[4]/div/div/div/table/tbody/tr[" + i + "]/td[6]"));
                    element = element.FindElement(By.TagName("use"));
                    string cryptocurrency = element.GetAttribute("xlink:href").Substring(6);

                    Bet bet = new Bet(link, sportName, eventName, username, odds, stake, cryptocurrency);

                    if (ConvertToUSD(stake, cryptocurrency) > 1000 && odds > 1.50)
                    {
                        Console.WriteLine("!!! HIGH STAKE BET - " + ConvertToUSD(stake, cryptocurrency) + "$ !!!");
                        Console.WriteLine(bet.ToString());
                    }

                    using (StreamWriter sw = new StreamWriter("bets.json", true)) //open json file
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(writer, bet); //add bet to json file
                    }

                }
            }
        }

        private static double ConvertToUSD(double value, string cryptocurrency)
        {
            int eth = 130;
            int btc = 6200;
            int ltc = 40;

            int usd;

            switch (cryptocurrency)
            {
                case "eth":
                    return Math.Round(eth* value,0);
                case "btc":
                    return Math.Round(btc * value, 0);
                case "ltc":
                    return Math.Round(ltc * value, 0);
                default:
                    return 1;
            }
        } 
    }
}