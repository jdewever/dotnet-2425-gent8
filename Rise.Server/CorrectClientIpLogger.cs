using Serilog.Core;
using Serilog.Events;

public class CorrectClientIpLogger : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (logEvent.Properties.TryGetValue("ClientIp", out var clientIp))
        {
            var ipValue = clientIp.ToString();
            if (ipValue == "\"::1\"")
            {
                ipValue = "localhost";
            }
            if (ipValue.StartsWith("\"::ffff:"))
            {
                ipValue = ipValue.Substring(7);
            }
            ipValue = ipValue.Trim('"');
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("ClientIp", ipValue));
        }
    }
}
