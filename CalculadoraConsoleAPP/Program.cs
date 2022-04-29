using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculadora
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //struct da empresa emissora
            long[] empresaCNPJs = new long[50];
            string[] nomeEmpresa = new string[50];
            string inputCNPJ = "";
            int index = 0;
            long numeroCNPJ;
            //struct clientes
            long[] cadastroClienteCNPJ = new long[100];
            string[] cadastroNomeCliente = new string[100];
            int cadastroIndexCliente = 0;
            long numeroCNPJCliente;

            //struct NF
            double[] nfValor = new double[100];
            string[] nfMes = new string[100];
            string[] nfAno = new string[100];
            long[] nfClienteCNPJ = new long[100];
            string[] nfNome = new string[100];
            long[] nfOrigemCNPJ = new long[100];

            while (true)
            {
                while (true) //Login
                {

                    if (SolicitarCNPJ(ref inputCNPJ, "") == 1)
                        return;
                    numeroCNPJ = NumeroCNPJ(inputCNPJ);
                    int status = VerificarCadastro(numeroCNPJ, ref empresaCNPJs, ref nomeEmpresa, ref index);
                    if (status == 0)
                        break;
                    else if (status == -2 || status == -3)
                        continue;
                }
                while (true) //Menu inicial
                {
                    MenuInicial();
                    int opcaoEscolhida;
                    opcaoEscolhida = Check_input(Console.ReadLine());
                    if (opcaoEscolhida == -1)
                    {
                        Console.WriteLine("Opção inválida. Por favor, escolha uma das opções abaixo:");
                        continue;
                    }
                    else if (opcaoEscolhida == -2 || opcaoEscolhida == -5)
                        break;
                    else if (opcaoEscolhida == -3)
                        continue;
                    else if (opcaoEscolhida == -4)
                        return;
                    else if (opcaoEscolhida == 1) //Cadastro de nota para o mês vigente
                    {
                        int status = SolicitarCNPJ(ref inputCNPJ, "cliente"); //Solicita CNPJ do Cliente
                        if (status == -2)
                            break;
                        else if (status == -3)
                            continue;
                        else if (status == -4)
                            return;
                        numeroCNPJCliente = NumeroCNPJ(inputCNPJ);
                        status = VerificarCadastro(numeroCNPJCliente, ref cadastroClienteCNPJ, ref cadastroNomeCliente, ref cadastroIndexCliente); //Solicita nome do cliente caso não esteja cadastrado
                        if (status == -2)
                            break;
                        else if (status == -3)
                            continue;
                        else if (status == -4)
                            return;
                        else if (status == 0)
                        {
                            int answerStatus = -1;
                            EmitirNota(ref answerStatus); //Verifica se o usuário deseja emitir NF
                            if (answerStatus == -2)
                                break;
                            else if (answerStatus == 0)
                            {
                                RegistrarNota(ref nfValor, ref nfMes, ref nfAno, ref nfNome, ref nfOrigemCNPJ, numeroCNPJ, ref nfClienteCNPJ, cadastroClienteCNPJ, cadastroNomeCliente, cadastroIndexCliente, 0); //Registra a NF na base
                                continue;
                            }
                            else
                                break;
                        }
                    }
                    else if (opcaoEscolhida == 2) //Registra NF para meses passados (apenas durante o ano vigente)
                    {
                        int status = SolicitarCNPJ(ref inputCNPJ, "cliente");
                        if (status == -2)
                            break;
                        else if (status == -3)
                            continue;
                        else if (status == -4)
                            return;
                        numeroCNPJCliente = NumeroCNPJ(inputCNPJ);
                        status = VerificarCadastro(numeroCNPJCliente, ref cadastroClienteCNPJ, ref cadastroNomeCliente, ref cadastroIndexCliente); //Solicita nome do cliente caso não esteja cadastrado
                        if (status == -2)
                            break;
                        else if (status == -3)
                            continue;
                        else if (status == -4)
                            return;
                        int mesEscolhido;
                        Console.WriteLine();
                        while (true)
                        {
                            MenuPadrao();
                            Console.Write("Informe o mês da emissão (01 à 12): ");
                            mesEscolhido = Checar_mes(Console.ReadLine().ToUpper()); //Verifica se o usuárioinformou um mês anterior ao corrente
                            if (mesEscolhido == -1)
                            {
                                Console.WriteLine("Por favor, escolha uma das opções abaixo:");
                                continue;
                            }
                            else if (mesEscolhido == -3)
                                continue;
                            else if (mesEscolhido == -4)
                                return;
                            else
                                break;
                        }
                        if (mesEscolhido == -2)
                            break;
                        RegistrarNota(ref nfValor, ref nfMes, ref nfAno, ref nfNome, ref nfOrigemCNPJ, numeroCNPJ, ref nfClienteCNPJ, cadastroClienteCNPJ, cadastroNomeCliente, cadastroIndexCliente, mesEscolhido); //Registra a NF na base
                    }
                    else if (opcaoEscolhida == 3)
                    {
                        int answerStatus;
                        while (true)
                        {
                            Console.WriteLine("Por favor, escolha uma das opções abaixo:");
                            Console.WriteLine("1 - Consultar por Cliente");
                            Console.WriteLine("2 - Consultar por Mês");
                            MenuPadrao();
                            Console.Write("Opção escolhida: ");
                            answerStatus = Check_input(Console.ReadLine().ToUpper());
                            if (answerStatus == -1)
                            {
                                Console.WriteLine("Por favor, escolha uma das opções abaixo:");
                                continue;
                            }
                            else if (answerStatus == -2 || answerStatus == 1 || answerStatus == 2)
                                break;
                            else if (answerStatus == -3)
                                continue;
                            else if (answerStatus == -4)
                                return;
                            else
                                continue;
                        }
                        if (answerStatus == -2)
                            break;
                        else if (answerStatus == 1) //Busca NFs do CNPJ solicitado && emitidas pelo CNPJ logado
                        {
                            Console.WriteLine();
                            Console.WriteLine("========================================================");
                            SolicitarCNPJ(ref inputCNPJ, "cliente");
                            Console.WriteLine();
                            numeroCNPJCliente = NumeroCNPJ(inputCNPJ);
                            bool cabecalho = false;

                            int indexSearch = 0;
                            while (nfClienteCNPJ[indexSearch] != 0) //Imprime tabela
                            {
                                if (nfClienteCNPJ[indexSearch] == numeroCNPJCliente && nfOrigemCNPJ[indexSearch] == numeroCNPJ)
                                {
                                    if (cabecalho == false)
                                    {
                                        for (int i = 0; i < 39; i++)
                                            Console.Write("_");
                                        Console.WriteLine();
                                        Console.Write("|Mês da emissão");
                                        for (int i = 0; i < (39 - 15 - 6); i++)
                                            Console.Write(" ");
                                        Console.WriteLine("Valor|");
                                        cabecalho = true;
                                    }
                                    Console.Write($"|{nfMes[indexSearch]}/{nfAno[indexSearch]}");
                                    int lenght = (nfValor[indexSearch].ToString("N2").Length);
                                    for (int i = 0; i < (37 - 7 - lenght - 3); i++)
                                        Console.Write(" ");
                                    Console.WriteLine($"R$ {nfValor[indexSearch].ToString("N2")}|");
                                }
                                indexSearch++;
                            }
                            if (cabecalho == false)
                            {
                                Console.WriteLine("Cliente não encontrado");
                                continue;
                            }
                            for (int i = 0; i < 39; i++)
                                Console.Write("-");
                            Console.WriteLine();
                        }
                        else if (answerStatus == 2) //Busca NFs do Mes solicitado && emitidas pelo CNPJ logado
                        {
                            Console.WriteLine();
                            Console.WriteLine("========================================================");
                            string inputMes;
                            int mesStatus;

                            while (true)
                            {
                                MenuPadrao();
                                Console.Write("Informe o mês da emissão (01 à 12) ou uma das opções acima: ");
                                inputMes = Console.ReadLine().ToUpper();
                                mesStatus = Checar_mes(inputMes);
                                if (mesStatus == -1)
                                {
                                    Console.WriteLine("Por favor, escolha uma das opções abaixo:");
                                    continue;
                                }
                                else if (mesStatus == -2)
                                    break;
                                else if (mesStatus == -3)
                                    continue;
                                else if (mesStatus == -4)
                                    return;
                                else
                                    break;
                            }
                            Console.WriteLine();
                            bool cabecalho = false;
                            int indexSearch = 0;
                            string mesBuscado = mesStatus.ToString("00");

                            while (nfClienteCNPJ[indexSearch] != 0)//Imprime tabela
                            {
                                if (nfMes[indexSearch] == mesBuscado && nfOrigemCNPJ[indexSearch] == numeroCNPJ)
                                {
                                    if (cabecalho == false)
                                    {
                                        for (int i = 0; i < 59; i++)
                                            Console.Write("_");
                                        Console.WriteLine();
                                        Console.Write("|Cliente");
                                        for (int i = 0; i < (60 - 8 - 14 - 6) / 2; i++)
                                            Console.Write(" ");
                                        Console.Write("Mês da emissão");
                                        for (int i = 0; i < (59 - 8 - 14 - 6) / 2; i++)
                                            Console.Write(" ");
                                        Console.WriteLine("Valor|");
                                        cabecalho = true;
                                    }
                                    int lenghtCliente = (nfNome[indexSearch].ToString().Length);
                                    int lenghtValor = (nfValor[indexSearch].ToString("N2").Length);

                                    Console.Write($"|{nfNome[indexSearch]}");
                                    for (int i = 0; i < (58 - 7 - lenghtCliente - lenghtValor) / 2; i++)
                                        Console.Write(" ");
                                    Console.Write($"{nfMes[indexSearch]}/{nfAno[indexSearch]}");
                                    for (int i = 0; i < ((58 - 7 - lenghtCliente - lenghtValor) / 2) - 3; i++)
                                        Console.Write(" ");
                                    Console.WriteLine($"R$ {nfValor[indexSearch].ToString("N2")}|");
                                }
                                indexSearch++;
                            }
                            if (cabecalho == false)
                            {
                                Console.WriteLine("Nenhuma NF encontrada para o período");
                                continue;
                            }
                            for (int i = 0; i < 59; i++)
                                Console.Write("-");
                            Console.WriteLine();
                        }
                    }
                }
                Console.WriteLine($"Conta do CNPJ {numeroCNPJ.ToString(@"00\.000\.000\/0000\-00")} deslogado.");
                Console.WriteLine("========================================================");
            }
        }
        static void EmitirNota(ref int answerStatus)
        {
            while (true)
            {
                MenuPadrao();
                Console.Write("Deseja emitir nota? Sim/Nao: ");
                answerStatus = Check_yes_no(Console.ReadLine().ToUpper());
                if (answerStatus == -1)
                    Console.WriteLine("Por favor, digite uma resposta válida.");
                else if (answerStatus == -2)
                    break;
                else if (answerStatus == -3)
                    continue;
                else if (answerStatus == -4)
                    return;
                else
                    break;
            }
        }

        static void RegistrarNota(ref double[] nfValor, ref string[] nfMes, ref string[] nfAno, ref string[] nfNome, ref long[] nfOrigemCNPJ, long numeroCNPJ,
            ref long[] nfClienteCNPJ, long[] cadastroClienteCNPJ, string[] cadastroNomeCliente, int cadastroIndexCliente, int mesEscolhido)
        {
            int nfIndex = 0;
            while (nfValor[nfIndex] != 0)
                nfIndex++;
            Console.Write("Informe o valor da Nota Fiscal: R$ ");
            nfValor[nfIndex] = ConvertToDouble(Console.ReadLine());
            while (nfValor[nfIndex] <= 0)
            {
                Console.WriteLine("Valor inválido.");
                Console.Write("Informe o valor da Nota Fiscal: R$ ");
                nfValor[nfIndex] = ConvertToDouble(Console.ReadLine());
            }
            if (mesEscolhido == 0)
            {
                nfMes[nfIndex] = DateTime.Now.ToString("MM");
                nfAno[nfIndex] = DateTime.Now.ToString("yyyy");
            }
            else
            {
                nfMes[nfIndex] = mesEscolhido.ToString("00");
                nfAno[nfIndex] = DateTime.Now.ToString("yyyy");
            }
            nfOrigemCNPJ[nfIndex] = numeroCNPJ;
            nfClienteCNPJ[nfIndex] = cadastroClienteCNPJ[cadastroIndexCliente];
            nfNome[nfIndex] = GetNomeCliente(nfClienteCNPJ[nfIndex], cadastroClienteCNPJ, cadastroNomeCliente);
            CalcularImposto(nfValor[nfIndex]);
            Console.ReadLine();
        }

        static void MenuInicial()
        {
            Console.WriteLine();
            Console.WriteLine("======================MENU INICIAL======================");
            Console.WriteLine("1 - Emitir Nova Nota fiscal");
            Console.WriteLine("2 - Cadastrar notas anteriores");
            Console.WriteLine("3 - Consultar notas anteriores");
            MenuPadrao();
            Console.WriteLine("SAIR - Deslogar usuário atual");
            Console.WriteLine();
            Console.WriteLine("========================================================");
            Console.Write("Opção escolhida: ");
        }

        static void MenuPadrao()
        {
            Console.WriteLine("Cancelar - Voltar para o menu anterior");
            Console.WriteLine("Voltar - Voltar apenas um passo");
            Console.WriteLine("Encerrar - Finalizar o programa");
        }

        static void CalcularImposto(double valorNF)
        {
            double RBT12;
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Por favor, informe o valor do seu faturamento nos últimos 12 meses.");
                Console.WriteLine("Serão aceitos somente valores entre R$ 0,01 à R$ 4.800.000,00.");
                Console.Write("Faturamento nos últimos 12 meses: R$ ");
                RBT12 = ConvertToDouble(Console.ReadLine());
                Console.WriteLine();
                if (RBT12 == -2)
                    continue;
                else if (RBT12 == -3)
                    return;
                else if (RBT12 == -1 || RBT12 < 0.01 || RBT12 > 4800000)
                    Console.WriteLine("Por favor, digite um valor de faturamento que esteja dentro dos limites descritos acima.");
                else
                    break;
            }
            int qtdFaixas = 6;

            //                        0       1    2
            //                     RBT12    ALIQ  VAD
            double[,] tabelaA = { { 180000, 0.06, 0 },
                                  { 360000, 0.112, 9360 },
                                  { 720000, 0.135 , 17640},
                                  { 1800000, 0.16 , 35640},
                                  { 3600000, 0.21, 125640 },
                                  { 4800000, 0.33, 648000 } };

            double ALIQ, VAD, VB, VD, ALIQF, VAL;

            VAL = 0;
            ALIQF = 0;

            for (int i = 0; i < qtdFaixas; i++)
            {
                if (RBT12 < tabelaA[i, 0])
                {
                    ALIQ = tabelaA[i, 1];
                    VAD = tabelaA[i, 2];
                    VB = Math.Round(RBT12 * ALIQ, 2);
                    VD = VB - VAD;
                    ALIQF = VD / RBT12;
                    VAL = Math.Round(valorNF * ALIQF, 2);
                    break;
                }
            }

            //                        0     1      2      3        4       5      6
            //                     RTB12   IRPJ  CSLL   COFINS  PIS/PASEP CPP    ISS
            double[,] tabelaR = { {180000, 0.04, 0.035, 0.1282, 0.0278, 0.434, 0.335 },
                                  {360000, 0.04, 0.035, 0.1405, 0.0305, 0.434, 0.32 },
                                  {720000, 0.04, 0.035, 0.1364, 0.0296, 0.434, 0.325 },
                                  {1800000, 0.04, 0.035, 0.1364, 0.0296, 0.434, 0.325 },
                                  {3600000, 0.04, 0.035, 0.1282, 0.0278, 0.434, 0.335 },
                                  {4800000, 0.35, 0.15, 0.1603, 0.0347, 0.305, 0 } };

            Console.WriteLine("================REPARTIÇÃO DOS TRIBUTOS=================");
            for (int i = 0; i < qtdFaixas; i++)
            {
                if (RBT12 < tabelaR[i, 0] && i == 4 && ALIQF > 0.1492537) //Condição específica para faixa 5 onde a Aliquota efetiva for maior que 14,92537%
                {
                    double diferenca = VAL * (1 - 0.05 - tabelaR[i, 1] - tabelaR[i, 2] - tabelaR[i, 3] - tabelaR[i, 4] - tabelaR[i, 5]);

                    Console.WriteLine($"IRPJ: R$ {String.Format("{0:0.00}", Math.Round(tabelaR[i, 1] * VAL + (diferenca * 0.0602), 2))}");
                    Console.WriteLine($"CSLL: R$ {String.Format("{0:0.00}", Math.Round(tabelaR[i, 2] * VAL + (diferenca * 0.0526), 2))}");
                    Console.WriteLine($"COFINS: R$ {String.Format("{0:0.00}", Math.Round(tabelaR[i, 3] * VAL + (diferenca * 0.1928), 2))}");
                    Console.WriteLine($"PIS: R$ {String.Format("{0:0.00}", Math.Round(tabelaR[i, 4] * VAL + (diferenca * 0.0418), 2))}");
                    Console.WriteLine($"INSS: R$ {String.Format("{0:0.00}", Math.Round(tabelaR[i, 5] * VAL + (diferenca * 0.6526), 2))}");
                    Console.WriteLine($"ISS: R$ {String.Format("{0:0.00}", Math.Round(0.05 * (VAL), 2))}");
                    Console.WriteLine($"VALOR TOTAL: R$ {String.Format("{0:0.00}", VAL)}");
                    Console.Write("========================================================");
                    break;
                }
                if (RBT12 < tabelaR[i, 0])
                {
                    Console.WriteLine($"IRPJ: R$ {String.Format("{0:0.00}", Math.Round(tabelaR[i, 1] * VAL, 2))}");
                    Console.WriteLine($"CSLL: R$ {String.Format("{0:0.00}", Math.Round(tabelaR[i, 2] * VAL, 2))}");
                    Console.WriteLine($"COFINS: R$ {String.Format("{0:0.00}", Math.Round(tabelaR[i, 3] * VAL, 2))}");
                    Console.WriteLine($"PIS: R$ {String.Format("{0:0.00}", Math.Round(tabelaR[i, 4] * VAL, 2))}");
                    Console.WriteLine($"INSS: R$ {String.Format("{0:0.00}", Math.Round(tabelaR[i, 5] * VAL, 2))}");
                    Console.WriteLine($"ISS: R$ {String.Format("{0:0.00}", Math.Round(tabelaR[i, 6] * VAL, 2))}");
                    Console.WriteLine($"VALOR TOTAL: R$ {String.Format("{0:0.00}", VAL)}");
                    Console.Write("========================================================");
                    break;
                }
            }

        }
        static string GetNomeCliente(long nfClienteCNPJ, long[] cadastroClienteCNPJ, string[] cadastroNomeCliente)
        {
            int index = 0;

            foreach (long CNPJ in cadastroClienteCNPJ)
            {
                if (CNPJ == nfClienteCNPJ)
                    return (cadastroNomeCliente[index]);
                index++;
            }
            return "";
        }

        static int Checar_mes(string value)
        {
            if (value.ToUpper() == "CANCELAR")
                return -2;
            else if (value.ToUpper() == "VOLTAR")
                return -3;
            else if (value.ToUpper() == "ENCERRAR")
                return -4;


            int mesAtual = Convert.ToInt32(DateTime.Now.ToString("MM"));
            int valueMes = Check_input(value);
            if (valueMes <= 0 || valueMes > 12)
            {
                Console.Write("Digite um valor de 1 à 12 que representam os meses de janeiro à dezembro. ");
                return -1;
            }
            if (valueMes <= mesAtual)
                return valueMes;
            else
            {
                Console.Write("Não é possível cadastrar notas com data futura. ");
                return -1;
            }
        }

        static int Check_yes_no(string value)
        {
            if (value.ToUpper() == "CANCELAR")
                return -2;
            else if (value.ToUpper() == "VOLTAR")
                return -3;
            else if (value.ToUpper() == "SIM")
                return 0;
            else if (value.ToUpper() == "NAO")
                return 1;
            else
                return -1;
        }

        #region converter input para opção do usuário
        static int Check_input(string value)
        {
            if (value.ToUpper() == "CANCELAR")
                return -2;
            else if (value.ToUpper() == "VOLTAR")
                return -3;
            else if (value.ToUpper() == "ENCERRAR")
                return -4;
            else if (value.ToUpper() == "SAIR")
                return -5;
            try
            {
                int result = Convert.ToInt32(value);
                return result;
            }
            catch (OverflowException)
            {
                return -1;
            }
            catch (FormatException)
            {
                return -1;
            }
        }
        #endregion
        #region solicitar CNPJ usuario e cliente
        static int SolicitarCNPJ(ref string inputCNPJ, string proprietario)
        {
            if (proprietario == "cliente")
                Console.Write("Digite o CNPJ do cliente: ");
            else
                Console.Write("Digite seu CNPJ: ");
            while (true)
            {
                inputCNPJ = Console.ReadLine();
                if (inputCNPJ == null)
                    continue;
                if (inputCNPJ.ToUpper() == "CANCELAR")
                    return -2;
                else if (inputCNPJ.ToUpper() == "VOLTAR")
                    return -3;
                else if (inputCNPJ.ToUpper() == "ENCERRAR")
                    return -4;
                bool isValid = ValidaCNPJ(inputCNPJ);
                if (isValid)
                    break;
                Console.WriteLine("CNPJ inválido");
                if (proprietario == "cliente")
                    Console.WriteLine("Digite o CNPJ do cliente (Somente números): ");
                else
                    Console.WriteLine("Digite seu CNPJ (Somente números)");

            }
            return 0;
        }
        #endregion
        #region converter input de CNPJ
        static long NumeroCNPJ(string vrCNPJ)
        {
            string CNPJ = vrCNPJ.Replace(".", "");
            CNPJ = CNPJ.Replace("/", "");
            CNPJ = CNPJ.Replace("-", "");
            return ConvertToLong(CNPJ);
        }
        #endregion
        #region verificadastro
        static int VerificarCadastro(long numeroCNPJ, ref long[] empresaCNPJs, ref string[] nomeEmpresa, ref int index)
        {
            foreach (long CNPJ in empresaCNPJs)
            {
                if (CNPJ == numeroCNPJ)
                    return 0;
                else if (CNPJ == 0)
                    break;
                index++;
            }
            empresaCNPJs[index] = numeroCNPJ;
            MenuPadrao();
            Console.WriteLine("Qual o nome da empresa?");
            Console.Write("Digite o nome da empresa ou uma das opções: ");
            while (true)
            {
                string inputNomeDaEmpresa = Console.ReadLine().ToUpper();
                if (inputNomeDaEmpresa == "CANCELAR")
                {
                    empresaCNPJs[index] = 0;
                    return -2;
                }
                else if (inputNomeDaEmpresa == "VOLTAR")
                {
                    empresaCNPJs[index] = 0;
                    return -3;
                }
                else if (inputNomeDaEmpresa.Length < 10)
                {
                    Console.WriteLine("Nome inválido. Por favor, verifique se o nome está correto (Deve ser composto por ao menos 10 caracteres)");
                    Console.Write("Digite o nome da empresa: ");
                    continue;
                }
                nomeEmpresa[index] = inputNomeDaEmpresa;
                break;
            }
            return 0;
        }
        #endregion
        #region validador de CNPJ
        static bool ValidaCNPJ(string vrCNPJ)
        {
            string CNPJ = vrCNPJ.Replace(".", "");
            CNPJ = CNPJ.Replace("/", "");
            CNPJ = CNPJ.Replace("-", "");
            int[] digitos, soma, resultado;
            int nrDig;
            string ftmt;
            bool[] CNPJOk;
            ftmt = "6543298765432";
            digitos = new int[14];
            soma = new int[2];
            soma[0] = 0;
            soma[1] = 0;
            resultado = new int[2];
            resultado[0] = 0;
            resultado[1] = 0;
            CNPJOk = new bool[2];
            CNPJOk[0] = false;
            CNPJOk[1] = false;
            try
            {
                for (nrDig = 0; nrDig < 14; nrDig++)
                {
                    digitos[nrDig] = int.Parse(CNPJ.Substring(nrDig, 1));
                    if (nrDig <= 11) //first check digit
                        soma[0] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig + 1, 1)));
                    if (nrDig <= 12)//second check digit
                        soma[1] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig, 1)));
                }
                for (nrDig = 0; nrDig < 2; nrDig++) //resultado[0] will be the first check digit and resultado[1] the second check digit
                {
                    resultado[nrDig] = (soma[nrDig] % 11);
                    if ((resultado[nrDig] == 0) || (resultado[nrDig] == 1))
                        CNPJOk[nrDig] = (digitos[12 + nrDig] == 0);
                    else
                        CNPJOk[nrDig] = (digitos[12 + nrDig] == (11 - resultado[nrDig]));
                }
                return (CNPJOk[0] && CNPJOk[1]);
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region conversor de string para long
        static long ConvertToLong(string value)
        {
            if (value.ToUpper() == "CANCELAR")
                return -2;
            else if (value.ToUpper() == "VOLTAR")
                return -3;
            else if (value.ToUpper() == "ENCERRAR")
                return -4;
            try
            {
                long result = Convert.ToInt64(value);
                return result;
            }
            catch (OverflowException)
            {
                return -1;
            }
            catch (FormatException)
            {
                return -1;
            }
        }
        #endregion
        #region conversor de string para double
        static double ConvertToDouble(string value)
        {
            if (value.ToUpper() == "CANCELAR")
                return -2;
            else if (value.ToUpper() == "VOLTAR")
                return -3;
            else if (value.ToUpper() == "ENCERRAR")
                return -4;
            try
            {
                double result = Convert.ToDouble(value);
                return result;
            }
            catch (OverflowException)
            {
                return -1;
            }
            catch (FormatException)
            {
                return -1;
            }
        }
        #endregion
    }
}