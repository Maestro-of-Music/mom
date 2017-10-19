#include <SoftwareSerial.h>
#include <MsTimer2.h>
#include <Adafruit_NeoPixel.h>
#define SEL0 8
#define SEL1 9
#define SEL2 10
#define NEOPIN 7
#define NOTE_SIZE 20
int blueTx=2;
int blueRx=3;
SoftwareSerial mySerial(blueTx, blueRx);
Adafruit_NeoPixel strip = Adafruit_NeoPixel(20, NEOPIN, NEO_GRB + NEO_KHZ800);
void setup() 
{
  Serial.begin(115200);
  mySerial.begin(115200);
  strip.begin();
  strip.show(); 
  pinMode(SEL0,OUTPUT);
  pinMode(SEL1,OUTPUT);
  pinMode(SEL2,OUTPUT);
}
bool LED_DATA[NOTE_SIZE]={0};
int PIANO_DATA[NOTE_SIZE];

double PIANO_STATE[NOTE_SIZE];
void loop()
{
  char data[101];
  int i,j;
  int n=0;
  int note=0;
  while(1) 
  {
    LED_Control();
    while(!mySerial.available()) int tt;
    if(mySerial.available())
    {
      char tmp=mySerial.read();
//      Serial.print(tmp);
      if(tmp=='#') continue;
      if(tmp=='/') break;
      data[n++]=tmp-'0';
     }
     if(analogRead(A0)>300)
     {
       for(i=0;i<20;i++)
       {
         strip.setPixelColor(i, strip.Color(0,0,0));
       }
       strip.show();
     }
  }
  
  for(i=0;i<n;i++) note=note*10+data[i];
//  Serial.print("->");
  Serial.println(note);
   PIANO_STATE[note]=10.0;
 //  update_led(note);
//  update_led(note);

  //PIANO_CHECK();
  
}
void LED_Control()
{
  for(int i=0;i<20;i++) 
  {
    if(PIANO_STATE[i]) PIANO_STATE[i]+=0.2;
    if(PIANO_STATE[i]>=108) PIANO_STATE[i]=0; 
    strip.setPixelColor(i, strip.Color(PIANO_STATE[i],PIANO_STATE[i],PIANO_STATE[i]));
  }
  strip.show();
}
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

