using Serilog.Core;
using Serilog.Events;

public class CorrectClientIpLogger : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (logEvent.Properties.TryGetValue("ClientIp", out var clientIp))
        {
            var ipValue = clientIp.ToString();
            if (ipValue.StartsWith("\"::ffff:"))
            {
                ipValue = ipValue.Substring(8, ipValue.Length - 9);
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("ClientIp", ipValue));
            }
        }
    }
}
