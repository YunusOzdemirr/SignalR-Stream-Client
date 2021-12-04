﻿using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CodeRethinked.SignalRStreaming.SignalR
{
    public class StreamHub : Hub
    {

        public static ConcurrentDictionary<string, string> _stock = new ConcurrentDictionary<string, string>();
        public static ConcurrentDictionary<string, string> _personelStock = new ConcurrentDictionary<string, string>();
        public ChannelReader<IEnumerable<string>> PriceLogStream(int delay)
        {
            var channel = Channel.CreateUnbounded<IEnumerable<string>>();
            _ = PriceLogAndViopWriter(channel.Writer, delay);

            return channel.Reader;
        }

        private async Task PriceLogAndViopWriter(ChannelWriter<IEnumerable<string>> writer, int delay)
        {
            string[] stdcodelist = { "USDTRY", "EURTRY", "EURUSD", "XU100", "XU030", "BRENT", "XGLD", "GLD" };
            //string dovizz = ";USDTRY;EURTRY;EURUSD;USDJPY;USDRUB;USDCNY;AUDUSD;GBPUSD;XAUUSD;GBPTRY";

            while (true)
            {
                Random random = new Random();
                foreach (var item in stdcodelist)
                {
                    Stock stock = new Stock()
                    {
                        price = random.Next(100, 200),
                        symbol = item
                    };
                    Task.Run(() =>
                    {
                        if (_stock.ContainsKey(stock.symbol))
                        {
                            var json = Newtonsoft.Json.JsonConvert.SerializeObject(stock);
                            _stock[stock.symbol] = json;
                        }
                        else
                        {
                            var json = Newtonsoft.Json.JsonConvert.SerializeObject(stock);
                            _stock.TryAdd(stock.symbol, json);
                        }
                        writer.WriteAsync(_stock.Values);
                    }).Wait();
                    await Task.Delay(delay);
                }
            }
        }

        public ChannelReader<IEnumerable<string>> LogStreamUserId(int delay)
        {
            var channel = Channel.CreateUnbounded<IEnumerable<string>>();
            _ = WriterByUserId(channel.Writer, delay);

            return channel.Reader;
        }
        private async Task WriterByUserId(ChannelWriter<IEnumerable<string>> writer, int delay)
        {
            string[] stdcodelist = { "USDTRY", "EURTRY", "EURUSD", "XU100", "XU030", "BRENT", "XGLD", "GLD" };
            //string dovizz = ";USDTRY;EURTRY;EURUSD;USDJPY;USDRUB;USDCNY;AUDUSD;GBPUSD;XAUUSD;GBPTRY";

            while (true)
            {
                foreach (var stock in stdcodelist)
                {
                    var abc = _stock.ContainsKey(stock);
                    if (abc)
                    {
                        Task.Run(() =>
                        {
                            if (_personelStock.ContainsKey(stock))
                            {
                                // var json = Newtonsoft.Json.JsonConvert.SerializeObject(_stock[stock]);
                                //_personelStock[stock] = json + "FavList";
                                _personelStock[stock] = _stock[stock] + "FavList";
                            }
                            else
                            {
                                // var json = Newtonsoft.Json.JsonConvert.SerializeObject(_stock[stock]);
                                //_personelStock.TryAdd(stock, json + "FavList");
                                _personelStock.TryAdd(stock, _stock[stock]+"FavList");
                            }
                            writer.WriteAsync(_personelStock.Values);
                        }).Wait();
                        await Task.Delay(delay);

                    }
                    //await writer.WriteAsync(item);
                }
            }
        }

    }
}
