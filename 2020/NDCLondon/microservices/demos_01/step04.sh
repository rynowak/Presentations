#! /bin/bash
echo "> kubectl get svc demo-app -o jsonpath=\"{ .status.loadBalancer.ingress[].ip }\""
ip=$(kubectl get svc demo-app -o jsonpath="{ .status.loadBalancer.ingress[].ip }")

while true
do
	curl -i http://$ip/
    sleep 1
done