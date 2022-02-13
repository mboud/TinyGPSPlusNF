# TinyGPSPlusNF - KitchenSink
Requires a gps device.

This sample code demonstrates just about every built-in operation of TinyGPSPlusNF.

Example based on a NEO-6M module (GY-NEO6MV2) and an ESP32 board (NodeMCU-32S ESP-WROOM-32).

Expected debug output with a gps fix:
```
KitchenSink
Demonstrating nearly every feature of TinyGPSPlusNF
Testing TinyGPSPlusNF library v1.0.0.0

LOCATION ; Fix Age=40ms ; Raw Lat=47[+468292667 billionths] ; Raw Long=-1[+549321167 billionths] ; Lat=47.468292 ; Long=-1.549321
LOCATION ; Fix Age=110ms ; Raw Lat=47[+468292667 billionths] ; Raw Long=-1[+549321167 billionths] ; Lat=47.468292 ; Long=-1.549321
LOCATION ; Fix Age=200ms ; Raw Lat=47[+468292667 billionths] ; Raw Long=-1[+549321167 billionths] ; Lat=47.468292 ; Long=-1.549321
LOCATION ; Fix Age=230ms ; Raw Lat=47[+468292667 billionths] ; Raw Long=-1[+549321167 billionths] ; Lat=47.468292 ; Long=-1.549321
LOCATION ; Fix Age=250ms ; Raw Lat=47[+468292667 billionths] ; Raw Long=-1[+549321167 billionths] ; Lat=47.468292 ; Long=-1.549321
LOCATION ; Fix Age=0ms ; Raw Lat=47[+468292333 billionths] ; Raw Long=-1[+549324500 billionths] ; Lat=47.468292 ; Long=-1.549324
DATE ; Fix Age=10ms ; Raw=100222 ; Year=2022 ; Month=2 ; Day=10
TIME ; Fix Age=10ms ; Raw=21054000 ; Hour=21 ; Minute=5 ; Second=40 ; Hundredths=0
SPEED ; Fix Age=10ms ; Raw=0.158 ; Knots=0.15 ; MPH=0.18 ; m/s=0.08 ; km/h=0.29
LOCATION ; Fix Age=30ms ; Raw Lat=47[+468292333 billionths] ; Raw Long=-1[+549324500 billionths] ; Lat=47.468292 ; Long=-1.549324
LOCATION ; Fix Age=0ms ; Raw Lat=47[+468292333 billionths] ; Raw Long=-1[+549324500 billionths] ; Lat=47.468292 ; Long=-1.549324
TIME ; Fix Age=0ms ; Raw=21054000 ; Hour=21 ; Minute=5 ; Second=40 ; Hundredths=0
ALTITUDE ; Fix Age=10ms ; Raw=15.7 ; Meters=15.7 ; Miles=0 ; KM=0.01 ; Feet=51.5
SATELLITES ; Fix Age=10ms ; Value=6
HDOP ; Fix Age=10ms ; hdop=1.41
LONDON ; Distance=488.33 km ; Course-to=13.48472 degrees [NNE]
DIAGS ; Chars=1960 ; Sentences-with-Fix=10 ; Failed-checksum=1 ; Passed-checksum=33
[...]
```

Note: "London" and "Diags" lines will be displayed every 5 seconds.