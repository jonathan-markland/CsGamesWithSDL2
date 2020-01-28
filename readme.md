SDLCoverLibrary - C# Games with SDL2 library on .Net Core
=========================================================

*SDLCoverLibrary* is my very small C# .Net Core library that 
provides for *2D retro game programming* at an introductory level.

![Demo Game 4 image](/Images/DemoGame4.jpg)
(Screenshot from DemoProgram4).

This is possibly of interest to those new to programming, since this 
can be used as-is, without needing to understand C#<->C++ interop, or
the SDL event loop, or set up any complex stuff.  Of course, this could 
be a springboard for further understanding of those things.

Note:  Retro means 80's, rather than mid-90's!

Tech stack
----------
This uses SDL2 and SDL2-CS (see the References section).

SDL2 is a famous graphics library used by many 2D games originally,
and is extended with 3D.  It is portable to Windows and
Linux, and has a small payload (just one or two DLLs).

Here, I expose a subset of the SDL2.DLL's 2D capability. I use the 
SDL2-CS interop library, and gives a more OO interface.

I include demonstration programs with the library.

Nuget
-----
This isn't on the Nuget package feed, because I think it isn't
significant enough, but I do think it is sufficient for someone 
new to game programming to get going.


How you use this
================
As a programmer you:

- Load your graphics bitmaps
- Load your sound effects
- Fill in the bodies of TWO functions, and you're done!

Of course, you will need to design your game, and define classes
to store its data model!

The two functions you need to write are:

- *OnPaint* : This function must take your data model, and draw it
  using the 'window.Renderer' object supplied.  The library will
  call your OnPaint as and when necessary.

- *OnFrameAdvance* : This function is called 50 times per second.
  You must take the current state of your data model, and update it
  in accordance with what events have happened in your game world
  during that 1/50th of a second.  You also receive and process
  the user's input in this function, as well as action the playing
  of sound effects.

Graphics
--------
The library provides you with a "retro screen" which is an off-screen 
bitmap, of dimensions that you specify.  Usually this would be something 
suitably low-res like 320 x 240, since we are being retro here ;)  
Your OnPaint function draws bitmapped images known as "Textures" onto
this retro screen.

You have the option of colour-key transparency in your bitmap images, 
and image stretching.

I also support drawing solid-filled rectangles.

(This is simple, but enough to do quite a lot of retro stuff.)

User Input
----------
My SDLCoverLibrary exposes limited user input:  I support
cursor key movement, and 'Z' to fire.  If you need more, the core
library is pretty easy to extend, but it isn't my intention for this
library to be all-encompassing, this is for introductory purposes.

Check out the Input class to see the event information you receive.

Sound
-----
You can easily load WAV file sound effects, and play them in a single
line of code within your OnFrameAdvance() function.

Linux
-----
Since this is .Net Core, this should all work on Linux, if the Linux 
version of the SDL2 DLL is inserted.  I have not tested this at the 
time of writing.


The demonstration programs included
===================================

DemoProgram1
------------
A static screen demo.
Loads map background and allied / enemy fleet symbols from Beach Head.
The DemoGameImplementation.cs file has some explanation in the two
event handling functions.

DemoProgram2
------------
Sine Wave demo.
Shows how a simple animation can be calculated in the OnPaint() function
using the gameTimeSeconds parameter alone, with no requirement for any 
data-model state.

DemoProgram3
------------
Graphics and sound demo.
This illustrated input processing, and sound playing in the OnFrameAdvance().
There is trivial use of game state.

DemoProgram4
------------
A side-scrolling shoot-em with much more significant game state modelling,
but one "screen" only.

The WorldModel class is the root of the game state, and I use a technique
of state-forwarding to the static Physics class because this clearly 
illustrates what state is *not* affected by the various physical processes
rather than attempting to achieve the same with the "private" keyword.

Some of the leaf-end model objects use OO private sections, but the
collection classes do not as yet.

It is intended this might be a "how to" talking point for further discussion
in pairs/groups.


Jonathan.


References
==========

The SDL2 library homepage.
https://www.libsdl.org/

SDL2# - C# Wrapper for SDL2 by Ethan Lee
https://github.com/flibitijibibo/SDL2-CS

