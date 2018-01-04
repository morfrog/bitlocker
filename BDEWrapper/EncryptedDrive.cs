using System;
using System.Collections.Generic;
using System.Management;

namespace BDEWrapper
{
	/// <summary>
	/// A wrapper for a Win32_EncryptableVolume instance 
	/// </summary>
	public class EncryptedDrive
    {
        // Internal properties
        /// <summary>
        /// A unique identifier for the volume on this system. Use this to associate a volume with other WMI provider classes, for example, Win32_Volume.
        /// </summary>
        private string m_sDeviceID;
        //private string m_sDriveLetter;
        //private string m_sPersistentVolumeID;
        //private int m_iProtectionStatus;
        private ManagementObject m_oWMIObj;

        /// <param name="PersistentVolumeID">A persistent identifier for the volume on this system. This identifier is exclusive to Win32_EncryptableVolume.</param>
        public EncryptedDrive(string DeviceID)
        {
            // store the persistence volume id
            this.m_sDeviceID = DeviceID.Trim();   
     
            // call the setup method
            this.connectToObject("localhost");
        }

        /// <param name="PersistentVolumeID">A persistent identifier for the volume on this system. This identifier is exclusive to Win32_EncryptableVolume.</param>
        /// <param name="MachineName">The machine on which the object exists.</param>
        public EncryptedDrive(string DeviceID, string MachineName)
        {
            // store the persistence volume id
            this.m_sDeviceID = DeviceID.Trim();   

            // call the setup method
            this.connectToObject(MachineName);
        }

        

        /// <summary>
        /// A unique identifier for the volume on this system. Use this to associate a volume with other WMI provider classes, for example, Win32_Volume.
        /// </summary>
        public string DeviceID
        {
            get
            {
                return (string)m_oWMIObj["DeviceID"];
            }
        }

        /// <summary>
        /// The drive letter of the volume. This identifier can be used to associate a volume with other WMI provider classes, for example Win32_Volume.
        /// </summary>
        /// <remarks>For volumes without drive letters, this value is NULL.</remarks>
        public string DriveLetter
        {
            get
            {
                return (string)m_oWMIObj["DriveLetter"];
            }
        }

        /// <summary>
        /// Gets whether the contents of the volume are accessible from the currently running operating system.
        /// </summary>
        public LockedStatus LockStatus
        {
            get
            {
                // setup return object
                ManagementBaseObject oResults;
                // execute the method
                if (ExecuteMethod("GetLockStatus", m_oWMIObj.GetMethodParameters("GetLockStatus"), out oResults) ==
                    OperationResults.S_OK)
                {
                    LockedStatus iLockStatus = (LockedStatus)((uint)oResults["LockStatus"]);
                    oResults = null;
                    return iLockStatus;
                }
                // throw an exception
                throw new ArgumentException("The returned an unknown result.");
            }
        }

        /// <summary>
        /// A persistent identifier for the volume on this system. This identifier is exclusive to Win32_EncryptableVolume.
        /// </summary>
        public string PersistentVolumeID
        {
            get
            {
                return (string)m_oWMIObj["PersistentVolumeID"];
            }
        }

        /// <summary>
        /// The status of the volume, whether or not BitLocker is protecting the volume.
        /// </summary>
        /// <remarks>It is possible for the protection status to change state between instantiation and when you check the value. To check the value of the ProtectionStatus property in real time, use the GetProtectionStatus method.</remarks>
        public ProtectionStatus ProtectionStatus
        {
            get
            {
                // get the most current status
                ManagementBaseObject oResults;
                if (ExecuteMethod("GetProtectionStatus", m_oWMIObj.GetMethodParameters("GetProtectionStatus"), out oResults) == OperationResults.S_OK)
                {
                    return (ProtectionStatus)((uint)oResults["ProtectionStatus"]);
                }
                else
                {
                    return ProtectionStatus.ProtectionUnknown;
                }
            }
        }

        /// <summary>
        /// Gets a list of key protectors that are associated with this instance
        /// </summary>
        public List<KeyProtector> KeyProtectors
        {
            get
            {
                // get a current list of objects
                ManagementBaseObject oInParams = m_oWMIObj.GetMethodParameters("GetKeyProtectors");
                oInParams["KeyProtectorType"] = 0;
                ManagementBaseObject oResults;
                if (ExecuteMethod("GetKeyProtectors", oInParams, out oResults) == OperationResults.S_OK)
                {
                    string[] szKeyProtectors = (string[])oResults["VolumeKeyProtectorID"];
                    // loop each object
                    List<KeyProtector> oProtectors = new List<KeyProtector>();
                    foreach (string sProtector in szKeyProtectors)
                    {
                        KeyProtector oProtector = new KeyProtector(this, sProtector);
                        oProtectors.Add(oProtector);
                    }
                    // release and return to caller
                    oInParams = null;
                    oResults = null;
                    szKeyProtectors = null;
                    return oProtectors;
                }
                return new List<KeyProtector>();
            }
        }

        /// <summary>
        /// Obtains an instance of the WMI object
        /// </summary>
        /// <param name="HostName">The hostname to connect to</param>
        private void connectToObject(string HostName)
        {
            // setup the management path object
            ManagementPath oWMIPath = new ManagementPath();
            oWMIPath.Server = HostName;
            oWMIPath.NamespacePath = @"root\CIMV2\Security\MicrosoftVolumeEncryption";

            // setup the relative path
            oWMIPath.RelativePath = String.Format("Win32_EncryptableVolume.DeviceID='{0}'", m_sDeviceID);

            // setup the connection object as we have to request special authentication
            ConnectionOptions oWMIConnOptions = new ConnectionOptions();
            oWMIConnOptions.Authentication = AuthenticationLevel.PacketPrivacy;

            // bind everything together in a scope
            ManagementScope oWMIScope = new ManagementScope(oWMIPath, oWMIConnOptions);

            // get an instance of the WMI object
            m_oWMIObj = new ManagementObject(oWMIScope, oWMIPath, null);

            // return to caller
            return;
        }

        internal OperationResults ExecuteMethod(string MethodName, object[] MethodParams, out ManagementBaseObject OutputParameters)
        {
            // invoke the method
            ManagementBaseObject outParams = (ManagementBaseObject)m_oWMIObj.InvokeMethod(
                MethodName, MethodParams);

            // read the return code and copy the results to the output variable
            OperationResults rtnCode = (OperationResults)((uint)outParams["returnValue"]);
            OutputParameters = outParams;

            // return to caller
            return rtnCode;
        }

        internal OperationResults ExecuteMethod(string MethodName, ManagementBaseObject InParams, out ManagementBaseObject OutputParameters)
        {
            // invoke the method
            ManagementBaseObject outParams = m_oWMIObj.InvokeMethod(
                MethodName, InParams, null);

            // read the return code and copy the results to the output variable
            OperationResults rtnCode = (OperationResults)((uint)outParams["returnValue"]);
            OutputParameters = outParams;
            
            // return to caller
            return rtnCode;
        }

        internal ManagementBaseObject GetMethodInputParameters(string MethodName)
        {
            return m_oWMIObj.GetMethodParameters(MethodName);
        }


        /// <summary>
        /// Secures the volume's encryption key with a specially formatted 48-digit password
        /// </summary>
        /// <param name="VolumeKeyProtectorID">After execution, assigns the new volume key protector ID.</param>
        /// <returns>The Win32 return code</returns>
        public OperationResults ProtectKeyWithNumericalPassword(out string VolumeKeyProtectorID)
        {
            ManagementBaseObject oResults;
            OperationResults iResult =  ExecuteMethod("ProtectKeyWithNumericalPassword", m_oWMIObj.GetMethodParameters("ProtectKeyWithNumericalPassword"), out oResults);
            VolumeKeyProtectorID = (string)oResults["VolumeKeyProtectorID"];
            return iResult;
        }

        /// <summary>
        /// Saves all external keys and related information that is needed for recovery to the Active Directory.
        /// </summary>
        /// <returns>The Win32 return code</returns>
        public OperationResults BackupRecoveryInformationToActiveDirectory()
        {
            OperationResults iResults = OperationResults.S_OK;
            // loop each numerical key protector
            foreach (KeyProtector oKeyProtector in KeyProtectors)
            {
                if (oKeyProtector.Type == KeyProtectorType.NumericalPassword)
                {
                    ManagementBaseObject oOutput;
                    ManagementBaseObject oInParams = m_oWMIObj.GetMethodParameters("BackupRecoveryInformationToActiveDirectory");
                    oInParams["VolumeKeyProtectorID"] = oKeyProtector.VolumeKeyProtectorID;
                    OperationResults iIndResults = ExecuteMethod("BackupRecoveryInformationToActiveDirectory",
                        oInParams, out oOutput);
                    if (iIndResults != OperationResults.S_OK)
                        iResults = iIndResults;
                }
            }
            
            // release and return
            return iResults;
        }

    }
}
