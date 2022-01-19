# TinyGPSPlus for .NET nanoFramework
TinyGPSPlusNF is a .NET nanoFramework port of the TinyGPSPlus Arduino library. See [mikalhart's TinyGPSPlus github repo](https://github.com/mikalhart/TinyGPSPlus) for an in-depth presentation of TinyGPSPlus.

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
int hd = lat.HoleDegrees; // Degrees hole part (absolute value)
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

```csharp
double v = gps.Speed.Value; // Raw speed in 100ths of a knot
double kt = gps.Speed.Knots; // Speed in knots
double mph = gps.Speed.Mph; // Speed in miles per hour
double mps = gps.Speed.Mps; // Speed in meters per second
double kmh = gps.Speed.Kmph; // Speed in kilometers per hour
```

### Course

```csharp
double v = gps.Course.Value; // Raw course in 100ths of a degree
double d = gps.Course.Degrees; // Course in degrees
```

### Altitude

```csharp
double v = gps.Altitude.Value; // Raw altitude in centimeters
double m = gps.Altitude.Meters; // Altitude in meters
double mi = gps.Altitude.Miles; // Altitude in miles
double km = gps.Altitude.Kilometers; // Altitude in kilometers
double ft = gps.Altitude.Feet; // Altitude in feet
```

### Satellites

```csharp
int s = gps.Satellites.Value; // Number of satellites in use
```

### HDOP

```csharp
double v = gps.Hdop.Value; // Horizontal Dilution of Precision
```

## Validity, update status and age

You can examine a property's value at any time, but unless your `TinyGPSPlus` instance has recently been fed from the GPS, it should not be considered valid and up-to-date. The boolean `IsValid` property will tell you whether the object contains any valid data and is safe to query.

Similarly, `IsUpdated` indicates whether the property's value has been updated (not necessarily changed) since the last time you queried it.

Lastly, if you want to know how stale an object's data is, refer to its `Age` property, which returns the number of ticks<sup>(1)</sup> since its last update. The greater the value returned, the likelier there is a problem like a lost fix.

_<sup>(1)</sup> A single tick represents one hundred nanoseconds or one ten-millionth of a second. There are 10,000 ticks in a millisecond and 10 million ticks in a second.  
Source: https://docs.microsoft.com/en-us/dotnet/api/system.datetime.ticks_

## Debugging

If your code was to fail, the cause could be found in the NMEA stream fed to your `TinyGPSPlus` instance. A broken, incomplete or empty NMEA sentence could cause problems.

Fortunately, it's pretty easy to determine what's going wrong using some built-in diagnostic properties:

* `CharsProcessed` - the total number of characters received by the object
* `SentencesWithFix` - the number of $GPRMC or $GPGGA sentences that had a fix
* `FailedChecksum` - the number of sentences of all types that failed the checksum test
* `PassedChecksum` - the number of sentences of all types that passed the checksum test

## Custom NMEA sentence extraction

_TODO_

## Establishing a fix

_TODO_

## Distance and course

_TODO_

## Examples

_TODO_

## Acknowledgements

Thank you to [Mikal Hart](https://github.com/mikalhart) for his work on TinyGPS++. This was my first time porting code from Arduino to nanoFramework and the documentation, ease of use as well as the readability of Mikal's code helped a lot.

Thank you to all the amazing people from the .NET nanoFramework community who helped me. Without them this port would not have seen the light of day. If you want to learn more, go have a look at [.NET nanoFramework's website](http://www.nanoframework.net/), [github](https://github.com/nanoframework), [twitter](https://twitter.com/nanoFramework) and [Discord server](https://discord.com/invite/gCyBu8T).