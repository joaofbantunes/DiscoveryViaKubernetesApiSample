# Discovery Via Kubernetes API Sample

Quickly checking out the usage of the Kubernetes API to discover... things! 😅

The idea, is to make use of the Kubernetes API, to allow resources to declare something about themselves, then have some infrastructure application discover those resources and act on them.

In this sample, we have a monitoring application which watches the cluster for service resources, checking if they have a certain annotation. This is interesting because it allows us to instead of having some central configuration, have applications declare things in their k8s manifests, then having the infra application gather that information.

## Trying out the sample

Start by creating the container image. There's a Docker compose file in the root just to make that easier, by running:

```bash
docker compose build
```

Then we can run the monitoring application:

```bash
kubectl apply -f ./k8s/monitor.yml
```

Start looking at the logs of the monitor app, as it will log messages to the console.

If there are already any services in the cluster, the monitor app should discover them immediately.

To test it out, there are a couple of sample Nginx services in the `k8s` folder. Applying like this:

```bash
kubectl apply -f ./k8s/sample-nginx-1.yml
```

Should make the monitor app log a message about the service.

If we change something in the service, the monitor app will pick it up. To test things out, we can change the annotation (named `some-sample-annotation`) and execute apply again.

If we delete the sample app, the monitor app should log a message about it as well.

```bash
kubectl delete -f ./k8s/sample-nginx-1.yml
```
