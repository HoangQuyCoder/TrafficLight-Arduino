#include "TrafficLight.h"
#include "Arduino.h"

// Set traffic light pins as OUTPUT
TrafficLight::TrafficLight(int pinXanh, int pinDo, int pinVang) {
  pinMode(pinXanh, OUTPUT);
  pinMode(pinDo, OUTPUT);
  pinMode(pinVang, OUTPUT);
  this->_pinXanh = pinXanh;
  this->_pinDo = pinDo;
  this->_pinVang = pinVang;
}

void TrafficLight::toggleYellowLights() {
  digitalWrite(_pinVang, HIGH);  // Turn on yellow light
  digitalWrite(_pinXanh, LOW);
  digitalWrite(_pinDo, LOW);

  delay(500);

  digitalWrite(_pinVang, LOW);  // Turn off yellow light
  digitalWrite(_pinXanh, LOW);
  digitalWrite(_pinDo, LOW);
}

void TrafficLight::setColor(byte color) {
  switch (color) {
    case 0:  // Green
      {
        digitalWrite(_pinXanh, HIGH);
        digitalWrite(_pinDo, LOW);
        digitalWrite(_pinVang, LOW);
        break;
      }
    case 1:  // Yellow
      {
        digitalWrite(_pinXanh, LOW);
        digitalWrite(_pinDo, LOW);
        digitalWrite(_pinVang, HIGH);
        break;
      }
    case 2:  // Red
      {
        digitalWrite(_pinXanh, LOW);
        digitalWrite(_pinDo, HIGH);
        digitalWrite(_pinVang, LOW);
        break;
      }
    default:
      {
        digitalWrite(_pinXanh, LOW);
        digitalWrite(_pinDo, LOW);
        digitalWrite(_pinVang, LOW);
        break;
      }
  }
}