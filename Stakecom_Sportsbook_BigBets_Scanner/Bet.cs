﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stakecom_Sportsbook_BigBets_Scanner
{
    public class Bet
    {
        string directLink;
        string sport;
        string match;
        string username;
        double odds;
        double stake;
        string currency;

        public Bet(string directLink, string sport, string match, string username, double odds, double stake, string currency)
        {
            this.directLink = directLink;
            this.sport = sport;
            this.match = match;
            this.username = username;
            this.odds = odds;
            this.stake = stake;
            this.currency = currency;
        }

        public override string ToString()
        {
            return directLink + "\n" + sport + "\n" + match + "\n" + username + "\n" + odds + "\n" + stake + "\n" + currency +"\n";
        }

    }
}