# Stakecom_Sportsbook_BigBets_Scanner

Stake.com is an online bookmaker, that allows you to bet using cryptocurrencies.
But the best thing is, that all bets are public! 
You can see how other people are betting (tip, stake, odds etc.) - so it's a great thing to analyse. 
Suddenly, API is not ready yet, so I decided to write my own bet parser, which allows me to save the data in text format.

My target is to collect data about all high stakes bet, and see if there is some pattern to find most valuable bets (maybe there is "big fish" with inside info?) and create some type of 'great bet' detector.

Milestones:
- (DONE) Create parser which allows to save information about bets to JSON file

- Create a live detector which will notificate about high stake bet

Next step: Make a loop which is going to look for new bets; and save everything in a file


Example Parse:
- sport: Ice Hockey
- matchname: PHI - BOS
- odds: 2,1
- stake: 0,12397704 btc
