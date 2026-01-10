# Architectural Style of My Solution

* Status: accepted
* Deciders: Alex Dobson
* Date: 2025-12-05

Technical Story: Choosing how to design the architecture of my Complaint Management System

## Context and Problem Statement

I need to choose how I design my system.  The system needs to be able to handle a large user base and to accommodate a 10% yearly increase while maintaining performance and reliability.

## Decision Drivers

* Scalability
* Ease of implementation
* Maintainability
* Reliability

## Considered Options

* Monolith
* Microservices
* Modular Monolith

## Decision Outcome

Chosen option: "Modular Monolith", because I was originally going to choose microservices as it would allow me to easily keep responsibilities in their own areas, but then I was worried about how difficult it will be to implement a microservice. This made me decide to develop my application as a monolith but structure it to mimic microservices. I was then talking to some other students and they brought up a modular monolith and I realised that it was what I was thinking of doing and I just didn't know it had a name.

I am choosing to structure my solution in this way because
- It will be easy for me to implement as, at the end of the day, it is still a monolith 
- Mimics microservices, and I have some experience developing microservices as it was the style used at my placement
- Looking into the future, it will be easier to split out into a microservice than a traditional monolith when the solution needs to scale
- Will make it easier for me to debug as I will know which areas of the application is responsible for.

### Positive Consequences

* Easy to deploy, as it is a single application
* Easier to migrate to a microservice than a traditional monolith when the solution needs to scale more.
* Easy to implement

### Negative Consequences

* Long deployment and build times as it is a single application, decreasing development speed
* I will have to be careful to ensure that I keep logic in the correct areas
* I will need to decide how to split up the responsibilities of the system
* I will have to try to keep coupling to a minimum

## Pros and Cons of the Options

### Monolith

A single application that handles all of the business logic of thee system

* Good, because easy to implement, due to simplicity
* Good, because easy to deploy, as it is a single application
* Bad, because does not scale well
* Bad, because once it is large, it gets harder to add and change features
* Bad, because long build times, slowing down development and deployment
* Bad, because a small change requires the whole application to be redeployed

### Microservices

A solution that is split out into multiple applications, each with its own responsibility. These applications communicate with each other and build the whole solution

* Good, because easy to know where something "lives". This is because each application has its own responsibility. This also increases maintainability
* Good, because shorter build and deployment times, this is because you are only ever building/deploying a part of the solution
* Good, because easy to scale, this is because each app is independent and can be scaled independently
* Bad, because easier to form knowledge silos, single or group of people knowing about a specific application and others not knowing much
* Bad, because deployment is more complicated than a monolith, this is because it is not a single application
* Bad, because difficult to implement initially, this is because you need a way for the apps to communicate with each other. I will be using .NET for my project so I would probably use something like protobuf

### Modular Monolith

The midpoint between a monolith architecture and a microservices architecture. It is a single application but is structured in a way that makes it easier to split out into individual applications later.

* Good, because easy to know where something "lives". This is because each area of the application has its own responsibility. This also increases maintainability
* Good, because easier to implement than microservices, due to simplicity
* Good, because easy to deploy, as it is a single application
* Good, because easier to migrate to a microservice than a traditional monolith when the solution needs to scale more.
* Bad, because does not scale well as it is still a single application
* Bad, because still has long deployment as it is still a single application
* Bad, because a small change requires the whole application to be redeployed still
* Bad, because no way to force logic into the correct areas in the monolith, meaning that parts of code can become tightly coupled and remove the benefit of being easier to split into individual applications

## Links

* Protobuf github - https://github.com/protobuf-net/protobuf-net
