## Section 1

Show frontend and backend projects that are plain. 

How do I get them to talk to each other?

- Starting both projects (meh in VS, kinda complicated in VSCode)
- Assigning each project a port (handled by IIS, but if I'm not using IIS in prod, do I want to use it in dev?)
- Getting frontend to talk to the backend

- show how tye can launch two services at once and assign each different ports
- show how logs are captured in the dashboard
- visit frontend and see logs in the dashboard (failed requests)

-- optional show debugging to catch startup errors

- show use of env-vars for service discovery - if I were doing this without tye, I would probably design a scheme for configuration...
- show use of configuration based on env-var provider
- show tye config package that adds extension method
- now this is convenient and idiomatic

- one of the great things about the Tye approach is that it makes development like production. We try to make it so that if it works on your machine it works everywhere. This works because we're building a .NET oriented toolset.

- show `tye run --docker`
- show `tye deploy -i`

We can do all of these things because we're building a .NET focused tool. The common case is that you don't need to put anything notable in your Dockerfile or kubernetes manifest.

## Section 2

So we've taken a hello-world application pretty far, but what about a more realistic app. You're going to have data stores - things to deploy that don't run your application code.

- show the application - three services and their purpose
- run the application - one of the services is crashing over and over because we need redis
- add redis and postgres and explain how connection strings work
- run this and see how we can see logs even for containers

Now we we're going to deploy this.

- show interactive deployment with secrets
  
But let's do something else first. Remember before when we had to a port-forward to be able to access the application? It sure would be nice if we had a way to make the application accept public traffic. Well tye has a feature for that.

- test ingress locally
- show interactive deployment with ingress

## Rude Q & A

Q. What does it mean that Tye's an experiment?
A. First, it means that we're building something in the open with the collaboration of the community. The roadmap and scope for the project aren't as fixed as most of the other things the .NET team does. What we learn from Tye will be inputs to planning for .NET 6 and future releases.

Practically speaking, what this means is that we're not sure what happens to the actual CLI we've built. The features that you see here might get blown up into a combination of investments into .NET, VS Code and VS. We like the developer experience of tye right now, but if you think about it I'm sure you'll see ways it could be improved or made approachable to more people.

Q. Who is this for?
A. Tye is for developers. It's about developer productivity and a workflow that make sense for developers. It's also a menu that you can choose from. Hopefully you find the local development toolset of tye essential for your day-to-day work.

I think there's spectrum that our teams and workplaces have with how mature your ops process is. You might not find the deployment functionality of tye useful with the backgound of what your team is already doing successfully. I think this is the thing we have the most to learn about.

Q. It only does k8s - can it do other things?
A. It *could*. We really like the idea of using tye to model and run applications and using another tool to do deployment. Take a look at OAM if you're into this sort of thing.

We really like the idea of using the same format for development and deployment. Problems creep in when you have to maintain things that aren't part of your dev-test workflow. Tye eliminates that. 

Q. There's lots of other developer tools out there - why this?
A. I think it's novel to try and make a tool that solves problems for you immediately from the moment you decide to write code to the point where you've got fully-automated deployments.

Most other developer tools give you building blocks - they aren't high level. Once you learn k8s you can just tools for k8s. We wanted to try and take .NET developers to new and strange places without learning or caring about the underlying tech.


