# POCbpmn

### Proof-of-concept of how to move business logic from Android application to BPMN scheme.
### In other words, listen and do what Bpmn engine says.

## Core parts:
1. Bpmn engine (Atlas engine .Net implementation)
2. Middleware - .Net based backend routing triggers and data between Bpmn and Android app (websockets)
3. Web dashboard - to reflect what the overall progress for different tasks (separate bpmn processes). Implemented in Vue.js
5. Android app (Jetpack Compose used)

Project part | Location | Can be opened with
------------ | ------------- | -------------
BPMN diagram definition | /Warehouse.Picking/Processes/Shift.bpmn | BPMN Studio
API | /Warehouse.Picking | IntelliJ Rider
Dashboard | /Warehouse.Picking/dashboard-app | IntelliJ Rider
Android client | /android | AndroidStudio

## Setup:

1. BMPN Studio [download](https://www.process-engine.io/downloads/)
2. IntelliJ Rider [download](https://www.jetbrains.com/rider/)
3. .Net [setup](https://docs.microsoft.com/en-us/dotnet/core/install/macos). Can also
   try [homebrew](https://formulae.brew.sh/cask/dotnet)

## Optional:

1. VueJs CLI to initiate VueJs projects [setup](https://cli.vuejs.org/guide/installation.html)

## Docs:

1. ProcessEngine
    * [Getting started](https://www.process-engine.io/docs/getting-started/)
2. VueJs
    * Official [docs](https://vuejs.org/v2/guide/)
    * [Add VueJs to ASP.NET project](https://medium.com/@weicheng0324094/the-easiest-way-to-build-a-spa-in-asp-net-core-vuejs-part-1-9c77de876d6d)
3. SignalR [official docs](https://docs.microsoft.com/en-us/aspnet/core/signalr/introduction?view=aspnetcore-5.0)
    * SignalR Java [Client](https://docs.microsoft.com/en-us/aspnet/core/signalr/java-client?view=aspnetcore-5.0)
