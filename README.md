# TinyGPSPlus for .NET nanoFramework
TinyGPSPlusNF is a .NET nanoFramework port of the TinyGPSPlus Arduino library. See [Mikal Hart's TinyGPSPlus GitHub repo](https://github.com/mikalhart/TinyGPSPlus) for an in-depth presentation of TinyGPSPlus.

This port is functionally identical to the original library. For now, there are no plans to add any new features.

The following documentation is taken from TinyGPS++ ([see here](http://arduiniana.org/libraries/tinygpsplus/)) and adapted to this port.

## Download and Installation

_TODO_ (nuget, etc.)

## Usage

Let's say you have an ESP32 hooked to an off-the-shelf GPS device and you want to display your altitude. You would simply create a `TinyGPSPlus` instance like this:

```csharp
TinyGPSPlus gps = new();
```

Repeatedly feed it characters from your GPS device:
```csharp
TODO
TODO
TODO
TODO
```

Then query it for the desired information:

```csharp
if (gps.Altitude.IsUpdated)
{
    Debug.WriteLine(gps.Altitude.Meters.ToString());
}
```

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

float d = lat.Degrees; // Latitude in degrees
bool n = lat.Negative; // Indicates whether the Degrees property is negative
ushort hd = lat.HoleDegrees; // Degrees hole part (absolute value)
uint b = lat.Billionths; // Degrees fractional part

// Identical for longitude.
```

### Date

```csharp
uint v = gps.Date.Value; // Raw date in DDMMYY format
byte y = gps.Date.Year; // Year (2000+)
byte m = gps.Date.Month; // Month (1-12)
byte d = gps.Date.Day; // Day (1-31)
```

### Time

```csharp
uint v = gps.Time.Value; // Raw time in HHMMSSCC format
byte h = gps.Time.Hour; // Hour (0-23)
byte m = gps.Time.Minute; // Minute (0-59)
byte s = gps.Time.Second; // Second (0-59)
byte c = gps.Time.Centisecond; // 100ths of a second (0-99)
```

### Speed

```csharp
float v = gps.Speed.Value; // Raw speed in 100ths of a knot
float kt = gps.Speed.Knots; // Speed in knots
float mph = gps.Speed.Mph; // Speed in miles per hour
float mps = gps.Speed.Mps; // Speed in meters per second
float kmh = gps.Speed.Kmph; // Speed in kilometers per hour
```

### Course

```csharp
float v = gps.Course.Value; // Raw course in 100ths of a degree
float d = gps.Course.Degrees; // Course in degrees
```

### Altitude

```csharp
float v = gps.Altitude.Value; // Raw altitude in centimeters
float m = gps.Altitude.Meters; // Altitude in meters
float mi = gps.Altitude.Miles; // Altitude in miles
float km = gps.Altitude.Kilometers; // Altitude in kilometers
float ft = gps.Altitude.Feet; // Altitude in feet
```

### Satellites

```csharp
int s = gps.Satellites.Value; // Number of satellites in use
```

### HDOP

```csharp
float v = gps.Hdop.Value; // Horizontal Dilution of Precision
```

## Validity, update status and age

You can examine a property's value at any time, but unless your `TinyGPSPlus` instance has recently been fed from the GPS, it should not be considered valid and up-to-date. The boolean `IsValid` property will tell you whether the object contains any valid data and is safe to query.

Similarly, `IsUpdated` indicates whether the property's value has been updated (not necessarily changed) since the last time you queried it.

Lastly, if you want to know how stale an object's data is, refer to its `Age` property, which returns the number of milliseconds since its last update. The greater the value returned, the likelier there is a problem like a lost fix.

## Debugging

If your code was to fail, the cause could be found in the NMEA stream fed to your `TinyGPSPlus` instance. A broken, incomplete or empty NMEA sentence could cause problems.

Fortunately, it's pretty easy to determine what's going wrong using some built-in diagnostic properties:

* `CharsProcessed` - the total number of characters received by the object
* `SentencesWithFix` - the number of $GPRMC or $GPGGA sentences that had a fix
* `FailedChecksum` - the number of sentences of all types that failed the checksum test
* `PassedChecksum` - the number of sentences of all types that passed the checksum test

## Custom NMEA sentence extraction

One of the great features of TinyGPSPlus is the ability to extract arbitrary data from any NMEA or NMEA-like sentence. Read up on some of the [interesting sentences](http://aprs.gids.nl/nmea/) there are out there, then check to make sure that your GPS receiver can generate them.

The idea behind custom extraction is that you tell TinyGPSPlus the sentence name and the field number you are interested in, like this:

```csharp
TinyGPSCustom magneticVariation = new TinyGPSCustom(gps, "GPRMC", 10);
```

This instructs TinyGPSPlus to keep an eye out for `$GPRMC` sentences, and extract the 10th comma-separated field each time one flows by. At this point, `magneticVariation` is a new object just like the built-in ones. You can query it just like the others:

```csharp
if (magneticVariation.IsUpdated)
{
    Debug.WriteLine("Magnetic variation is:");
    Debug.WriteLine(magneticVariation.Value);
}
```

By default, custom values will be returned as `string`. There is a new feature in TinyGPSPlusNF that allows you to instruct your `TinyGPSCustom` object to treat the value as a numeric one. You have to change how you instanciate the object:

```csharp
TinyGPSCustom distanceToWaypoint = new TinyGPSCustom(gps, "GPBWC", 10, true);
```

The value will be treated as a `float` and can be used in your code depending on your need. For example:

```csharp
if (distanceToWaypoint.NumericValue.Value < 10)
{
    Debug.WriteLine("You're less than ten miles to your waypoint.");
}
```

## Establishing a fix

A `TinyGPSPlus` instance depends on your code to be fed with valid and current NMEA GPS data. To ensure its world-view is continually up-to-date, three things must happen:

1. You must continually feed the object serial NMEA data with `Encode()`.
2. The NMEA sentences must pass the checksum test.
3. For built-in (non-custom) objects, the NMEA sentences must self-report themselves as valid. That is, if the `$GPRMC` sentence reports a validity of `"V"` (void) instead of `"A"` (active), or if the `$GPGGA` sentence reports fix type `"0"` (no fix), then the position and altitude information is discarded (though time and date are retained).

## Distance and course

If your application has some notion of a "waypoint" or destination, it is sometimes useful to be able to calculate the distance to that waypoint and the direction, or "course", you must travel to get there. The `TinyGPSPlus` type provides two static methods to get this information: `DistanceBetween()` and `CourseTo()`. A third one, `Cardinal()`, transforms the course in friendly, human-readable compass directions.

```csharp
private const float EIFFEL_TOWER_LAT = 48.85826f;
private const float EIFFEL_TOWER_LNG = 2.294516f;

// [...]

float distanceKm = TinyGPSPlus.DistanceBetween(
    gps.Location.Latitude.Degrees,
    gps.Location.Longitude.Degrees,
    EIFFEL_TOWER_LAT,
    EIFFEL_TOWER_LNG) / 1000;

float courseTo = TinyGPSPlus.CourseTo(
    gps.Location.Latitude.Degrees,
    gps.Location.Longitude.Degrees,
    EIFFEL_TOWER_LAT,
    EIFFEL_TOWER_LNG);

string cardinal = TinyGPSPlus.Cardinal(courseTo);

Debug.Write("Distance (km) to Eiffel Tower: ");
Debug.WriteLine(distanceKm.ToString("N2"));

Debug.Write("Course to Eiffel Tower: ");
Debug.WriteLine(courseTo.ToString("N2"));

Debug.Write("Human readable directions: ");
Debug.WriteLine(cardinal);
```

## Examples

TinyGPSPlusNF ships with [several sample solutions](https://github.com/mboud/TinyGPSPlusNF/tree/main/examples) which range from the simple to the more elaborate. Start with `BasicExample`, which demonstrates library basics without even requiring a GPS device, then move on to `FullExample` and `KitchenSink`. Later, see if you can understand how to do custom extractions with some of the other examples.

## Acknowledgements

Thank you to [Mikal Hart](https://github.com/mikalhart) for his work on TinyGPS++. This was my first time porting code from Arduino to .NET nanoFramework and the documentation, ease of use as well as the readability of Mikal's code helped a lot.

Thank you to all the amazing people from the .NET nanoFramework community who helped me. Without them this port would not have seen the light of day. If you want to learn more, go have a look at [.NET nanoFramework's website](http://www.nanoframework.net/), [GitHub](https://github.com/nanoframework), [Twitter](https://twitter.com/nanoFramework) and [Discord server](https://discord.com/invite/gCyBu8T).