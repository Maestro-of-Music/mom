#include <Wire.h>
#include <MsTimer2.h>
#include <Adafruit_NeoPixel.h>
#define Slave1 1 // Slave address
#define LED0 5
#define LED1 6
#define LED2 7
#define sel0 9
#define sel1 10
#define sel2 11
#define sel3 12
#define SIG A0
Adafruit_NeoPixel strip[3] = { 
  Adafruit_NeoPixel(12, LED0, NEO_GRB + NEO_KHZ800), 
  Adafruit_NeoPixel(12, LED1, NEO_GRB + NEO_KHZ800), 
  Adafruit_NeoPixel(12, LED2, NEO_GRB + NEO_KHZ800) 
};
int LED[3][12];
int muxPin[4] = { sel0,sel1,sel2,sel3 };
int muxChannel[12][4] = {
  { 0,0,0,0 }, //channel 0
  { 1,0,0,0 }, //channel 1
  { 0,1,0,0 }, //channel 2
  { 1,1,0,0 }, //channel 3
  { 0,0,1,0 }, //channel 4
  { 1,0,1,0 }, //channel 5
  { 0,1,1,0 }, //channel 6
  { 1,1,1,0 }, //channel 7
  { 0,0,0,1 }, //channel 8
  { 1,0,0,1 }, //channel 9
  { 0,1,0,1 }, //channel 10
  { 1,1,0,1 }, //channel 11
};
int stack[101];
char white_note[7] = { 0,2,4,5,7,9,11 };
char black_note[5] = { 1,3,6,8,10 };
int init_data[12];
bool state[12];
void setup() {
  Wire.begin(Slave1); // init to i2c-slavek
  Wire.onRequest(sendToMaster); // if master request data -> then excute sendToMaster func
  Serial.begin(9600);
  pinMode(sel0, OUTPUT);
  pinMode(sel1, OUTPUT);
  pinMode(sel2, OUTPUT);
  pinMode(sel3, OUTPUT);
  for (int i = 0; i < 12; i++) init_data[i] = readMUX(i);
  MsTimer2::set(20, LED_shift);
  MsTimer2::start();
  strip[0].begin();
  strip[0].show();
  strip[1].begin();
  strip[1].show();
  strip[2].begin();
  strip[2].show();
}

int top = 0;
void loop() {
  LED_ON();
  Wire.onReceive(record);
  for (int i = 0; i < 7; i++) {
    if (init_data[white_note[i]] - readMUX(white_note[i]) > 100)
    {
      if (state[white_note[i]] == 0)
      {
        stack[top++] = white_note[i];
        state[white_note[i]] = 1;
      }
      LED[2][white_note[i]] = 0;
    }
    else state[white_note[i]] = 0;
  }
  for (int i = 0; i < 5; i++)
  {
    if (readMUX(black_note[i]) - init_data[black_note[i]] > 30)
    {
      if (state[black_note[i]] == 0)
      {
        stack[top++] = black_note[i];
        state[black_note[i]] = 1;
      }
      LED[2][black_note[i]] = 0;
    }
    else state[black_note[i]] = 0;
  }

}

void record(char receiveNum) {
  char tmp = Wire.read();
  tmp -= 'A';
  LED[0][tmp] = 1;
  //Serial.println(int(tmp));
}

void sendToMaster() {
  while (top)
  {
    //Serial.println(top);
    Wire.write(stack[--top] + 'A');
  }
}

int readMUX(int num)
{
  for (int i = 0; i < 4; i++) {
    digitalWrite(muxPin[i], muxChannel[num][i]);
  }
  return analogRead(SIG);
}

void LED_ON()
{
  for (int i = 0; i < 3; i++)
  {
    for (int j = 0; j < 12; j++)
    {
      if (LED[i][j]) strip[i].setPixelColor(j, strip[i].Color(LED[i][j] * 3, LED[i][j] * 3, LED[i][j] * 3));
      else strip[i].setPixelColor(j, strip[i].Color(0, 0, 0));
    }
    strip[i].show();
  }
}
void LED_shift()
{
  for (int i = 0; i < 2; i++)
  {
    for (int j = 0; j < 12; j++)
    {
      if (LED[i][j]) ++LED[i][j];
      if (LED[i][j] > 30)
      {
        if (i < 2)
        {
          LED[i + 1][j] = 1;
          LED[i][j] = 0;
        }
      }
    }
  }
}
