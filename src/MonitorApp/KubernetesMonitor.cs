using k8s;
using k8s.Models;

namespace MonitorApp;

public class KubernetesMonitor(ILogger<KubernetesMonitor> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await MonitorAsync(stoppingToken);
            }
            catch (TaskCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // let it shut down gracefully
            }
        }
    }

    private async Task MonitorAsync(CancellationToken ct)
    {
        var config = KubernetesClientConfiguration.InClusterConfig();
        var client = new Kubernetes(config);
        
        // should use ListNamespacedServiceWithHttpMessagesAsync if wanted to get things from specific namespaces
        // but right now watching the whole cluster 
        var response = client.CoreV1.ListServiceForAllNamespacesWithHttpMessagesAsync(
            watch: true,
            cancellationToken: ct);

        await foreach (var (type, service) in response.WatchAsync<V1Service, V1ServiceList>(cancellationToken: ct))
        {
            logger.LogInformation("""
                                  {Type}: service "{Service}" (namespace "{Namespace}"), with annotation "{Annotation}"
                                  """,
                type,
                service.Metadata.Name,
                service.Metadata.Namespace(),
                GetAnnotation(service));
        }

        static string GetAnnotation(V1Service service)
        {
            if (service.Metadata?.Annotations is null)
            {
                return "N/A";
            }

            service.Metadata.Annotations.TryGetValue("some-sample-annotation", out var maybeAnnotation);
            return !string.IsNullOrWhiteSpace(maybeAnnotation) ? maybeAnnotation : "N/A";
        }
    }
}