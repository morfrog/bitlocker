using System;
using System.Management;

namespace BDEWrapper
{
	/// <summary>
	/// A wrapper of a key protector on a specific Win32_EncryptableVolume instance
	/// </summary>
	public class KeyProtector
    {
        private EncryptedDrive m_oEncryptedDrive;
        private string m_sVolumeKeyProtectorID;

        internal KeyProtector(EncryptedDrive EncryptedDrive, string VolumeKeyProtectorID)
        {
            // assign the encrypted drive object
            m_oEncryptedDrive = EncryptedDrive;
            m_sVolumeKeyProtectorID = VolumeKeyProtectorID;

            return;
        }

        /// <summary>
        /// An unsigned integer that specifies the type of the key protector.
        /// </summary>
        public KeyProtectorType Type
        {
            get
            {
                ManagementBaseObject oResults;
                ManagementBaseObject oInParams = m_oEncryptedDrive.GetMethodInputParameters("GetKeyProtectorType");
                oInParams["VolumeKeyProtectorID"] = m_sVolumeKeyProtectorID;
                if (m_oEncryptedDrive.ExecuteMethod("GetKeyProtectorType", oInParams, out oResults) == OperationResults.S_OK)
                {
                    return (KeyProtectorType)((uint)oResults["KeyProtectorType"]);
                }
                return KeyProtectorType.UnknownOrOtherProtectorType;
            }
        }

        /// <summary>
        /// A unique string identifier used to manage an encrypted volume key protector.
        /// </summary>
        public string VolumeKeyProtectorID
        {
            get
            {
                return m_sVolumeKeyProtectorID;
            }
        }

        /// <summary>
        /// Changes a PIN associated with an encrypted volume.
        /// </summary>
        /// <param name="NewPIN">A user-specified personal identification string. This string must consist of a sequence of 4 to 20 digits or, if the "Allow enhanced PINs for startup" group policy is enabled, 4 to 20 letters, symbols, spaces, or numbers.</param>
        /// <returns>The Win32 Return Code</returns>
        public OperationResults ChangePIN(string NewPIN)
        {
            // change the PIN
            ManagementBaseObject oInParams = m_oEncryptedDrive.GetMethodInputParameters("ChangePIN");
            oInParams["VolumeKeyProtectorID"] = m_sVolumeKeyProtectorID;
            oInParams["NewPIN"] = NewPIN;

            // execute the method
            ManagementBaseObject oResults;
            OperationResults iResult = m_oEncryptedDrive.ExecuteMethod("ChangePIN", oInParams, out oResults);

            // release and return
            oResults = null;
            oInParams = null;
            return iResult;
        }

        /// <summary>
        /// Deletes a given key protector for the volume.
        /// </summary>
        /// <returns>The Win32 Return Code</returns>
        public OperationResults Delete()
        {
            // delete the key protector
            ManagementBaseObject oInParams = m_oEncryptedDrive.GetMethodInputParameters("DeleteKeyProtector");
            oInParams["VolumeKeyProtectorID"] = m_sVolumeKeyProtectorID;

            // execute the method
            ManagementBaseObject oResults;
            OperationResults iResult = m_oEncryptedDrive.ExecuteMethod("DeleteKeyProtector", oInParams, out oResults);

            // release and return
            oResults = null;
            oInParams = null;
            return iResult;
        }

        /// <summary>
        /// Uses the new passphrase to obtain a new derived key.
        /// </summary>
        /// <param name="NewPassphrase">An updated string that specifies the passphrase.</param>
        /// <returns>The Win32 Return Code</returns>
        public OperationResults ChangePassphrase(string NewPassphrase)
        {
            // change the PIN
            ManagementBaseObject oInParams = m_oEncryptedDrive.GetMethodInputParameters("ChangePassphrase");
            oInParams["VolumeKeyProtectorID"] = m_sVolumeKeyProtectorID;
            oInParams["NewPassphrase"] = NewPassphrase;

            // check if the drive is currently protected
            if (m_oEncryptedDrive.ProtectionStatus == ProtectionStatus.ProtectionOn)
            {
                // disable protection
                ManagementBaseObject oDisableResults;
                if (m_oEncryptedDrive.ExecuteMethod("DisableKeyProtectors", m_oEncryptedDrive.GetMethodInputParameters("DisableKeyProtectors"), out oDisableResults)
                    != OperationResults.S_OK)
                    throw new InvalidOperationException("Could not disable the key protectors.");
                oDisableResults = null;
            }

            // execute the method
            ManagementBaseObject oResults;
            OperationResults iResult = m_oEncryptedDrive.ExecuteMethod("ChangePassphrase", oInParams, out oResults);
            if (iResult == OperationResults.S_OK)
            {
                // change the internal identifier
                m_sVolumeKeyProtectorID = (string)oResults["NewProtectorID"];
            }

            // check if the drive is currently protected
            if (m_oEncryptedDrive.ProtectionStatus == ProtectionStatus.ProtectionOff)
            {
                // disable protection
                ManagementBaseObject oEnableResults;
                if (m_oEncryptedDrive.ExecuteMethod("EnableKeyProtectors", m_oEncryptedDrive.GetMethodInputParameters("EnableKeyProtectors"), out oEnableResults)
                    != OperationResults.S_OK)
                    throw new InvalidOperationException("Could not enable the key protectors.");
                oEnableResults = null;
            }


            // release and return
            oResults = null;
            oInParams = null;
            return iResult;
        }
    }
}
