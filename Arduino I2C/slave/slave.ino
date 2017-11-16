#include <Wire.h>
 
#define Slave1 1 // Slave address
char rec[128]; // buffer
 
void setup() {
   Wire.begin(Slave1); // init to i2c-slave
   Wire.onRequest(sendToMaster); // if master request data -> then excute sendToMaster func
   Serial.begin(9600);
}
 
void loop () {
   Wire.onReceive(record); // if slave receve data -> then excute record func
   if(rec[0] == 1)
   {
     Serial.println("Slave1 : Hi!");
     rec[0]=0;
     delay(1);
   }
}
 
void record(int receiveNum){
   for(int i = 0; i < receiveNum; i++){
     rec[i] = Wire.read();
   }
}

void sendToMaster() {
   Wire.write(65);
}
 
