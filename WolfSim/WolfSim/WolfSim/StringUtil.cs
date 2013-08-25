﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WolfSim
{
    class StringUtil
    {
        private static string[] fortunes = new string[]
            {
                "Some men are discovered; others are found out.",
                "Words must be weighed, not counted.",
                "By failing to prepare, you are preparing to fail.",
                "He who spends a storm beneath a tree, takes life with a grain of TNT.",
                "You attempt things that you do not even plan because of your extreme stupidity.",
                "Take care of the luxuries and the necessities will take care of themselves.",
                "Words are the voice of the heart.",
                "Your mind understands what you have been taught; your heart, what is true.",
                "A king's castle is his home.",
                "He who has a shady past knows that nice guys finish last.",
                "The universe is laughing behind your back.",
                "The best prophet of the future is the past.",
                "It is a poor judge who cannot award a prize.",
                "Even the boldest zebra fears the hungry lion.",
                "Money will say more in one moment than the most eloquent lover can in years.",
                "Money may buy friendship but money can not buy love.",
                "Might as well be frank, monsieur. It would take a miracle to get you out of Casablanca.",
                "Creditors have much better memories than debtors.",
                "Many pages make a thick book.",
                "Every purchase has its price.",
                "Do not underestimate the power of the Force.",
                "You will step on the night soil of many countries.",
                "Mind your own business, Spock. I'm sick of your halfbreed interference.",
                "He who invents adages for others to peruse takes along rowboat when going on cruise.",
                "Of all forms of caution, caution in love is the most fatal.",
                "If you suspect a man, don't employ him.",
                "The Tree of Learning bears the noblest fruit, but noble fruit tastes bad.",
                "Stop searching forever. Happiness is unattainable.",
                "A man who fishes for marlin in ponds will put his money in Etruscan bonds.",
                "A good memory does not equal pale ink.",
                "How sharper than a hound's tooth it is to have a thankless serpent.",
                "You dialed 5483",
                "It's later than you think.",
                "Mistakes are oft the stepping stones to failure.",
                "It's not reality that's important, but how you perceive things.",
                "Promptness is its own reward, if one lives by the clock instead of the sword.",
                "Like winter snow on summer lawn, time past is time gone.",
                "Far duller than a serpent's tooth it is to spend a quiet youth.",
                "Let not the sands of time get in your lunch.",
                "The attacker must vanquish; the defender need only survive.",
                "Standing on head makes smile of frown, but rest of face also upside down.",
                "Deprive a mirror of its silver and even the Czar won't see his face.",
                "Man's horizons are bounded by his vision.",
                "To criticize the incompetent is easy; it is more difficult to criticize the competent.",
                "He who has imagination without learning has wings but no feet.",
                "Men seldom show dimples to girls who have pimples.",
                "Troglodytism does not necessarily imply a low cultural level.",
                "You cannot kill time without injuring eternity.",
                "As goatherd learns his trade by goat, so writer learns his trade by wrote.",
                "One man tells a falsehood, a hundred repeat it as true.",
                "Crazee Edeee,  his prices are INSANE!!!",
                "It is better to wear out than to rust out.",
                "When the wind is great, bow before it; when the wind is heavy, yield to it.",
                "The wise shepherd never trusts his flock to a smiling wolf.",
                "It is the wise bird who builds his nest in a tree.",
                "How you look depends on where you go.",
                "A plucked goose doesn't lay golden eggs.",
                "A man who turns green has eschewed protein.",
                "Put not your trust in money, but put your money in trust.",
                "Even a hawk is an eagle among crows.",
                "Even the smallest candle burns brighter in the dark.",
                "People who take cat naps don't usually sleep in a cat's cradle.",
                "A truly wise man never plays leapfrog with a Unicorn.",
                "Do not clog intellect's sluices with bits of knowledge of questionable uses.",
                "Let him who takes the Plunge remember to return it by Tuesday.",
                "Try to divide your time evenly to keep others happy.",
                "You have mail.",
                "His heart was yours from the first moment that you met.",
                "Sin has many tools, but a lie is the handle which fits them all.",
                "Let a fool hold his tongue and he will pass for a sage.",
                "With clothes the new are best, with friends the old are best.",
                "He is truly wise who gains wisdom from another's mishap.",
                "Beware of a dark-haired man with a loud tie.",
                "Today is the last day of your life so far.",
                "Flee at once, all is discovered.",
                "Man who falls in vat of molten optical glass makes spectacle of self.",
                "Go directly to jail.  Do not pass Go, do not collect $200.",
                "For a good time, call 8367-3100.",
                "Those who can, do; those who can't, simulate.",
                "Those who can, do; those who can't, write.  Those who can't write work for the Bell Labs Record.",
                "God does not play dice.",
                "This fortune is inoperative.  Please try another.",
                "Laugh, and the world ignores you.  Crying doesn't help either.",
                "No amount of genius can overcome a preoccupation with detail.",
                "You will feel hungry again in another hour.",
                "You now have Asian Flu.",
                "God made the integers; all else is the work of Man.",
                "Disk crisis, please clean up!",
                "You auto buy now.",
                "Many are called, few are chosen.  Fewer still get to do the choosing.",
                "Try the Moo Shu Pork.  It is especially good today.",
                "Many are cold, but few are frozen.",
                "The early worm gets the bird.",
                "He who hesitates is sometimes saved.",
                "Time is nature's way of making sure that everything doesn't happen at once.",
                "The future isn't what it used to be. (It never was.)",
                "Can't open /usr/lib/fortunes.",
                "If God had wanted you to go around nude, He would have given you bigger hands.",
                "It is better to have loved and lost than just to have lost.",
                "A journey of a thousand miles begins with a cash advance from Sam.",
                "Disk crunch - please clean up.",
                "Center meeting at 4pm in 2C-543",
                "I will never lie to you.",
                "Spock: We suffered 23 casualties in that attack, Captain.",
                "Your computer account is overdrawn.  Please reauthorize.",
                "1 bulls, 3 cows",
                "It's hard to get ivory in Africa, but in Alabama the Tuscaloosa.",
                "Waste not, get your budget cut next year.",
                "Old MacDonald had an agricultural real estate tax abatement.",
                "Snow Day - stay home.",
                "Save gas, don't eat beans.",
                "All that glitters has a high refractive index.",
                "Ignore previous fortune.",
                "When in doubt, lead trump.",
                "23. ...  r-q1",
                "unix soit qui mal y pense",
                "Even a cabbage may look at a king.",
                "Honi soit la vache qui rit.",
                "No directory",
                "Don't eat yellow snow.",
                "One Bell System - it works.",
                "One Bell System - it sometimes works.",
                "* UNIX is a Trademark of Bell Laboratories.",
                "chess tonight",
                "External Security:",
                "Peters hungry, time to eat lunch.",
                "MOUNT TAPE U1439 ON B3, NO RING",
                "A foolish consistency is the hobgoblin of little minds.",
                "IOT trap -- core dumped",
                "IOT trap -- mos dumped",
                "/usr/news/gotcha",
                "Rotten wood can not be carved - Confucius (Analects, Book 5, Ch. 9)",
                "System going down at 1:45 this afternoon for disk crashing.",
                ": is not an identifier",
                "Quantity is no substitute for quality, but its the only one we've got.",
                "Those who can do, those who can't, write.",
                "The more things change, the more they'll never be the same again.",
                "New crypt. See /usr/news/crypt.",
                "You might have mail.",
                "You can't go home again, unless you set $HOME",
                "You are in a maze of twisty little passages, all alike.",
            };

        private static string[][] gameFortunes = new string[][]
        {
            new string[]
            {
            "It is never to late to be what you might have been."
            },
            new string[]
            {
                "When you get into a tight place and everything goes against you, till it seems as though you could not hold on a minute",
                "longer, never give up then, for that is just the place and time that the tide will turn."
            },
            new string[]
            {
                "The door that nobody else will go in at, seems always to swing open widely for me."
            },
            new string[]
            {
                "Sometimes I'm terrified of my heart; of its constant hunger for whatever it is it wants. The way it stops and starts."
            },
            new string[]
            {
                "I dread the events of the future, not in themselves but in their results."
            },
            new string[]
            {
                "There are moments when even to the sober eye of reason, the world of our sad humanity may assume the semblance of Hell."
            },
            new string[]
            {
                "The boundaries which divide Life from Death are at best shadowy and vague. Who shall say where the one ends, and where the",
                "other begins?"
            },
            new string[]
            {
                "The fury of a demon instantly possessed me. I knew myself no longer. My original soul seemed, at once, to take its flight",
                "from my body; and a more than fiendish malevolence, gin-nurtured, thrilled every fibre of my frame."
            },
            new string[]
            {
                "Great sorrow or great joy should bring intense hunger--not abstinence from food, as our novelists will have it."
            }
        };

        private static string[][] Lore = new string[][]
        {
            new string[]
            {
                "People have said that it is a cursed human who transforms into a hungry animal - a were-creature, ",
                "perhaps a were-weasel or a were-wombat.  But these people are wrong."
            },
            new string[]
            {
                "Listen!  I know the real story.  It all came about from a noble experiment gone horribly wrong, a ",
                "brilliant scientist who wanted to do good."
            },
            new string[]
            {
                "Professor Fidelia Penrosa Fisher sought a cure for the darkness in our souls.  She devoted her life",
                "to her elixir.  She hoped to make the first test easy, and so she picked the best and kindest person she knew."
            },
            new string[]
            {
                "It was a dark and stormy night when Professor Fisher poured the silvery liquid into a flask, and invited",
                "her friend to drink.  At first, nothing happened."
            },
            new string[]
            {
                "Poor Fidelia Fisher!  She saw her intelligent and good friend soon descend into cruel ferocity.  Shortly",
                "thereafter, she was eaten, and thus the only hope for cure for this condition perished."
            },
            new string[]
            {
                "I hear tell that the poor drinker of that well-intentioned elixir remains oblivious and does not know what",
                "has happened.  Then, sometimes, a monster from inside overpowers the poor soul.  A savage hunger is loosed on us all!"
            },
        };

        private static string[][] FatherDialog = new string[][]
        {
            new string[]
            {
                "Did you notice how the Willoughby's house is for sale now?  That's because they were all eaten just last week."
            }
        };

        private static string[][] MotherDialog = new string[][]
        {
            new string[]
            {
                "The poor dear really isn't to blame, you know -- It's all becaue of the curse."
            }
        };

        private static int LoreNum = 0;

        public static string[] RandomFather()
        {
            return FatherDialog[Game1.rand.Next() % FatherDialog.Length];
        }

        public static string[] RandomMother()
        {
            return MotherDialog[Game1.rand.Next() % MotherDialog.Length];
        }

        public static string[] RandomFortune()
        {
            return new string[]{fortunes[Game1.rand.Next() % fortunes.Length]};
        }

        public static string[] RandomGameFortune()
        {
            return gameFortunes[Game1.rand.Next() % gameFortunes.Length];
        }

        public static string[] NextLore()
        {
            if (LoreNum >= Lore.Length)
            {
                LoreNum = 0;
            }
            return Lore[LoreNum++];
        }
    }
}
