﻿using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;

namespace Tricherie
{
    public class Bot
    {
       public static TwitchClient client;
        readonly string changeStatsId = "custom-reward-id=f88c3bed-a091-4b9b-abd3-a27d7a684070";
        readonly string damageMeId = "custom-reward-id=43d90440-3213-4ec7-9dd1-158e3a679080";
        readonly string healMeId = "custom-reward-id=6651b43a-47e4-4c3c-b899-cd0c0a2f145c";

        WriteMemory memoryWriter;
        string[] allStats = new string[] { "vigueur", "esprit", "endurance", "force", "dexterite", "intelligence", "foi", "esoterisme" };
        string stats;
        string message;
        string messageChoix;
        string messageValeur;
        bool erreurBool = false;
       

        public Bot()
        {
            ConnectionCredentials credentials = new ConnectionCredentials(Properties.Resources.username, Properties.Resources.token);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            client = new TwitchClient(customClient);
            client.Initialize(credentials, "siul008");

            client.OnLog += Client_OnLog;
            client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnConnected += Client_OnConnected;
            client.OnMessageReceived += Client_OnMessageReceived;

            client.Connect();

            memoryWriter = new WriteMemory();
        }

            private void Client_OnLog(object sender, TwitchLib.Client.Events.OnLogArgs e)
        {
            Console.WriteLine($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
            
            
            if(e.Data.Contains(changeStatsId))
            {
                var myString = e.Data.ToString();
                var splitString = myString.Split(':');
                message = splitString[3].ToLower();
                Debug.WriteLine(message);
                ChangeStats(message);
            }
            else if(e.Data.Contains(damageMeId))
            {
                var myString = e.Data.ToString();
                var splitString = myString.Split(':');
                message = splitString[3].ToLower();
                DamageMe(message);
            }
            else if (e.Data.Contains(healMeId))
            {
                var myString = e.Data.ToString();
                var splitString = myString.Split(':');
                message = splitString[3].ToLower();
                HealMe(message);
            }

        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Connected to {e.AutoJoinChannel}");
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            Console.WriteLine("Voici lé croupié");
            client.SendMessage(e.Channel, "Je suis un bot siul00Hi");
        }
        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message.StartsWith("!tomate") && e.ChatMessage.IsSubscriber)
            {
                client.SendMessage("siul008", "vous etes stupides");
            }
        }

        private void ChangeStats(string messageInput)
        {
            var messageSplit = message.Split('=');
            messageChoix = messageSplit[0];
            messageValeur = messageSplit[1];
            if (!Regex.IsMatch(messageValeur, @"[a-zA-Z]"))
            {
                if (messageChoix.Contains("vig"))

                { stats = allStats[0]; }

                else if (messageChoix.Contains("esp") || (messageChoix.Contains("mind")))

                { stats = allStats[2]; }

                else if (messageChoix.Contains("end"))

                { stats = allStats[1]; }

                else if (messageChoix.Contains("force") || (messageChoix.Contains("str")))

                { stats = allStats[3]; }

                else if (messageChoix.Contains("dex"))

                { stats = allStats[4]; }

                else if (messageChoix.Contains("int"))

                { stats = allStats[5]; }

                else if (messageChoix.Contains("foi") || (messageChoix.Contains("faith")))

                { stats = allStats[6]; }

                else if (messageChoix.Contains("eso") || (messageChoix.Contains("arcane")))

                { stats = allStats[7]; }

                else
                {
                    client.SendMessage("siul008", $"Erreur de Syntaxe à : ( {messageChoix} ), le message doit ressembler à : vigueur:1 ou foi:-1");
                    erreurBool = true;
                }

                if(erreurBool == false)
                {
                    memoryWriter.WriteOnMemoryStats(messageValeur, stats);
                }
            }
            else
            {
                client.SendMessage("siul008", $"Erreur de Syntaxe à : ( {messageValeur} ), le message doit ressembler à : vigueur:20");
            }
        }
        private void DamageMe(string damage)
        {
            if (!Regex.IsMatch(damage, @"[a-zA-Z]"))
            {
                memoryWriter.DamageAtmHp(damage);
                client.SendMessage("siul008", $"Je rêve ou tu viens de me mettre {damage} dégats ?");
            }
            else
            {
                client.SendMessage("siul008", $"Erreur de Syntaxe à : ( {damage} ), le message doit ressembler à : 157");
            }
        }
        private void HealMe(string heal)
        {
            if (!Regex.IsMatch(heal, @"[a-zA-Z]"))
            {
                memoryWriter.HealAtmHP(heal);
                client.SendMessage("siul008", $"Merci pour les {heal} hp ! siul00Love ?");
            }
            else
            {
                client.SendMessage("siul008", $"Erreur de Syntaxe à : ( {heal} ), le message doit ressembler à : 157");
            }
            
        }
        public static void CreateMessage(string message)
        {
            client.SendMessage("siul008", message);
        }

    }
}
