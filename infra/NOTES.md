# AWS API Gateway Mock API (Proof of Concept)

## Introduction

[Personal View, Talking Head]

Alright, so, not too long ago, I was brushing up on some of that AWS training. And it got me to take a closer look at their API Gateways. And something caught my eye. Apparently, you can use it with mock integrations! With mock data. Instead of having to wire it up to a bunch of ready-to-go backend services or repos, you can just configure it with some test data right there in the console.

And, since I’m already a bit of a fanboy for using mock APIs, this got me wanting to take it for a test drive... to see if it's actually a practical, fully-capable tool that I can, in good faith, recommend to other developers and testers. Lord knows, with all the code AI's been cranking out these days, a test engineer's gonna need all the help they can get.

Now, mock APIs are actually pretty useful for helping frontend (or client) developers who want to be able to work independently, writing code and showing it off to customers and stakeholders. To be able iterate without having to wait for backend services to get built and deployed. 

For that matter, maybe your architect hasn't actually decided which of those backend dependencies are gonna be the best fit for your solution. They're gonna want to be able to defer some of those important architecture decision until the "last responsible moment"... or, at least, until you've had a chance to sort out all those fuzzy business requirements.

Or maybe you want to be able to mitigate some of that real risk, or sunk cost fallacy, that comes with doing vertical-slice or full-stack development, feature by feature. Before vetting what it actually is that you're trying to build. Before figuring out why you're trying to build it.

So yeah, Mock APIs, just like in-process mock objects, let you code right up to the edge of the part of the system that you care about. I mean, with mocks, it's actually possible to get your client applications into a totally "code complete" state before a single line of code's been written for the backend. That's basically what we're talking about with this whole "domain-driven design" thing.

## Walkthrough Dialog
[Dialog matched to what's being demonstrated]

[README.md Preview]
Alright, so, even though this is just a proof-of-concept, I still had to take a couple small decisions. For one, I'm using a Mac, so I decided to use VSCode. And it's pretty robust. I figured it'd be good see how it handles proper .NET solutions. And for that matter, I decided to use the latest version of the framework. .NET 10 just dropped with that Long-Term Support, and I've been wanting to get familiar with some of that new sytactic sugar. I think, for the most part, what you're gonna see here is pretty simple and self-documenting, regardless of what ever language you prefer to use. That said, just remember, this is all throw-away code. You can go ahead and download it, but you definitely don't want to use it for production.

[Architecture View of the Solution]
Now, as far as the architecture goes, it's pretty simple. I'm not going to be deploying this thing into the cloud or anything, so running it locally is going to be just fine. There's no UI, so I figured I can just spin up a test project some unit tests that'll cover all the use cases that I want to show you. The tests are going to invoke methods in the client libarary that'll, in turn, make calls to a transport layer responsible for handling all the HTTP calls to my Mock API. I went ahead and called it "Library Catalog", so all my use cases fit within a basic business domain of a public Library. Get your books, place holds, and so on. The transport hides all the gory details about AWS and the API Gateway itself. The gateway's going to be regional, so no global edge location stuff needed for today. And finally, instead of having the gateway configured to call some Lambda- or EC2-hosted backend logic, I'm going to configure it to leverage it's built-in mock integrations.

[LibraryCatalogTests.cs View]
Ok! So, for this demo, I'm going to take a bit of a Test-Driven Approach to set up the Mock API. So let's take a look at the Library Catalog Tests file. Up at the top you can see I'm using Xunit for the tests. And here you got some private fields. A configurable base URI for the endpoints. Reusable catalog object and a transport class that gets injected to keep the business logic and the API specific logic separate. And we can skim the tests. We can get a catalog book record by ISBN. Or a list, by title. And here we're going to simulate paging. And down at the bottom? A couple variations on posting a hold.

[Testing View]
Alright, so now we can click on the Testing View and do a little red-green-refactor. I'll just run all the tests, and we can watch them fail! That's good. It means our code's not actually hooked up to anything yet. And we can get started configuring our API on AWS!







