using Serilog.Core;
using Serilog.Events;

public class EmojiEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        string emoji = logEvent.Level switch
        {
            LogEventLevel.Debug => "🔧",
            LogEventLevel.Error => "❌",
            LogEventLevel.Warning => "⚠️",
            LogEventLevel.Information => "ℹ️",
            LogEventLevel.Verbose => "🔍",
            _ => string.Empty
        };

        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Emoji", emoji));
    }
}
