# Tracker SDK and API

Contents:

* [Summary](#summary)
  * [Issue](#issue)
  * [Decision](#decision)
  * [Status](#status)
* [Details](#details)
  * [Assumptions](#assumptions)
  * [Constraints](#constraints)
  * [Positions](#positions)
  * [Argument](#argument)
  * [Implications](#implications)
* [Related](#related)
  * [Related decisions](#related-decisions)
  * [Related requirements](#related-requirements)
  * [Related artifacts](#related-artifacts)
  * [Related principles](#related-principles)
* [Notes](#notes)

<br/>

## Summary


### Issue

We wanted to have schedular services to perform specific functionality for one of CGT product iConnect on timely basis.
Below are some of those details -
These services needs to run in background and straigh forward solution is to use .Net Console application and host using Task Schedular. However, below are some of the issues list which  wanted to have a soltion -

  * In distributed architecture creating services for each and every new funtionality and maintaining across different environment is time confusing.
  
  * There needs to be a common solution to these type of background processes to run on even driven architecture and setup with minimal efforts.

  * We want developer experience to be fast and reliable, for the setup they should not worry about what Messaging queue is getting used, tracking the messages, logging and monitoring.
  


<br/>

### Decision

Decided to create new Tracker API for abstracting event driven background services.


<br/>

### Status

Created a basic draft with creating trackers, notifications and Publisher Subscriber examples to showcase usage.


<br/>

## Details


<br/>

### Assumptions

All the usual devops assumptions, such as in the book Accelerate.

  * The Tracker API and SDK has used on Dotnet Core 3.1 for App Development, MSMQ for Queue and SQL for data storage and Queue backplane
  * Different application can consume Tracker SDK or can call over REST OData API to create/update/view trackers. Even they can use API to retrigger tracker incase of failure scenarios.
  * Tracker API also have notification support for general as well as for trackers
  * Application can subscribe to Tracker API notification channel to received Realtime notification updates
  * Tracker API can use Memory Cache or Redis cache for caching
  * The design uses MSMQ as Queue. However, any other pub-sub provider Queue can be used like Rabbit Queue, AWS SNS or Azure Topics
  * The design uses  as Signal R. However, any other Realtime messaging provider can be used


<br/>

### Constraints

None known. 


<br/>

### Positions

<br/>

### Argument


Have considered different comparisions for tracking distributed application including advance Queue like Rabbit MQ, Azure Topics, Azure SNS which does provide Queue messages which we can view. 

However, these are very limited functionlities compared the solution we were looking for including view, update, trigger, re-trigger, realtime notifications.

Gathered information from peers on blogs and Hacker News.

<br/>

### Implications

Unknown as of.

<br/>

## Related


<br/>

### Related decisions

If we Tracker SDK and API several other and related product can use Tracker SDK and API for distributed event driven architecture.

This can help bring seamless experiece to product suit on tracking user requests, updating those requests, retriggring and real time notifications.

<br/>

### Related requirements

We want build a interactive UI for Tracker API which can enable register new application, see their application specific requests, update/retrigger requests incase of failure scenarios.

Real time notification channel to subscrive and receive notifications across multiple devices

<br/>

### Related artifacts

None as of.

<br/>

### Related principles

None as of.

<br/>

## Notes

None as of.

