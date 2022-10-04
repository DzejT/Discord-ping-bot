using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using Discord;



namespace resender
{
    public abstract class FilterOption
    {
        public string[] filters { get; set; }
        public List<Channel> channels { get; set; }
        public abstract bool toSend(IReadOnlyCollection<Embed> embeds, ChannelManager channelManager);

        public abstract bool toSend(string message, ChannelManager channelManager);

    }
}
