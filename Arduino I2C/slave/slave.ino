#include <Wire.h>

#define Slave1 1 // Slave address
char rec[128]; // buffer
char hi;
void setup() {
   Wire.begin(Slave1); // init to i2c-slavek
   Wire.onRequest(sendToMaster); // if master request data -> then excute sendToMaster func
   Serial.begin(9600);
   pinMode(2,INPUT_PULLUP);
   pinMode(3,INPUT_PULLUP);
}
int stack[101];
int top=0;
void loop () {
  Wire.onReceive(record);
  if(!digitalRead(2))
  {
    stack[++top]=2;
  }
  if(!digitalRead(3))
  {
    stack[++top]=3;
  }
}
 
void record(int receiveNum){

  // int tmp=int(Wire.read());
  char c=Wire.read();
   Serial.println(c);
}

void sendToMaster() {
  if(top)
  {
   Wire.write(stack[top--]);
  }
}
 
