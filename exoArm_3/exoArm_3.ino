#include <Servo.h>
#include <WString.h> 

Servo armServo;

/*   CONSTANTS                                                    */
#define LED_TURN_ON_TIMEOUT  1000       //Timeout for LED power time (defines how long the LED stays powered on) in milliseconds
#define LED_PIN 13                      //Pin number on which the LED is connected
#define SERIAL_BAUDRATE 9600            //Baud-Rate of the serial Port
#define STX "\x02"                      //ASCII-Code 02, text representation of the STX code
#define ETX "\x03"                      //ASCII-Code 03, text representation of the ETX code
#define RS  "$"                         //Used as RS code

#define xPin A0
#define yPin A1
#define zPin A2

#define GLED 3
#define RLED 4
#define PLED 5

/*   WARNING, ERROR AND STATUS CODES                              */
//STATUS
#define MSG_METHOD_SUCCESS 0                      //Code which is used when an operation terminated  successfully
//WARNINGS
#define WRG_NO_SERIAL_DATA_AVAILABLE 250            //Code indicates that no new data is AVAILABLE at the serial input buffer
//ERRORS
#define ERR_SERIAL_IN_COMMAND_NOT_TERMINATED -1   //Code is used when a serial input commands' last char is not a '#'
/******************************************************************/

//From GUI
String noOfTimes = "5";
String motorSpeed = "1";

int countTimes = 0;
int mS = 0;

int minVal = 265;
int maxVal = 402;

double x = 0;    // variable to store the servo position
double y = 0;
double z = 0;
double z1 = 0;
double y1 = 0;
double x1 = 0;

int servoPos = 90;
int servoFlag = 0;
int servoDefault = 90;

int servoMin = 50;
int servoMax = 130;

int safetyFlag = 0;
int safetyCount = 0;

int readSerialInputString(String *command);
//void WriteDummyData();
//void WriteDummyData1();

void setup()
{
  pinMode(LED_PIN, OUTPUT);
  pinMode(GLED, OUTPUT);
  pinMode(RLED, OUTPUT);
  pinMode(PLED, OUTPUT);

  armServo.attach(9);
  Serial.begin(SERIAL_BAUDRATE);
  initArm();
}
void loop()
{
  String command = "";  //Used to store the latest received command
  int serialResult = 0; //return value for reading operation method on serial in put buffer
  //String count="";
  //String speedMode ="";
  int i=0;

  serialResult = readSerialInputCommand(&command);
  if(serialResult == MSG_METHOD_SUCCESS){
    if(command[0] == '$'){//Request for sending data via Serial Interface
      //For demonstration purposes this only writes dummy data
      noOfTimes ="";
      for(i=2;command[i]!='$';i++)
      {
        noOfTimes = noOfTimes + command[i];
      }
      motorSpeed ="";
      motorSpeed +=command[i+1];

//      for(i=i+1;command[i]!='#';i++)
//      {
//        motorSpeed = motorSpeed +command[i];
//      }
//Serial.print("noOfTimes: ");
//Serial.print(noOfTimes);
//Serial.print(" motorSpeed: ");
//Serial.println(motorSpeed);
      safetyFlag=0;
      while(!safetyFlag) {
        digitalWrite(GLED,LOW);
        digitalWrite(RLED,HIGH);
      int cmax = noOfTimes.toInt();

      accelUpdate();

      if(abs(z-z1)<1)
      {
        safetyCount++;
      }
      else safetyCount = 0;
      if (safetyCount == 3 || countTimes == cmax)
      {
        safetyFlag = 1;
        safetyCount = 0;
      }
      //Serial.println("AAAAAAAAAA: ");
      //Serial.print(noOfTimes);
      //Serial.println("TTTTTTTTTTTTTTTT: ");
      //Serial.print(motorSpeed);
      if (safetyFlag == 0)
      {
        moveServo();
      }
      else {
        //armServo.write(servoDefault);
        //for(;;);
      }

      printData();
      z1 = z;
      y1 = y;
      x1 = x;
      delay(10);
      }
      digitalWrite(RLED,LOW);
      digitalWrite(GLED,HIGH);
      countTimes=0;
      //WriteDummyData();
      //delay(1000);
      //WriteDummyData1();
    }
  }

  if(serialResult == WRG_NO_SERIAL_DATA_AVAILABLE){//If there is no data AVAILABLE at the serial port, let the LED blink
    digitalWrite(LED_PIN, HIGH);
    delay(250);
    digitalWrite(LED_PIN, LOW);
    delay(250);
  }
  else{
    if(serialResult == ERR_SERIAL_IN_COMMAND_NOT_TERMINATED){//If the command format was invalid, the led is turned off for two seconds
      digitalWrite(LED_PIN, LOW);
      delay(2000);
    }
  }
}

void initArm()
{
  digitalWrite(GLED, HIGH);
  digitalWrite(RLED, HIGH);
  digitalWrite(PLED, HIGH);
  armServo.write(servoDefault);  
}

void accelUpdate()
{
  int x1, x2, y1, y2, z1, z2;

  x1 = analogRead(xPin);
  y1 = analogRead(yPin);
  z1 = analogRead(zPin);

  x2 = map(x1, minVal, maxVal, -90, 90);
  y2 = map(y1, minVal, maxVal, -90, 90);
  z2 = map(z1, minVal, maxVal, -90, 90);

  x = RAD_TO_DEG * (atan2(-y2, -z2) + PI);
  y = RAD_TO_DEG * (atan2(-x2, -z2) + PI);
  z = RAD_TO_DEG * (atan2(-y2, -x2) + PI);
}

int readSerialInputCommand(String *command){

  int operationStatus = MSG_METHOD_SUCCESS;//Default return is MSG_METHOD_SUCCESS reading data from com buffer.

  //check if serial data is available for reading
  if (Serial.available()) {
    char serialInByte;//temporary variable to hold the last serial input buffer character

    do{//Read serial input buffer data byte by byte 
      serialInByte = Serial.read();
      *command = *command + serialInByte;//Add last read serial input buffer byte to *command pointer
    }
    while(serialInByte != '#' && Serial.available());//until '#' comes up or no serial data is available anymore

      if(serialInByte != '#') {
      operationStatus = ERR_SERIAL_IN_COMMAND_NOT_TERMINATED;
    }
  }
  else{//If not serial input buffer data is AVAILABLE, operationStatus becomes WRG_NO_SERIAL_DATA_AVAILABLE (= No data in the serial input buffer AVAILABLE)
    operationStatus = WRG_NO_SERIAL_DATA_AVAILABLE;
  }

  return operationStatus;
}

void moveServo()
{
  int servoSpeed;
  mS = motorSpeed.toInt();

  if (mS == 0) servoSpeed = 1;

  if (mS == 1) servoSpeed = 3;

  if (mS == 2) servoSpeed = 5;

  if (servoFlag == 0)
    servoPos += servoSpeed;
  else servoPos -= servoSpeed;

  if (servoPos >= servoMax) servoFlag = 1;  
  if (servoPos <= servoMin) servoFlag = 0;
  if (servoPos <= servoMin) 
  {
    countTimes=countTimes+1;
  //  serComm();
  }

  armServo.write(servoPos);

}
//void WriteDummyData(){
//  Serial.print(STX);
//  Serial.print("5");
//  Serial.print(ETX);
//}

//void WriteDummyData1(){
//  Serial.print(STX);
//  Serial.print("6");
//  Serial.print(ETX);
//}

void serComm()
{
  String countT = String(countTimes);
  Serial.print(STX);
  Serial.print(countT);
  Serial.print(ETX);
  //printData();
}
void printData()
{
  Serial.print("x: ");
  Serial.print(x);
  Serial.print(" y: ");
  Serial.print(y);
  Serial.print(" z: ");
  Serial.print(z);
  Serial.println(" ");
  Serial.print("safetyFlag: ");
  Serial.print(safetyFlag);
  Serial.print(" servoFlag: ");
  Serial.print(servoFlag);  
  Serial.println(" ");
  Serial.print("mS: ");
  Serial.print(mS);
  Serial.print(" countTimes: ");
  Serial.print(countTimes);  
  Serial.print(" servoPos: ");
  Serial.print(servoPos);
  Serial.println(" ");

}


