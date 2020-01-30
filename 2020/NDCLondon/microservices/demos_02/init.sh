#! /bin/bash

echo "setting current namespace to 'blazing-pizza'"
echo "$ kubectl config set-context --current --namespace blazing-pizza"
kubectl config set-context --current --namespace blazing-pizza
