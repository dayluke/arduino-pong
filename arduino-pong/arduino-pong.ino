const int ledPin = 13;

int incomingByte;

// Ultrasonic pingPin.
const int pingPin[2] = {8, 7};
// Ultrasonic echoPin.
const int echoPin[2] = {9,6};

long time;

void setup() {
  Serial.begin(9600);
  pinMode(ledPin, OUTPUT);
}

void loop() {
  time = micros();
  // Code for passing potentiometer results to Unity.
  if (Serial.available() > 0) {
    incomingByte = Serial.read();
    if (incomingByte == 'H') {
      digitalWrite(ledPin, HIGH);
    }

    if (incomingByte == 'L') {
      digitalWrite(ledPin, LOW);
    }

    if (incomingByte == 'P') {
      Serial.print(analogRead(A5));
      Serial.print("-");
      Serial.println(analogRead(A4));
    } 

    // Code for passing ultrasonic sensor to Unity.
    if (incomingByte == 'D')
    {
      long ping[2];

      for (int i = 0; i < 2; i++)
      {
        pinMode(pingPin[i], OUTPUT);
        
        digitalWrite(pingPin[i], LOW);
        delayMicroseconds(2);
        digitalWrite(pingPin[i], HIGH);
        delayMicroseconds(10);
        digitalWrite(pingPin[i], LOW);
      
        pinMode(echoPin[i], INPUT);
        ping[i] = pulseIn(echoPin[i], HIGH);
      }
      
      Serial.print(ping[0]);
      Serial.print("-");
      Serial.println(ping[1]);
      
    }   
  }

  while (Serial.available())
  {
    Serial.read();
  }
}
