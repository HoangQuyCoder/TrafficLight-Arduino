#include "Led7Seg.h"
#include "Arduino.h"

Led7Seg::Led7Seg(int pin1, int pin2, int pin3, int pin4, int pin5, int pin6, int pin7, int pin8) {
  this->_pin1 = pin1;
  this->_pin2 = pin2;
  this->_pin3 = pin3;
  this->_pin4 = pin4;
  this->_pin5 = pin5;
  this->_pin6 = pin6;
  this->_pin7 = pin7;
  this->_pin8 = pin8;
}

void Led7Seg::calculator(Led7Seg ledOther){
  this->_tXanh = ledOther._tDo - ledOther._tVang;
  this->_tDo = ledOther._tXanh + ledOther._tVang;
  this->_tVang = ledOther._tDo <= 3 ? 0 : 3;
}

void Led7Seg::set(int tXanh, int tDo, int tVang) {
  this->_tXanh = tXanh;
  this->_tDo = tDo;
  this->_tVang = tVang;
}

void Led7Seg::setXanh(int tXanh) {
  this->_tXanh = tXanh;
}

void Led7Seg::setVang(int tVang) {
  this->_tVang = tVang;
}
void Led7Seg::setDo(int tDo) {
  this->_tDo = tDo;
}

int Led7Seg::getXanh() {
  return this->_tXanh;
}

int Led7Seg::getDo() {
  return this->_tDo;
}

int Led7Seg::getVang() {
  return this->_tVang;
}