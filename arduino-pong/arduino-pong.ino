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

    if (incomingByte == 'I') {
      Serial.print(analogRead(A4));
      Serial.print("-");
      Serial.println(analogRead(A5));
    } 
  }

  // Code for passing ultrasonic sensor to Unity.
  long duration;
  long cm;
  pinMode(pingPin, OUTPUT);
  
  digitalWrite(pingPin, LOW);
  delayMicroseconds(2);
  digitalWrite(pingPin, HIGH);
  delayMicroseconds(10);
  digitalWrite(pingPin, LOW);

  pinMode(echoPin, INPUT);
  duration = pulseIn(echoPin, HIGH);

  Serial.println(duration);
}
