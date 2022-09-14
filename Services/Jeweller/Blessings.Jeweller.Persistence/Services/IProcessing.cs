namespace Blessings.JewellerApi.Services;

public interface IProcessing
{
    Task<Jeweller.Domain.Jeweller?> JewellerChecker();
}