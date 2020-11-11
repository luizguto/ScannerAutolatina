#include <ArduinoJson.h>

//Definição das variáveis
int aPinLeitura = A0;

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

  //Serial.println(R1);

  if(val<60){
    return 0;
  }
  
  return R1;
}
  
void loop() {

  int resultado = 0;
  resultado = lerValorOhms(aPinLeitura);

  //Se não tiver resistor "Insira resistor"
  if(resultado == 0){
    Serial.write("Sem dados");
  }
  else{
    DynamicJsonDocument doc(1024);

    doc["sensor"] = "Sensor 1";
    doc["valor"] = resultado;
    doc["tipo"] = "Ohms";
    
    serializeJson(doc, Serial);
    // This prints:
    // {"sensor":"gps","time":1351824120,"data":[48.756080,2.302038]}

  }

  delay(10000);

}
