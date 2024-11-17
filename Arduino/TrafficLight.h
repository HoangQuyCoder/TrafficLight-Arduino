#ifndef TrafficLight_h
#define TrafficLight_h
#include "Arduino.h"
#include <RTClib.h>

class TrafficLight
{
public:
    TrafficLight(int pinXanh, int pinDo, int pinVang);
    void toggleYellowLights();
    void setColor(byte color);

private:
    int _pinXanh;
    int _pinDo;
    int _pinVang;
};

#endif