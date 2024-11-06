#ifndef Led_h
#define Led_h
#include "Arduino.h"

class Led
{
public:
    Led(int tXanh, int tDo, int tVang);
    void setXanh(int tXanh);
    void setVang(int tVang);
    void setDo(int tDo);
    void set(int tXanh, int tDo, int tVang);
    int getXanh();
    int getDo();
    int getVang();
    void calculator(Led ledOther);

private:
    int _tXanh;
    int _tDo;
    int _tVang = 3;
    
};

#endif