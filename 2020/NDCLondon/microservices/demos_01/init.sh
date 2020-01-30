#! /bin/bash

echo "setting current namespace to 'default'"
echo "$ kubectl config set-context --current --namespace default"
kubectl config set-context --current --namespace default