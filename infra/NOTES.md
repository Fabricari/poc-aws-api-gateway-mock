# AWS API Gateway Mock API (Proof of Concept)

## Introduction

[Personal View, Talking Head]

Alright, so, not too long ago, I was brushing up on some of that AWS training. And it got me to take a closer look at their API Gateways. And something caught my eye. Apparently, you can use it with mock integrations! With mock data. Instead of having to wire it up to a bunch of ready-to-go backend services or repos, you can just configure it with some test data right there in the console.

And, since I’m already a bit of a fanboy for using mock APIs, this got me wanting to build a proof-of-concept and take it for a test drive... to see if it's actually a practical, fully-capable tool that I can, in good faith, recommend to other developers and testers. Lord knows, with all the code AI's been cranking out these days, a test engineer's gonna need all the help they can get.

Now, mock APIs are actually pretty useful for helping frontend (or client) developers who want to be able to work independently, writing code and showing it off to customers and stakeholders. To be able iterate without having to wait for backend services to get built and deployed. 

For that matter, maybe your architect hasn't actually decided which of those backend dependencies are gonna be the best fit for your solution. They're gonna want to be able to defer some of those important architecture decision until the "last responsible moment"... or, at least, until you've had a chance to sort out all those fuzzy business requirements.

Or maybe you want to be able to mitigate some of that real risk, or sunk cost fallacy, that comes with doing vertical-slice or full-stack development, feature by feature. Before vetting what it actually is that you're trying to build. Before figuring out why you're trying to build it.

So yeah, Mock APIs, just like in-process mock objects, let you code right up to the edge of the part of the system that you care about. I mean, with mocks, it's actually possible to get your client applications into a totally "code complete" state before a single line of code's been written for the backend. That's basically what we're talking about with this whole "domain-driven design" thing.

## Walkthrough Dialog
[Dialog matched to what's being demonstrated]

[README.md Preview]
Ok, so, here we go. Even though this is just a proof-of-concept, I still had to make a couple small decisions. For one, I'm using a Mac, so I decided to use VSCode. It gets the job done, and I figured it'd be good see how it handles proper .NET solutions. And for that matter, I decided to use the latest version of the framework. .NET 10 just dropped, and it's got that Long-Term Support. And I've been wanting to get more familiar with some of that new syntactic sugar anyway. I think, for the most part, what you're gonna see here is pretty simple and self-documenting, regardless of what ever language you prefer to use. That said, just remember, this is all throw-away code. You can go ahead and download it, but you definitely don't want to use it for production.

[Architecture View of the Solution]
Now, as far as the architecture goes, it's pretty simple. I'm not going to be deploying this app into the cloud or anything, so running it local's going to be just fine. There's no UI, so I figured I can just spin up a test project with some unit tests. And that'll cover all the use cases I want to show you. The tests are going to invoke methods in a client library that'll, in turn, make calls to a transport layer responsible for handling all the HTTP calls to the API. I went ahead and called it "Library Catalog", so all my use cases fit within a basic business domain of a public Library. Get your books, place holds, and so on. The transport hides all the gory details about AWS and the API Gateway itself. The gateway's going to be regional, so we're not going to need any of that global edge-location stuff. And finally, instead of having the gateway configured to call some Lambda- or EC2-hosted backend logic, I'm going to configure it to leverage the REST API's built-in mock integrations.

[LibraryCatalogTests.cs View]
Ok! So, for this demo, I'm going to take a bit of a Test-Driven Approach to set up the Mock API. So let's open up the Library Catalog Tests file. Up at the top you can see I'm using Xunit. And here you got some private fields. A configurable base URI for endpoints. Reusable catalog object and a transport class that gets injected to keep the business and API-specific logic separated. And I'll skim through the tests real quick. First, we're going to get a library catalog record by ISBN. And then, a list of books by title. And here we're going to simulate paging. And down at the bottom? A couple variations on POSTing a hold on a book.

[Testing View]
Alright, so now we can click on the Testing View and do a little red-green-refactor. I'll just run all the tests, and we can watch them fail! And that's a good thing. It means our code's not actually hooked up to anything yet. So now, we can just get started setting up the Mock API!

[API Gateway Home Page]
Ok, so... here you can see I've already logged in. If you wanna follow along, you're going to need to set up an AWS account. If it's your first time, they got a free tier. But if not, don't worry. What we're doing here is super cheap. Like. just pennies. Just don't D.O.S. yourself with infinite-loops without configuring throttles and other precautions first. But that's out of scope for what I'm going to show you today. Now, once you're logged in, go to the API Gateway page and click "Create an API."

[AWS, Choose an API Type]
And, as you can see, they got quite a few options. REST API's the one we want for mock integrations. Click "Build."

[AWS, Create REST API]
Now, we're going to keep the default, "New API." And from there you'll give it name. "Mock Library Catalog," or whatever. Here, you can see the options for the endpoint type. Since it's just a PoC running from my laptop, "Regional's" going to be fine. If you need to configure what version of TLS you want, you can pick a value for Security Policy. I'm just going to leave it blank for default. From there, scroll down and click "Create API."

[AWS, Resources]
And that takes you to your API's Resources page. In this case "resources" are the things you access and manipulate with REST. So let's create one. And since we're taking a TDD approach, let's pop back over into the code and see what our first test is looking for.

[VSCode View]
Starting with our "Get Catalog Record by ISBN" test, we can find our "library catalog" class and head down the stack; go to definition. Here, you can see there's no real business logic; it's just a passthrough method on to the transport. And since we want the transport class to encapsulate all that AWS-specific logic, we can find the endpoint's been set here. It gets passed into a generic "Get 200 Json" method. Here, you can see that we expect a "catalog" resource with the API stage set to "dev." There's also that trailing segment for a catalog-specific ISBN value that we just passed in.

[AWS, Resources]
So let's go configure that. Resource name "catalog." Create resource. And we'll create another resource under "catalog" for "ISBN." Notice we got those curly braces. That tells AWS that this is going to be a dynamic path parameter. Now, this is where we want to attach a method to the resource. Click "create method." Pick your method type, "get." And notice you can use any REST-friendly verb here. Delete, patch, post, put... 

[VSCode View]
Again, with our code deciding how we want to do the implementation, we can just pop back into the code and see how we're using this Get-Async method off of that Http-Client. 

[AWS, Resources]
From there, this is where we chose our integration type. You can point to Lambdas, other HTTP endpoints, or even AWS-managed services. But what we want here is the mock. Select that, scroll down, and you can view and configure the method request inputs. Expand "Request Paths" and you'll see that our isbn path-parameter's already been added. Alright! So, you can just click "Create Method."

[AWS, Method Execution]
And with the new "GET" method selected, you can see the execution flow. Each part's got information about the request and response and how to intercept and transform it, if needed. For Mock integrations, this is where we go to configure and test the mocks themselves. On the "Method Request" tab, you'll see what input parameters our method expects. That's what we just set up with that "ISBN" path segment. For this basic, static GET request, configuring the Integration Request tab's really easy. Just confirm that we're expecting a 200-response, right here in the Mapping Template. And then, under this Method Response tab, we're just going to verify that we have a 200 response configured, by default, to match what we have in the Integration Request Mapping Template. Great! Now, under the Integration Response section, this is where we want to make our first set of changes.

[AWS, Edit Integration Response]
For the 200 success-response we'll want to return some JSON. Now, I went ahead and typed up some mapping-template code in advance, and I'll just copy it into the template body field here. It's pretty simple. Two hashes for comments. And it's got some helper methods that can give you access to some of the input parameters: paths, querystrings, headers. And you can print them out as part of the response object that your tests might be expecting. Here, we got a JSON representation of a "Catalog Record" object. Now, the syntax is a little clunky; it uses Apache's Velocity Template Language that's been around for a while. If you've typed out any conditional logic for directives or debugging, this might feel familiar. It is pretty limited, as it should be. If you need anything more flexible, something that simulates more complex behaviors of a backend, that's when you're going to want to consider integrating with a Lambda or something similar. But that's a slippery slope, because then you're one step closer to actually building out the real thing. And that kinda defeats the point of creating mock APIs. Ok, so once you have that in place, you can go ahead and save your Integration Response information.

[AWS, Method Execution]
From there, you're going to want to hit that "test" tab. This is where you can put in some test values and making sure you're getting the responses you expect. Super helpful for troubleshooting the templates. You'll see the response the way your client will see it. Along with the Status Code and execution log. Now, with that, you're ready to deploy your API! From here, you'll get prompted to select or create a stage name. We'll enter "dev" to match the stage name we used in our URI path in our code. After that, it's live! Now, for GET methods, this is easy to test. Copy the "Invoke URL" and paste it in your browser. You'll have to type out the resource segments, "catalog" and any ISBN number. Hit enter. There's your response. And you can see your ISBN value embedded in the JSON. Great!

[VS Code]
Now, if you simply hit run on your unit test, it's still going to fail. That's because you've got to copy-paste that Invoke URL into your base-URI field. Once you got that in place... look at that! The first test passed. Ship it! Alright! With that we can move on to the next test. "Get Catalog Records by Title." Here, we want the API to return a list of catalog items where the title contains the word, "Mistborn." (You might notice a theme here.) Go down the stack. Notice the pass-through method in the "Library Catalog" class. Then down again to the transport method. From here, we want to take note of the resource path. Instead of passing the parameter in as a path segment, we're going to use a query-string. Fair enough.

[AWS, Console]
Back in the AWS console, click on the "Resources" item in your API menu. And check out your resource tree, click the "catalog" resource, and from here we're going to create a new method. Just like before, we'll select the "GET" method type, "mock" as the integration type, but this time, you're going to configure some query string parameters. Click "add query string," enter "title," and make it required. We're not really going to use the required option here, but it's helpful if you want to add some basic validation in the Integration Request section. Click "create method." And with that, you'll notice we got the same default 200 method-response as before. Status code 200's been prepopulated for us here. Now, go to the Integration Response tab, and just like before, we're going to want to add a mapping template body that meets our code's requirements. Paste it in, and, this time, notice it's a little bit different. Here you got an array of 3 JavaScript objects with books that have "Mistborn" in the title. Groovy. Now click "Save." Just like before, we're going to want to test the method, but this time we can type out different values for the query string. For this simple example, what we type out won't make a difference since we hardcoded the array. But it's good to know you have the option. You can verify the response object, the status code. And you're good to go. Click "Deploy API." This time, your "dev" stage shows up. Pick that. Click "Deploy." Copy the URL.

[Browser Chrome]
Plop it in the location bar. Type out the rest of the path: "catalog", the query-string, "title", "Mistborn". And there you go. Notice that it doesn't actually matter what book title you type in; we hadn't added conditional logic to the template yet. But we'll do something like that in the next example.

[VS Code]
Ok, back into your test view. Run the individual test. And it passes! But, here, you're also going to want re-run all the other tests. Make sure only the first two tests pass and that the other three are still red. TDD! Great! Next test! "List View by Author. Paginate and Accumulate." This time, we're going to test what it's like to query for a list of books, one set at a time. For the purpose of this test, we'll have the code loop through all the pages until it runs out, then aggregate it all into one big list of books. Here, I figured Brandon Sanderson was a safe bet. Dude's got so many books! We'll set the page size to five. And then some simple assertions: Make sure we got more than one set of books. Then, we'll test that the full list that got returned is sorted in order by title and isbn.

Now, heading down the stack. Notice we got the looping logic in the Library Catalog class. I consider this to be business logic. After all, the API doesn't care what you do with the paged results. Maybe we want to do some kind of client side caching or whatever. The client class is going to use the nextToken response value to enable some basic paging logic. Again, just a proof-of-concept. Not meant for production!

Ok, further down the stack. The transport method is building up the query string. Author, limit, optional next-token. Got your "dev" staging segment. "Catalog." And a new sub-resource, "Page" with the query string appended. Let's go build it out!

[AWS, Console]
Back to our API's resource section. Click on "catalog". Create a resource. Name it "page" and click the "create" button. Create a method. Get. Mock. We'll add some query string parameters: author, limit, nextToken. "Create method." Now, just like before, we'll go to the Integration Response tab and edit our 200 success response. And once again, we'll paste in the JSON for our mapping template. Now, this time, we've got a little bit more going on. At the top, we're grabbing the querystring values with the input param helper. But we're going to take that token value and use it for some inline conditionals. If it's blank, we'll return page one. Notice that another next-token value is being returned with the response that can be used for follow-up requests. Pass that in, "p2," and you get page two. "p3" for page three. And after that, like, if someone tried to search for "p4", we'll just return an empty set and end the loop. There's better ways to do paging, but this is good enough to give you a taste of what these mocks can do. Alright! Click save.

And then we can test it. Throw in some query string values. Leave out the nextToken value. You should get page one. Add next-token, "p2", you should get page two. And then "p3," page three. And "p4" for an empty result set. Good to go! Don't forget to deploy the API. Into "dev". Grab the URL. Test it in the browser. Ah! Don't forget the question mark goes AFTER the resource. Ok, looks good!

[VS Code]
Back into the code. Run all your tests. 3 out 5! Great.

All right. Final stretch. We're going to build the API endpoint for these next two test cases together. That's because we want to test setting the response status codes conditionally based on data that gets posted to the Mock. For the first test, we're going to simulate placing a hold on a book and getting a "success" response sent back to us. And we'll do that by passing in an ISBN and valid patron ID. The second test is similar, except we'll be expecting a rejection due to an expired Patron ID. Now, for the success scenario, we're going to assert the Patron ID is sent back, but also that the Hold Reply's data model is populated with the appropriate Hold Status Placed enum value. For the rejected hold scenario, we're checking for the Hold Status Rejected and Hold Reason Code, Patron Card Expired values. Notice that the tests aren't checking for actual HTTP Response status codes. The responses should get mapped to what the calling code expects.

Alright. Go to definition and down the stack we go. One more time, and into the transport. Now, something to point out here. Unlike our previous test cases, we're mocking a POST to the REST API with conditional responses. This one's going to get a bit more complicated. For one, we're going to pass in some helper functions, actions, that'll help us map the JSON response into our .NET data objects with enums. You'll notice that the conflict response object has two enums getting set. If you scroll to the top, you can see we've got a readonly field initialized with our JSON Serialization Options. Including, this one for mapping Enums. I guess it's common enough that they baked it into the framework. Pretty cool.

Anyway, if you scroll back down to where we're calling our POST helper, you can see where we set the path for our resources. We've got our "dev" staging segment. And a new "hold" resource. Makes sense. Let's go build it.

[AWS, Console]
Back to our API Resources. Click on the root. Then "Create Resource." "Holds" as the resource name. Then, once again, "create method." This time, we're selecting "post." And then "mock." Now, you might think that we'd pick up our posted values from this request body section. But this is more about defining contracts and validation for when the API is integrating with non-mocked backends. We're going to get our posted values another way. For now, click "create method."

Now, first, we're going to make some changes to our Method Response section. If you look back in the code, you'll notice that our transport class is expecting 201 and 409 response codes. Created and Conflict. As opposed to the usual Success or OK. That said, back in our Method Response section, we're first going to want to delete the 200 response. After that, create a method response. Set the status code to 201 and hit "save". And then, do it again for 409. You should see both of them on your method response tab.

From there, we're going to also make a change to the integration request settings. We'll have to make changes to its inbound mapping template. So click "edit" and scroll down to make changes. First, you'll notice we changed the default status from 200 to 201. But also, because our client posted the data, we're not going to have access to it through the usual "input params" helper. That's because the body usually just gets forwarded on to the backend to get dealt with. But, if we want to make the data available for our integration response mapping template, we've got to copy the request data into the context. It feels a little hacky, but AWS has some decent documentation on how to use the context override with their mapping templates. I've linked them in the repo. Now, don't forget to hit "save", then on to the Integration Response tab.

Just like before, we're going to delete the 200 response. Then add one for the status code of 201. Now, if the backend integration supported conditional responses, we'd also create one for 409. But we're going to add our own status override in the mapping template. So you can just hit save. Next, hit "edit" on the newly created 201 response and you'll see the familiar mapping template field. Click "add mapping template", and enter "application/json" as the content type. It's blank by default.

For the template body, paste in the Velocity template text. Here, we got a couple new things going on. You're able to retrieve the body request values that you stored into the context. Set those to local variables. Then we can echo the ISBN and patron ID back to the client. But, what's more is that we can conditionally set the response based on the patron ID. Here, it's real basic. If it's the expected expired value, we'll override the status code to 409 and shape a rejected payload to send back to the client. Otherwise, we can send back a "placed" response. And that's it. Hit "save".

From here, this is where the testing tab proves to be really helpful. Since we can't really do posts in a browser. Not without a tool like Postman or a test harness. So, go ahead and enter a request body for the expected 201 created response. And then the same for 409. And there you go. Hit deploy.

[VS Code]
Now, here's where you might get a little tripped up. If you head back to your test view a bit too quickly, before AWS has had a chance to properly deploy the API, your tests might still fail at first. That's ok, just give it a minute, and try again. And there you go.

[AWS Console]
Now, one last thing to do. Unless this is a production deployment, it's a good idea to delete your unused APIs when you're done. And that's real easy to do. Go to the API page, make sure your Mock API is selected. Click "delete" up there on the top. It's going to ask you to verify. And there you go.

I do want to encourage you to play with this even if you don't have a free tier account. The API Gateway is pay-as-you-go and you'd have to make like a 100,000,000 calls before you even get to one dollar. As you can see here, all my tests haven't even added up to a penny. And I have budget alarms set up all over the place.

## Conclusion
[Talking Head]
Over time, testing software across distributed systems's gotten more and more complicated. And it doesn't help that automated tools are churning out so much more software that's damn near impossible to review with traditional testing methodologies. And while mock APIs don't eliminate the need for unit tests, end-to-end tests, and all the other tools in your tool chest. It's definitely one that you're going to want to add to the kit.

Alright! My name's Steve Harrison. And you can catch me on YouTube or LinkedIn. And I'll see you there!



