#include <ArduinoJson.h>

//Definição das variáveis
int aLeituraAr = A1;
int aLeituraAgua = A0;

void setup() {
 
  Serial.begin(9600); //Inicializa a Serial
    
}

//Leitura de parametros Ohms
int lerValorOhms(int numeroPorta)
{
  int R1 = 0;
  int R2 = 1000; //Resistencia base 1000 Ohms
  float val = 0;
  float Vout = 0.0;
  int Vin = 5; //Tensao de entrada 5v
  
  val = 1.0*analogRead (numeroPorta); //Aquisição analógica de valores pelo pino A0
  Vout = (val*Vin)/1024;              // Fórmula para calcular o Vout
  R1 = (R2*(Vin-Vout))/Vout;          //Fòrmula do divisor de tensão

  float tensao = val * (  5.0 / 1024);

  if(val<60){
    return 0;
  }
  
  return R1;
}

float LerValorVolts(int numeroPorta)
{
  float Val = 0;
  float Vout = 0.0;

  Val = analogRead(numeroPorta);
  Vout = Val * (5.0 / 1024);

  return Vout;
}
  
void loop() {

  float sensorTemperaturaAgua = 0;
  float sensorTemperaturaAr = 0;
  sensorTemperaturaAgua = LerValorVolts(aLeituraAgua);
  sensorTemperaturaAr = LerValorVolts(aLeituraAr);

  //Se não tiver resistor "Insira resistor"
  //if(resultado == 0){
    //Serial.write("Sem dados");
  //}
  //else{
        
    DynamicJsonDocument tempAgua(1024);
    tempAgua["SensorType"] = 2;
    tempAgua["Value"] = sensorTemperaturaAgua;
    
    //Serial.println(tempAgua);
    serializeJson(tempAgua, Serial);
    Serial.write("\n");
    //Serial.println();
    //DynamicJsonDocument tempAr(1024);
    tempAgua["SensorType"] = 1;
    tempAgua["Value"] = sensorTemperaturaAr;
    
    //Serial.println(tempAr);
    serializeJson(tempAgua, Serial);
    Serial.write("\n");
    //Serial.println();
  //}

  delay(2000);

}
