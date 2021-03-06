![JTech](https://i.imgur.com/4vq4FiS.png)

An industrial automation overhaul for the game [Rust](https://playrust.com) using [Oxide](https://github.com/OxideMod/Oxide).

[![Build status](https://ci.appveyor.com/api/projects/status/64yx2tj8elrdhhm0?svg=true)](https://ci.appveyor.com/project/jacobcoughenour/jtech) [![Chat](https://img.shields.io/badge/chat-on%20discord-7289da.svg)](https://discord.gg/D2eag6R)

[Trello Roadmap](https://trello.com/b/oXiOcmBo/)

---

### Environment Setup

Visual Studio 2017 is recommended
You can get the Community Edition for free [here](https://www.visualstudio.com/downloads/)

You need a Rust dedicated server with the latested version of Oxide installed.
[Setting up a rust server](http://oxidemod.org/threads/setting-up-a-rust-server-with-windows.5743/)

Once you have the server setup, you need to create an *_Environment Variable_* on your system called **RustServerDir** with the path to your server rust_dedicated folder as the value (Ex: C:\steamcmd\steamapps\common\rust_dedicated).  The system variable is used by the .csproject file for assembly references and the build output.
[How to set an Environment Variable](https://superuser.com/a/284351)

Now you can open JTech.sln in Visual Studio and get started!

### Building from Source

To build and run Jtech on your server, just build the Jtech project (Ctrl-Shift-B).  This starts the PluginMerger script that combines all the .cs files to Jtech.cs and saves it to the build folder and your server plugins folder.  If you have the your server running, oxide auto reloads the plugin for you.
