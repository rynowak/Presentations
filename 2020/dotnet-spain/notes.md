# Understanding your superpowers

Hello, this is Discovering your Superpowers - understanding cloud application models

**next**

I'd like to thank my good friends at plain concepts for bringing me back this year, as well as our other sponsors.

**next**

## Personal Intro

I'm Ryan Nowak - I've spent most of my career at Microsoft working on programming technologies like ASP.NET Core, MVC, and Blazor.

I rencently made a move to Azure Incubations where I work on strategic innovation for Azure as a platform.

**next**

## Agenda

First of all - what is an Application Model? That's probably a new term for a lot of people.

Secondly have can you think systematically about cloud runtimes and deployment environments.

And hopefully by the end you can understand runtime environments to easily and more expertly design applications.

## Topic Intro

Before we dig in... why does this matter? Well we need to learn new technologies all of the time. The cloud technologies we use to host applications are no different, they go through periods of change an innovation.

**next**

My journey in 2020 took me a different part of Microsoft as company. For some reason I decided that while quarantined was the best time to interview and take on new challenges.

My expertise in this area comes from a few years of working to support Microservices developers using ASP.NET Core - and learning from them all of the things that they find difficult.

Developers are smart people, and there's a good chance that you feel like you understand your code really well. But how well do you understand the systems that run your code? 

I found that when talking to developers almost everything except your code is hard to get right. 

My new role is about raising the level of abstraction for cloud applications to become more developer friendly.

**next**

So let's introduce the application model as a framework

**next**

This is my personal definition, chosen because it's useful.

An Application Model describes the interface between software components and a runtime environment..

You can alternatively think of an application model as ... whatever configuration data is understood by the thing you're deploying to.

**next**

Let's unpack a little bit with some examples:

Software components are whatever you provide - the software you write.

Interface is the contract used to perform some task, for example you could use networking as the interface to communicate between services. You could use a file to store configuration data.

And the runtime environment is the stack that you're deploying to.

**next**

When I talk to developers about these topics, they find deployment most frustrating because it's where you go from "it works on my machine" to "not working on someone else's machine"

My theory about this is that we habitually get something working as quickly as possible, and then think about deployment as the last stop only after we're happy.

We're let down when we go to deploy and then the concepts we use when we think about the application don't match what's offered by the runtime.

**next**

To me it's a classic impedance mismatch problem. We think about what we want to do in one way, but the system requires a different kind of thinking.

**next**

In English, we have a term Yak Shaving - that refers to solving a series of small problems to accomplish a big task. Without a framework to think about putting an application in production - you cannot predice the set of small problems you have to solve.

## Applying Systematic Thinking

To solve this let's be systematic.

We'll do three steps:
- think about the needs of the application
- understand the options that the deployment environment provides
- and then map those needs to the best options

Since we're talking about cloud and microservices, I'm going to use the term Application to refer to a whole software system, and Service to refer to each unit that makes it up.

Let's start very basic - Deployment: I need the application to actually start - all of the files and depenendencies needs to be in place. 

Secondly, I need to be able to communicate to do something useful, if this is a website so I need to be able to accept web traffic. I also might need to talk to other services or databases.

You can think about connections in and connections out a little differently.

- For a connection in you need to *have* an address - other people need to be able to find you. This is most commonly going to be a port for HTTP.
- For an outgoing connection you need to know the address - how do you reach the other service. You might also need some credentials 

next, what about configuration?

We will want a way to make some of our application behaviors configurable. It also might be a good way to communicate things like what addresses we're using to talk to other services.

and lastly, diagnostics. We need some kind of logging - that way we can diagnose problems with a running app, or being able to collect data about what kinds of failures occur.

**next**

The app we're going to use first looks like this. Let's quickly run through what it needs.

**next**

These will be simple ASP.NET Core 3.1 applications so we don't really need anything special for deployment - just the files produced by a publish.

We'll need the ability to serve web traffic from both of them. You can think of the frontend as serving public traffic, and the backend as only serving internal traffic.

With our framework we just discussed, it means that each service needs to be assigned an address so its reachable. The frontend service needs to know the backend service's address so they can communicate.

We'll make all of these settings configurable for flexibility. 

And for diagnostics we're using logging, so we need that to go somewhere. 

## Demo: The Process

So let's map these concerns to our code and tools - and then to understand how these 4 concepts work:

- Deployment
- Communication
- Configuration 
- Diagnostics

For deployment - we have to look no further that `dotnet publish`. It's possible no explained the difference between `dotnet run` and `dotnet publish` before clearly. `dotnet run` runs your application *in-place* - using your content files from where they already are on disk and using .dlls from the where they are downloaded by nuget.

**do a publish**

When `dotnet run` builds your application code - you can find the binaries in the `bin/` folder. But if you copy this folder somewhere else it won't work. `dotnet publish` on the other hand creates a runnable output of your application that can be used portably.

But we're not done talking about deployment yet. `dotnet publish` assumes that a compatible version of .NET Core is installed where you're going run. If you want to you can use the `--runtime` argument to include the runtime in the output. We call this a self-contained publish.

Next - let's talk about incoming HTTP - we need the application to listen. That code is provided as part of ASP.NET Core - this code interfaces with the operating system to handle HTTP requests by opening a port.

I'll start by running the backend application

**dotnet run**

Which port? This is something that ASP.NET Core provides some opinions about. By default ASP.NET Core will listen on port 5000 and 5001 - this is configured in launchSettings.json.

**show launchsettings**

Since we have two services to run here, I'm going to update the frontend to listen on different ports.

**edit frontend launchsettings**

You can also configure this using the command line

**dotnet run with --urls**

or you can configure this with environment variables

**URLS=... dotnet run**

or you can configure this in code

**show configure web host for backend**

I don't really recommend using code to configure this except in special cases where you really know what you're doing.

--

Our concern when running web applications as processes is to avoid a port conflict because a port can only be used by a single process at a time.

So if we go with 5000 for the backend, we need to make sure that the frontend application chooses a different point, and we need to feed this address with the 5000 in somehow.

Again, ASP.NET Core provides some default opinions about configuration. The configuration system can gather and compose configuration data from across different formats. You get some configuration files on disk as part of the template, and the command line and environment variable providers for config by default. I showed already a few different ways to configure the listening port - that was going through the configuration system.

**show reading environment variable in frontend startup**

**show configure configuration in program.cs**

There are cases that are good for all of these options. Command Line and environment variables are usually discussed together because they have similar tradeoffs. environment variables are a little bit easier to compose typically.

configuration files are better when you have highly structued data (like json), that you want to source control next to the application. It's a good choice for data that you don't want to hardcode in application code, or that doesn't neatly translate to key-value-pairs. Additional configuration files can change. For instance the logging system in asp.net core will watch for configuration changes and update the log verbosity.

Lets use a file to specify the address of the backend and try running the frontend.

**edit configuration file and dotnet run**

We can see that this is working, not only did we get 

environment variables or command line arguments are good for infromation that you don't want to source control next to the app like secrets, or deployment-specific configuration. environment variables are clumsy when your data is highly structured since they are just key value pairs. The set of environment variables are read into configuration when the process starts are aren't expected to change for the lifetime of the process.

Here's the same example but with an environment variable

**undo configuration file and dotnet run with env-var**

It's not a mistake. The ASP.NET Core configuration system is heirarchical. The double-underscore is how you express nesting.

We also wanted to talk about logging and diagnostics. ASP.NET Core will log to the console by default, and for our purposes that seems right.

All of these default behaviors I'm talking about live in this method call. Here in Program.cs is all the boilerplate code that you probably never modify. It actually does have a purpose, it does all of these useful things that we've just seen.

**show web host defaults** 

I would be possible for you to leave this out, create an empty host builder, and then have complete control over all of the details.

That's all we're doing with this basic application so let's jump to the next topic.

## Process Summary

So here's a map of what we learned from this. The top row is the runtime environment (the process) - the middle row is the different concerns we have to manage - and the bottom row is the different options we have provided by the runtime. 

For example if we want to provide some configuration values, we have 3 options.

**next**

However it's difficult to be productive with raw processes. You have to manually assign and manage the ports, URLs and file locations of everything. Its definitely doable but hard to scale.

However we all do this all the time! Using processes is the typical thing to do when you're doing local development or debugging. 

This insight is one of the ones that lead to the creation of the Tye project. If you're developing cloud or microservices applications you really owe it to yourself to check this out. My collegue Glenn is doing a talk on Tye as part of this event.

## Hosted Environments

We can't really talk about processes without talking about what I'm calling "Hosted Environments".

Hosted environments are a little bit of a special category that is a hybrid. In a hosted enviroment you don't get to start a process - instead a process starts you. Examples of this would be something like: IIS, Azure Web Apps, or Azure Functions. 

The tradeoff here is that the environment is more prescriptive about communication and configuration. As a result it might be simpler for your use case. You might get a richer SDK to work with compared to something like HTTP.

**next**

Here's the same diagram but generalized to Hosted Environments like IIS. You don't control the process, so you don't get to use things like environment variables or command line arguments.

If you think about this as something like Azure Functions - it's a simpler model. You get the ability to bind to various kinds of inputs built into the host. 

**next** 

In general for Cloud-hosted servers there's two ways that they work. There's a proxy-based model and an in-process process. 

With the proxy approach - you get to run a normal process, and some proxy server will forward traffic to your process. In an in-process model - you're hosted inside a server the like the Hosted Environment we just discussed.

**next**

These models come with different tradeoffs. Proxy-based models are optimized for the service to be independent of the cloud runtime. Any langauge or framework will do. The in-process model is coupled to the server or runtime - you have to use a supported programming technology, but the API can be richer. The application code ends up being responsible for less using the in-process model.

However, the in-process model is declining in usage as its being replaced with more modern technology and and the increasingly polyglot nature of our world.

**next**

Speaking of .NET specifically - OLD ASP.NET on .NET Framework uses the in-process hosting model in IIS. This is largely the reason why there had to be an ASP.NET Core in the first place - the libraries for doing web programming in .NET were coupled to IIS and its architecture rather than neutral abstractions.

ASP.NET Core was designed from day 1 to use a platform-neutral, multi-server architecture. It was one of the design goals to give the developer control over the process so that it worked the same in development and production. If you're using ASP.NET Core you're probably familiar with Kestrel - the cross-platform server, but there's also another one. We have a server implementation that uses http.sys - a Windows kernel driver that has very high usage rate inside Microsoft by teams that have a big investment in Windows security and monitoring. 

How we interface with IIS has changed over time as well. IIS is still popular with .NET Core users and it's the server used in Azure Web Apps. We started using a reverse proxy approach with IIS but it ended up having poor diagnostics and was slow - so we now do a bit of a hybrid. ASP.NET Core can plug itself into IIS's hosting APIs, but we don't really expose those concepts to developers. This ends up fixing most of the issues we encountered with our earlier attempts.

**next**

So there were processes, and two ways to host server applications.

And then for a long time nothing happened...

## Containers Intro

The idea behind containers has been around for a long time, since the 1970s in fact. Let's isolate a process so that it can have its own filesystem and control its access to operating system resources.

The modern container arrived in 2006 when control groups were added to the Linux kernel. Linux gained the ability to group processes so that they can share a namespace for control over operating system resources. In addition to the container infrastructure in Linux, container runtimes like Docker layer on their own functionality to make containers much easier to use, as well as a management UI and CLI that abstracts over the OS-level features.

Containers have become ubiquitous since they first entered the mainstream. This is a graphic from the Cloud Native Computing Foundation's survey. Keep in mind this is the CNCF audience - who are mostly Kubernetes users, so you should expect usage to be high. You'll notice that almost everyone is using containers in dev and test, and over the last few years many more people have started using them in production.

**next**

We won't get extremely technical about how containers work in this talk, but we'll explain some of the benefits and how to leverage them. 

## Demo: Introducing Containers

Use the backend sample and containerize it:

- show listing files in the container
- show networking, listening port, port forwarding in docker
- show console I/O (error)
- describe docker file
- show a two-phase docker file
- show uploading an image


## Container Wrap Up

So what we found using containers is that a whole lot more capabilities were added. Due to their isolated nature the host can assign hostnames to each container, and can also give each container its own namespace to avoid port conclicts.

Additionally, we can package whatever dependencies we want in an image so we don't have to worry about what's installed on the machine where it's running.

And since a container ultimately launches a process, we have the same capabilities as well.

**next**

One of the biggest benefits of a container is that with registries and versioning we have now a robust system for managing a catalog of images. It starts to feel like something that we can use naturally as part of a CI/CD pipeline or other automated deployment.

**next**

So what's not great is maintaining those Dockerfiles. We end up copy-pasting a lot because so much of a Dockerfile is boilerplate. If they aren't part of the development flow then they can easily bitrot. 

Additionally, Dockerfiles fight with the way we build .NET code in a lot of cases. You need to manage the set of files you copy into Docker when doing a build, but as .NET developers we really like to have multi-project services, or dependencies on files outside the project's directly like a `nuget.config`.

**next**

Here's some of my advice to simplify if you find this difficult. A lot of users seem to really like the two-phase build technique that's popular for native-compiled programming languages. I'd encourage you to think about whether you're really getting a lot out of it.

If you build one service per-repo things can be really simple. Use the repo root as the Docker context and copy everything in. This can be really flexible and gives pretty much any option you want for how to build the images. I always like to highlight this recipe because it gets you out of manually managing lots of files. 

If you build multiple services from the same repo, consider whether you should abandon 2-phase builds. You can write a script to do a publish on a project and then build the image from the output without ever creating a dockerfile.

## More than one at a time

So something you may have noticed so far is that all of the technologies we've looked at really only do one thing at a time. And yes, we're using a very simple application as the example because we have to manage each service separately.

Now we'll look at a more sophisticated app and some technologies that do more than one thing at a time.

**next**

So here's our bigger app. It's got three websites, a redis, and a postgres database. It's a simple voting application where you can vote for whether you like dogs or cats better and see the results update in realtime.

let's use our systematic approach to plan this application.

**next**

First, what does each service need

These are ASP.NET Core applications using 3.1.

There's more communication happening here - our code will communicate over HTTP - we also need to connect to redis and postgres.

And like before we want to inject the URLs and connection strings using config.

**next**

For the runtime we'll be using docker-apps - which was renamed from docker-compose.

In terms of deployment, it can deploy containers. It can also deploy our redis and postgres instances as containers.

For communication, like we saw in the last demo, we will automatically get a hostname to use to refer to each service, and we will need to manually choose ports for anything we want to expose externally.

I also want to highlight here that docker provides a capability for managing secrets separately from the standard features for config. This is meaningful because in a production app, we'd likely use a cloud hosted database, and we'd want to secure access to the connection string. For local development using config with docker-compose will be fine.

**next**

And lastly, lets map our requirments to the platform features

We'll use the ASP.NET Core base image to provide our dependencies. We'll use images for redis and postgres for development.

We'll rely on the docker-assigned hostnames for each service, and port 80 for our web applications.

We'll use environment variables to pass in our connection strings and service URLs.

So let's take a look.

## Demo: Docker-Compose

Show the docker-compose file 
- reiterate all of the key points
- run the demo

## Kubernetes

Now that we've gotten this far, we can comfortably make the jump to Kubernetes. We'll need a little bit of background on it to proceed.

**next**

First of all let's just say that Kubernetes is a much more complicated system than anything we've looked at so far. 

When we talk about something like docker - it's simpler to configure because running a container is the main thing docker does. Kubernetes on the other hand supports many different kinds of objects - so running a container isn't really the main thing it does. 

Next, we're used to management of hostnames for containers automatically from docker. In Kubernetes not only is it not automatic, it's not even the same object. So that's new, our services don't map 1:1 to a single config entry in the application model of kubernetes, more like 1:2 or more.

Lastly, the object format of Kubernetes is pretty complicated compared to what we've seen so far. The amount of concerns we have to express and how they are expressed is the same as our previous examples, but the volume of YAML produced is much larger.

**next**

Before we do a demo let's look at a brief diagram.

I mentioned that we'd have a separate object to configure hostnames so that's off to the right side.

Other than that there's another levels of nesting happening here. Kubernetes layers even more features on the container. We have richer monitoring features like health checks, and the ability to run multiple containers side-by-side or run a container as an initialization task. 

I'm omitting a lot of details here because I want to focus on what's most relevant to how we design and model our code.

Let's do a demo.

## Demo: Kubernetes

## Wrap Up

Thanks for your attention, I'm going to wrap up some of the key points. 

**next**

Application models lets us describe the interface between your software and a runtime environment. 

We can apply systematic thinking to help with both understanding and planning the needs of services, and how to configure a deployment.

**next**

Think about your service's requirements in terms of 4 categories: Deployment, Communication, Configuration, and Diagnostics

Understand what options your platform provides for each of these concerns.

Then write down a mapping of your requirements into those configuration options. This should result in whatever manifest you need to deploy.

**next**

Applying these principles for ASP.NET Core doesn't take much work.

First of all understand what the framework provides for Configuration, Logging, and Server features.

Avoid hardcoding connection strings or URLs of other services, drive them with configuration. 

And lastly check out Glenn Conron's talk at this conference, and project Tye to make multi-service development easier.

**next**

As another note, there were a few topics I wanted to include in this talk but didn't really get to due to complexity. We only really got to look at logging as a diagnostics technology because every platform provides logging. When you look past that you might extend the platform by collection structure logs, distributed tracing, and metrics. These are key technologies that should be part of your design and deployment consideration.

**next**

That's all from me. Thanks!