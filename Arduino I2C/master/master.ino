#include <Wire.h>
#include <SoftwareSerial.h>
#define Slave1 1 // Slave address
#define Slave2 2
#define Slave3 3

int blueTx = 2;
int blueRx = 3;

SoftwareSerial mySerial(blueTx, blueRx);
void setup() {
   Wire.begin(); // Init to i2c-master
   Serial.begin(9600); // Init to serial
   mySerial.begin(9600);
}
 
void loop() {

/////////////////Android->Master->Slave///////////////////
//
  if(mySerial.available())
  {
    delay(1);
    char data=mySerial.read();
    int device=data/12+1;
    Serial.print("data :");
    Serial.print(data);
    Serial.print(" device : ");
    Serial.println(device);
    Wire.beginTransmission(device);
    Wire.write((data-1)%12+'A');
    Wire.endTransmission();
  }
//
/////////////////////////////////////////////////////////

//////////////////Slave->Master Request//////////////////
//
 for(int i=1;i<=3;i++)
   {
       Wire.requestFrom(i, 1); // Request 1byte data to slave1
       if(Wire.available())
       {
        char s = Wire.read(); // Read data
        if(s>=1){
        Serial.println(int((i-1)*12+s));
        mySerial.write((i-1)*12+s);
        }
       }
       else break;
   }
//
/////////////////////////////////////////////////////////
 
}
