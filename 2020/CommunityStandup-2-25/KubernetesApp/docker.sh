#! /bin/bash
docker build . -t rynowak/kubernetesapp
docker push rynowak/kubernetesapp