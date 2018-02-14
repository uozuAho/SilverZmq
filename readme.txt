# Silverlight <--> console comms using SignalR and ZeroMQ

Original Silverlight + SignalR derived from
https://www.codeproject.com/Tips/787033/SignalR-in-Silverlight

# Getting started

- After building, copy SlZmqWeb\bin\i386 to SlZmqWeb, otherwise the web
  app can't find libzmq.dll. Only required on first build. Meh.
- Run the web app, wait for a bit (10-20 seconds) for signalR to get off its butt
- Run ZmqConsoleClient\bin\Debug\ZmqConsoleClient.exe. It sends messages 
  to the silverlight app via ZeroMQ and signalR, and prints the responses.
  Enter END in the silverlight textbox and press the button to stop
  the receive loop in the console client.
