Troschuetz.Random
=================

*Fully managed library providing various random number generators and distributions.*

## Introduction ##

All the hard work behind this library was done by Stefan Troschütz, and for which I thank him very much. What I have done with his great project, was simply to refactor and improve his code, while offering a new Python-style random class.

Please visit the [page of the original project](http://goo.gl/rN7my) in order to get an overview of the contents of this library. Unluckily, linked article also contains the only documentation available.

## About this repository and the maintainer ##

Everything done on this repository is freely offered on the terms of the project license. You are free to do everything you want with the code and its related files, as long as you respect the license and use common sense while doing it :-)

I maintain this project during my spare time, so I can offer limited assistance and I can offer **no kind of warranty**.

## Tester ##

A simple, yet effective, WinForms application is available in order to test the Troschuetz.Random library. As for the rest of the code, that application was completely written by Stefan Troschütz and what I did was simply to adapt it to the new refactored code.

The tester is not distributed on NuGet, but it can be [downloaded here](https://drive.google.com/open?id=0B8v0ikF4z2BicWpNRy1WeXJVaE0).

## Extensibility ##

After a request from a user, the library has been modified in order to allow it to be easily extended or modified. As of version 4.0.0, these extensibility cases are supported:
* Defining a custom generator by extending the AbstractGenerator class.
* Defining a custom distribution, by extending the AbstractDistribution class and by implementing either IContinuousDistribution or IDiscreteDistribution.
* Change the core definition of a standard distribution, by redefining the static Sample delegate, used to generate distributed numbers, and the static IsValidParam/AreValidParams delegate, used to validate parameters.

For your convenience, below you can find an example of how you can implement all features above: