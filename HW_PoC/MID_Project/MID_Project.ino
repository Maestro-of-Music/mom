#include <SoftwareSerial.h> //시리얼통신 라이브러리 호출
#include <MsTimer2.h>
int blueTx=2;   //Tx (보내는핀 설정)at
int blueRx=3;   //Rx (받는핀 설정)
SoftwareSerial mySerial(blueTx, blueRx);  //시리얼 통신을 위한 객체선언
 
void setup() 
{
  Serial.begin(115200);   //시리얼모니터
  mySerial.begin(115200); //블루투스 시리얼
}

void loop()
{
  char data[101];
  int i,j;
  int n=0;
  int note=0;
  if(mySerial.available())
  {
    while(1) 
    { 
      if(mySerial.available())
      {
        char tmp=mySerial.read();
        if(tmp=='#') continue;
        if(tmp=='/') break;
        data[n++]=tmp-'0';
       }
    }
    for(i=0;i<n;i++) note=note*10+data[i];
   
    Serial.println(note);
  }
  if (Serial.available()) {         
    mySerial.write(Serial.read());  //시리얼 모니터 내용을 블루추스 측에 WRITE
  }
//  while(!Serial.available()) {         
 //   mySerial.write(Serial.read());  //시리얼 모니터 내용을 블루추스 측에 WRITE
 // }
}


