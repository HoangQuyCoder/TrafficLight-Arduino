#ifndef GiaoLo_h
#define GiaoLo_h
#include "Arduino.h"
#include "Led7Seg.h"

class GiaoLo
{
public:
    GiaoLo(int pinSCLK, int pinRCLK, int pinDIO, Led7Seg ledOne, Led7Seg ledTwo, Led7Seg ledThree, Led7Seg ledFour);
    void displayNumber(int num1, int num2);
    void turnOff7Segment();

private:
    int _pinSCLK;
    int _pinRCLK;
    int _pinDIO;
    Led7Seg _ledOne, _ledTwo, _ledThree, _ledFour;
};

#endif