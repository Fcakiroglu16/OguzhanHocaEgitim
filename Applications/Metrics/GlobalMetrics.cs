using System.Diagnostics.Metrics;

namespace Applications.Metrics;

public class GlobalMetrics
{
    private readonly Counter<long> _requestCounter;

    public GlobalMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create("API1");

        _requestCounter = meter.CreateCounter<long>(
            name: "products.getall.request.count",
            description: "GetAll Products endpoint'ine yapılan istek sayısı");
    }

    public void RecordRequest()
    {
        _requestCounter.Add(1);
    }
}
