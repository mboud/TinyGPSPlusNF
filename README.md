# TinyGPSPlus for nanoFramework
This is a nanoFramework port of the TinyGPSPlus Arduino library. See [mikalhart's TinyGPSPlus github repo](https://github.com/mikalhart/TinyGPSPlus) for an in-depth presentation of TinyGPSPlus.

This port is functionally identical to the original library. For now, there are no plans to add any new features.

The following documentation is taken from TinyGPS++ ([see here](http://arduiniana.org/libraries/tinygpsplus/)) and adapted to this port.

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
var lat = gps.Location.Latitude; // Latitude details
var lng = gps.Location.Longitude; // Longitude details

double d = lat.Degrees; // Latitude in degrees
bool n = lat.Negative; // Indicates whether the Degrees property is negative
int hd = lat.Deg; // Degrees hole part (absolute value)
long b = lat.Billionths; // Degrees fractional part

// Identical for longitude.
```

### Date

```csharp
int v = gps.Date.Value; // Raw date in DDMMYY format
int y = gps.Date.Year; // Year (2000+)
int m = gps.Date.Month; // Month (1-12)
int d = gps.Date.Day; // Day (1-31)
```

### Time

```csharp
int v = gps.Time.Value; // Raw time in HHMMSSCC format
int h = gps.Time.Hour; // Hour (0-23)
int m = gps.Time.Minute; // Minute (0-59)
int s = gps.Time.Second; // Second (0-59)
int c = gps.Time.Centisecond; // 100ths of a second (0-99)
```

### Speed

_TODO_

### Course

_TODO_

### Altitude

_TODO_

### Satellites

_TODO_

### HDOP

_TODO_

## Validity, update status and age

_TODO_

## Debugging

_TODO_

## Custom NMEA sentence extraction

_TODO_

## Establishing a fix

_TODO_

## Distance and course

_TODO_

## Examples

_TODO_

## Acknowledgements

A big thank you to [Mikal Hart](https://github.com/mikalhart)'s work on TinyGPS++. This was my first time porting code from Arduino to nanoFramework and the documentation, ease of use as well as the readability of Mikal's code helped a lot.

A huge thank you to all the amazing people from the nanoFramework community who helped me. Without them this port would not have seen the light of day. Go have a look at [.NET nanoFramework's github](https://github.com/nanoframework), [website](http://www.nanoframework.net/), [twitter](https://twitter.com/nanoFramework) and [Discord server](https://discord.com/invite/gCyBu8T).