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
            double[] RBT12 = new double[50];
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

        Login:
            while (true)
            {
                switch (SolicitarCNPJ(ref inputCNPJ, ""))
                {
                    case -2:
                    case -3:
                    case -5: continue;
                    case -4: return;
                    default:
                        break;
                }
                numeroCNPJ = NumeroCNPJ(inputCNPJ);
                switch (VerificarCadastro(numeroCNPJ, ref empresaCNPJs, ref nomeEmpresa, ref index, "Empresa"))
                {
                    case -2:
                    case -3: continue;
                    case -4: return;
                    case -5:
                        {
                            Console.WriteLine($"Conta do CNPJ {numeroCNPJ.ToString(@"00\.000\.000\/0000\-00")} deslogado.");
                            Console.WriteLine("========================================================");
                            goto Login;
                        }
                    default:
                        break;
                }
                while (true)
                {
                MenuInicial:
                    MenuInicial();
                    int opcaoMenuPrincipal;
                    opcaoMenuPrincipal = Check_input(Console.ReadLine());
                    if (opcaoMenuPrincipal == -1)
                    {
                        Console.WriteLine("Opção inválida. Por favor, escolha uma das opções abaixo:");
                        continue;
                    }
                    else if (opcaoMenuPrincipal == -2 || opcaoMenuPrincipal == -3)
                        continue;
                    else if (opcaoMenuPrincipal == -4)
                        return;
                    else if (opcaoMenuPrincipal == -5)
                    {
                        Console.WriteLine($"Conta do CNPJ {numeroCNPJ.ToString(@"00\.000\.000\/0000\-00")} deslogado.");
                        Console.WriteLine("========================================================");
                        goto Login;
                    }
                    else if (opcaoMenuPrincipal == 1 || opcaoMenuPrincipal == 2)
                    {
                    SolicitarValorNF:
                        double valorNF = 0;
                        switch (SolicitarValorNF(ref valorNF))
                        {
                            case -2:
                            case -3: continue;
                            case -4: return;
                            case -5:
                                {
                                    Console.WriteLine($"Conta do CNPJ {numeroCNPJ.ToString(@"00\.000\.000\/0000\-00")} deslogado.");
                                    Console.WriteLine("========================================================");
                                    goto Login;
                                }
                            default:
                                break;
                        }
                    SolicitarCNPJCliente:
                        string clienteNFInput = "";

                        switch (SolicitarCNPJ(ref clienteNFInput, "cliente"))
                        {
                            case -2: goto MenuInicial;
                            case -3: goto SolicitarValorNF;
                            case -4: return;
                            case -5:
                                {
                                    Console.WriteLine($"Conta do CNPJ {numeroCNPJ.ToString(@"00\.000\.000\/0000\-00")} deslogado.");
                                    Console.WriteLine("========================================================");
                                    goto Login;
                                }
                            default:
                                break;
                        }
                        numeroCNPJCliente = NumeroCNPJ(inputCNPJ);
                    SolicitarNomeCliente:
                        switch (VerificarCadastro(numeroCNPJCliente, ref cadastroClienteCNPJ, ref cadastroNomeCliente, ref cadastroIndexCliente, "cliente"))//Solicita nome do cliente caso não esteja cadastrado
                        {
                            case -2: goto MenuInicial;
                            case -3: goto SolicitarCNPJCliente;
                            case -4: return;
                            case -5:
                                {
                                    Console.WriteLine($"Conta do CNPJ {numeroCNPJ.ToString(@"00\.000\.000\/0000\-00")} deslogado.");
                                    Console.WriteLine("========================================================");
                                    goto Login;
                                }
                            default:
                                break;
                        }
                    SolicitarMesEmissao:
                        int mesEscolhido = 0;
                        int anoEscolhido = 0;
                        if (opcaoMenuPrincipal == 2)
                        {
                            Console.WriteLine();
                            switch (Solicitar_mes(ref mesEscolhido)) //Verifica se o usuário informou um mês anterior ao corrente
                            {
                                case -1: continue;
                                case -2: goto MenuInicial;
                                case -3: goto SolicitarNomeCliente;
                                case -4: return;
                                case -5:
                                    {
                                        Console.WriteLine($"Conta do CNPJ {numeroCNPJ.ToString(@"00\.000\.000\/0000\-00")} deslogado.");
                                        Console.WriteLine("========================================================");
                                        goto Login;
                                    }
                                default: break;
                            }
                        SolicitarAnoEmissao:
                            switch (Solicitar_ano(ref anoEscolhido)) //Verifica se o usuário informou um mês anterior ao corrente
                            {
                                case -1: continue;
                                case -2: goto MenuInicial;
                                case -3: goto SolicitarMesEmissao;
                                case -4: return;
                                case -5:
                                    {
                                        Console.WriteLine($"Conta do CNPJ {numeroCNPJ.ToString(@"00\.000\.000\/0000\-00")} deslogado.");
                                        Console.WriteLine("========================================================");
                                        goto Login;
                                    }
                                default: break;
                            }
                            int mesAtual = Convert.ToInt32(DateTime.Now.ToString("MM"));
                            int anoAtual = Convert.ToInt32(DateTime.Now.ToString("yyyy"));


                            if (mesEscolhido > mesAtual && anoEscolhido >= anoAtual || anoEscolhido > anoAtual)
                            {
                                Console.Write("Não é possível cadastrar notas com data futura. ");
                                goto SolicitarMesEmissao;
                            }
                        }
                    EmitirNota:
                        switch (EmitirNota())
                        {
                            case 1:
                            case -2:
                            case -3: goto MenuInicial;
                            case -4: return;
                            case -5:
                                {
                                    Console.WriteLine($"Conta do CNPJ {numeroCNPJ.ToString(@"00\.000\.000\/0000\-00")} deslogado.");
                                    Console.WriteLine("========================================================");
                                    goto Login;
                                }
                            default: break;
                        }
                    RegistrarNF:
                        int nfIndex = 0;
                        RegistrarNota(ref nfValor, ref nfMes, ref nfAno, ref nfNome, ref nfOrigemCNPJ, ref nfIndex, numeroCNPJ, ref nfClienteCNPJ, cadastroClienteCNPJ, cadastroNomeCliente, cadastroIndexCliente, valorNF, mesEscolhido, anoEscolhido);
                        CalcularRBT12(ref RBT12[index], ref nfValor, ref nfMes, ref nfAno, ref nfOrigemCNPJ, ref nfIndex, numeroCNPJ, mesEscolhido, anoEscolhido);
                        if (opcaoMenuPrincipal == 1)
                            CalcularImposto(nfValor[nfIndex], RBT12[index]);
                    }
                    else if (opcaoMenuPrincipal == 3)
                    {
                    OpcaoConsulta:
                        int opcaoSubMenu = 0;
                        OpcaoConsulta(ref opcaoSubMenu);
                        if (opcaoSubMenu == -2 || opcaoSubMenu == -3)
                            goto MenuInicial;
                        else if (opcaoSubMenu == -4)
                            return;
                        else if (opcaoSubMenu == -5)
                        {
                            Console.WriteLine($"Conta do CNPJ {numeroCNPJ.ToString(@"00\.000\.000\/0000\-00")} deslogado.");
                            Console.WriteLine("========================================================");
                            goto Login;
                        }
                        else if (opcaoSubMenu != 1 && opcaoSubMenu != 2)
                        {
                            Console.WriteLine("Opção inválida.");
                            goto OpcaoConsulta;
                        }
                        else if (opcaoSubMenu == 1) //Busca NFs do CNPJ solicitado && emitidas pelo CNPJ logado
                        {
                        SolicitarCNPJ:
                            Console.WriteLine();
                            Console.WriteLine("========================================================");
                            int status = SolicitarCNPJ(ref inputCNPJ, "cliente");
                            if (status == -1)
                                goto SolicitarCNPJ;
                            else if (status == -2)
                                goto MenuInicial;
                            else if (status == -3)
                                goto OpcaoConsulta;
                            else if (status == -4)
                                return;
                            else if (status == -5)
                            {
                                Console.WriteLine($"Conta do CNPJ {numeroCNPJ.ToString(@"00\.000\.000\/0000\-00")} deslogado.");
                                Console.WriteLine("========================================================");
                                goto Login;
                            }
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
                                        ImprimirClienteMes(nfValor, nfMes, nfAno, nfOrigemCNPJ, numeroCNPJ, nfClienteCNPJ, numeroCNPJCliente);
                                        for (int i = 0; i < 39; i++)
                                            Console.Write("-");
                                        Console.WriteLine();
                                        goto MenuInicial;
                                    }
                                }
                                indexSearch++;
                            }
                            if (cabecalho == false)
                            {
                                Console.WriteLine("Sem NFs cadastradas para o cliente informado.");
                                goto MenuInicial;
                            }
                        }
                        else if (opcaoSubMenu == 2)
                        {
                            int inputMes = 0;
                            int mesStatus;
                            while (true)
                            {
                                mesStatus = Solicitar_mes(ref inputMes);
                                if (mesStatus == -1)
                                {
                                    Console.WriteLine("Por favor, escolha uma das opções abaixo:");
                                    continue;
                                }
                                else if (mesStatus == -2)
                                    goto MenuInicial;
                                else if (mesStatus == -3)
                                    goto OpcaoConsulta;
                                else if (mesStatus == -4)
                                    return;
                                else if (mesStatus == -5)
                                {
                                    Console.WriteLine($"Conta do CNPJ {numeroCNPJ.ToString(@"00\.000\.000\/0000\-00")} deslogado.");
                                    Console.WriteLine("========================================================");
                                    goto Login;
                                }
                                else
                                    break;
                            }
                            Console.WriteLine();
                            bool cabecalho = false;
                            int indexSearch = 0;

                            while (nfClienteCNPJ[indexSearch] != 0)
                            {
                                if (Convert.ToInt32(nfMes[indexSearch]) == inputMes && nfOrigemCNPJ[indexSearch] == numeroCNPJ)
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
                                        ImprimirMes(nfValor, nfMes, nfAno, nfOrigemCNPJ, numeroCNPJ, nfClienteCNPJ, nfNome, inputMes);
                                        for (int i = 0; i < 59; i++)
                                            Console.Write("-");
                                        Console.WriteLine();
                                        goto MenuInicial;
                                    }
                                }
                                indexSearch++;
                            }
                            if (cabecalho == false)
                            {
                                Console.WriteLine("Nenhuma NF encontrada para o mês informado.");
                                goto MenuInicial;
                            }
                        }
                    }
                }
                Console.WriteLine($"Conta do CNPJ {numeroCNPJ.ToString(@"00\.000\.000\/0000\-00")} deslogado.");
                Console.WriteLine("========================================================");
            }
        }

        static int SolicitarValorNF(ref double valorNF)
        {
            string input;
            while (true)
            {
                Console.Write("Informe o valor da Nota Fiscal: R$ ");
                input = Console.ReadLine().ToUpper();
                if (input == "CANCELAR")
                    return -2;
                else if (input == "VOLTAR")
                    return -3;
                else if (input == "ENCERRAR")
                    return -4;
                else if (input == "SAIR")
                    return -5;
                else
                    valorNF = ConvertToDouble(input);
                if (valorNF <= 0)
                    Console.WriteLine("Valor inválido.");
                else
                    break;
            }
            return 0;
        }
        static int EmitirNota()
        {
            int answerStatus;
            while (true)
            {
                Console.WriteLine("========================================================");
                //MenuPadrao();
                Console.Write("Deseja emitir nota? Sim/Nao: ");
                answerStatus = Check_yes_no(Console.ReadLine().ToUpper());
                if (answerStatus == -1)
                    Console.WriteLine("Por favor, digite uma resposta válida.");
                else
                    return answerStatus;
            }
        }
        static void CalcularRBT12(ref double RBT12, ref double[] nfValor, ref string[] nfMes, ref string[] nfAno, ref long[] nfOrigemCNPJ, ref int nfIndex, long numeroCNPJ, int mesEscolhido, int anoEscolhido)
        {
            if (mesEscolhido == 0 && anoEscolhido == 0 || nfOrigemCNPJ[nfIndex] != numeroCNPJ)
                return;
            int mesAtual = Convert.ToInt32(DateTime.Now.ToString("MM"));
            int anoAtual = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            int nfMesInt = Convert.ToInt32(nfMes[nfIndex]);
            int nfAnoInt = Convert.ToInt32(nfAno[nfIndex]);
            if ((nfAnoInt == anoAtual && nfMesInt < mesAtual) || (nfAnoInt == anoAtual - 1 && nfMesInt >= mesAtual))
                RBT12 += nfValor[nfIndex];
        }
        static void RegistrarNota(ref double[] nfValor, ref string[] nfMes, ref string[] nfAno, ref string[] nfNome, ref long[] nfOrigemCNPJ, ref int nfIndex, long numeroCNPJ,
            ref long[] nfClienteCNPJ, long[] cadastroClienteCNPJ, string[] cadastroNomeCliente, int cadastroIndexCliente, double valorNF, int mesEscolhido, int anoEscolhido)
        {
            while (nfValor[nfIndex] != 0)
                nfIndex++;

            nfValor[nfIndex] = valorNF;//valorNF variavel

            if (mesEscolhido == 0 && anoEscolhido == 0)
            {
                nfMes[nfIndex] = DateTime.Now.ToString("MM");
                nfAno[nfIndex] = DateTime.Now.ToString("yyyy");
            }
            else
            {
                nfMes[nfIndex] = mesEscolhido.ToString("00");
                nfAno[nfIndex] = anoEscolhido.ToString("0000");
            }
            nfOrigemCNPJ[nfIndex] = numeroCNPJ;
            nfClienteCNPJ[nfIndex] = cadastroClienteCNPJ[cadastroIndexCliente];
            nfNome[nfIndex] = GetNomeCliente(nfClienteCNPJ[nfIndex], cadastroClienteCNPJ, cadastroNomeCliente);
        }

        static void MenuInicial()
        {
            Console.WriteLine();
            Console.WriteLine("======================MENU INICIAL======================");
            Console.WriteLine("1 - Emitir Nova Nota fiscal");
            Console.WriteLine("2 - Cadastrar notas anteriores");
            Console.WriteLine("3 - Consultar notas anteriores");
            //MenuPadrao();
            Console.WriteLine();
            Console.WriteLine("========================================================");
            Console.Write("Opção escolhida: ");
        }

        static void MenuPadrao()
        {
            Console.WriteLine("Cancelar - Voltar para o menu anterior");
            Console.WriteLine("Voltar - Voltar apenas um passo");
            Console.WriteLine("Encerrar - Finalizar o programa");
            Console.WriteLine("SAIR - Deslogar usuário atual");
        }

        static int CalcularImposto(double valorNF, double RBT12)
        {
            if (RBT12 < 0 || RBT12 > 4800000)
                Console.WriteLine("Por favor, o total de faturamento dos últimos 12 meses nào está entre R$ 0 à 4.800.000.");
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
                if (RBT12 < tabelaA[i, 0] && RBT12 > 180000)
                {
                    ALIQ = tabelaA[i, 1];
                    VAD = tabelaA[i, 2];
                    VB = Math.Round(RBT12 * ALIQ, 2);
                    VD = VB - VAD;
                    ALIQF = VD / RBT12;
                    VAL = Math.Round(valorNF * ALIQF, 2);
                    break;
                }
                else if (RBT12 < 180000)
                {
                    VAL = valorNF * 0.06;
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
                else if (RBT12 < tabelaR[i, 0])
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
            return 0;
        }

        static void OpcaoConsulta(ref int opcaoSubMenu)
        {
            while (true)
            {
                Console.WriteLine("Por favor, escolha uma das opções abaixo:");
                Console.WriteLine("1 - Consultar por Cliente");
                Console.WriteLine("2 - Consultar por Mês");
                //MenuPadrao();
                Console.Write("Opção escolhida: ");
                opcaoSubMenu = Check_input(Console.ReadLine().ToUpper());
                if (opcaoSubMenu == -1)
                {
                    Console.WriteLine("Por favor, escolha uma das opções abaixo:");
                    continue;
                }
                else if (opcaoSubMenu < 0)
                    return;
                else
                    break;
            }
        }


        static int Solicitar_mes(ref int mesEscolhido)
        {
            string value;
            Console.WriteLine("========================================================");
        SolicitarMes:
            Console.Write("Digite um valor de 1 à 12 que representam os meses de janeiro à dezembro: ");
            value = Console.ReadLine().ToUpper();
            if (value == "CANCELAR")
                return -2;
            else if (value == "VOLTAR")
                return -3;
            else if (value == "ENCERRAR")
                return -4;
            else if (value == "SAIR")
                return -5;
            mesEscolhido = Check_input(value);
            if (mesEscolhido <= 0 || mesEscolhido > 12)
                goto SolicitarMes;
            return 0;
        }
        static int Solicitar_ano(ref int anoEscolhido)
        {
        SolicitarAno:
            string value;
            Console.Write("Informe o Ano (4 dígitos) da NF que deseja emitir: ");
            value = Console.ReadLine().ToUpper();
            anoEscolhido = Check_input(value);
            if (anoEscolhido < 0)
                return anoEscolhido;
            else if (anoEscolhido < 1900)
            {
                Console.WriteLine("Por favor, verifique se o ano digitado está correto. ");
                goto SolicitarAno;
            }
            else
                return 0;
        }

        static int Check_yes_no(string value)
        {
            if (value.ToUpper() == "CANCELAR")
                return -2;
            else if (value.ToUpper() == "VOLTAR")
                return -3;
            else if (value.ToUpper() == "ENCERRAR")
                return -4;
            else if (value.ToUpper() == "SAIR")
                return -5;
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
        static int SolicitarCNPJ(ref string inputCNPJ, string papel)
        {
            //MenuPadrao();
            while (true)
            {
                Console.WriteLine("========================================================");
                if (papel == "cliente")
                    Console.Write("Digite o CNPJ do cliente: ");
                else
                    Console.Write("Digite seu CNPJ: ");
                inputCNPJ = Console.ReadLine().ToUpper();
                if (inputCNPJ == null)
                    continue;
                if (inputCNPJ.ToUpper() == "CANCELAR")
                    return -2;
                else if (inputCNPJ.ToUpper() == "VOLTAR")
                    return -3;
                else if (inputCNPJ.ToUpper() == "ENCERRAR")
                    return -4;
                else if (inputCNPJ.ToUpper() == "SAIR")
                    return -5;
                bool isValid = ValidaCNPJ(inputCNPJ);
                if (isValid)
                    break;
                Console.WriteLine("CNPJ inválido");

            }
            return 0;
        }
        #endregion
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
        static int VerificarCadastro(long numeroCNPJ, ref long[] empresaCNPJs, ref string[] nomeEmpresa, ref int index, string tipo)
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
            Console.WriteLine("========================================================");
            Console.Write($"Digite o nome da {tipo}: ");
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
                else if (inputNomeDaEmpresa == "ENCERRAR")
                {
                    empresaCNPJs[index] = 0;
                    return -4;
                }
                else if (inputNomeDaEmpresa == "SAIR")
                {
                    empresaCNPJs[index] = 0;
                    return -5;
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
            else if (value.ToUpper() == "SAIR")
                return -5;
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
            else if (value.ToUpper() == "SAIR")
                return -5;
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
        static void ImprimirClienteMes(double[] nfValor, string[] nfMes, string[] nfAno, long[] nfOrigemCNPJ, long numeroCNPJ, long[] nfClienteCNPJ, long numeroCNPJCliente)
        {
            int mesMax = Convert.ToInt32(DateTime.Now.ToString("MM"));
            int anoMax = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            int mesBase = mesMax;
            int anoBase = anoMax;
            int contador = 0;
            int qtdNFs = 0;
            while (nfValor[contador] != 0)
            {
                if (nfOrigemCNPJ[contador] == numeroCNPJ && nfClienteCNPJ[contador] == numeroCNPJCliente)
                {
                    if (anoBase == Convert.ToInt32(nfAno[contador]) && mesBase > Convert.ToInt32(nfMes[contador]))
                        mesBase = Convert.ToInt32(nfMes[contador]);
                    else if (anoBase > Convert.ToInt32(nfAno[contador]) && mesBase >= Convert.ToInt32(nfMes[contador]))
                    {
                        anoBase = Convert.ToInt32(nfAno[contador]);
                        mesBase = Convert.ToInt32(nfMes[contador]);
                    }
                }
                qtdNFs++;
                contador++;
            }

            while (anoBase <= anoMax)
            {
                contador = 0;
                while (mesBase <= 12)
                {
                    contador = 0;
                    double somaMes = 0;
                    while (contador <= qtdNFs)
                    {
                        if (nfOrigemCNPJ[contador] == numeroCNPJ && nfClienteCNPJ[contador] == numeroCNPJCliente && mesBase == Convert.ToInt32(nfMes[contador]) && anoBase == Convert.ToInt32(nfAno[contador]))
                            somaMes = nfValor[contador];
                        contador++;
                    }
                    if (somaMes > 0)
                    {
                        Console.Write($"|{mesBase.ToString("00")}/{anoBase.ToString("0000")}");
                        int lenght = (somaMes.ToString("N2").Length);
                        for (int i = 0; i < (37 - 7 - lenght - 3); i++)
                            Console.Write(" ");
                        Console.WriteLine($"R$ {somaMes.ToString("N2")}|");
                    }
                    mesBase++;
                }
                mesBase = 0;
                anoBase++;
            }
        }

        static void ImprimirMes(double[] nfValor, string[] nfMes, string[] nfAno, long[] nfOrigemCNPJ, long numeroCNPJ, long[] nfClienteCNPJ, string[] nfNome, int mesBuscado)
        {
            int mesMax = Convert.ToInt32(DateTime.Now.ToString("MM"));
            int anoMax = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            int mesBase = mesMax;
            int anoBase = anoMax;
            int contador = 0;
            int qtdNFs = 0;
            while (nfValor[contador] != 0)
            {
                if (nfOrigemCNPJ[contador] == numeroCNPJ)
                {
                    if (anoBase == Convert.ToInt32(nfAno[contador]) && mesBase > Convert.ToInt32(nfMes[contador]))
                        mesBase = Convert.ToInt32(nfMes[contador]);
                    else if (anoBase > Convert.ToInt32(nfAno[contador]) && mesBase >= Convert.ToInt32(nfMes[contador]))
                    {
                        anoBase = Convert.ToInt32(nfAno[contador]);
                        mesBase = Convert.ToInt32(nfMes[contador]);
                    }
                }
                qtdNFs++;
                contador++;
            }

            while (anoBase <= anoMax)
            {
                contador = 0;
                while (mesBase <= 12)
                {
                    contador = 0;
                    while (contador <= qtdNFs)
                    {
                        if (mesBuscado == Convert.ToInt32(nfMes[contador]) && nfOrigemCNPJ[contador] == numeroCNPJ && mesBase == Convert.ToInt32(nfMes[contador]) && anoBase == Convert.ToInt32(nfAno[contador]))
                        {
                            int lenghtCliente = (nfNome[contador].ToString().Length);
                            int lenghtValor = (nfValor[contador].ToString("N2").Length);

                            Console.Write($"|{nfNome[contador]}");
                            for (int i = 0; i < (58 - 7) / 2 - lenghtCliente; i++)
                                Console.Write(" ");
                            Console.Write($"{nfMes[contador]}/{nfAno[contador]}");
                            for (int i = 0; i < ((58 - 7) / 2 - lenghtValor) - 3; i++)
                                Console.Write(" ");
                            Console.WriteLine($"R$ {nfValor[contador].ToString("N2")}|");
                        }
                        contador++;
                    }
                    mesBase++;
                }
                mesBase = 0;
                anoBase++;
            }
        }

    }
}
