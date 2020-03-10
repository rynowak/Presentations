#! /bin/bash
docker build . -t rynowak/standup-backend
docker push rynowak/standup-backend