#include <TimerOne.h>
#include "GiaoLo.h"
#include "Led.h"
#include "TrafficLight.h"

#define x1 8
#define v1 9
#define d1 10
#define x2 11
#define v2 12
#define d2 13

#define SCLK 5  // Shift Clock Pin
#define RCLK 4  // Latch Clock Pin
#define DIO 3   // Data Input Pin

bool stopMode = false;
bool yellowFlashing = false;

byte t = 0;
byte t1, t2;

byte t_vang = 3;
byte t_xanh = 14;
byte t_do = 22;

GiaoLo giaoLo(SCLK, RCLK, DIO);
Led ledOne(t_xanh, t_do, t_vang);
Led ledTwo(t_xanh, t_do, t_vang);
TrafficLight trafficLightOne(x1, d1, v1);
TrafficLight trafficLightTwo(x2, d2, v2);

void setup() {
  Serial.begin(9600);

  // Initialize TimerOne to call updateTimers every 1 second
  Timer1.initialize(1000000);            // 1,000,000 microseconds = 1 second
  Timer1.attachInterrupt(updateTimers);  // Attach the timer interrupt
}

// Mode: 0.Light mode  1.Stop mode   2.Night mode
void loop() {
  if (Serial.available() >= 3) {
    byte mode = Serial.read();

    if (mode == 0) {
      byte flag = Serial.read();
      byte t_xanh = Serial.read();
      byte t_do = Serial.read();

      if (flag == 0) {
        ledOne.setXanh(t_xanh);
        ledOne.setDo(t_do);
        if (t_do <= 3) {
          ledOne.setVang(0);
        }
        ledTwo.calculator(ledOne);

      } else if (flag == 1) {
        ledTwo.setXanh(t_xanh);
        ledTwo.setDo(t_do);
        if (t_do <= 3) {
          ledTwo.setVang(0);
        }
        ledOne.calculator(ledTwo);
      }

      stopMode = false;
      yellowFlashing = false;
      resetTimers();

    } else if (mode == 1) {
      byte colorLed1 = Serial.read();
      byte colorLed2 = Serial.read();

      stopMode = true;
      yellowFlashing = false;

      giaoLo.turnOff7Segment();
      trafficLightOne.setColor(colorLed1);
      trafficLightTwo.setColor(colorLed2);
      resetTimers();

    } else if (mode == 2) {
      byte colorLed1 = Serial.read();
      byte colorLed2 = Serial.read();

      yellowFlashing = true;
      stopMode = false;

      giaoLo.turnOff7Segment();
      resetTimers();
    }
  }

  if (stopMode) {
    return;
  }

  // Traffic light control logic based on current time `t`
  if (yellowFlashing) {
    trafficLightOne.toggleYellowLights();
    trafficLightTwo.toggleYellowLights();
    return;
  }

  displayLight();
  // Send t1, t2
  Serial.print(t1);
  Serial.print(",");
  Serial.println(t2);

  giaoLo.displayNumber(t1, t2);
}

void updateTimers() {
  t++;
  t1--;
  t2--;
}

void resetTimers() {
  t = 0;
  t1 = 0;
  t2 = 0;
}

void displayLight() {
  if (t == 0) {
    trafficLightOne.setColor(0);
    trafficLightTwo.setColor(2);
    t1 = ledOne.getXanh();
    t2 = ledTwo.getDo();
    Serial.println("Green: LED 1 - Red: LED 2");

  } else if (t == ledOne.getXanh() && ledOne.getVang() > 0) {
    trafficLightOne.setColor(1);
    trafficLightTwo.setColor(2);
    t1 = ledOne.getVang();
    Serial.println("Yellow: LED 1 - Red: LED 2");

  } else if (t == ledTwo.getDo()) {
    trafficLightOne.setColor(2);
    trafficLightTwo.setColor(0);
    t1 = ledOne.getDo();
    t2 = ledTwo.getXanh();
    Serial.println("Green: LED 2 - Red: LED 1");

  } else if (t == ledOne.getXanh() + ledOne.getDo() && ledOne.getVang() > 0) {
    trafficLightOne.setColor(2);
    trafficLightTwo.setColor(1);
    t2 = ledOne.getVang();
    Serial.println("Yellow: LED 2 - Red: LED 1");

  } else if (t == ledOne.getXanh() + ledOne.getDo() + ledOne.getVang()) {
    t = 0;
  }
}