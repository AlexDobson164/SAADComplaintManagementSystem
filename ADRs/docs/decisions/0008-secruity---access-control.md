# Security - Access Control

* Status: accepted
* Deciders: Alex Dobson
* Date: 2025-12-08

Technical Story: Need a way to be able to decide if a user can access specific features of the solution

## Context and Problem Statement

My solution will need a way for the system to know what features to allow each user to use

## Decision Drivers

* Ease of Implementation
* Appropriateness to Project Brief

## Considered Options

* Role Based
* Attribute Based

## Decision Outcome

Chosen option: "Role Based", because the project specification has a set of clearly defined roles, this means that Role Based access is the obvious choice for this project. Role Based Access Control will likely also match client company structure

### Positive Consequences

* Easy to implement
* Easily matches project specification

### Negative Consequences

* System might grow to have too many roles in the future.

## Pros and Cons of the Options

### Role Based

The system has a set of Roles, each with their own permission levels and access to features on the system

* Good, because Easy to implement
* Good, because Follows most company structures
* Bad, because Can not remove access to features for specific users
* Bad, because Role Based Access Control can lead to a system having too many roles.

### Attribute Based

defines what the user can access in the system by using a set of attributes assigned to the user

* Good, because More flexible, you can remove access to features
* Bad, because Complex implementation
* Bad, because Does not scale well, each API call will need to check for the appropriate attribute for the API called
