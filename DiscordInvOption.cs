using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using Discord;


namespace resender
{
    [Serializable]
    public class DiscordInvOption : FilterOption
    {
        public override bool toSend(IReadOnlyCollection<Embed> embeds, ChannelManager channelManager)
        {
            IEnumerator<Embed> e =  embeds.GetEnumerator();
            e.MoveNext();
            try
            {
                if ((Convert.ToString(e.Current.Type).IndexOf("Gifv") < 0) && (e.Current.Url.IndexOf("discord.gg/") >= 0 || e.Current.Description.IndexOf("discord.gg/") >= 0))
                {
                    return true;
                }
            }
            catch
            {
                if ((Convert.ToString(e.Current.Type).IndexOf("Gifv") < 0) && (e.Current.Description.IndexOf("discord.gg/") >= 0))
                {
                    return true;
                }
            }
            return false;
        }

        public override bool toSend(string message, ChannelManager channelManager)
        {
            if (message.IndexOf("discord.gg/") >= 0 && message.IndexOf("gif") < 0)
            {
                return true;
            }
            return false;
        }

    }

    public class TwitterOption : FilterOption
    {


        public override bool toSend( IReadOnlyCollection<Embed> embeds, ChannelManager channelManager)
        {

            IEnumerator<Embed> e = embeds.GetEnumerator();
            e.MoveNext();
            try
            {
                if ((Convert.ToString(e.Current.Type).IndexOf("Gifv") < 0) && (e.Current.Description.IndexOf("https://twitter.com/") >= 0 || e.Current.Url.IndexOf("https://twitter.com/") >= 0))
                {
                    return true;
                }
            }   
            catch
            {
                if ((Convert.ToString(e.Current.Type).IndexOf("Gifv") < 0) && (e.Current.Description.IndexOf("https://twitter.com/") >= 0))
                {
                    return true;
                }
            }
            return false;
        }
        public override bool toSend(string message, ChannelManager channelManager)
        {
            if (message.IndexOf("https://twitter.com/") >= 0 && message.IndexOf("gif") < 0)
            {
                return true;
            }
            return false;
        }


    }

    public class MessagesOption : FilterOption
    {
        public override bool toSend(IReadOnlyCollection<Embed> embeds, ChannelManager channelManager)
        {
            IEnumerator<Embed> e = embeds.GetEnumerator();
            e.MoveNext();
            foreach(var item in filters)
            {
                string description = e.Current.Description.ToLower();
                try
                {
                    if ((Convert.ToString(e.Current.Type).IndexOf("Gifv") < 0) && (e.Current.Url.IndexOf(item) >= 0 || description.IndexOf(item) >= 0))
                    {
                        return true;
                    }
                } 
                catch
                {
                    if ((Convert.ToString(e.Current.Type).IndexOf("Gifv") < 0) && (e.Current.Description.IndexOf(item) >= 0))
                    {
                        return true;
                    }
                }
            }
            return false;   
        }

        public override bool toSend(string message, ChannelManager channelManager)
        {
            message = message.ToLower();
            foreach(var item in filters)
            {
                if (message.IndexOf(item) >= 0 && message.IndexOf("gif") < 0)
                {
                    return true;
                }
            }
            return false;
        }

    }

    public class LinksOption : FilterOption
    {

        public override bool toSend(IReadOnlyCollection<Embed> embeds, ChannelManager channelManager)
        {

           

            IEnumerator<Embed> e = embeds.GetEnumerator();
            e.MoveNext();
            try
            {
                bool x = channelManager.twitter_links.toSend(embeds, channelManager);
                if (x)
                    return false;

                x = channelManager.discord_invites.toSend(embeds, channelManager);
                if (x)
                    return false;

                if ((Convert.ToString(e.Current.Type).IndexOf("Gifv") < 0) && (e.Current.Url.IndexOf("http") >= 0 || e.Current.Description.IndexOf("http") >= 0))
                {
                    return true;
                }
            }
            catch
            {
                if ((Convert.ToString(e.Current.Type).IndexOf("Gifv") < 0) && (e.Current.Description.IndexOf("http") >= 0))
                {
                    return true;
                }
            }
            
            return false;    
        }

        public override bool toSend(string message, ChannelManager channelManager)
        {
            bool x = channelManager.twitter_links.toSend(message, channelManager);
            if (x)
                return false;

            x = channelManager.discord_invites.toSend(message, channelManager);
            if (x)
                return false;

            if (message.IndexOf("http") >= 0 && message.IndexOf("gif") < 0)
            {
                return true;
            }
            return false;
        }
    }

    public class EmbedOption : FilterOption
    {
        public override bool toSend(IReadOnlyCollection<Embed> embeds, ChannelManager channelManager)
        {
            if (embeds.Count > 0)
                return true;
            else
                return false;
        }

        public override bool toSend(string message, ChannelManager channelManager)
        {
            return false;
        }
    }

    public class AllOption : FilterOption
    {
        public override bool toSend(IReadOnlyCollection<Embed> embeds, ChannelManager channelManager)
        {
            return true;
        }

        public override bool toSend(string message, ChannelManager channelManager)
        {
            return true;
        }
    }

}
