# Frontend Technologies

* Status: accepted
* Deciders: Alex Dobson
* Date: 2025-12-05

Technical Story: I need to choose what to use for developing the frontend of my solution

## Context and Problem Statement

I need to choose what technologies to use when developing the frontend of my solution, I have already decided to use TypeScript to add types to JavaScript and Tailwind CSS to help write css

## Decision Drivers

* Previous Experience
* Ease of use
* Long Term Support

## Considered Options

* Angular
* Blazor

## Decision Outcome

Chosen option: "Blazor", because I am much more confident with C# than I am with JS or TS. Angular was the frontend framework that was used by my placement, meaning I have some experience with it, but my possition was as a backend developer so I don't have much experince with it. I have also used Blazor a bit before, meaing I have a similar amount of experience witth both options. Both options have learnig curves but as I am already using .NET for the backend, the learning curve will be less for Blazor.

### Positive Consequences

* You can write C# code to run in the browser, replacing JS.
* As it is part of the .NET framework, which means it will be easier for devs to work on both frontend and backend
* Components mean that I can reuse some parts of the website

### Negative Consequences

* Some concerns about scalability, unsure if this is still relevant

## Pros and Cons of the Options

### Angular

Developed and maintained by Google. Based on TypeScript.

* Good, because Provides support for updating versions
* Good, because Uses components, meaning that elements of the site can easiy be reused
* Good, because Scales well as shown by some of the sites the second linked site
* Bad, because Support for major releases are normally only 18 months

### Blazor

Also part of .NET, meaning that it is maintained by Microsoft

* Good, because As it is part of the .NET framework, which means it will be easy for devs to work on both frontend and backend
* Good, because You can write C# code to run in the browser, replacing JS.
* Good, because Also uses components
* Bad, because Even though you can use C# instead of JS, you might still need to write some JS
* Bad, because When Blazor first released, there were concerns about it not scaling very well. I can not find this concern for modern vertions though.

## Links

* Angular versioning and releases - https://angular.dev/reference/releases
* Sites using Angular - https://www.madewithangular.com/sites
