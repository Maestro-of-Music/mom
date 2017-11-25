#include <SoftwareSerial.h>
#include <MsTimer2.h>
#include <Adafruit_NeoPixel.h>
#include <AltSoftSerial.h>

#define SEL0 8
#define SEL1 9
#define SEL2 10
#define NEOPIN 7
#define NOTE_SIZE 5

int blueTx = 2;
int blueRx = 3;
bool LED_DATA[NOTE_SIZE] = { 0 };
int PIANO_DATA[NOTE_SIZE];
double PIANO_STATE[NOTE_SIZE];
bool PIANO_CHK[NOTE_SIZE];
int changed = -1;

SoftwareSerial mySerial(blueTx, blueRx);
Adafruit_NeoPixel strip = Adafruit_NeoPixel(NOTE_SIZE, NEOPIN, NEO_GRB + NEO_KHZ800);
////////////////////////////////////////////////////////////////////
void setup()
{
  Serial.begin(115200);
  mySerial.begin(38400);
  strip.begin();
  strip.show();
}
void loop()
{
  char data[1001];
  int i, j;
  int n = 0, note = 0;
  char tmp;
  /////////////안드로이드->아두이노 신호 읽기///////////
  if (mySerial.available())
  {
    delay(3);
    if (mySerial.read() == '#')
    {
      char tmp2;
      do { 
        if (mySerial.available())
        {
          delay(3);
          data[n++] = mySerial.read();
        }
      } while (data[n - 1] != '/');
      for (i = 0; i < n - 1; i++) note = note * 10 + data[i] - '0';
      //Serial.println(note);
    }
  }
  ////////////////////////////////////////////////////// 

  /////////////받은 데이터를 이용해 LED제어/////////////
  if (note)
  {
    PIANO_STATE[note - 1] = 1;
    LED_Control();
  }
  //////////////////////////////////////////////////////

  read_piano();

}


void read_piano()
{
  /* Serial.print(PIANO_STATE[0]);
  Serial.print(' ');
  Serial.print(PIANO_STATE[1]);
  Serial.print(' ');
  Serial.print(PIANO_STATE[2]);
  Serial.print(' ');
  Serial.print(PIANO_STATE[3]);
  Serial.print(' ');
  Serial.println(PIANO_STATE[4]);*/
  if (analogRead(A0) < 350)
  {
    if (PIANO_CHK[0] == 0)
    {
      Serial.println("C");
      mySerial.write("A");
      if (PIANO_STATE[0])
      {
        strip.setPixelColor(0, strip.Color(0, 255, 0));
        PIANO_STATE[0] = 0;
      }
      else strip.setPixelColor(0, strip.Color(255, 0, 0));
      strip.show();
      delay(200);
      LED_Control();
    }
    PIANO_CHK[0] = 1;
  }
  else PIANO_CHK[0] = 0;

  if (analogRead(A2) < 350)
  {
    if (PIANO_CHK[2] == 0)
    {
      Serial.println("D");
      mySerial.write("C");
      if (PIANO_STATE[2]) 
      {
        strip.setPixelColor(2, strip.Color(0, 255, 0));
        PIANO_STATE[2] = 0;
      }
      else strip.setPixelColor(2, strip.Color(255, 0, 0));
      strip.show();
      delay(200);
      LED_Control();
    }
    PIANO_CHK[2] = 1;
  }
  else PIANO_CHK[2] = 0;

  if (analogRead(A4) < 600)
  {
    if (PIANO_CHK[4] == 0)
    {
      Serial.println("E");
      mySerial.write("E");
      if (PIANO_STATE[4]) 
      {
        strip.setPixelColor(4, strip.Color(0, 255, 0));
        PIANO_STATE[4] = 0;
      }
      else strip.setPixelColor(4, strip.Color(255, 0, 0));
      strip.show();
      delay(200);
      LED_Control();
    }
    PIANO_CHK[4] = 1;
  }
  else PIANO_CHK[4] = 0;

  if (analogRead(A1) > 55)
  {
    if (PIANO_CHK[1] == 0)
    {
      Serial.println("C#");
      mySerial.write("B");
      if (PIANO_STATE[1]) 
      {
        strip.setPixelColor(1, strip.Color(0, 255, 0));
        PIANO_STATE[1] = 0;
      }
      else strip.setPixelColor(1, strip.Color(255, 0, 0));
      strip.show();
      delay(200);
      LED_Control();
    }
    PIANO_CHK[1] = 1;
  }
  else PIANO_CHK[1] = 0;

  if (analogRead(A3) > 960)
  {
    if (PIANO_CHK[3] == 0)
    {
      Serial.println("D#");
      mySerial.write("D");
      if (PIANO_STATE[3])
      {
        strip.setPixelColor(3, strip.Color(0, 255, 0));
        PIANO_STATE[3] = 0;
      }
      else strip.setPixelColor(3, strip.Color(255, 0, 0));
      strip.show();
      delay(200);
      LED_Control();
    }
    PIANO_CHK[3] = 1;
  }
  else PIANO_CHK[3] = 0;
}
void LED_Control()
{
  for (int i = 0; i < NOTE_SIZE; i++)
  {
    if (PIANO_STATE[i])
      strip.setPixelColor(i, strip.Color(128, 128, 128));
    else
      strip.setPixelColor(i, strip.Color(0, 0, 0));
  }
  strip.show();
}

/*
void LED_Control2()
{
  for (int i = 0; i < 20; i++)
  {
    if (PIANO_STATE[i]) PIANO_STATE[i] += 0.2;
    if (PIANO_STATE[i] >= 108) PIANO_STATE[i] = 0;
    strip.setPixelColor(i, strip.Color(PIANO_STATE[i], PIANO_STATE[i], PIANO_STATE[i]));
  }
  strip.show();
}
*/
/*
void update_led(int note)
{
strip.setPixelColor(note, strip.Color(PIANO_STATE[note],PIANO_STATE[note],PIANO_STATE[note]));
strip.show();
}

int MUX(int sel)
{
bool arr[3];
for(int i=0;i<3;i++) arr[i] = (sel>>i) & 1;
for(int i=2,j=SEL0;i>=0;i--,j++)
{
if(arr[i]) digitalWrite(j,HIGH);
else digitalWrite(j,LOW);
}
return analogRead(A0);
}
*/
/*
void PIANO_CHECK()
{
for(int i=0;i<NOTE_SIZE;i++)
{
int tmp=MUX(i);
if(i==0 || i==2 || i==4)
{
if(tmp>128) PIANO_STATE[i]=1;
else PIANO_STATE[i]=0;
}
else
{
if(tmp<128) PIANO_STATE[i]=1;
else PIANO_STATE[i]=0;
}
if(PIANO_STATE[i]==1)
{
strip.setPixelColor(i, strip.Color(0,0,0));
strip.show();
}
}
}
*/
