using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stone.BusinessServices;
using Stone.BusinessEntities;
using Stone.Common;
using Stone.BusinessServices.Config;
using Stone.Cipher;
using System.Collections.Generic;

namespace Stone.TestApi
{
    [TestClass]
    public class ApiTransactionTest
    {
        static List<TransactionEntity> lstRollBack = new List<TransactionEntity>();

        [ClassInitialize]
        public static  void Startup(TestContext context)
        {
            try
            {
                //AutoMapper
                AutoMapperConfig.RegisterMappings();

                CardService cardService = new CardService();
                cardService.Create(new CardEntity
                {
                    Attempts = 0,
                    Balance = 1000,
                    Blocked = false,
                    CardBrand = new CardBrandEntity { Id = 1, Name = "VISA" },
                    CardholderName = "saldo insuficiente",
                    CardType = new CardTypeEntity { Id = (int)CardTypeEntity.Types.Chip },
                    ExpirationDate = DateTime.MaxValue,
                    Limit = 2000,
                    LimitUsed = 0,
                    Number = "111111111111",
                    Password = "1234"
                });

                cardService.Create(new CardEntity
                {
                    Attempts = 0,
                    Balance = 1000,
                    Blocked = true,
                    CardBrand = new CardBrandEntity { Id = 1, Name = "VISA" },
                    CardholderName = "cartão bloqueado",
                    CardType = new CardTypeEntity { Id = (int)CardTypeEntity.Types.Chip },
                    ExpirationDate = DateTime.MaxValue,
                    Limit = 2000,
                    LimitUsed = 0,
                    Number = "222222222222",
                    Password = "1234"
                });

                cardService.Create(new CardEntity
                {
                    Attempts = 0,
                    Balance = 1000,
                    Blocked = false,
                    CardBrand = new CardBrandEntity { Id = 1, Name = "VISA" },
                    CardholderName = "cartão expirado",
                    CardType = new CardTypeEntity { Id = (int)CardTypeEntity.Types.Chip },
                    ExpirationDate = DateTime.Now.AddDays(-1),
                    Limit = 2000,
                    LimitUsed = 0,
                    Number = "333333333333",
                    Password = "1234"
                });

                cardService.Create(new CardEntity
                {
                    Attempts = 0,
                    Balance = 1000,
                    Blocked = false,
                    CardBrand = new CardBrandEntity { Id = 1, Name = "VISA" },
                    CardholderName = "bloquear cartao",
                    CardType = new CardTypeEntity { Id = (int)CardTypeEntity.Types.Chip },
                    ExpirationDate = DateTime.MaxValue,
                    Limit = 2000,
                    LimitUsed = 0,
                    Number = "444444444444",
                    Password = "1234"
                });

                cardService.Create(new CardEntity
                {
                    Attempts = 0,
                    Balance = 1000,
                    Blocked = false,
                    CardBrand = new CardBrandEntity { Id = 1, Name = "VISA" },
                    CardholderName = "limite excedido",
                    CardType = new CardTypeEntity { Id = (int)CardTypeEntity.Types.Chip },
                    ExpirationDate = DateTime.MaxValue,
                    Limit = 2000,
                    LimitUsed = 0,
                    Number = "555555555555",
                    Password = "1234"
                });
            }
            catch (Exception)
            {
                Assert.Fail();
            } 
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            try
            {
                TransactionService transactionService = new TransactionService();
                foreach (TransactionEntity t in lstRollBack)
                {
                    transactionService.Delete(t.Id);
                }
            }
            catch { }

            try
            {
                CardService cardService = new CardService();
                cardService.Delete(cardService.GetByNumber("111111111111").Id);
                cardService.Delete(cardService.GetByNumber("222222222222").Id);
                cardService.Delete(cardService.GetByNumber("333333333333").Id);
                cardService.Delete(cardService.GetByNumber("444444444444").Id);
                cardService.Delete(cardService.GetByNumber("555555555555").Id);
            }
            catch { }
        }
        
        [TestMethod]
        public void SenhaIncorreta()
        {
            TransactionService transactionService = new TransactionService();
            try
            {
                TransactionEntity transacation1 = new TransactionEntity
                {
                    ClientCode = new Guid("d358de2e-886f-43e1-8237-cfb03343a1f7"),
                    Amount = 20,
                    Number = 1,
                    TransactionType = new TransactionTypeEntity
                    {
                        Id = (int)TransactionTypeEntity.Types.Debito
                    },
                    Card = new CardEntity
                    {
                        Number = "111111111111",
                        Password = StringCipher.Encrypt("12345"),
                    }
                };

                TransactionEntity.TransactionReturnEnum rtn1 = transactionService.DoTransaction(transacation1);
                lstRollBack.Add(transacation1);
                Assert.AreEqual(TransactionEntity.TransactionReturnEnum.Senha_invalida, rtn1);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void TresTentativasSenhaIncorreta()
        {
            TransactionService transactionService = new TransactionService();
            try
            {
                TransactionEntity transacation1 = new TransactionEntity
                {
                    ClientCode = new Guid("d358de2e-886f-43e1-8237-cfb03343a1f7"),
                    Amount = 20,
                    Number = 1,
                    TransactionType = new TransactionTypeEntity
                    {
                        Id = (int)TransactionTypeEntity.Types.Debito
                    },
                    Card = new CardEntity
                    {
                        Number = "444444444444",
                        Password = StringCipher.Encrypt("12345"),
                    }
                };

                TransactionEntity transacation2 = new TransactionEntity
                {
                    ClientCode = new Guid("d358de2e-886f-43e1-8237-cfb03343a1f7"),
                    Amount = 20,
                    Number = 1,
                    TransactionType = new TransactionTypeEntity
                    {
                        Id = (int)TransactionTypeEntity.Types.Debito
                    },
                    Card = new CardEntity
                    {
                        Number = "444444444444",
                        Password = StringCipher.Encrypt("12345"),
                    }
                };

                TransactionEntity transacation3 = new TransactionEntity
                {
                    ClientCode = new Guid("d358de2e-886f-43e1-8237-cfb03343a1f7"),
                    Amount = 20,
                    Number = 1,
                    TransactionType = new TransactionTypeEntity
                    {
                        Id = (int)TransactionTypeEntity.Types.Debito
                    },
                    Card = new CardEntity
                    {
                        Number = "444444444444",
                        Password = StringCipher.Encrypt("12345"),
                    }
                };

                TransactionEntity transacation4 = new TransactionEntity
                {
                    ClientCode = new Guid("d358de2e-886f-43e1-8237-cfb03343a1f7"),
                    Amount = 20,
                    Number = 1,
                    TransactionType = new TransactionTypeEntity
                    {
                        Id = (int)TransactionTypeEntity.Types.Debito
                    },
                    Card = new CardEntity
                    {
                        Number = "444444444444",
                        Password = StringCipher.Encrypt("12345"),
                    }
                };

                TransactionEntity.TransactionReturnEnum rtn1 = transactionService.DoTransaction(transacation1);
                lstRollBack.Add(transacation1);
                Assert.AreEqual(TransactionEntity.TransactionReturnEnum.Senha_invalida, rtn1);

                TransactionEntity.TransactionReturnEnum rtn2 = transactionService.DoTransaction(transacation2);
                lstRollBack.Add(transacation2);
                Assert.AreEqual(TransactionEntity.TransactionReturnEnum.Senha_invalida, rtn2);

                TransactionEntity.TransactionReturnEnum rtn3 = transactionService.DoTransaction(transacation3);
                lstRollBack.Add(transacation3);
                Assert.AreEqual(TransactionEntity.TransactionReturnEnum.Senha_invalida, rtn3);

                TransactionEntity.TransactionReturnEnum rtn4 = transactionService.DoTransaction(transacation4);
                lstRollBack.Add(transacation4);
                Assert.AreEqual(TransactionEntity.TransactionReturnEnum.Cartao_bloqueado, rtn4);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            
        }

        [TestMethod]
        public void CartaoBloqueado()
        {
            TransactionService transactionService = new TransactionService();
            try
            {
                TransactionEntity transacation1 = new TransactionEntity
                {
                    ClientCode = new Guid("d358de2e-886f-43e1-8237-cfb03343a1f7"),
                    Amount = 20,
                    Number = 1,
                    TransactionType = new TransactionTypeEntity
                    {
                        Id = (int)TransactionTypeEntity.Types.Debito
                    },
                    Card = new CardEntity
                    {
                        Number = "222222222222",
                        Password = StringCipher.Encrypt("1234"),
                    }
                };

                TransactionEntity.TransactionReturnEnum rtn1 = transactionService.DoTransaction(transacation1);
                Assert.AreEqual(TransactionEntity.TransactionReturnEnum.Cartao_bloqueado, rtn1);
                lstRollBack.Add(transacation1);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void CartaoInvalido()
        {
            TransactionService transactionService = new TransactionService();
            try
            {
                TransactionEntity transacation1 = new TransactionEntity
                {
                    ClientCode = new Guid("d358de2e-886f-43e1-8237-cfb03343a1f7"),
                    Amount = 20,
                    Number = 1,
                    TransactionType = new TransactionTypeEntity
                    {
                        Id = (int)TransactionTypeEntity.Types.Debito
                    },
                    Card = new CardEntity
                    {
                        Number = "888888888888",
                        Password = StringCipher.Encrypt("1234"),
                    }
                };

                TransactionEntity.TransactionReturnEnum rtn1 = transactionService.DoTransaction(transacation1);
                Assert.AreEqual(TransactionEntity.TransactionReturnEnum.Cartao_invalido, rtn1);

            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void CartaoVencido()
        {
            TransactionService transactionService = new TransactionService();
            try
            {
                TransactionEntity transacation1 = new TransactionEntity
                {
                    ClientCode = new Guid("d358de2e-886f-43e1-8237-cfb03343a1f7"),
                    Amount = 20,
                    Number = 1,
                    TransactionType = new TransactionTypeEntity
                    {
                        Id = (int)TransactionTypeEntity.Types.Debito
                    },
                    Card = new CardEntity
                    {
                        Number = "333333333333",
                        Password = StringCipher.Encrypt("1234"),
                    }
                };

                TransactionEntity.TransactionReturnEnum rtn1 = transactionService.DoTransaction(transacation1);
                Assert.AreEqual(TransactionEntity.TransactionReturnEnum.Cartao_vencido, rtn1);
                lstRollBack.Add(transacation1);

            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void ValorInvalido()
        {
            TransactionService transactionService = new TransactionService();
            try
            {
                TransactionEntity transacation1 = new TransactionEntity
                {
                    ClientCode = new Guid("d358de2e-886f-43e1-8237-cfb03343a1f7"),
                    Amount = Convert.ToDecimal(0.09),
                    Number = 1,
                    TransactionType = new TransactionTypeEntity
                    {
                        Id = (int)TransactionTypeEntity.Types.Debito
                    },
                    Card = new CardEntity
                    {
                        Number = "111111111111",
                        Password = StringCipher.Encrypt("1234"),
                    }
                };

                TransactionEntity.TransactionReturnEnum rtn1 = transactionService.DoTransaction(transacation1);
                Assert.AreEqual(TransactionEntity.TransactionReturnEnum.Valor_invalido, rtn1);
                lstRollBack.Add(transacation1);

            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void SaldoInsuficienteEmUmaOperacao()
        {
            TransactionService transactionService = new TransactionService();
            try
            {
                TransactionEntity transacation1 = new TransactionEntity
                {
                    ClientCode = new Guid("d358de2e-886f-43e1-8237-cfb03343a1f7"),
                    Amount = Convert.ToDecimal(1000.2),
                    Number = 1,
                    TransactionType = new TransactionTypeEntity
                    {
                        Id = (int)TransactionTypeEntity.Types.Debito
                    },
                    Card = new CardEntity
                    {
                        Number = "111111111111",
                        Password = StringCipher.Encrypt("1234"),
                    }
                };

                TransactionEntity.TransactionReturnEnum rtn1 = transactionService.DoTransaction(transacation1);
                Assert.AreEqual(TransactionEntity.TransactionReturnEnum.Saldo_insuficiente, rtn1);
                lstRollBack.Add(transacation1);

            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void SaldoInsuficienteVariasOperacoes()
        {
            TransactionService transactionService = new TransactionService();
            try
            {
                TransactionEntity transacation1 = new TransactionEntity
                {
                    ClientCode = new Guid("d358de2e-886f-43e1-8237-cfb03343a1f7"),
                    Amount = Convert.ToDecimal(200),
                    Number = 1,
                    TransactionType = new TransactionTypeEntity
                    {
                        Id = (int)TransactionTypeEntity.Types.Debito
                    },
                    Card = new CardEntity
                    {
                        Number = "111111111111",
                        Password = StringCipher.Encrypt("1234"),
                    }
                };

                TransactionEntity transacation2 = new TransactionEntity
                {
                    ClientCode = new Guid("d358de2e-886f-43e1-8237-cfb03343a1f7"),
                    Amount = Convert.ToDecimal(700),
                    Number = 1,
                    TransactionType = new TransactionTypeEntity
                    {
                        Id = (int)TransactionTypeEntity.Types.Debito
                    },
                    Card = new CardEntity
                    {
                        Number = "111111111111",
                        Password = StringCipher.Encrypt("1234"),
                    }
                };

                TransactionEntity transacation3 = new TransactionEntity
                {
                    ClientCode = new Guid("d358de2e-886f-43e1-8237-cfb03343a1f7"),
                    Amount = Convert.ToDecimal(300),
                    Number = 1,
                    TransactionType = new TransactionTypeEntity
                    {
                        Id = (int)TransactionTypeEntity.Types.Debito
                    },
                    Card = new CardEntity
                    {
                        Number = "111111111111",
                        Password = StringCipher.Encrypt("1234"),
                    }
                };

                TransactionEntity.TransactionReturnEnum rtn1 = transactionService.DoTransaction(transacation1);
                Assert.AreEqual(TransactionEntity.TransactionReturnEnum.Aprovado, rtn1);
                lstRollBack.Add(transacation1);

                TransactionEntity.TransactionReturnEnum rtn2 = transactionService.DoTransaction(transacation2);
                Assert.AreEqual(TransactionEntity.TransactionReturnEnum.Aprovado, rtn2);
                lstRollBack.Add(transacation2);

                TransactionEntity.TransactionReturnEnum rtn3 = transactionService.DoTransaction(transacation3);
                Assert.AreEqual(TransactionEntity.TransactionReturnEnum.Saldo_insuficiente, rtn3);
                lstRollBack.Add(transacation3);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void LimiteExcedidoEmUmaOperacao()
        {
            TransactionService transactionService = new TransactionService();
            try
            {
                TransactionEntity transacation1 = new TransactionEntity
                {
                    ClientCode = new Guid("d358de2e-886f-43e1-8237-cfb03343a1f7"),
                    Amount = Convert.ToDecimal(2000.2),
                    Number = 1,
                    TransactionType = new TransactionTypeEntity
                    {
                        Id = (int)TransactionTypeEntity.Types.Credito
                    },
                    Card = new CardEntity
                    {
                        Number = "555555555555",
                        Password = StringCipher.Encrypt("1234"),
                    }
                };

                TransactionEntity.TransactionReturnEnum rtn1 = transactionService.DoTransaction(transacation1);
                Assert.AreEqual(TransactionEntity.TransactionReturnEnum.Limite_excedido, rtn1);
                lstRollBack.Add(transacation1);

            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void LimiteExcedidoEmVariasOperacoes()
        {
            TransactionService transactionService = new TransactionService();
            try
            {
                TransactionEntity transacation1 = new TransactionEntity
                {
                    ClientCode = new Guid("d358de2e-886f-43e1-8237-cfb03343a1f7"),
                    Amount = Convert.ToDecimal(400),
                    Number = 1,
                    TransactionType = new TransactionTypeEntity
                    {
                        Id = (int)TransactionTypeEntity.Types.Credito
                    },
                    Card = new CardEntity
                    {
                        Number = "555555555555",
                        Password = StringCipher.Encrypt("1234"),
                    }
                };

                TransactionEntity transacation2 = new TransactionEntity
                {
                    ClientCode = new Guid("d358de2e-886f-43e1-8237-cfb03343a1f7"),
                    Amount = Convert.ToDecimal(1400),
                    Number = 1,
                    TransactionType = new TransactionTypeEntity
                    {
                        Id = (int)TransactionTypeEntity.Types.Credito
                    },
                    Card = new CardEntity
                    {
                        Number = "555555555555",
                        Password = StringCipher.Encrypt("1234"),
                    }
                };

                TransactionEntity transacation3 = new TransactionEntity
                {
                    ClientCode = new Guid("d358de2e-886f-43e1-8237-cfb03343a1f7"),
                    Amount = Convert.ToDecimal(300),
                    Number = 1,
                    TransactionType = new TransactionTypeEntity
                    {
                        Id = (int)TransactionTypeEntity.Types.Credito
                    },
                    Card = new CardEntity
                    {
                        Number = "555555555555",
                        Password = StringCipher.Encrypt("1234"),
                    }
                };

                TransactionEntity.TransactionReturnEnum rtn1 = transactionService.DoTransaction(transacation1);
                Assert.AreEqual(TransactionEntity.TransactionReturnEnum.Aprovado, rtn1);
                lstRollBack.Add(transacation1);

                TransactionEntity.TransactionReturnEnum rtn2 = transactionService.DoTransaction(transacation2);
                Assert.AreEqual(TransactionEntity.TransactionReturnEnum.Aprovado, rtn2);
                lstRollBack.Add(transacation2);

                TransactionEntity.TransactionReturnEnum rtn3 = transactionService.DoTransaction(transacation3);
                Assert.AreEqual(TransactionEntity.TransactionReturnEnum.Limite_excedido, rtn3);
                lstRollBack.Add(transacation3);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }
    }
}
