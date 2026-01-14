# Security - Authentication

* Status: accepted
* Deciders: Alex Dobson
* Date: 2025-12-07

Technical Story: Need to decide on a way for the user to auth into the system, so that they can access features for their role

## Context and Problem Statement

We need a way for the system to be able to identify who is making the call, so that the system can allow them to use features they should be able to use and features they shouldn't be able to use.

## Decision Drivers

* Ease of Implementation
* Security

## Considered Options

* Token Based Authentication (Basic Authentication)
* Session Based Authentication

## Decision Outcome

Chosen option: "Token Based Authentication", because As I am only developing a proof of concept, I do not need to worry too much about data leaks. Basic Authentication is also relatively safe if the API call uses HTTPS.
With it being the easier option to implement, I feel that it is going to be more effective to use Basic Auth for the proof of concept. After all, I can swap the auth method to session based if the project goes beyond a proof of concept.

### Positive Consequences

* Do not need to worry about sessions

### Negative Consequences

* Might need to change the auth method to Session Based later.

## Pros and Cons of the Options

### Token Based Authentication

The username and password for the user are sent over in the API call

* Good, because Easy to implement
* Good, because Do not need to worry about sessions
* Bad, because Vulnerable to sniffing attacks, leaking the user's username and password if the HTTPS is not used

### Session Based Authentication

The user hits an API with their username and password, and it returns a unique ID for their session which is then sent with all other API calls

* Good, because The user only needs to auth once to use the API
* Good, because A session can be ended at any time
* Bad, because Not easy to revoke. This means that if it is stolen, the attacker can act as the user until it expires
* Bad, because Can be hard to scale
* Bad, because Malicious actors could steal the unique ID and pretend to be the user until the session ends
