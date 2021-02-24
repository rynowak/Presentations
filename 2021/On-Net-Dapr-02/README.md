# Dapr Community Standup

## Prep

Remove Dapr from BankFrontend

have dapr getting started open
blob storage binding: https://docs.dapr.io/operations/components/setup-bindings/supported-bindings/blobstorage/
Open storage explorer

## Demo Walkthrough 

Speak to dapr CLI & dapr init at the command line. Show Dapr run of a single app.

Callouts: 

- Running dapr locally by default includes a redis container and a local zipkin
- this is the only time you'll see dapr run in this presentation because Tye has a better experience

Show the application running - talk to its features

- talk about how we're running it with Tye and best way to manage both the services and application code
- show `./components` folder and speak to how it works

---

Let's add dapr to the frontend 

Callouts:

- this adds tracing, e2e encryption, service discovery, and middleware like rate limiting
- this is really easy because we can reuse existing code that does HttpClient

Show zipkin dashboard with tracing now for our transactions

---

Now we haven't talked about the server side, let's see some of those things in action.

Walk through the server code and talk about the state store.

Callouts:

- State store has optimistic concurrency built in. By default it's last write wins, but you can opt in to other modes.

