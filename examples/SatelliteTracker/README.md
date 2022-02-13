# TinyGPSPlusNF - SatelliteTracker
Requires a gps device.

This sample code demonstrates how to use an array of TinyGPSCustom objects to monitor all the visible satellites.

Satellite numbers, elevation, azimuth, and signal-to-noise ratio are not normally tracked by TinyGPSPlusNF, but by using TinyGPSCustom we get around this.

The simple code also demonstrates how to use arrays of TinyGPSCustom objects, each monitoring a different field of the $GPGSV sentence.

Example based on a NEO-6M module (GY-NEO6MV2) and an ESP32 board (NodeMCU-32S ESP-WROOM-32).

Expected debug output with a gps fix:
```
SatelliteTracker
Monitoring satellite location and signal strength using TinyGPSCustom
Testing TinyGPSPlusNF library v1.0.0.0

Sats=5 ; Nums= 2 6 10 11 12 19 22 24 25 29 31 32 ; Elevation= 14 15 3 8 78 15 27 48 58 17 5 41 ; Azimuth= 108 64 248 104 41 37 312 120 257 187 296 296 ; SNR= 23 18 0 25 11 13 11 25 25 18 11 29
Sats=5 ; Nums= 25 29 31 32 ; Elevation= 58 17 5 41 ; Azimuth= 257 187 296 296 ; SNR= 25 18 11 29
Sats=5 ; Nums= 25 29 31 32 ; Elevation= 58 17 5 41 ; Azimuth= 257 187 296 296 ; SNR= 25 18 11 29
Sats=5 ; Nums= 25 29 31 32 ; Elevation= 58 17 5 41 ; Azimuth= 257 187 296 296 ; SNR= 25 18 11 29
Sats=5 ; Nums= 25 29 31 32 ; Elevation= 58 17 5 41 ; Azimuth= 257 187 296 296 ; SNR= 25 18 11 29
Sats=5 ; Nums= 25 29 31 32 ; Elevation= 58 17 5 41 ; Azimuth= 257 187 296 296 ; SNR= 25 18 11 29
Sats=5 ; Nums= 2 6 10 11 12 19 22 24 25 29 31 32 ; Elevation= 14 15 3 8 78 15 27 48 58 17 5 41 ; Azimuth= 108 64 248 104 41 37 312 120 257 187 296 296 ; SNR= 23 18 11 25 14 14 12 26 25 19 12 28
Sats=5 ; Nums= 25 29 31 32 ; Elevation= 58 17 5 41 ; Azimuth= 257 187 296 296 ; SNR= 25 19 12 28
Sats=5 ; Nums= 25 29 31 32 ; Elevation= 58 17 5 41 ; Azimuth= 257 187 296 296 ; SNR= 25 19 12 28
Sats=5 ; Nums= 25 29 31 32 ; Elevation= 58 17 5 41 ; Azimuth= 257 187 296 296 ; SNR= 25 19 12 28
Sats=5 ; Nums= 25 29 31 32 ; Elevation= 58 17 5 41 ; Azimuth= 257 187 296 296 ; SNR= 25 19 12 28
Sats=5 ; Nums= 25 29 31 32 ; Elevation= 58 17 5 41 ; Azimuth= 257 187 296 296 ; SNR= 25 19 12 28
Sats=5 ; Nums= 2 6 10 11 12 19 22 24 25 29 31 32 ; Elevation= 14 15 3 8 78 15 27 48 58 17 5 41 ; Azimuth= 108 64 248 104 41 37 312 120 257 187 296 296 ; SNR= 23 19 12 25 16 15 13 25 25 18 13 28
Sats=5 ; Nums= 25 29 31 32 ; Elevation= 58 17 5 41 ; Azimuth= 257 187 296 296 ; SNR= 25 18 13 28
Sats=5 ; Nums= 25 29 31 32 ; Elevation= 58 17 5 41 ; Azimuth= 257 187 296 296 ; SNR= 25 18 13 28
Sats=5 ; Nums= 25 29 31 32 ; Elevation= 58 17 5 41 ; Azimuth= 257 187 296 296 ; SNR= 25 18 13 28
Sats=5 ; Nums= 25 29 31 32 ; Elevation= 58 17 5 41 ; Azimuth= 257 187 296 296 ; SNR= 25 18 13 28
Sats=5 ; Nums= 25 29 31 32 ; Elevation= 58 17 5 41 ; Azimuth= 257 187 296 296 ; SNR= 25 18 13 28
[...]
```
