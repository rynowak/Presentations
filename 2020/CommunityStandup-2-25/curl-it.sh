#! /bin/bash
echo "> kubectl get svc kubernetesapp -o jsonpath=\"{ .status.loadBalancer.ingress[].ip }\""
ip=$(kubectl get svc kubernetesapp -o jsonpath="{ .status.loadBalancer.ingress[].ip }")

while true
do
	curl -i http://$ip/
    sleep 1
    echo ""
    echo ""
    echo ""
done