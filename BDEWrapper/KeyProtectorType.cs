namespace BDEWrapper
{
	/// <summary>
	/// Indicates the type of a given key protector
	/// </summary>
	public enum KeyProtectorType
    {
        UnknownOrOtherProtectorType = 0,
        TrustedPlatformModule = 1,
        ExternalKey = 2,
        NumericalPassword = 3,
        TPMandPIN = 4,
        TPMandStartupKey = 5,
        TPMandPINandStartupKey = 6,
        PublicKey = 7,
        Passphrase = 8,
    }
}
