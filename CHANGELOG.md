# Changelog for Troschuetz.Random #

### v4.0.8 (2016-11-04) ###

* Updated dependencies and reduced .NET Standard 1.1 references.
* Relicensed source code under MIT license (ISSUE#2 by Corniel Nobel).

### v4.0.7 (2016-10-30) ###

* Updated Thrower to v3.0.4 (PR#1 by Corniel Nobel).
* Updated NUnit to v3.5.0 (PR#1 by Corniel Nobel).
* Default seed now takes into account process ID.
* Default seed now [uses a GUID](http://stackoverflow.com/a/18267477/1880086) to improve randomness.
* Updated broken Google-hosted links.
* Fixed a bug in CategoricalDistribution. Weights were not normalized correctly.
* Changed how ParetoDistribution is computed. Now a transformation based on exponential is used.

### v4.0.5 (2016-09-18) ###

* Added new .NET Standard 1.1 library.
