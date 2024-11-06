#ifndef GiaoLo_h
#define GiaoLo_h
#include "Arduino.h"

class GiaoLo
{
public:
    GiaoLo(int pinSCLK, int pinRCLK, int pinDIO);
    void displayNumber(int num1, int num2);
    void turnOff7Segment();

private:
    int _pinSCLK;
    int _pinRCLK;
    int _pinDIO;
};

#endif