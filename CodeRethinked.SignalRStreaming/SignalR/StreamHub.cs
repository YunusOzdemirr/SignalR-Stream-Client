using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CodeRethinked.SignalRStreaming.SignalR
{
    public class StreamHub : Hub
    {
        public ChannelReader<int> DelayCounter(int delay)
        {
            var channel = Channel.CreateUnbounded<int>();

            _ = WriteItems(channel.Writer,  delay);

            return channel.Reader;
        }

        private async Task WriteItems(ChannelWriter<int> writer, int delay)
        {
            for (; ; )
            {
                //For every 5 items streamed, add twice the delay
              

                await writer.WriteAsync(100);
                await Task.Delay(delay);
            }

            writer.TryComplete();
        }
    }
}
