using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using Discord.Webhook;
using resender;

namespace _01_basic_ping_bot
{
    class Program
    {
        private static ConfigToken configToken;
        //private ulong guild = 794642161340776448;
        //private ulong guild = 660823622109233152;
        private static ulong guild;

        private readonly DiscordSocketClient _client;
        private static Dictionary<ulong, string> webhooks;
        private static ChannelManager channelManager;
        private static List<string> webhook_ids = new List<string>();
    
        // Discord.Net heavily utilizes TAP for async, so we create
        // an asynchronous context from the beginning.
        static void Main(string[] args)
        {
            Console.Title = "Selfbot by Kx#3580";

            //chans = JsonConvert.DeserializeObject<List<Channel>>(File.ReadAllText("channels.json"));


            channelManager = JsonConvert.DeserializeObject<ChannelManager>(File.ReadAllText("channels.json"));
            channelManager.AddAllOptions();
            guild = channelManager.guild;
            for(int i = 0; i < channelManager.user_messages.filters.Length; i++)
            {
                channelManager.user_messages.filters[i] = channelManager.user_messages.filters[i].ToLower();
            }


            //return;
            webhooks = new Dictionary<ulong, string>();

            configToken = JsonConvert.DeserializeObject<ConfigToken>(File.ReadAllText("ConfigToken.json"));

            while (string.IsNullOrEmpty(configToken.Token))
            {
                Console.Write("Your Discord token: ");
                configToken.Token = Console.ReadLine();
            }

            File.WriteAllText("ConfigToken.Json", JsonConvert.SerializeObject(configToken));

            //configToken.Token = "mfa.qf8cJjBOLJ7TdSaVSZNO3fqw-BuIBDLvU0vGd7c5rejvfrDNuLL7DvIxMfhYg3bkHyai2Yg7rVKZPIVgXvSF";


            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public Program()
        {
            // It is recommended to Dispose of a client when you are finished
            // using it, at the end of your app's lifetime.
            _client = new DiscordSocketClient();

            _client.Log += LogAsync;
            _client.Ready += ReadyAsync;
            _client.MessageReceived += MessageReceivedAsync;
        }

        public async Task MainAsync()
        {
            // Tokens should be considered secret data, and never hard-coded.
            await _client.LoginAsync(TokenType.User, configToken.Token);
            await _client.StartAsync();

            // Block the program until it is closed.
            await Task.Delay(Timeout.Infinite);
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        // The Ready event indicates that the client has opened a
        // connection and it is now safe to access the cache.
        private Task ReadyAsync()
        {
            Console.WriteLine($"{_client.CurrentUser} is connected!");
            Console.Title = $"Now using {_client.CurrentUser} | Selfbot by Kx#3580";


            // ---------------- get webhooks ----------------
            SocketGuild guildObj = _client.GetGuild(guild);
            if (guildObj == null)
            {
                Console.WriteLine("guild obj is null");
            }

            var x = Task.Run(async () => await guildObj.GetWebhooksAsync());
            IReadOnlyCollection<Discord.Rest.RestWebhook> hooks = x.GetAwaiter().GetResult();
            for (int i = 0; i < hooks.Count; i++)
            {
                var hook = hooks.ElementAt(i);
                webhooks[hook.ChannelId] = "https://discord.com/api/webhooks/" + hook.Id + "/" + hook.Token;
            }

            foreach(var item in webhooks)
            {
                webhook_ids.Add(item.Value);
            }

            // ----------------------------------------------

            return Task.CompletedTask;
        }

        // This is not the recommended way to write a bot - consider
        // reading over the Commands Framework sample.
        private async Task MessageReceivedAsync(SocketMessage message)
        {
            //The bot should never respond to itself.
            if (message.Author.Id == _client.CurrentUser.Id)
            return;


            //// t
            //if (message.Channel.Id == 794636029004218418)
            //{
            //    // get from 779369313235632209
            //    var msg = await _client.GetGuild(519841696171819009).GetTextChannel(779369313235632209).GetMessagesAsync(5).ToListAsync();
            //    //var msg = await _client.GetGuild(691029953860599880).GetTextChannel(791647954724323348).GetMessagesAsync(5).ToListAsync();
            //    var msgs = msg.ElementAt(0);
            //    //Console.WriteLine(msgs.ElementAt(0).);
            //    var embed = msgs.ElementAt(0).Embeds.ToList().ElementAt(0).ToEmbedBuilder().Build();
            //    SendHookTest(embed, "https://discord.com/api/webhooks/794636794343325707/VL7s7F97_eUM46k4kneUC-vo90rO2rAWcf-q-djdV7Rn8FTgQnApl37FXaFSiHjDvKJ7");
            //}

            var chan = new List<FilterOption>();

            chan = channelManager.ChanExists(message.Channel.Id);

            if (chan != null)
            {
                foreach(var item in chan)
                {
                    foreach(var channs in item.channels)
                    {
                        if (message.Channel.Id == channs.to)
                            return;
                    }
                    
                    bool x;
                    try
                    {
                        x = item.toSend(message.Embeds, channelManager);
                    }
                    catch
                    {
                        x = item.toSend(message.Content, channelManager);
                    }
                    if (x == true)
                    {
                        foreach(var id in item.channels)
                        {
                            if(id.from == message.Channel.Id)
                            {
                                await SendHook(message, id.to);
                            }
                        }
                    }
                }
            }


            //Console.WriteLine("[{0}] Message received", DateTime.Now);


            //if (chans.Exists(x => x.from == message.Channel.Id))
            //{
            //    var matches = chans.Where(p => p.from == message.Channel.Id);
            //    foreach (var item in matches)
            //    {
            //        try
            //        {
            //           Console.WriteLine("[{0}] resending", message.Channel.Id);
            //            await SendHook(message, item.to);
            //        }
            //        catch
            //        {
            //            Console.WriteLine("[{0}] error while resending", message.Channel.Id);
            //        }
            //    }
            //}
        }

        private void SendHook(SocketMessage message, object to) => throw new NotImplementedException();

        private async Task SendHookTest(Embed org_msg, string webhook)
        {

            EmbedFooter footer = new EmbedFooter();
            footer.IconUrl = "https://cdn.discordapp.com/icons/794642161340776448/ff61ddcbeab45bcdeecfd30b826833e9";
            footer.Text = "Firewall | [" + DateTime.Now.ToString("HH:mm:ss") + "]";
            org_msg.Footer = footer;


            using (var client = new DiscordWebhookClient(webhook))
            {
                await client.SendMessageAsync(embeds: new[] { org_msg });
            }
        }

        private async Task SendHook(SocketMessage message, ulong webhookId)
        {
            if (message.Embeds.Count == 0)
            {

                List<Embed> embedai = new List<Embed>();
                var chnl = message.Channel as SocketGuildChannel;
                var guild = chnl.Guild.Id;
                string link = "[Message link](https://discord.com/channels/" + guild + "/" + message.Channel.Id + "/" + message.Id+ ")";

                string webhook = webhooks[webhookId];

                string content = message.Content + '\n' + link + '\n';

                if(message.Attachments.Count() > 0)
                {
                    foreach(var item in message.Attachments)
                    {
                        var embed = new EmbedBuilder();
                        embed.WithImageUrl(item.Url);
                        embedai.Add(embed.Build());
                    }    
                }


                using (var client = new DiscordWebhookClient(webhook))
                {
                    //await client.SendMessageAsync(embeds: new[] { org_msg });
                    await client.SendMessageAsync(content, username: message.Author.Username, embeds: embedai);
                }
            }

            else
            {
                Embed org_msg = message.Embeds.ElementAt(0);

                EmbedFooter footer = new EmbedFooter();
                footer.IconUrl = "https://cdn.discordapp.com/icons/794642161340776448/ff61ddcbeab45bcdeecfd30b826833e9";
                footer.Text = "Firewall | [" + DateTime.Now.ToString("HH:mm:ss") + "]";
                org_msg.Footer = footer;

                Color color = new Color(0x9e76ff);
                org_msg.Color = color;

                string webhook = webhooks[webhookId];

                using (var client = new DiscordWebhookClient(webhook))
                {
                    await client.SendMessageAsync(embeds: new[] { org_msg }, username: message.Author.Username);
                    //await client.SendMessageAsync();
                }
            }

        }
    }
}
