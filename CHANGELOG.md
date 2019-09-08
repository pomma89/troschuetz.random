# Changelog for Troschuetz.Random #

## v4.3.1 (2019-08-09)

* Added support for Span of bytes (PR #8 by Konstantin Safonov).
* Fixed issue #6.

## v4.3.0 (2018-03-11)

* Removed DLL compiled for .NET 4.7.1, it is not necessary.
* Removed dependency on Thrower.
* Added NextUIntInclusiveMaxValue to IGenerator (issue #5).
* Marked IGenerator.NextUIntExclusiveMaxValue as obsolete (issue #5).
* Changed behavior of IGenerator.NextUInt, now it does not return uint.MaxValue (issue #5).

## v4.2.0 (2017-10-28)

* Added support for .NET Standard 2.0 and .NET Framework 4.7.1.
* Removed graphical tester from NuGet package.

## v4.1.3 (2017-01-08)

* Updated Thrower to v4.0.6.
* Added unit tests for Portable and .NET Standard 1.1.

## v4.1.2 (2016-12-25)

* The behavior of TRandom.Next(int, int) was different from the built-in's (ISSUE#4 by Zhouxing-Su).
* Changed behavior of NextDouble, NextUInt, Doubles, Integers, UnsignedIntegers according to issue #4.
* Updated Thrower to v4.0.5.

## v4.1.1 (2016-12-17)

* Updated Thrower to v4.

## v4.0.8 (2016-12-04)

* Updated dependencies and reduced .NET Standard 1.1 references.
* Relicensed source code under MIT license (ISSUE#2 by Corniel Nobel).

## v4.0.7 (2016-10-30)

* Updated Thrower to v3.0.4 (PR#1 by Corniel Nobel).
* Updated NUnit to v3.5.0 (PR#1 by Corniel Nobel).
* Default seed now takes into account process ID.
* Default seed now [uses a GUID](http://stackoverflow.com/a/18267477/1880086) to improve randomness.
* Updated broken Google-hosted links.
* Fixed a bug in CategoricalDistribution. Weights were not normalized correctly.
* Changed how ParetoDistribution is computed. Now a transformation based on exponential is used.

## v4.0.5 (2016-09-18)

* Added new .NET Standard 1.1 library.
