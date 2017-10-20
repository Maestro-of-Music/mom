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
bool PIANO_CHK[NOTE_SIZE];
int changed=-1;
void loop()
{
  char data[101];
  int i,j;
  int n=0;
  int note=0;
  char tmp;
//////////////////////////////////////////////////////
  if(mySerial.available() && mySerial.read()=='#')    
  {
    char tmp2;
    do{ if(mySerial.available()) data[n++]=mySerial.read(); }while(data[n-1]!='/');
    for(i=0;i<n-1;i++) note=note*10+data[i]-'0';
    Serial.println(note); 
    
  }
//////////////////////////////////////////////////////  
  if(note) 
  {
    PIANO_STATE[note-1]=1;
    LED_Control();
  }
  read_piano();
}
void read_piano()
{
  if(analogRead(A0)<500) 
  {
    if(PIANO_CHK[0]==0)
    {
      Serial.println("C");
      PIANO_STATE[0]=0;
      LED_Control();
      mySerial.write("C");
    }
    PIANO_CHK[0]=1;
  }
  else PIANO_CHK[0]=0;
  
  if(analogRead(A2)<350) 
  {
    if(PIANO_CHK[2]==0)
    {
      Serial.println("D");
      PIANO_STATE[2]=0;
      LED_Control();
      mySerial.write("D");
    }
    PIANO_CHK[2]=1;
  }
  else PIANO_CHK[2]=0;

  if(analogRead(A4)<600) 
  {
    if(PIANO_CHK[4]==0)
    {
      Serial.println("E");
      PIANO_STATE[4]=0;
      LED_Control();
      mySerial.write("E");
    }
    PIANO_CHK[4]=1;
  }
  else PIANO_CHK[4]=0;

  if(analogRead(A1)>38) 
  {
    if(PIANO_CHK[1]==0)
    {
      Serial.println("C#");
      PIANO_STATE[1]=0;
      LED_Control();
      mySerial.write("C#");
    }
    PIANO_CHK[1]=1;
  }
  else PIANO_CHK[1]=0;

  if(analogRead(A3)>960) 
  {
    if(PIANO_CHK[3]==0)
    {
      Serial.println("D#");
      PIANO_STATE[3]=0;
      LED_Control();
      mySerial.write("D#");
    }
    PIANO_CHK[3]=1;
  }
  else PIANO_CHK[3]=0;
}
void LED_Control()
{
  for(int i=0;i<20;i++) 
  {
    if(PIANO_STATE[i]) 
    {
      strip.setPixelColor(i, strip.Color(50,50,50));
    }
    else
    {
      strip.setPixelColor(i, strip.Color(0,0,0));
    }
      
  }
  strip.show();
}
void LED_Control2()
{
  for(int i=0;i<20;i++) 
  {
    if(PIANO_STATE[i]) PIANO_STATE[i]+=0.2;
    if(PIANO_STATE[i]>=108) PIANO_STATE[i]=0; 
    strip.setPixelColor(i, strip.Color(PIANO_STATE[i],PIANO_STATE[i],PIANO_STATE[i]));
  }
  strip.show();
}
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
