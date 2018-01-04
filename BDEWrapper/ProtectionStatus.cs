namespace BDEWrapper
{
	/// <summary>
	/// The status of the volume, whether or not BitLocker is protecting the volume. This value is stored when the class is instantiated. It is possible for the protection status to change state between instantiation and when you check the value. To check the value of the ProtectionStatus property in real time, use the GetProtectionStatus method.
	/// </summary>
	public enum ProtectionStatus : uint
    {
        /// <summary>
        /// The volume is not encrypted, partially encrypted, or the volume's encryption key for the volume is available in the clear on the hard disk.
        /// </summary>
        ProtectionOff = 0,
        /// <summary>
        /// The volume is fully encrypted and the encryption key for the volume is not available in the clear on the hard disk.
        /// </summary>
        ProtectionOn = 1,
        /// <summary>
        /// The volume protection status cannot be determined. One potential cause is that the volume is in a locked state.
        /// </summary>
        ProtectionUnknown = 2,
    }
}
