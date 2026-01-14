# Database Technology

* Status: accepted
* Deciders: Alex Dobson
* Date: 2025-12-06

Technical Story: Need to choose a relevant database to use to store the data for the Complaint Management System

## Context and Problem Statement

I need to choose a database that will scale well with the solution as the userbase grows.

## Decision Drivers

* Scalability
* Ease of Connection to Backend
* Support Life

## Considered Options

* PostgreSQL
* SQLite

## Decision Outcome

Chosen option: "PostgreSQL", because PostgreSQL better fits the requirements of this project. This is because it can handle more data types than SQLite and the solution needs to be able to accommodate the 10% yearly increase in users. The support life for new versions of PostgreSQL is also longer than other technologies that I am using in this project as well as there being a guide on how to upgrade it.

### Positive Consequences

* PG scales well
* Supports datatypes other databases do not (DateTime, GUIDs/UUIDs, etc )
* Can add roles on the database level if needed, this will allow us to limit what each app will be able to see if the solution is split out into a microservice in the future

### Negative Consequences

* More difficult to set up than other databases

## Pros and Cons of the Options

### PostgreSQL

An open source. object relational database. Allows for the creation of roles that only have specific privileges in the database, such as only being able to interact with specific tables

* Good, because Supports datatypes other databases do not (DateTime, GUIDs/UUIDs, etc )
* Good, because It is free
* Good, because Versions are supported for about 5 years (see PG versions link)
* Good, because Scales well
* Bad, because More difficult to set up than some other databases

### SQLite

A free and open source relational database. The list of systems that use it are listed in the "SQLite usage" link.

* Good, because Lightweight
* Good, because Is also free
* Good, because The developers intend to keep supporting it until 2050
* Bad, because Has a limited number of data types that it can store (text, integer, real and blob)
* Bad, because The SQLite site says its self that it should only be used for small to medium websites.

## Links

* PG versions - https://www.postgresql.org/support/versioning/
* SQLite usage - https://sqlite.org/mostdeployed.html
