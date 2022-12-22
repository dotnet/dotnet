# Problems and inconsistencies

Client connection to runner, does not detect that we are connected in the same way as runner to testhost.

Client to runner needs a message sent back (testsession.connected), while testhost does not need any message because the underlying tcp client will detect that testhost connected and will send it work right away. What is the unified way to do this?

There are some requests that need no response, like extensions initialize.

Translation layer uses 4 different messages for run, but sends the same payload in all of them with mixed info that is not necessary for some of the messages (testcases / sources, isDebug). When running with sources and default testhost launcher, we send (from translation layer) incorrectly the same message as when running with testcases.

The messages between runner and testhost have the same names, and sometimes different payloads.
