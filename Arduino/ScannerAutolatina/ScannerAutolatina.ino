#include <ArduinoJson.h>

//Definição das variáveis
int aLeituraAgua = A0;
int aLeituraAr = A1;
int aLeituraMAP = A2;
int aLeituraPosicaoBorboleta = A3;

void setup(){

  Serial.begin(9600); //Inicializa a Serial

}

//Leitura de parametros Ohms
int lerValorOhms(int numeroPorta) {

  int R1 = 0;
  int R2 = 1000; //Resistencia base 1000 Ohms
  float val = 0;
  float Vout = 0.0;
  int Vin = 5; //Tensao de entrada 5v

  val = 1.0 * analogRead(numeroPorta); //Aquisição analógica de valores pelo pino A0
  Vout = (val * Vin) / 1024;           // Fórmula para calcular o Vout
  R1 = (R2 * (Vin - Vout)) / Vout;     //Fòrmula do divisor de tensão

  float tensao = val * (5.0 / 1024);

  if (val < 60)
  {
    return 0;
  }

  return R1;
}

//Realiza a leitura da porta e converte em volts
float LerValorVolts(int numeroPorta) {
  float Val = 0;
  float Vout = 0.0;

  Val = analogRead(numeroPorta);
  Vout = Val * (5.0 / 1024);

  return Vout;
}

void EnviarJsonSerial(DynamicJsonDocument doc) {
  serializeJson(doc, Serial);
  Serial.write("\n");
}

void loop() {

  DynamicJsonDocument doc(1024);

  //Sensor Ar
  doc["SensorType"] = 1;
  doc["Value"] = LerValorVolts(aLeituraAr);
  serializeJson(doc, Serial);
  Serial.write("\n");
  //EnviarJsonSerial(doc);

  //Sensor Agua
  doc["SensorType"] = 2;
  doc["Value"] = LerValorVolts(aLeituraAgua);
  serializeJson(doc, Serial);
  Serial.write("\n");
  //EnviarJsonSerial(doc);

  //Sensor Hall
  /*doc["SensorType"] = 3;
  doc["Value"] = LerValorVolts(aLeituraAgua);
  EnviarJsonSerial(doc);*/

  //Sensor Sonda Lambsa
  /*doc["SensorType"] = 4;
  doc["Value"] = LerValorVolts(aLeituraAgua);
  EnviarJsonSerial(doc);*/

  //Sensor MAP
  doc["SensorType"] = 5;
  doc["Value"] = LerValorVolts(aLeituraMAP);
  serializeJson(doc, Serial);
  Serial.write("\n");
  //EnviarJsonSerial(doc);

  //Sensor Posição da Borboleta
  doc["SensorType"] = 7;
  doc["Value"] = LerValorVolts(aLeituraPosicaoBorboleta);
  serializeJson(doc, Serial);
  Serial.write("\n");
  //EnviarJsonSerial(doc);
  
  delay(2000);
  
}
