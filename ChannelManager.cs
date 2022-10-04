using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace resender
{
    [Serializable]
    public class ChannelManager
    {
        public ulong guild { get; set; }
        public DiscordInvOption discord_invites { get; set; }
        public TwitterOption twitter_links { get; set; }
        public MessagesOption user_messages { get; set; }
        public LinksOption all_links { get; set; }
        public EmbedOption embeded_messages { get; set; }
        public AllOption all_messages { get; set; }
        public List<FilterOption> allOption { get; set; }

        public ChannelManager()
        {
            
        }

        public void AddAllOptions()
        {
            allOption = new List<FilterOption>();
            allOption.Add(discord_invites);
            allOption.Add(twitter_links);
            allOption.Add(user_messages);
            allOption.Add(all_links);
            allOption.Add(all_messages);
            allOption.Add(embeded_messages);
        }

        public List<FilterOption> ChanExists(ulong channelID)
        {
            var items = new List<FilterOption>();
            foreach (var item in allOption)
            {
                foreach (var channel in item.channels)
                {
                    if (channel.from == channelID)
                    {
                        items.Add(item);
                    }                    
                }
            }
            return items;
        }
    }
}
