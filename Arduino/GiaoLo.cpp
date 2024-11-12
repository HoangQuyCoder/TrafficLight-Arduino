#include "GiaoLo.h"
#include "Arduino.h"
byte led7seg[] = {
  0xc0, 0xf9, 0xa4, 0xb0, 0x99, 0x92, 0x82, 0xf8, 0x80, 0x90
};

// Set shift register control pins as OUTPUT
GiaoLo::GiaoLo(int pinSCLK, int pinRCLK, int pinDIO, Led7Seg ledOne, Led7Seg ledTwo, Led7Seg ledThree, Led7Seg ledFour) {
  pinMode(pinSCLK, OUTPUT);
  pinMode(pinRCLK, OUTPUT);
  pinMode(pinDIO, OUTPUT);
  this->_pinSCLK = pinSCLK;
  this->_pinRCLK = pinRCLK;
  this->_pinDIO = pinDIO;

  this->_ledOne = ledOne;
  this->_ledTwo = ledTwo;
  this->_ledThree = ledThree;
  this->_ledFour = ledFour;
}

void GiaoLo::displayNumber(int num1, int num2) {
  shiftOut(_pinDIO, _pinSCLK, MSBFIRST, led7seg[num2 % 10]);
  shiftOut(_pinDIO, _pinSCLK, MSBFIRST, led7seg[num2 / 10]);
  shiftOut(_pinDIO, _pinSCLK, MSBFIRST, led7seg[num1 % 10]);
  shiftOut(_pinDIO, _pinSCLK, MSBFIRST, led7seg[num1 / 10]);
  digitalWrite(_pinRCLK, LOW);
  digitalWrite(_pinRCLK, HIGH);
}

// Turn off all segments of the 7-segment display
void GiaoLo::turnOff7Segment() {
  shiftOut(_pinDIO, _pinSCLK, MSBFIRST, 0xff);
  shiftOut(_pinDIO, _pinSCLK, MSBFIRST, 0xff);
  shiftOut(_pinDIO, _pinSCLK, MSBFIRST, 0xff);
  shiftOut(_pinDIO, _pinSCLK, MSBFIRST, 0xff);

  digitalWrite(_pinRCLK, LOW);
  digitalWrite(_pinRCLK, HIGH);
}