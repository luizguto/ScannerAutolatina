// ========================================================================================================
// --- Bibliotecas Auxiliares ---
#include <Wire.h>
#include <LiquidCrystal.h>

// ========================================================================================================
// --- Mapeamento de Hardware ---
#define menu 7
#define enter 6
#define luzInjecao 5
#define input_carro 8

// ========================================================================================================
// --- Protótipo das Funções ---
void readButts();
void list_menu();
void menu_select();
void ler_codigos();
void ver_codigos();
void limpar_codigos();

// ========================================================================================================
// --- Declaração de Objetos ---
LiquidCrystal lcd(2, 4, 10, 11, 12, 13);

// ========================================================================================================
// --- Variáveis Globais ---
int line[3] = {0, 1, 2},
    line_bk[3],
    contador_maximo_erros = 10,
    contador_erros_encontrados = 0,
    menu_number = 1,
    index;

int numeroTemp = 0,
    numero1 = 0,
    numero2 = 0;

boolean menu_flag = 0,
        enter_flag = 0,
        sub_menu = 0,
        executou = false,
        processoFinalizado = false;

unsigned long startMillis,
    currentMillis;

typedef struct
{
        int code;
        String value;
} errorCode;

typedef struct
{
        int code;
        int type;
} error;

error erros_encontrados[10];

const errorCode errosCode[]{
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
    {96, "Circuito da bomba aberto; bateria"}};

// ========================================================================================================
// --- Configurações Iniciais ---
void setup()
{
        Serial.begin(9600);
        pinMode(menu, INPUT_PULLUP);
        pinMode(enter, INPUT_PULLUP);

        lcd.begin(16, 4);
        lcd.setCursor(1, 0);
        lcd.print("    Scanner     ");
        lcd.setCursor(1, 1);
        lcd.print("  Autolatina    ");
        delay(5000);

        lcd.begin(16, 4);
        lcd.setCursor(0, 0);
        lcd.print(">");

} //end setup

// ========================================================================================================
// --- Loop Infinito ---
void loop()
{

        if (!sub_menu)
        {
                lcd.setCursor(0, 0);
                lcd.print(">");
                lcd.setCursor(1, line[0]);
                lcd.print("1)Teste Estatico");
                lcd.setCursor(1, line[1]);
                lcd.print("2)Visualizar    ");
                lcd.setCursor(1, line[2]);
                lcd.print("3)Limpar        ");
        }

        readButts();

} //end loop

void readButts()
{

        if (!digitalRead(menu))
                menu_flag = 0x01;
        if (!digitalRead(enter))
                enter_flag = 0x01;

        if (digitalRead(menu) && menu_flag)
        {
                menu_flag = 0x00;
                list_menu();
                menu_number += 1;
                if (menu_number > 3)
                        menu_number = 1;

        } //end if menu

        if (digitalRead(enter) && enter_flag)
        {
                enter_flag = 0x00;
                sub_menu = !sub_menu;
                menu_select();

        } //end if menu

} //end readButts

void list_menu()
{
        for (int i = 2; i > -1; i--)
        {
                index = i - 1;
                line_bk[i] = line[i];

                if (index < 0)
                        line[i] = line_bk[i + 2];

                else
                        line[i] = line[i - 1];
        }

} //end list_menu

void menu_select()
{

        if (sub_menu)
        {
                switch (menu_number)
                {
                case 1:                
                        lcd.setCursor(1, 0);
                        lcd.print(" Lendo defeitos");
                        lcd.setCursor(0, 1);
                        lcd.print("   existentes  ");
                        ler_codigos(1);

                        lcd.setCursor(1, 0);
                        lcd.print(" Lendo defeitos");
                        lcd.setCursor(0, 1);
                        lcd.print("    passados   ");
                        ler_codigos(2);

                        ver_todos_codigos();
                        break;
                case 2:
                        ver_todos_codigos();
                        break;
                case 3:
                        limpar_codigos();
                        lcd.setCursor(1, 0);
                        lcd.print("Codigos zerados ");
                        lcd.setCursor(0, 1);
                        lcd.print("                ");
                        delay(1000);

                        break;
                } //end switch

                lcd.setCursor(0, 0);
                lcd.print(">");

                lcd.setCursor(1, 0);
                lcd.print(" Voltar        ");
                lcd.setCursor(0, 1);
                lcd.print("               ");
        }
} //end menu_select

void ler_codigos(int type)
{
        processoFinalizado = false;
        while (!processoFinalizado)
        {
                //int sensorValue = analogRead(A1);
                int sensorValue = digitalRead(input_carro);
                startMillis = millis();
                Serial.println(sensorValue);
                if (sensorValue == HIGH)
                {
                        //Incrementa o numero
                        numeroTemp++;
                        //while (sensorValue == analogRead(A1))
                        while (sensorValue == digitalRead(input_carro))
                        {
                                delay(10);
                        }

                        executou = false;
                        startMillis = millis();
                }
                else if (sensorValue == LOW)
                {
                        //while (sensorValue == analogRead(A1) && !processoFinalizado)
                        while (sensorValue == digitalRead(input_carro) && !processoFinalizado)
                        {
                                delay(10);
                                currentMillis = millis();

                                long diferenca = currentMillis - startMillis;
                                if (diferenca >= 2000 &&
                                    diferenca <= 2500 && !executou)
                                {
                                        Serial.print("Foi um digito: ");
                                        Serial.println(numeroTemp);
                                        executou = true;
                                        startMillis = millis();

                                        if (numero1 == 0)
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
                                                erros_encontrados[contador_erros_encontrados].code = numero1 * 10 + numero2;
                                                erros_encontrados[contador_erros_encontrados].type = type;
                                                Serial.println(erros_encontrados[contador_erros_encontrados].code);
                                                contador_erros_encontrados++;
                                                numero1 = numero2 = numeroTemp = 0;
                                        }
                                }
                                else if (diferenca >= 5000)
                                {
                                        //Processo Finalizado
                                        //Apresentar códigos
                                        processoFinalizado = true;
                                        Serial.println("Fim");
                                }
                        }
                }
        }
}

void ver_todos_codigos()
{
        lcd.setCursor(1, 0);
        lcd.print("    Defeitos   ");
        lcd.setCursor(0, 1);
        lcd.print("   existentes  ");
        delay(1000);
        ver_codigos(1);

        lcd.setCursor(1, 0);
        lcd.print("    Defeitos   ");
        lcd.setCursor(0, 1);
        lcd.print("    Passados   ");
        delay(1000);
        ver_codigos(2);
}

void ver_codigos(int type)
{
        lcd.setCursor(0, 0);
        lcd.print(contador_erros_encontrados);
        lcd.print(" - Codigo(s)   ");
        lcd.setCursor(0, 1);
        lcd.write("Encontrado(s)  ");
        delay(2000);
        lcd.clear();

        for (int i = 0; i <= contador_erros_encontrados; ++i)
        {
                if (erros_encontrados[i].type == type)
                {
                        for (uint8_t j = 0; j < sizeof(errosCode) / sizeof(errorCode); ++j)
                        {
                                if (erros_encontrados[i].code == errosCode[j].code)
                                {
                                        int caract = errosCode[j].value.length();
                                        lcd.print(errosCode[j].code);
                                        lcd.print(" - ");
                                        lcd.print(errosCode[j].value);
                                        delay(1500);

                                        while (caract >= 0)
                                        {
                                                lcd.scrollDisplayLeft();
                                                delay(350);
                                                caract -= 2;
                                        }

                                        lcd.clear();
                                }
                        }
                }
        }
}

void limpar_codigos()
{

        for (int i = 0; i <= contador_maximo_erros; ++i)
        {
                erros_encontrados[i] = 0;
        }
}