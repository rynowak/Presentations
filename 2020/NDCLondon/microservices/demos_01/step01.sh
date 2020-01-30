#! /bin/bash

echo "> kubectl apply -f demo_app1.yaml"
kubectl apply -f demo_app1.yaml

echo "> kubectl get pods -w"
kubectl get pods -w