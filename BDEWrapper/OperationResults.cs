namespace BDEWrapper
{
	/// <summary>
	/// The code that is returned by the method call. This can be either a success or failure depending on the code returned.
	/// </summary>
	public enum OperationResults : uint
    {
        /// <summary>
        /// The method was successful.
        /// </summary>
        S_OK = 0,
        /// <summary>
        /// A bootable CD/DVD is found in this computer. Remove the CD/DVD and restart the computer.
        /// <value>2150694960</value>
        /// </summary>
        FVE_E_BOOTABLE_CDDVD = 2150694960,
        /// <summary>
        /// The NewPIN parameter contains characters that are not valid. When the "Allow enhanced PINs for startup" Group Policy is disabled, only numbers are supported.
        /// </summary>
        FVE_E_INVALID_PIN_CHARS = 2150695066,
        /// <summary>
        /// The VolumeKeyProtectorID parameter does not refer to a key protector of the type "Numerical Password" or "External Key". Use either the ProtectKeyWithNumericalPassword or ProtectKeyWithExternalKey method to create a key protector of the appropriate type.
        /// </summary>
        FVE_E_INVALID_PROTECTOR_TYPE = 2150694970,
        /// <summary>
        /// The volume is locked.
        /// </summary>
        FVE_E_LOCKED_VOLUME = 2150694912,
        /// <summary>
        /// BitLocker is not enabled on the volume. Add a key protector to enable BitLocker.
        /// </summary>
        FVE_E_NOT_ACTIVATED = 2150694920,
        /// <summary>
        /// The NewPIN parameter supplied is either longer than 20 characters, shorter than 4 characters, or shorter than the minimum length specified by Group Policy.
        /// </summary>
        FVE_E_POLICY_INVALID_PIN_LENGTH = 2150695016,
        /// <summary>
        /// The provided key protector does not exist on the volume.
        /// </summary>
        FVE_E_PROTECTOR_NOT_FOUND = 2150694963,
        /// <summary>
        /// No compatible Trusted Platform Module (TPM) is found on this computer.
        /// </summary>
        TBS_E_SERVICE_NOT_RUNNING = 2150121480,
        /// <summary>
        /// The VolumeKeyProtectorID parameter does not refer to a valid KeyProtectorType.
        /// </summary>
        E_INVALIDARG = 2147942487,
        /// <summary>
        /// The last key protector for a partially or fully encrypted volume cannot be removed if key protectors are enabled. Use DisableKeyProtectors before removing this last key protector to ensure that encrypted portions of the volume remain accessible.
        /// </summary>
        FVE_E_KEY_REQUIRED = 2150694941,
        /// <summary>
        /// The passphrase provided does not meet the minimum or maximum length requirements.
        /// </summary>
        FVE_E_POLICY_INVALID_PASSPHRASE_LENGTH = 2150695040,
        /// <summary>
        /// The passphrase does not meet the complexity requirements set by the administrator in group policy.
        /// </summary>
        FVE_E_POLICY_PASSPHRASE_TOO_SIMPLE = 2150695041,
        /// <summary>
        /// The control block for the encrypted volume was updated by another thread.
        /// </summary>
        FVE_E_OVERLAPPED_UPDATE = 2150694948,
        /// <summary>
        /// The key protector is not supported by the version of BitLocker Drive Encryption currently on the volume.
        /// </summary>
        FVE_E_KEY_PROTECTOR_NOT_SUPPORTED = 2150695017,
        /// <summary>
        /// The provided key protector already exists on this volume.
        /// </summary>
        FVE_E_PROTECTOR_EXISTS = 2150694960,
        /// <summary>
        /// This key protector cannot be deleted because it is being used to automatically unlock the volume.
        /// </summary>
        FVE_E_VOLUME_BOUND_ALREADY = 2150694943,
        /// <summary>
        /// Unknown return code 2147943568. If you have details of this return code, please contact the project.
        /// </summary>
        UNKNOWN_2147943568 = 2147943568,
    }
}
