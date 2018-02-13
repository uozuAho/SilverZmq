# Silverlight <--> console comms using SignalR and ZeroMQ

Original Silverlight + SignalR derived from
https://www.codeproject.com/Tips/787033/SignalR-in-Silverlight

# Getting started

- After building, copy SlZmqWeb\bin\i386 to SlZmqWeb, otherwise the web
  app can't find libzmq.dll. Meh.
- Run the web app, wait for a bit (for signalR to get off its butt)
- Run ZmqConsoleClient. It sends messages to the silverlight app via
  ZeroMQ and signalR, and prints the response

# Todo

- Replace synchronous request/response with dual 1-way message queues.
  Clients will deal with synchronisation.