# AWS API Gateway Mock API (Proof of Concept)

## Introduction

Now, not too long ago, I was brushing up on some of that AWS training. And it got me to take a closer look at their API Gateways. And something caught my eye. Apparently, you can use it with mock integrations! With mock data. Instead of having to wire it up to a bunch of ready-to-go backend services or repos, you can just configure it with some test data right there in the console.

And, since I’m already a bit of a fanboy for using mock APIs, this got me wanting to take it for a test drive... to see if it's actually a practical, fully-capable tool that I can, in good faith, recommend to other developers and testers. Lord knows, with all that code AI's been cranking out these days, a test engineer's gonna need all the help they can get.

Now, mock APIs are actually pretty useful for helping frontend (or client) developers who want to be able to work independently, writing code and showing it off to customers and stakeholders. To be able iterate without having to wait for backend services to get built and deployed. 

For that matter, maybe your architect hasn't actually decided which of those backend dependencies are gonna be the best fit for your solution. They're gonna want to be able to defer some of those important architecture decision until the "last responsible moment"... or, at least, until you've had a chance to sort out all those fuzzy business requirements.

Then, of course, you've got some real risk, or sunk cost fallacy, that comes with doing vertical-slice or full-stack development, feature by feature. Before vetting what it actually is that you're trying to build. Before figuring out why you're trying to build it.

So yeah, Mock APIs, just like in-process mock objects, let you code right up to the edge of the part of the system that you care about. I mean, with mocks, it's actually possible to get your client applications into a totally "code complete" state before a single line of code's been written for the backend. That's basically what they're talking about with that whole "domain-driven design" thing, right?


## PoC Architecture
(Architecture View)
Ok, so for this PoC, I wanted a fairly minimalistic design. No need to deploy the system to the cloud, I can just run it locally. And, for that matter, no need to build out a fancy front end, I decided to use a test project with use-case-focused unit tests to invoke my client library.