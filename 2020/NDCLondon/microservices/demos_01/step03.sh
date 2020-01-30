#! /bin/bash

echo "> kubectl apply -f demo_app2.yaml"
kubectl apply -f demo_app2.yaml

echo "> kubectl get deployments -w"
kubectl get deployments -w