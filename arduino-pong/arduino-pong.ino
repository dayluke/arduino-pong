const int ledPin = 13;

int incomingByte;

// Ultrasonic pingPin.
const int pingPin = 7;
// Ultrasonic echoPin.
const int echoPin = 6;

void setup() {
  Serial.begin(9600);
  pinMode(ledPin, OUTPUT);
}

void loop() {
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
      Serial.print(analogRead(A4));
      Serial.print("-");
      Serial.println(analogRead(A5));
    } 

    // Code for passing ultrasonic sensor to Unity.
    if (incomingByte == 'D')
    {
      int loopCount = 20;
      long allDurations[loopCount];
      
      for (int i = 0; i < loopCount; i++)
      {
        long sum = 0;
        long cm;
        pinMode(pingPin, OUTPUT);
        
        digitalWrite(pingPin, LOW);
        delayMicroseconds(2);
        digitalWrite(pingPin, HIGH);
        delayMicroseconds(10);
        digitalWrite(pingPin, LOW);
      
        pinMode(echoPin, INPUT);
        allDurations[i] = pulseIn(echoPin, HIGH);
        sum += pulseIn(echoPin, HIGH);        
      }
      
      long mean = sum / loopCount   
      long sqrSum = 0;

      for (int i = 0; i < loopCount; i++)
      {
        sqrSum += (allDurations[i] - mean) * (allDurations[i] - mean);
      }

      variance = sqrSum / loopCount;
      
    }   
  }
}
