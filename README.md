# TinyGPSPlus for nanoFramework
This is a nanoFramework port of the TinyGPSPlus Arduino library. See [mikalhart's TinyGPSPlus github repo](https://github.com/mikalhart/TinyGPSPlus) for an in-depth presentation of TinyGPSPlus.

This port is functionally identical to the original library. For now, there are no plans to add any new features.

The following documentation is taken from TinyGPS++ and adapted to this port.

## Download and Installation

_TODO_ (nuget, etc.)

## Usage

_TODO_

## Feeding GPS data

_TODO_

## The TinyGPSPlusNF Object Model

The main TinyGPSPlus object contains several properties.

* `Location` - the latest position fix
* `Date` - the latest date fix (UTC)
* `Time` - the latest time fix (UTC)
* `Speed` - current ground speed
* `Course` - current ground course
* `Altitude` - latest altitude fix
* `Satellites` - the number of visible, participating satellites
* `Hdop` - horizontal dilution of precision

Each provides properties to examine its current value, sometimes in multiple formats and units. All examples here after are based on a `TinyGPSPlus` instance named `gps`.

### Location

The `Location` property exposes two sub-properties of type `TinyGPSDegrees` named `Latitude` and `Longitude`. See example bellow.

```csharp
TinyGPSDegrees lat = gps.Location.Latitude; // Latitude details
TinyGPSDegrees lng = gps.Location.Longitude; // Longitude details

double d = lat.Degrees; // Latitude in degrees
bool n = lat.Negative; // Indicates wheter the Degrees property is negative
int hd = lat.Deg; // Degrees hole part (absolute value)
long b = lat.Billionths; // Degrees fractional part

// Identical for longitude.
```

## Validity, Update status and Age

_TODO_
