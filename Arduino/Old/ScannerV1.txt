#include <LiquidCrystal.h>

LiquidCrystal display(2,4,10,11,12,13);

typedef struct { 
  int code;
  String value;
} errorCode;

const errorCode errosCode[] {
    {11, "Sistema OK"},
    {15, "Falha na unidade EFI"},
    {19, "Sem tensao no pino 26"},
    {21, "Temperatura da agua fora da faixa"},
    {22, "Pressao do coletor fora da faixa"},
    {23, "Posicao da borboleta fora da faixa"},
    {24, "Temperatura do ar fora da faixa"},
    {51, "Temperatura agua abaixo da faixa"},
    {52, "Circuito PSPS aberto"},
    {53, "Sinal da borboleta acima da faixa"},
    {54, "Temp. do ar abaixo da faixa"},
    {61, "Temp. da agua acima da faixa"},
    {85, "Falha no sistema de canister"},
    {87, "Falha no circuito da bomba"},
    {95, "Circuito da bomba aberto; terra"},
    {96, "Circuito da bomba aberto; bateria"}
};

int errosEncontrados[10];
int index = 0;
int idLuzInjecao = 5;

void setup()
{
  display.begin(16,2);
  Serial.begin(9600);
    
  pinMode(idLuzInjecao, OUTPUT);
  
  display.print("Inicializando...");
  //delay(2000);
  display.clear();
  
  display.print("Carregando dados...");
  //delay(2000);
  display.clear();
  
  
  /*
  for(uint8_t i = 0; i < sizeof(errosCode)/sizeof(errorCode); ++i) {
    
   // digitalWrite(3, HIGH);
    
    int caract = errosCode[i].value.length();
    //display.print(errosCode[i].code);
    //display.print(" - ");
    display.print(errosCode[i].value);
    delay(1500);
    //digitalWrite(3, LOW);
    while(caract != 0){
      display.scrollDisplayLeft();
      delay(350);
      caract--;
    }
    
    display.clear();
  }*/
  
}

void ImprimirErros()
{
  
  display.setCursor(0,0);
  display.print(index);
  display.print(" - Codigo(s)");
  display.setCursor(0,1);
  display.write("Encontrado(s)");
  delay(2000);
  display.clear();
  
  for(int i = 0; i <= index; ++i){
        
    for(uint8_t j = 0; j < sizeof(errosCode)/sizeof(errorCode); ++j) {
      
     
      if(errosEncontrados[i] == errosCode[j].code) 
    	{
        	
          int caract = errosCode[j].value.length();
          display.print(errosCode[j].code);
          display.print(" - ");
          display.print(errosCode[j].value);
          delay(1500);

          while(caract != 0){
            display.scrollDisplayLeft();
            delay(350);
            caract--;
          }

          display.clear();
    	}
    }
    

  }
  /*
  for(uint8_t i = 0; i < sizeof(errosCode)/sizeof(errorCode); ++i) {
    
    digitalWrite(3, HIGH);
    
    int caract = errosCode[i].value.length();
    //display.print(errosCode[i].code);
    //display.print(" - ");
    display.print(errosCode[i].value);
    delay(1500);
    digitalWrite(3, LOW);
    while(caract != 0){
      display.scrollDisplayLeft();
      delay(350);
      caract--;
    }
    
    display.clear();
  }*/
}

unsigned long startMillis;
unsigned long currentMillis;
int numeroTemp = 0;
int numero1 = 0;
int numero2 = 0;
bool executou = false;
bool processoFinalizado = false;

void loop()
{
  imprimirCodigos:
  if (processoFinalizado)
    return;
  
  // read the input on analog pin 0:
  int sensorValue = analogRead(A1);
  // Convert the analog reading (which goes from 0 - 1023) to a voltage (0 - 5V):
  //float voltage = sensorValue * (5.0 / 1023.0);
  // print out the value you read:
  //Serial.println(sensorValue);
  
  if(sensorValue == 1023)
  {
     
     digitalWrite(6, HIGH);
     
     //Incrementa o numero
     numeroTemp++;
     while(sensorValue == analogRead(A1))
     {
       delay(10);
     }
    
     executou = false;
     startMillis = millis();
    
  }
  else if (sensorValue == 0)
  {
          
     digitalWrite(6, LOW);
     while(sensorValue == analogRead(A1))
     {
       delay(10);
     //}
     	currentMillis = millis();  //get the current "time" (actually the number of milliseconds since the program started)
     
       long diferenca = currentMillis - startMillis;
       if (diferenca >= 2000 && 
           diferenca <= 2500 && !executou)  //test whether the period has elapsed
       {
         Serial.print("Foi um digito: ");
         Serial.println(numeroTemp);
         executou = true;
         startMillis = millis();

         if(numero1 == 0)
         {
           numero1 = numeroTemp;
         }
         else if (numero2 == 0)
         {
           numero2 = numeroTemp;
         }
         numeroTemp = 0;

          if (numero1 != 0 && numero2 != 0)
          {
            Serial.print("Codigo encontrado: ");
            //Serial.print(numero1);
            //Serial.println(numero2);
            
            errosEncontrados[index] = numero1 * 10 + numero2;
            Serial.println(errosEncontrados[index]);
            index++;
            numero1 = numero2 = numeroTemp = 0;
          }
       }
       else if (diferenca >= 6000)
       {
         //Processo Finalizado
         //Apresentar códigos
         processoFinalizado = true;
         Serial.println("Fim");
         ImprimirErros();
         goto imprimirCodigos;
       }
     }
  }
  
  /*
 contact=digitalRead(6);
 if (contact == HIGH) {
   Serial.println ("It works");
 }*/
  
  //Serial.println(contact);
  
  
   
  /*
  display.print("Lendo codigos de erro...");
  delay(2000);
  display.clear();
  
  int sensorValue = analogRead(A0);
  Serial.println(sensorValue);
  
  delay(1000);
  */
}

