#include <Wire.h>
 
#define Slave1 1 // Slave address
 
void setup() {
   Wire.begin(); // Init to i2c-master
   Serial.begin(9600); // Init to serial
   pinMode(2,INPUT_PULLUP);
}
 
void loop() {
   Wire.requestFrom(Slave1, 1); // Request 1byte data to slave1
   if(Wire.available())
   {
    char s1 = Wire.read(); // Read data
    Serial.println(s1);
   }
   
   if(!digitalRead(2)) // PIN2 -> INPUT_PULLUP
   {
     Serial.print("Send");
     Wire.beginTransmission(Slave1); // Begin transmission
     Wire.write(1); // Write data on buffer
     Wire.endTransmission(); // Send buffer data and end transmission
     delay(1);
   }
   
}
