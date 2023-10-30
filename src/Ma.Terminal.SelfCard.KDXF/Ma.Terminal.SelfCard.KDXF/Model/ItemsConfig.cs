using CommunityToolkit.Mvvm.DependencyInjection;
using Ma.Terminal.SelfCard.KDXF.WebApi;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ma.Terminal.SelfCard.KDXF.Model
{
    public class ItemsConfig
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();

        public int Cards { get; set; }
        public int Discards { get; set; }
        public int Status { get; set; }

        public static ItemsConfig Read()
        {
            return JsonSerializer.Deserialize<ItemsConfig>(File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}config.json"));
        }

        public void Save()
        {
            File.WriteAllText($"{AppDomain.CurrentDomain.BaseDirectory}config.json", JsonSerializer.Serialize(this));

            Task.Run(async () =>
            {
                try
                {
                    var machine = Ioc.Default.GetRequiredService<Machine>();
                    var api = Ioc.Default.GetRequiredService<Requester>();
                    await api.ReportDevice(new
                    {
                        Deviceid = machine.MachineNo,
                        Discards,
                        Cards,
                        Status,
                        Msg = Cards <= 0 ? "卡片不足" : string.Empty,
                    });
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    _logger.Error(ex.StackTrace);
                }

            });
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
