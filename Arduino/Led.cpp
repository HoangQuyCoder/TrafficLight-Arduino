#include "Led.h"
#include "Arduino.h"

Led::Led(int tXanh, int tDo, int tVang) {
  this->_tXanh = tXanh;
  this->_tDo = tDo;
  this->_tVang = tVang;
}

void Led::calculator(Led ledOther){
  this->_tXanh = ledOther._tDo - ledOther._tVang;
  this->_tDo = ledOther._tXanh + ledOther._tVang;
  this->_tVang = ledOther._tDo <= 3 ? 0 : 3;
}

void Led::set(int tXanh, int tDo, int tVang) {
  this->_tXanh = tXanh;
  this->_tDo = tDo;
  this->_tVang = tVang;
}

void Led::setXanh(int tXanh) {
  this->_tXanh = tXanh;
}

void Led::setVang(int tVang) {
  this->_tVang = tVang;
}
void Led::setDo(int tDo) {
  this->_tDo = tDo;
}

int Led::getXanh() {
  return this->_tXanh;
}

int Led::getDo() {
  return this->_tDo;
}

int Led::getVang() {
  return this->_tVang;
}