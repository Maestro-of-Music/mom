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
   mySerial.begin(115200);
}
 
void loop() {

/////////////////Android->Master->Slave///////////////////
//

  if(mySerial.available())
  {
    delay(1);
    char data=mySerial.read();
    data-='!';
    int device=data/12+1;
    Serial.print("data :");
    Serial.print(int(data));
    Serial.print(" device : ");
    Serial.println(device);
    Wire.beginTransmission(device);
    Wire.write((data-1)%12+'A'+1);
    Wire.endTransmission();
  }
//
/////////////////////////////////////////////////////////

//////////////////Slave->Master Request//////////////////
//
 for(int i=1;i<=3;i++)
   {
       Wire.requestFrom(i, 1); // Request 1byte data to slave1
       delay(3);
       
        char s = Wire.read(); // Read data
        if((s>='A' && s<='Z') || (s>='a' && s<='z')){
        Serial.println(s);
        mySerial.write((i-1)*12+(s-'A')+33);
        }
       
       else break;
   }
//
/////////////////////////////////////////////////////////
 
}
