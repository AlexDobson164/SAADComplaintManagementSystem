# Security Options - API Key

* Status: accepted
* Deciders: Alex Dobson
* Date: 2025-12-07

Technical Story: Need to decide on what security options to use for the proof of concept, this decision is specifically if we are going to use an API Key

## Context and Problem Statement

Am I going to implement an API key for the proof of concept

## Decision Drivers

* Difficulty of Implementing
* Utiliy of having an API key

## Considered Options

* Using API Key
* Not using API Key

## Decision Outcome

Chosen option: "Using API Key", because the clear benefits of implementing API keys far out weigh any disadvantages. To be honest, when i was making the arguments for not using API keys, I was really struggling to come up with reasons.

### Positive Consequences

* Easily able to know what company to associate a request with
* helps to stop people just randomly hitting the endpoint to get data

### Negative Consequences

* Every API call will need to include the correct API key for a company

## Pros and Cons of the Options

### Using API Key

Develop my solution and make every api request into my system include an API key

* Good, because It will let me know what company a request is comming from, this is esspecialy useful for a consumer openening a ticket as the consumers will not have an account
* Good, because It would also let the solution track how much each company hits our endpoints in the future
* Good, because it is quite easy to implement in C#
* Bad, because Every request will need to include the api key for a company that is in the database

### Not using API Key

Not use an api key in my solution

* Good, because It would be easier because i wouldn't need to learn how to do it
* Bad, because I wouldn't know what companies requests are coming from
* Bad, because Setting up new companies will be a little bit more involved
