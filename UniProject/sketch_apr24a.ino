int val; 
int ledpin=13;

void setup() 
{ 
  Serial.begin(9600);
  pinMode(ledpin,OUTPUT);
  digitalWrite(ledpin,LOW);
} 

void loop()
{ 
  static bool isLEDOn = false;
  val=Serial.read(); 

  if(val=='1')
  { 
    Serial.println("Read ON");
    if(isLEDOn)
    {
      Serial.println("Already On :(");
    }
    else
    {
      isLEDOn = true;
      digitalWrite(ledpin,HIGH); 
      Serial.println("You've turned me on....:)");
    }
  }
  if(val=='0')
  {
    Serial.println("Read OFF");
    if(false==isLEDOn)
    {
      Serial.println("Already Off :(");
    }
    else
    {
      isLEDOn = false;
      digitalWrite(ledpin,LOW);
      Serial.println("You've turned me off....:)");
    }
  }
  
}
