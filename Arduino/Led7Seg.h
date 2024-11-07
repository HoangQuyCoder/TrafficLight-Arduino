#ifndef Led7Seg_h
#define Led7Seg_h
#include "Arduino.h"

class Led7Seg
{
public:
    Led7Seg(int pin1 = 1, int pin2 = 1, int pin3 = 1, int pin4 = 1, int pin5 = 1 , int pin6 = 1, int pin7 = 1 , int pin8 = 1);
    void setXanh(int tXanh);
    void setVang(int tVang);
    void setDo(int tDo);
    void set(int tXanh, int tDo, int tVang);
    int getXanh();
    int getDo();
    int getVang();
    void calculator(Led7Seg ledOther);

private:
    int _tXanh;
    int _tDo;
    int _tVang = 3;
    int _pin1, _pin2, _pin3, _pin4, _pin5, _pin6, _pin7, _pin8;
    
};

#endif