using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using static RPC_Bot.Modules.Global_Variables;

namespace RPC_Bot.Services
{
public class HangManClass
{
    public Dictionary<SocketUser, GameInstanceClass> GameInstances = new Dictionary<SocketUser, GameInstanceClass>();
    public List<string> friends;
    public List<string> words;
    string regWord = "[A-Z]";
    public Dictionary<string, GuildEmote> emotes = new Dictionary<string, GuildEmote>();
    public Random rnd = new Random();
    public HangManClass(List<string> _words, List<string> _friends, Dictionary<string, GuildEmote> hangEmotes)
    {
        words = _words;
        friends = _friends;
        emotes = hangEmotes;
    }
    public void NewGame(SocketMessage _message)
    {
        if (GameInstances.ContainsKey(_message.Author))
        {
            if (GameInstances[_message.Author].GameEnded == true)
                GameInstances.Remove(_message.Author);
            else
            {               
                _message.Channel.SendMessageAsync($"You lost 5 points because you haven't finished your previous game.\r\n {GameInstances[_message.Author].Player.Username} now has {RegisteredList[GameInstances[_message.Author].Player.Id].HangManScore} points!");
                        RegisteredList[GameInstances[_message.Author].Player.Id].HangManScore -= 5;
                if (RegisteredList[GameInstances[_message.Author].Player.Id].HangManScore < 0)
                    RegisteredList[GameInstances[_message.Author].Player.Id].HangManScore = 0;
                GameInstances.Remove(_message.Author);
                renewRegisterList = true;
            }
        }
        if (GameInstances.ContainsKey(_message.Author) == false)
        {
            string a = words[rnd.Next(0, (words.Count - 1))];
            string c = friends[rnd.Next(0, (friends.Count - 1))];
            string b="";

            if (a.ToUpper().Contains("GLYPH OF"))
            {
                b = "GLYPH OF ";
                for (int i = 9; i <= a.Length - 1; i++)
                {
                    if (Regex.IsMatch(a[i].ToString().ToUpper(), "[A-Z]"))
                        b = b + " " + "\\_";
                    else
                        b = b + " " + a[i];
                }
            }
            else
            {
                foreach (char chr in a)
                {
                    if (Regex.IsMatch(chr.ToString().ToUpper(), "[A-Z]"))
                        b = b + " " + "\\_";
                    else
                        b = b + " " + chr;
                }
            }

            RestUserMessage mes;
            mes = _message.Channel.SendMessageAsync($@"{_message.Author.Username}, your {c} is captured! Find a correct item to save him! Use `?guess a` to guess by letters, `?word abc` to guess a whole item name.
{b}").Result;
            GameInstances.Add(_message.Author, new GameInstanceClass(a, c, _message.Author, mes));
        }
    }
}

public class GameInstanceClass
{
    public SocketUser Player;
    private string EndGameString;
    private string EndGameFailString;
    public string Word;
    List<LetterClass> Letters = new List<LetterClass>();
    public int Lives;
    public string LettersUsed = "Letters used: ";
    public string myfriend;
    public bool GameEnded = false;
    public RestUserMessage Message;
    public GameInstanceClass(string _word, string _friend, SocketUser _player, RestUserMessage _message)
    {
        Word = Strings.UCase(_word);
        Player = _player;
        myfriend = _friend;
        EndGameString = $@"{Player.Username} rescued his {myfriend} and earned {10 - Lives} points!
{Player.Username} now has {RegisteredList[Player.Id].HangManScore} points!";
        EndGameFailString = $@"{Player.Username} failed to rescue his {myfriend}and lost {5} points!
{Player.Username} now has {RegisteredList[Player.Id].HangManScore} points!";
        Message = _message;

        foreach (char ch in Word)
        {
            Letters.Add(new LetterClass(ch));
                if (Regex.IsMatch(ch.ToString(), "[^A-Z]"))
                Letters.Last().guessed = true;
        }

            if (Word.Contains(" LV"))
            {
                for (int i = Word.Length-1; i >= Word.Length - 5; i--)
                    Letters[i].guessed = true;
            }
            if (Word.Contains("GLYPH OF"))
            {
                for (int i = 0; i <= 8; i++)
                    Letters[i].guessed = true;
            }
            Lives = 0;
    }

    public async Task<bool> GuessWord(string Input)
    {
        string returnString = "";
        if (GameEnded)
            return false;

        string a = "";

        if (Strings.UCase(Word) == Strings.UCase(Input))
        {
            returnString = LifeString(Lives);
            returnString = returnString + Constants.vbCrLf + LettersUsed;
            foreach (LetterClass ch in Letters)
                a = a + " " + Strings.UCase(ch.letter);
            RegisteredList[Player.Id].HangManScore += (10 - Lives);
            returnString = returnString + Constants.vbCrLf + a;
            EndGameString = $@"{Player.Username} rescued his {myfriend} and earned {10 - Lives} points!
{Player.Username} now has {RegisteredList[Player.Id].HangManScore} points!";
            returnString = returnString + Constants.vbCrLf + EndGameString;
        }
        else
        {
            Lives = 8;
            returnString = LifeString(Lives);
            returnString = returnString + Constants.vbCrLf + LettersUsed;
            foreach (LetterClass ch in Letters)
            {
                if (ch.guessed == true)
                    a = a + " " + Strings.UCase(ch.letter);
                else
                   a = a + " " + Strings.UCase(ch.letter);
                    //a = a + @" \_";
            }
            RegisteredList[Player.Id].HangManScore -= 5;
            if (RegisteredList[Player.Id].HangManScore < 0)
                RegisteredList[Player.Id].HangManScore = 0;
            returnString = returnString + Constants.vbCrLf + a;
            EndGameFailString = $@"{Player.Username} failed to rescue his {myfriend} and lost {5} points!
{Player.Username} now has {RegisteredList[Player.Id].HangManScore} points!";
            returnString = returnString + Constants.vbCrLf + EndGameFailString;
        }
        GameEnded = true;
            EndGameUpdateDB();
        renewRegisterList = true;
        await Message.ModifyAsync(msg => msg.Content = "" + returnString + "");
        return true;
    }
    public async Task<bool> Guess(char Input)
    {
        if (GameEnded)
            return false;
        bool guessed = false;
        Input = Strings.UCase(Input);

            if (Word.Contains(Input))
            {
                foreach (LetterClass ch in Letters)
                {
                    if (ch.letter == Input)
                    {
                        if (ch.guessed == false)
                        {
                            ch.guessed = true;
                            guessed = true;
                        }
                    }
                }
            }

        if (guessed == false)
            Lives = Lives + 1;

        string returnString = ""; // $"{Player.Username}, your {myfriend} is captured!{vbCrLf}"
            if (Lives > 0)
                returnString = LifeString(Lives);

        string a = "";
        LettersUsed = LettersUsed + $"{Input}, ";
        returnString = returnString + Constants.vbCrLf + LettersUsed;
        foreach (LetterClass ch in Letters)
        {
            if (ch.guessed == true)
                a = a + " " + Strings.UCase(ch.letter);
            else
                a = a + @" \_";
        }
        bool win = true;
        foreach (LetterClass ch in Letters)
        {
            if (ch.guessed == false)
            {
                win = false;
                break;
            }
        }
        returnString = returnString + Constants.vbCrLf + a;
        if (Lives == 8)
        {
            GameEnded = true;
            renewRegisterList = true;
            RegisteredList[Player.Id].HangManScore -= 5;
            if (RegisteredList[Player.Id].HangManScore < 0)
                RegisteredList[Player.Id].HangManScore = 0;
            EndGameFailString = $@"{Player.Username} failed to rescue his {myfriend} and lost {5} points!
{Player.Username} now has {RegisteredList[Player.Id].HangManScore} points!";
            returnString = returnString + Constants.vbCrLf + EndGameFailString;
                EndGameUpdateDB();
        }
        if (win == true)
        {
            GameEnded = true;
            renewRegisterList = true;
            RegisteredList[Player.Id].HangManScore += (10 - Lives);
            EndGameString = $@"{Player.Username} rescued his {myfriend} and earned {10 - Lives} points!
{Player.Username} now has {RegisteredList[Player.Id].HangManScore} points!";
            returnString = returnString + Constants.vbCrLf + EndGameString;
               EndGameUpdateDB();
        }
        await Message.ModifyAsync(msg => msg.Content= "" + returnString + "");
        return true;
    }

        async void EndGameUpdateDB()
        {
            using (UserContext cont = new UserContext())
            {
                cont.Users.UpdateRange(RegisteredList.Values.ToList());
                await cont.SaveChangesAsync();
            }
        }
    string LifeString(int mlive)
    {
        string[][] matrix = new string[8][];

        string c = "";

        Regex myregex = new Regex(@":[\w]*_[\d]*:");
        switch (Lives)
        {
            case 8:
                {
                    matrix[0] = new string[] { ":hang_01:", ":hang_02:", ":hang_03:", ":hang_04:", ":hang_05:" };
                    matrix[1] = new string[] { ":hang_06:", ":hang_07:", ":hang_08:", ":hang_13:", ":hang_10:" };
                    matrix[2] = new string[] { ":hang_11:", ":hang_12:", ":hang_13:", ":hang_13:", myfriend };
                    matrix[3] = new string[] { ":hang_16:", ":hang_17:", ":hang_18:", ":chad_01:", ":chad_021:", ":chad_03:" };
                    matrix[4] = new string[] { ":hang_16:", ":hang_17:", ":hang_18:", ":chad_04:", ":chad_05:", ":chad_06:" };
                    matrix[5] = new string[] { ":hang_16:", ":hang_17:", ":hang_18:", ":hang_13:", ":chad_08:", ":chad_09:" };
                    matrix[6] = new string[] { ":hang_21:", ":hang_22:", ":hang_23:", ":hang_24:", ":hang_13:", ":hang_13:" };
                    matrix[7] = new string[] { ":hang_26:", ":hang_27:", ":hang_28:", ":hang_28:", ":hang_13:", ":hang_13:" };
                    break;
                }

            case 7:
                {
                    matrix[0] = new string[] { ":hang_01:", ":hang_02:", ":hang_03:", ":hang_04:", ":hang_05:" };
                    matrix[1] = new string[] { ":hang_06:", ":hang_07:", ":hang_08:", ":hang_13:", ":hang_10:" };
                    matrix[2] = new string[] { ":hang_11:", ":hang_12:", ":hang_13:", ":hang_13:", myfriend };
                    matrix[3] = new string[] { ":hang_16:", ":hang_17:", ":hang_18:", ":chad_01:", ":chad_021:", ":chad_03:" };
                    matrix[4] = new string[] { ":hang_16:", ":hang_17:", ":hang_18:", ":chad_04:", ":chad_05:", ":chad_06:" };
                    matrix[5] = new string[] { ":hang_16:", ":hang_17:", ":hang_18:", ":hang_13:", ":chad_08:", ":chad_09:" };
                    matrix[6] = new string[] { ":hang_21:", ":hang_22:", ":hang_23:", ":hang_24:", ":tab_01:", ":tab_02:" };
                    matrix[7] = new string[] { ":hang_26:", ":hang_27:", ":hang_28:", ":hang_28:", ":tab_03:", ":tab_04:" };
                    break;
                }

            case 6:
                {
                    matrix[0] = new string[] { ":hang_01:", ":hang_02:", ":hang_03:", ":hang_04:", ":hang_05:", ":hang_13:" };
                    matrix[1] = new string[] { ":hang_06:", ":hang_07:", ":hang_08:", ":hang_13:", ":hang_10:", ":hang_13:" };
                    matrix[2] = new string[] { ":hang_11:", ":hang_12:", ":hang_13:", ":hang_14:", ":hang_15:", ":hang_13:" };
                    matrix[3] = new string[] { ":hang_16:", ":hang_17:", ":hang_18:", ":hang_19:", ":hang_20:", ":hang_13:" };

                    matrix[4] = new string[] { ":hang_16:", ":hang_17:", ":hang_18:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", myfriend };
                    matrix[5] = new string[] { ":hang_16:", ":hang_17:", ":hang_18:", ":hang_13:", ":hang_13:", ":hang_13:", ":chad_01:", ":chad_021:", ":chad_03:" };
                    matrix[6] = new string[] { ":hang_21:", ":hang_22:", ":hang_23:", ":hang_13:", ":tab_01:", ":tab_02:", ":chad_04:", ":chad_05:", ":chad_06:" };
                    matrix[7] = new string[] { ":hang_26:", ":hang_27:", ":hang_28:", ":hang_13:", ":tab_03:", ":tab_04:", ":hang_13:", ":chad_08:", ":chad_09:" };
                    break;
                }

            case 5:
                {
                    matrix[0] = new string[] { ":hang_01:", ":hang_02:", ":hang_03:", ":hang_04:", ":hang_05:", ":hang_13:" };
                    matrix[1] = new string[] { ":hang_06:", ":hang_07:", ":hang_08:", ":hang_13:", ":hang_13:", ":hang_13:" };
                    matrix[2] = new string[] { ":hang_11:", ":hang_12:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:" };
                    matrix[3] = new string[] { ":hang_16:", ":hang_17:", ":hang_18:", ":hang_13:", ":hang_13:", ":hang_13:" };

                    matrix[4] = new string[] { ":hang_16:", ":hang_17:", ":hang_18:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", myfriend };
                    matrix[5] = new string[] { ":hang_16:", ":hang_17:", ":hang_18:", ":hang_13:", ":hang_13:", ":hang_13:", ":chad_01:", ":chad_021:", ":chad_03:" };
                    matrix[6] = new string[] { ":hang_21:", ":hang_22:", ":hang_23:", ":hang_13:", ":tab_01:", ":tab_02:", ":chad_04:", ":chad_05:", ":chad_06:" };
                    matrix[7] = new string[] { ":hang_26:", ":hang_27:", ":hang_28:", ":hang_13:", ":tab_03:", ":tab_04:", ":hang_13:", ":chad_08:", ":chad_09:" };
                    break;
                }

            case 4:
                {
                    matrix[0] = new string[] { ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:" };
                    matrix[1] = new string[] { ":hang_06:", ":hang_07:", ":hang_08:", ":hang_13:", ":hang_13:", ":hang_13:" };
                    matrix[2] = new string[] { ":hang_11:", ":hang_12:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:" };
                    matrix[3] = new string[] { ":hang_16:", ":hang_17:", ":hang_18:", ":hang_13:", ":hang_13:", ":hang_13:" };

                    matrix[4] = new string[] { ":hang_16:", ":hang_17:", ":hang_18:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", myfriend };
                    matrix[5] = new string[] { ":hang_16:", ":hang_17:", ":hang_18:", ":hang_13:", ":hang_13:", ":hang_13:", ":chad_01:", ":chad_021:", ":chad_03:" };
                    matrix[6] = new string[] { ":hang_21:", ":hang_22:", ":hang_23:", ":hang_13:", ":tab_01:", ":tab_02:", ":chad_04:", ":chad_05:", ":chad_06:" };
                    matrix[7] = new string[] { ":hang_26:", ":hang_27:", ":hang_28:", ":hang_13:", ":tab_03:", ":tab_04:", ":hang_13:", ":chad_08:", ":chad_09:" };
                    break;
                }

            case 3:
                {
                    matrix[0] = new string[] { ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:" };
                    matrix[1] = new string[] { ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:" };
                    matrix[2] = new string[] { ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:" };
                    matrix[3] = new string[] { ":hang_16:", ":hang_17:", ":hang_18:", ":hang_13:", ":hang_13:", ":hang_13:" };

                    matrix[4] = new string[] { ":hang_16:", ":hang_17:", ":hang_18:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", myfriend };
                    matrix[5] = new string[] { ":hang_16:", ":hang_17:", ":hang_18:", ":hang_13:", ":hang_13:", ":hang_13:", ":chad_01:", ":chad_021:", ":chad_03:" };
                    matrix[6] = new string[] { ":hang_21:", ":hang_22:", ":hang_23:", ":hang_13:", ":tab_01:", ":tab_02:", ":chad_04:", ":chad_05:", ":chad_06:" };
                    matrix[7] = new string[] { ":hang_26:", ":hang_27:", ":hang_28:", ":hang_13:", ":tab_03:", ":tab_04:", ":hang_13:", ":chad_08:", ":chad_09:" };
                    break;
                }

            case 2:
                {
                    matrix[0] = new string[] { ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:" };
                    matrix[1] = new string[] { ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:" };
                    matrix[2] = new string[] { ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:" };
                    matrix[3] = new string[] { ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:" };

                    matrix[4] = new string[] { ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", myfriend };
                    matrix[5] = new string[] { ":hang_16:", ":hang_17:", ":hang_18:", ":hang_13:", ":hang_13:", ":hang_13:", ":chad_01:", ":chad_021:", ":chad_03:" };
                    matrix[6] = new string[] { ":hang_21:", ":hang_22:", ":hang_23:", ":hang_13:", ":tab_01:", ":tab_02:", ":chad_04:", ":chad_05:", ":chad_06:" };
                    matrix[7] = new string[] { ":hang_26:", ":hang_27:", ":hang_28:", ":hang_13:", ":tab_03:", ":tab_04:", ":hang_13:", ":chad_08:", ":chad_09:" };
                    break;
                }

            case 1:
                {
                    matrix[0] = new string[] { ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:" };
                    matrix[1] = new string[] { ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:" };
                    matrix[2] = new string[] { ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:" };
                    matrix[3] = new string[] { ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:" };

                    matrix[4] = new string[] { ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", myfriend };
                    matrix[5] = new string[] { ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":chad_01:", ":chad_021:", ":chad_03:" };
                    matrix[6] = new string[] { ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":tab_01:", ":tab_02:", ":chad_04:", ":chad_05:", ":chad_06:" };

                    matrix[7] = new string[] { ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":tab_03:", ":tab_04:", ":hang_13:", ":chad_08:", ":chad_09:" };
                    break;
                }

            case 0:
                {
                    matrix[0] = new string[] { ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:" };
                    matrix[1] = new string[] { ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:" };
                    matrix[2] = new string[] { ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:" };
                    matrix[3] = new string[] { ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:" };

                    matrix[4] = new string[] { ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", myfriend };

                    matrix[5] = new string[] { ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":chad_01:", ":chad_021:", ":chad_03:" };
                    matrix[6] = new string[] { ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":chad_04:", ":chad_05:", ":chad_06:" };

                    matrix[7] = new string[] { ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":hang_13:", ":chad_08:", ":chad_09:" };
                    break;
                }
        }


        for (var k = 0; k <= matrix.Length - 1; k++)
        {
            for (int j = 0; j <= matrix[k].Length - 1; j++)
            {
                if (HangManGame.emotes.ContainsKey(matrix[k][j]))
                    matrix[k][j] = HangManGame.emotes[matrix[k][j]].ToString();
            }
        }


        for (int k = 0; k <= matrix.Length - 1; k++)
        {
            for (var j = 0; j <= matrix[k].Length - 1; j++)
                c = c + matrix[k][j];
            c = c + Constants.vbCrLf;
        }
        return c;
    }

    class LetterClass
    {
        public char letter;
        public bool guessed = false;
        public LetterClass(char chr)
        {
            letter = chr;
            guessed = false;
        }
    }
}

static class StringExtensions
{
    public static string Replace(this string s, int index, int length, string replacement)
    {
        var builder = new StringBuilder();
        builder.Append(s.Substring(0, index));
        builder.Append(replacement);
        builder.Append(s.Substring(index + length));
        return builder.ToString();
    }
}
}

