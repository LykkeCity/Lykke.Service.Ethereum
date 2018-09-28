using Microsoft.ApplicationInsights;

namespace Lykke.Service.Ethereum.Core.Telemetry
{
    public static class TelemetryClientExtensions
    {
        public static IEventTracker StartEvent(
            this TelemetryClient telemetryClient,
            string eventName)
        {
            return new EventTracker(eventName, telemetryClient);
        }
    }
}
