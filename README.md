# GraphSocialApp
Implementation of a console-based social networking application (similar to Twitter) 

At this project Microsoft Graph Engine is used

http://www.graphengine.io/ 

Graph Engine = RAM Store + Computation Engine + Graph Model
Graph Engine (GE) is a distributed, in-memory, large graph processing engine, underpinned by a strongly-typed RAM store and a general computation engine.

The distributed RAM store provides a globally addressable high-performance key-value store over a cluster of machines. Through the RAM store, GE enables the fast random data access power over a large distributed data set.

The capability of fast data exploration and distributed parallel computing makes GE a natural large graph processing platform. GE supports both low-latency online query processing and high-throughput offline analytics on billion-node large graphs.

Details of the commands 

posting: (user name) -> (message)

reading: (user name)

following: (user name) follows (another user)

wall: (user name) wall

Scenarios
 Posting: Alice can publish messages to a personal timeline

Alice -> I love the weather today

Bob -> Damn! We lost!

Bob -> Good game though.

 Reading: Bob can view Alice’s timeline

Alice

I love the weather today (5 minutes ago)

Bob

Good game though. (1 minute ago)

Damn! We lost! (2 minutes ago)

 Following: Charlie can subscribe to Alice’s and Bob’s timelines, and view an aggregated list of all subscriptions

Charlie -> I'm in New York today! Anyone want to have a coffee?

Charlie follows Alice

Charlie wall

Charlie - I'm in New York today! Anyone want to have a coffee? (2 seconds ago)

Alice - I love the weather today (5 minutes ago)


Charlie follows Bob

Charlie wall

Charlie - I'm in New York today! Anyone wants to have a coffee? (15 seconds ago)

Bob - Good game though. (1 minute ago)

Bob - Damn! We lost! (2 minutes ago)

Alice - I love the weather today (5 minutes ago)
