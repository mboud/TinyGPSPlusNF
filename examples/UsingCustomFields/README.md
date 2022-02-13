# TinyGPSPlusNF - UsingCustomFields
Requires a gps device.

This sample demonstrates TinyGPSPlusNF's capacity for extracting custom fields from any NMEA sentence. TinyGPSPlusNF has built-in facilities for extracting latitude, longitude, altitude, etc. from the `$GPGGA` and  `$GPRMC` sentences, but with the `TinyGPSCustom` type, you can extract other NMEA fields, even from non-standard NMEA sentences.

Example based on a NEO-6M module (GY-NEO6MV2) and an ESP32 board (NodeMCU-32S ESP-WROOM-32).

Expected debug output with a gps fix:
```
UsingCustomFields
Demonstrating how to extract any NMEA field using TinyGPSCustom
Testing TinyGPSPlusNF library v1.0.0.0

ALT=28.9 meters
SATS=5
PDOP=2.92
HDOP=1.89
VDOP=2.23
[...]
```